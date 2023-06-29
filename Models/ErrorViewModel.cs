namespace Store.Models
{
    public class ErrorViewModel
    {
#nullable enable
        public string? RequestId { get; set; }
#nullable disable
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
