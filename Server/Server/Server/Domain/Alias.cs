using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Domain
{
    public class Alias
    {
        [Key]
        public Guid Id { get; set; }
        public string EntityName { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Display { get; set; }
        public string Source { get; set; }

        public TypeSettings TypeSetting { get; set; }
        public Guid Group_FK { get; set; }
        [JsonIgnore]
        public virtual Group Group { get; set; }
        public virtual IList<User> Users { get; set; }


    }
}
