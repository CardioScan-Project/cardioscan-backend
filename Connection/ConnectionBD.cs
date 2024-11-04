namespace CardioScanAPI.Connection
{
    public class ConnectionBD
    {
        private string connectionString = string.Empty;
        public ConnectionBD()
        {
           var constructor = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            connectionString = constructor.GetSection("ConnectionStrings:eConexion").Value;
        }

        public string cadenaSQL()
        {
            return connectionString;
        }
    }
}
