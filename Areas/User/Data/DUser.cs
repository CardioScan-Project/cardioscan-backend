using System.Data.SqlClient;
using System.Data;
using CardioScanAPI.Connection;
using CardioScanAPI.Areas.User.Models.User;
using CardioScanAPI.Areas.General.Models;
using ChatBot.API.Utils;

namespace ChatBot.API.Areas.Dashboard.Data
{
    public class DUser
    {
        ConnectionBD cn = new ConnectionBD();

        private const string spUser_GetDoctorInfo = "User_GetDoctorInfo";
        private const string spUser_CreatePatient = "User_CreatePatient";
        private const string spUser_ChangePassword = "User_ChangePassword";
        private const string spUser_ValidateEmail = "User_ValidateEmail";

        public async Task<DoctorInfo> GetDoctorInfo(int doctorId)
        {
            var doctorInfo = new DoctorInfo();

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spUser_GetDoctorInfo, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("DoctorId", doctorId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            doctorInfo = new DoctorInfo
                            {
                                FirstName = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(DoctorInfo.FirstName))),
                                LastName = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(DoctorInfo.LastName))),
                                Email = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(DoctorInfo.Email)))
                            };
                        }
                    }
                }
            }

            return doctorInfo;
        }

        public async Task<Response> CreatePatient(Patient patient)
        {
            var response = new Response();

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spUser_CreatePatient, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("FirstName", patient.FirstName);
                    cmd.Parameters.AddWithValue("LastName", patient.LastName);
                    cmd.Parameters.AddWithValue("Dni", patient.Dni);
                    cmd.Parameters.AddWithValue("BirthDate", AuxiliaryMethods.StringToDatetime(patient.BirthDate));
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = new Response
                            {
                                Codigo = reader.GetFieldValue<int>(reader.GetOrdinal(nameof(Response.Codigo))),
                                Respuesta = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(Response.Respuesta)))
                            };
                        }
                    }
                }
            }

            return response;
        }

        public async Task<Response> ChangePassword(int doctorId, string newPassword)
        {
            var response = new Response();

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spUser_ChangePassword, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("DoctorId", doctorId);
                    cmd.Parameters.AddWithValue("NewPassword", newPassword);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = new Response
                            {
                                Codigo = reader.GetFieldValue<int>(reader.GetOrdinal(nameof(Response.Codigo))),
                                Respuesta = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(Response.Respuesta)))
                            };
                        }
                    }
                }
            }

            return response;
        }

        public async Task<ResponseEmail> ValidateEmail(string email)
        {
            var responseEmail = new ResponseEmail();

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spUser_ValidateEmail, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Email", email);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            responseEmail = new ResponseEmail
                            {
                                Codigo = reader.GetFieldValue<int>(reader.GetOrdinal(nameof(ResponseEmail.Codigo))),
                                Respuesta = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(ResponseEmail.Respuesta))),
                                Email = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(ResponseEmail.Email))),
                                Name = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(ResponseEmail.Name)))
                            };
                        }
                    }
                }
            }

            return responseEmail;
        }
    }
}
