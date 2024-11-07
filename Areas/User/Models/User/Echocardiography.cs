namespace CardioScanAPI.Areas.User.Models.User
{
    public class Echocardiography
    {
        public int Id { get; set; }
        public int ScreeningId { get; set; }
        public string? ImagePath { get; set; }
        public string? CutType { get; set; }
    }
}
