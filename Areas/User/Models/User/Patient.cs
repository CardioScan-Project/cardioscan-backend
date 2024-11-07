namespace CardioScanAPI.Areas.User.Models.User
{
    public class Patient
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Dni { get; set; }
        public string? BirthDate { get; set; }
    }
}
