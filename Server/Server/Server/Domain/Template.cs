using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Domain
{
    public class Template
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Style { get; set; }
        public Guid Language_FK { get; set; }
        [JsonIgnore]
        public virtual Language Language { get; set; }
        public Guid Log_Doc_FK { get; set; }
        [JsonIgnore]
        public virtual Log_Doc Log_Doc { get; set; }
        [JsonIgnore]
        public virtual IList<Group> Groups { get; set; }


    }
}
