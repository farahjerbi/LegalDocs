using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Configuration;
using Server.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Server.DTOs;
using RazorEngine.Templating;
using AutoMapper;
using Aspose.Html.Converters;
using Newtonsoft.Json;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using Server.Services;
using Aspose.Diagram;
using User = Server.Domain.User;
using Microsoft.AspNetCore.Hosting;
using System.Web.Http.Results;
using System.Diagnostics;
using OpenAI_API;
using Aspose.Slides.Export.Web;
using System.Text;
using GoogleTranslateFreeApi;
using Language = GoogleTranslateFreeApi.Language;
using OpenAI_API.Moderation;
using Newtonsoft.Json.Linq;
using Aspose.Cells;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly Context _dbContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ITranslateService _translationService;
        private readonly IPdfService _pdfService;



        public TemplateController(Context dbContext, IMapper mapper, IFileService fileService,
            IWebHostEnvironment webHostEnvironment , ITranslateService translateService , IPdfService pdfService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _translationService = translateService;
            _pdfService= pdfService;

        }



        [HttpPost("addTemplate")]
        public async Task<IActionResult> AddTemplate(TemplateAddDto templateRequest)
        {
            var language = await _dbContext.Languages.SingleOrDefaultAsync(l => l.Id_Language == templateRequest.IdLanguage);

            if (language == null)
            {
                return BadRequest("Language not found.");
            }

            var Doc = await _dbContext.Log_Docs.SingleOrDefaultAsync(l => l.Id_Doc == templateRequest.IdDoc);

            if (Doc == null)
            {
                return BadRequest("Doc not found.");
            }

            var newTemplate = _mapper.Map<Template>(templateRequest);
            newTemplate.Language = language;
            newTemplate.Log_Doc = Doc;

            _dbContext.Templates.Add(newTemplate);
            await _dbContext.SaveChangesAsync();

            return Ok(newTemplate);
        }



        [HttpGet("getTemplatesByUser/{userId}")]
        public async Task<IActionResult> GetTemplatesWithAliasesByUser(Guid userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var templates = await _dbContext.Templates
                .Where(t => t.Groups.Any(group => group.Aliases.Any(alias => alias.Users.Any(u => u.Id == userId))))
                .Include(t => t.Groups)
                .ThenInclude(g => g.Aliases)
                .ThenInclude(a => a.Users)
                .ToListAsync();

            var templateDtos = new List<TemplateAddDto>();

            foreach (var template in templates)
            {
                foreach (var group in template.Groups)
                {
                    group.Aliases = group.Aliases.Where(alias => alias.Users.Any(u => u.Id == userId)).ToList();
                }

                var templateDto = _mapper.Map<TemplateAddDto>(template);
                templateDtos.Add(templateDto);
            }

            return Ok(templateDtos);
        }





        [HttpGet("getTemplateByUser/{userId}/{templateId}")]
        public async Task<IActionResult> GetTemplateByUser(Guid userId, Guid templateId)
        {

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var template = await _dbContext.Templates
                .Where(t => t.Id == templateId)
                .Include(t => t.Groups)
                .ThenInclude(g => g.Aliases)
                 .ThenInclude(a => a.Users)
                .FirstOrDefaultAsync();

            if (template == null)
            {
                return NotFound("Template not found.");
            }

            foreach (var group in template.Groups)
            {
                group.Aliases = group.Aliases.Where(alias => alias.Users.Any(u => u.Id == userId)).ToList();
            }

            var templateDto = _mapper.Map<TemplateAddDto>(template);


            return Ok(templateDto);
        }




        [HttpPost("AssignAliases/{userId}")]
        public async Task<IActionResult> AddGroupAliases(List<AssignAliasDto> aliasRequests, Guid userId, Guid templateId)
        {

            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var template = await _dbContext.Templates
                    .Where(t => t.Id == templateId)
                    .Include(t => t.Groups)
                    .ThenInclude(g => g.Aliases)
                     .ThenInclude(a => a.Users)
                    .FirstOrDefaultAsync();


                if (template.Groups.Any(g => g.Aliases.Any(a => a.Users.Any(u => u.Id == userId))))
                {
                    return BadRequest("User has already assigned aliases in this template.");
                }


                foreach (var aliasRequest in aliasRequests)
                {
                    var group = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Aliases.Any(a => a.Id == aliasRequest.Id));

                    if (group == null)
                    {
                        return NotFound("Group not found.");
                    }

                    var existingAlias = await _dbContext.Aliases
                        .Include(a => a.TypeSetting)
                        .FirstOrDefaultAsync(a => a.Id == aliasRequest.Id);

                    if (existingAlias == null)
                    {
                        return NotFound("Alias not found.");

                    }

                    if (existingAlias.TypeSetting.FieldType != "file")
                    {
                        var newTypeSetting = new TypeSettings
                        {
                            DefaultValue = aliasRequest.DefaultValue,
                            FieldType = existingAlias.TypeSetting.FieldType,
                            ReadOnly = existingAlias.TypeSetting.ReadOnly,
                            Required = existingAlias.TypeSetting.Required
                        };

                        var newAlias = new Alias
                        {
                            Id = Guid.NewGuid(),
                            EntityName = existingAlias.EntityName,
                            Title = existingAlias.Title,
                            Type = existingAlias.Type,
                            Display = existingAlias.Display,
                            Source = existingAlias.Source,
                            TypeSetting = newTypeSetting,
                            Group_FK = group.GroupId,
                            Users = new List<User>()
                        };

                        newAlias.Users.Add(user);

                        _dbContext.Aliases.Add(newAlias);
                    }

                }

                await _dbContext.SaveChangesAsync();

                var result = await _dbContext.Aliases
                    .Include(c => c.TypeSetting)
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error assigning aliases: {ex.Message}");
            }
        }



        [HttpGet("getTemplate/{templateId}")]
        public async Task<IActionResult> GetTemplate(Guid templateId)
        {
            var template = await _dbContext.Templates
                .Where(t => t.Id == templateId)
                .Include(t => t.Groups)
                    .ThenInclude(g => g.Aliases)
                        .ThenInclude(a => a.TypeSetting)
                .Include(t => t.Groups)
                    .ThenInclude(g => g.Aliases)
                        .ThenInclude(a => a.Users)  // Include the Users 
                .FirstOrDefaultAsync();

            if (template == null)
            {
                return NotFound("Template not found.");
            }

            foreach (var group in template.Groups)
            {
                group.Aliases = group.Aliases.Where(alias => !alias.Users.Any()).ToList();
            }

            var templateDto = _mapper.Map<TemplateAddDto>(template);

            templateDto.IdDoc = template.Log_Doc_FK;
            templateDto.IdLanguage = template.Language_FK;

            return Ok(templateDto);
        }





        [HttpGet("generatePdf/{userId}/{templateId}")]
        public async Task<IActionResult> GeneratePDF(Guid userId, Guid templateId)
        {
            IActionResult getTemplateResult = await GetTemplateByUser(userId, templateId);

            try
            {

                if (getTemplateResult is OkObjectResult okResult && okResult.Value is TemplateAddDto templateDto)
                {

                    var document = new PdfDocument();

                    string htmlContent = _pdfService.RenderStandardTemplate(templateDto);

                    PdfGenerator.AddPdfPages(document, htmlContent, PageSize.A4);

                    byte[] response = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        document.Save(ms);
                        response = ms.ToArray();
                    }

                    string fileName = "LegalDoc_" + templateDto.Name + ".pdf";
                    return File(response, "application/pdf", fileName);
                }
                else
                {
                    return BadRequest("Template information not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating PDF: {ex.Message}");
            }
        }



        [HttpPost("translateTemplate")]
        public async Task<IActionResult> TranslateTemplate([FromBody] TemplateAddDto template, [FromQuery] string language)
        {
            try
            {
                // Make sure the provided template JSON is valid
                if (template == null)
                {
                    return BadRequest("Invalid template JSON.");
                }

                // Translate the provided template to the specified language
                var translatedTemplate = await _translationService.TranslateTemplate(template, language);

                return Ok(translatedTemplate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error translating template: {ex.Message}");
            }
        }




        [HttpPost("generatePdfTemplate")]
        public async Task<IActionResult> GeneratePDFForTemplate([FromBody] TemplateAddDto templateDto)
        {
            try {
                var document = new PdfDocument();

                string htmlContent = _pdfService.RenderStandardTemplate(templateDto);

                PdfGenerator.AddPdfPages(document, htmlContent.ToString(), PageSize.A4);

                byte[] response = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    document.Save(ms);
                    response = ms.ToArray();
                }

                string fileName = "LegalDoc.pdf";

                return File(response, "application/pdf", fileName);
            }
                    
                
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating PDF: {ex.Message}");
            }
        }





        [HttpPost("upload/{aliasId}/{userId}")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile, Guid aliasId, Guid userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var alias = await _dbContext.Aliases.Where(t => t.Id == aliasId).Include(t => t.TypeSetting).FirstOrDefaultAsync();

            var result = _fileService.SaveImage(imageFile);

            if (result.Item1 == 1)
            {
                var newTypeSetting = new TypeSettings
                {
                    DefaultValue = result.Item2,
                    FieldType = alias.TypeSetting.FieldType,
                    ReadOnly = alias.TypeSetting.ReadOnly,
                    Required = alias.TypeSetting.Required
                };

                var newAlias = new Alias
                {
                    Id = Guid.NewGuid(),
                    EntityName = alias.EntityName,
                    Title = alias.Title,
                    Type = alias.Type,
                    Display = alias.Display,
                    Source = alias.Source,
                    TypeSetting = newTypeSetting,
                    Group_FK = alias.Group_FK,
                    Users = new List<User>()
                };

                newAlias.Users.Add(user);

                _dbContext.Aliases.Add(newAlias);
                await _dbContext.SaveChangesAsync();
                return Ok(new { Message = "Image uploaded successfully", FileName = result.Item2 });
            }
            else
            {
                return BadRequest(new { Message = result.Item2 });
            }
        }




        [HttpPost("upload")]
        public IActionResult UploadImage([FromForm] IFormFile imageFile)
        {

            var result = _fileService.SaveImage(imageFile);

            if (result.Item1 == 1)
            {
                return Ok(new { Message = "Image uploaded successfully", FileName = result.Item2 });
            }
            else
            {
                return BadRequest(new { Message = result.Item2 });
            }
        }


        [HttpDelete("delete/{userId}/{templateId}")]
        public async Task<IActionResult> DeleteTemplateUser(Guid userId, Guid templateId)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var template = await _dbContext.Templates.Where(t => t.Id == templateId)
                    .Include(t => t.Groups)
                 .ThenInclude(g => g.Aliases)
                  .ThenInclude(a => a.Users)
                 .FirstOrDefaultAsync();

                if (user == null || template == null)
                {
                    return NotFound("User or Template not found.");
                }

                var aliasesToDelete = template.Groups.SelectMany(g => g.Aliases).Where(a => a.Users.Any(u => u.Id == userId))
                .ToList();


                foreach (var alias in aliasesToDelete)
                {
                    _dbContext.Aliases.Remove(alias);
                }

                await _dbContext.SaveChangesAsync();

                return Ok("Aliases deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting aliases: {ex.Message}");
            }

        }


        [HttpPut("update/{userId}/{templateId}")]

        public async Task<IActionResult> UpdateTemplateUser(Guid userId, Guid templateId, List<AssignAliasDto> aliasRequests)
        {
            try
            {

                IActionResult getTemplateResult = await GetTemplateByUser(userId, templateId);

                if (getTemplateResult is OkObjectResult okResult && okResult.Value is TemplateAddDto templateDto)
                {
                    foreach (var alias in aliasRequests)
                    {
                        var oldAlias = _dbContext.Aliases.Where(a => a.Id == alias.Id).FirstOrDefault();
                        oldAlias.TypeSetting.DefaultValue = alias.DefaultValue;
                    }
                    await _dbContext.SaveChangesAsync();
                }
                return Ok("Template updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating aliases: {ex.Message}");
            }


        }


          


       
    
    }
}

