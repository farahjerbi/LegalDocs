using System.ComponentModel.DataAnnotations;

namespace Server.Domain
{
    public class Log_Doc
    {
        [Key]
        public Guid Id_Doc { get; set; }
        public string Name { get; set; }    
        public Status Status { get; set; }
        public virtual IList<Template> Templates { get; set; }

    }
}
