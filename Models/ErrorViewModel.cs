namespace net_il_mio_fotoalbum.Models
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string message)
        {
        }

        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
