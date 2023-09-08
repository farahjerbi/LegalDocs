using Aspose.Html.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.DTOs;
using System.Text;

namespace Server.Services
{
    public class TranslateService:ITranslateService
    {
        private readonly string _apiKey = "";
        private readonly string _apiUrl ="https://api.openai.com/v1/engines/text-davinci-003/completions";

        public class TranslationChoice
        {
            public string text { get; set; }
        }

        public class TranslationResponse
        {
            public List<TranslationChoice> choices { get; set; }
        }

        public async Task<TemplateAddDto> TranslateTemplate(TemplateAddDto template, string language)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                    var templateJson = JsonConvert.SerializeObject(template);

                    string prompt = $"Translate the template's name value, TitleSection of the groups value and the title of aliases value into {language} and give me back the updated template:\r\n{templateJson}";

                    var requestData = new
                    {
                        prompt,
                        max_tokens = 3000,
                        temperature = 0.3,
                        top_p = 1.0,
                        frequency_penalty = 0.0,
                        presence_penalty = 0.0
                    };

                    var jsonRequest = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(_apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<TranslationResponse>(jsonResponse);

                        if (responseObject.choices != null && responseObject.choices.Any())
                        {
                            string translatedJson = responseObject.choices[0].text.Trim(); // Get the first choice and remove leading/trailing spaces
                            translatedJson = translatedJson.Replace("\n", ""); // Remove "\n" characters
                            Console.WriteLine($"Response: {translatedJson}");

                            var templateDto = JsonConvert.DeserializeObject<TemplateAddDto>(translatedJson);


                            return templateDto;
                        }
                        else
                        {
                            throw new Exception("No translation choices found in the response.");
                        }
                    }
                    else
                    {
                        throw new Exception($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the detailed exception and response information
                Console.WriteLine($"Error: {ex.Message}");

                throw new Exception($"Error: {ex.Message}");
            }
        }




    }
}
