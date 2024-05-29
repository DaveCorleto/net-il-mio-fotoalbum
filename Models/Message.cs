namespace net_il_mio_fotoalbum.Models
{
    public class Message
    {

        public int Id { get; set; }

        public string Text { get; set; }


        public Message() { }
        public Message(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}