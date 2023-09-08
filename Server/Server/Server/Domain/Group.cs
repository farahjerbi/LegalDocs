using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Domain
{
    public class Group
    {
        [Key]
        public Guid GroupId { get; set; }
        public string TitleSection { get; set; }
        public Boolean IsRepeatCard { get; set; }
        public int GridDisplay { get; set; }
        public Boolean Disabled { get; set; }
        public Guid Template_FK { get; set; }
        public virtual Template Template { get; set; }
        [JsonIgnore]
        public virtual IList<Alias> Aliases { get; set; }



    }
}
