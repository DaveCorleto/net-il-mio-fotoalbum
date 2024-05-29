using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    [Table("Emails")]
    public class Email
    {
        public int Id { get; set; }

        public string Text { get; set; }

 
        public Email() { }

        public Email(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}

