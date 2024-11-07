namespace CardioScanAPI.Areas.User.Models.User
{
    public class Pregnancy
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string? LastMenstrualPeriod { get; set; }
    }
}
