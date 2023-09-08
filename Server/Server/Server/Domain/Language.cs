using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Domain
{
    public class Language
    {
        [Key]
        public Guid Id_Language { get; set; }
        public string NameOfLanguage { get; set; }
        [JsonIgnore]
        public virtual IList<Template> Templates { get; set; }

    }
}
