namespace CardioScanAPI.Areas.General.Models.Auth
{
    public class UserCredential
    {
        public string? Mensaje { get; set; }
        public int Codigo { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Correo { get; set; }
        public string? DocumentoIdentidad { get; set; }
    }
}
