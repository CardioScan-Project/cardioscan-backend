using System.Data.SqlClient;
using System.Data;
using CardioScanAPI.Connection;
using CardioScanAPI.Areas.General.Models.Auth;

namespace ChatBot.API.Areas.General.Data
{
    public class DAuth
    {
        ConnectionBD cn = new ConnectionBD();

        private const string spAuth_Login = "Auth_Login";

        public async Task<ResponseLogin> IniciarSesion(string email, string password)
        {
            var data = new ResponseLogin();

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spAuth_Login, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Email", email);
                    cmd.Parameters.AddWithValue("Password", password);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data = new ResponseLogin
                            {
                                Codigo = reader.GetFieldValue<int>(reader.GetOrdinal(nameof(ResponseLogin.Codigo))),
                                Mensaje = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(ResponseLogin.Mensaje))),
                                Username = reader.IsDBNull(nameof(ResponseLogin.Username)) ? string.Empty : reader.GetFieldValue<string>(reader.GetOrdinal(nameof(ResponseLogin.Username))),
                                DoctorId = reader.IsDBNull(nameof(ResponseLogin.DoctorId)) ? 0 : reader.GetFieldValue<int>(reader.GetOrdinal(nameof(ResponseLogin.DoctorId))),
                            };
                        }
                    }
                }
            }

            return data;
        }
    }
}
