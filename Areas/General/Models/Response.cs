namespace CardioScanAPI.Areas.General.Models
{
    public class Response
    {
        public int Codigo { get; set; }
        public string? Respuesta { get; set; }
    }
    public class ResponseEmail
    {
        public int Codigo { get; set; }
        public string? Respuesta { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
    }
}
