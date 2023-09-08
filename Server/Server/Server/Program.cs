using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RazorEngine.Templating;
using Server.Configuration;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//DbContextConfig 
builder.Services.AddDbContext<Context>(options =>

  options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);


//SERVICES
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ITranslateService, TranslateService>();
builder.Services.AddScoped<IPdfService, PdfService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Resources"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
