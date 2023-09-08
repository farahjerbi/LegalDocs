using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Domain
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        [JsonIgnore]
        public virtual IList<Alias> Aliases { get; set; }

    }
}
