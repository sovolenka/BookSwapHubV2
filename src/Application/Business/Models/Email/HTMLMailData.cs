namespace Business.Models.Mail;

public class HTMLMailData
{
    public string EmailToId { get; set; } = string.Empty;
    public string EmailToName { get; set; } = string.Empty;
    public string EmailSubject { get; set; } = string.Empty;
    public string EmailBodyHTML { get; set; } = string.Empty;
    public string EmailBodyText { get; set; } = string.Empty;
}
