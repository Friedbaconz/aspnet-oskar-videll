namespace Presentation.WebApp.Models.Error;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool? Success => !string.IsNullOrEmpty(RequestId);
}
