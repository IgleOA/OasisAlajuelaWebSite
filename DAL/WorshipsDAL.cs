using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class WorshipsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        
        public Worships Details(int WorshipID)
        {
            Worships details = new Worships();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspReadWorships]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pWorshipID = new SqlParameter
                {
                    ParameterName = "@WorshipID",
                    SqlDbType = SqlDbType.Int,
                    Value = WorshipID
                };
                SqlCmd.Parameters.Add(pWorshipID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        details.WorshipID = Convert.ToInt32(dr["WorshipID"]);
                        details.ScheduledDate = Convert.ToDateTime(dr["ScheduledDate"]);
                        details.Capacity = Convert.ToInt32(dr["Capacity"]);
                        details.Available = Convert.ToInt32(dr["Available"]);
                        details.Unavailable = Convert.ToInt32(dr["Unavailable"]);
                        details.Booked = Convert.ToInt32(dr["Booked"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return details;
        }
    }
}
