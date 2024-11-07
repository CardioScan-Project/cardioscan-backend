using System.Data.SqlClient;
using System.Data;
using CardioScanAPI.Connection;
using CardioScanAPI.Areas.User.Models.User;
using CardioScanAPI.Areas.General.Models;
using ChatBot.API.Utils;

namespace ChatBot.API.Areas.User.Data
{
    public class DUser
    {
        ConnectionBD cn = new ConnectionBD();

        private const string spUser_GetDoctorInfo = "User_GetDoctorInfo";
        private const string spUser_CreatePatient = "User_CreatePatient";
        private const string spUser_ChangePassword = "User_ChangePassword";
        private const string spUser_ValidateEmail = "User_ValidateEmail";
        private const string spUser_GetPatientList = "User_GetPatientList";
        private const string spUser_AddPrenancy = "User_AddPrenancy";
        private const string spUser_GetPregnancies = "User_GetPregnancies";
        private const string spUser_GetScreeningList = "User_GetScreeningList";
        private const string spUser_EchocardiographiesByScreening = "User_EchocardiographiesByScreening";
        private const string spUser_AddEchocardiography = "User_AddEchocardiography";

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

        public async Task<List<Patient>> GetPatientList(int doctorId, string firstName, string lastName, string dni)
        {
            var data = new List<Patient>();

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spUser_GetPatientList, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("DoctorId", doctorId);
                    if (!string.IsNullOrEmpty(firstName))
                        cmd.Parameters.AddWithValue("FirstName", firstName);
                    if (!string.IsNullOrEmpty(lastName))
                        cmd.Parameters.AddWithValue("LastName", lastName);
                    if (!string.IsNullOrEmpty(dni))
                        cmd.Parameters.AddWithValue("Dni", dni);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data.Add(new Patient
                            {
                                Id = reader.GetFieldValue<int>(reader.GetOrdinal(nameof(Patient.Id))),
                                FirstName = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(Patient.FirstName))),
                                LastName = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(Patient.LastName))),
                                BirthDate = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(Patient.BirthDate))),
                            });
                        }
                    }
                }
            }

            return data;
        }

        public async Task<Response> AddPrenancy(int patientId, string lastMenstrualPeriod)
        {
            var response = new Response();

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spUser_AddPrenancy, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("PatientId", patientId);
                    cmd.Parameters.AddWithValue("LastMenstrualPeriod", AuxiliaryMethods.StringToDatetime(lastMenstrualPeriod));
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

        public async Task<List<Pregnancy>> GetPregnancies(int patientId)
        {
            var data = new List<Pregnancy>();

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spUser_GetPregnancies, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("PatientId", patientId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data.Add(new Pregnancy
                            {
                                Id = reader.GetFieldValue<int>(reader.GetOrdinal(nameof(Pregnancy.Id))),
                                LastMenstrualPeriod = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(Pregnancy.LastMenstrualPeriod))),
                            });
                        }
                    }
                }
            }

            return data;
        }

        public async Task<List<Screening>> GetScreeningList(int doctorId, int patientId, string date)
        {
            var data = new List<Screening>();

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spUser_GetScreeningList, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("DoctorId", doctorId);
                    if (patientId != 0)
                        cmd.Parameters.AddWithValue("PatientId", patientId);
                    if (!string.IsNullOrEmpty(date))
                        cmd.Parameters.AddWithValue("Date", AuxiliaryMethods.StringToDatetime(date));
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data.Add(new Screening
                            {
                                Id = reader.GetFieldValue<int>(reader.GetOrdinal(nameof(Screening.Id))),
                                StudyDate = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(Screening.StudyDate))),
                                Result = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(Screening.Result))),
                                Observations = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(Screening.Observations))),
                            });
                        }
                    }
                }
            }

            return data;
        }

        public async Task<List<Echocardiography>> EchocardiographiesByScreening(int screeningId)
        {
            var data = new List<Echocardiography>();

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spUser_EchocardiographiesByScreening, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ScreeningId", screeningId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data.Add(new Echocardiography
                            {
                                Id = reader.GetFieldValue<int>(reader.GetOrdinal(nameof(Echocardiography.Id))),
                                ImagePath = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(Echocardiography.ImagePath))),
                                CutType = reader.GetFieldValue<string>(reader.GetOrdinal(nameof(Echocardiography.CutType))),
                            });
                        }
                    }
                }
            }

            return data;
        }

        public async Task<Response> AddEchocardiography(Echocardiography echocardiography)
        {
            var response = new Response();

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spUser_AddEchocardiography, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ScreeningId", echocardiography.ScreeningId);
                    cmd.Parameters.AddWithValue("ImagePath", echocardiography.ImagePath);
                    cmd.Parameters.AddWithValue("CutType", echocardiography.CutType);
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
    }
}
