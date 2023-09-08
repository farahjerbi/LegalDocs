using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain
{
    [Owned]
    public class TypeSettings
    {
        [Key]
        public Guid IdSettings { get; set; }
        public string FieldType { get; set; }
        public string DefaultValue { get; set; }
        public Boolean ReadOnly { get; set; }
        public Boolean Required { get; set; }

    }
}
