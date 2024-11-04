namespace CardioScanAPI.Areas.General.Models.Auth
{
    public class ResponseLogin
    {
        public int Codigo { get; set; }
        public string? Mensaje { get; set; }
        public string? Username { get; set; }
        public int DoctorId { get; set; }
    }
}
