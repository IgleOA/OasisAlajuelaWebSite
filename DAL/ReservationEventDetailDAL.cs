using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class ReservationEventDetailDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        
        public ReservationEventDetail Details(int EventID, int UserID)
        {
            ReservationEventDetail details = new ReservationEventDetail();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspReadReservationEventDetails]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pEventID = new SqlParameter
                {
                    ParameterName = "@EventID",
                    SqlDbType = SqlDbType.Int,
                    Value = EventID
                };
                SqlCmd.Parameters.Add(pEventID);

                SqlParameter pUserID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = UserID
                };
                SqlCmd.Parameters.Add(pUserID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        details.EventID = Convert.ToInt32(dr["EventID"]);
                        details.ScheduledDate = Convert.ToDateTime(dr["ScheduledDate"]);
                        details.Capacity = Convert.ToInt32(dr["Capacity"]);
                        details.Available = Convert.ToInt32(dr["Available"]);
                        details.Unavailable = Convert.ToInt32(dr["Unavailable"]);
                        details.Booked = Convert.ToInt32(dr["Booked"]);
                        details.MaxToReserve = Convert.ToInt32(dr["MaxToReserve"]);
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
