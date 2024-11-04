using System.Data.SqlClient;
using System.Data;
using CardioScanAPI.Connection;
using CardioScanAPI.Areas.General.Models.Auth;
using CardioScanAPI.Areas.Dashboard.Models.Dashboard;

namespace ChatBot.API.Areas.Dashboard.Data
{
    public class DDashboard
    {
        ConnectionBD cn = new ConnectionBD();

        private const string spDashboard_EchocardiogramsByMonth = "Dashboard_EchocardiogramsByMonth";
        private const string spDashboard_PatientsByMonth = "Dashboard_PatientsByMonth";
        private const string spDashboard_ScreeningByTrimester = "Dashboard_ScreeningByTrimester";

        public async Task<int> EchocardiogramsByMonth(int month)
        {
            var number = 0;

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spDashboard_EchocardiogramsByMonth, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Month", month);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            number = reader.GetFieldValue<int>(reader.GetOrdinal("Total"));
                        }
                    }
                }
            }

            return number;
        }

        public async Task<int> PatientsByMonth(int month)
        {
            var number = 0;

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spDashboard_PatientsByMonth, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Month", month);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            number = reader.GetFieldValue<int>(reader.GetOrdinal("Total"));
                        }
                    }
                }
            }

            return number;
        }

        public async Task<List<Trimester>> ScreeningByTrimester(int year)
        {
            var trimesters = new List<Trimester>();

            using (var sql = new SqlConnection(cn.cadenaSQL()))
            {
                using (var cmd = new SqlCommand(spDashboard_ScreeningByTrimester, sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Year", year);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            trimesters.Add(new Trimester
                            {
                                Quarter = reader.GetFieldValue<int>(reader.GetOrdinal(nameof(Trimester.Quarter))),
                                Count = reader.GetFieldValue<int>(reader.GetOrdinal(nameof(Trimester.Count)))
                            });
                        }
                    }
                }
            }

            return trimesters;
        }
    }
}
