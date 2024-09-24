namespace StudentAdminPortal.Models.Entities
{
     public class Reminder
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }
    public required Student Student { get; set; }

    public required string Message { get; set; }
    public DateTime SentDate { get; set; }
}

}