namespace CardioScanAPI.Areas.User.Models.User
{
    public class Screening
    {
        public int Id { get; set; }
        public string? StudyDate { get; set; }
        public string? Result { get; set; }
        public string? Observations { get; set; }
    }
}
