namespace StudentAdminPortal.Models.AdmiinDto
{
    public class SendReminderDto
    {
        public required Guid StudentId { get; set; }
        public required string Message { get; set; }
    }
}
