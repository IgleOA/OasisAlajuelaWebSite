using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;

namespace DAL
{
    public class UpcommingEventsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public List<UpcommingEvents> List(DateTime Startdate, bool UpCommingFlag)
        {
            List<UpcommingEvents> List = new List<UpcommingEvents>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadUpcommingEvents]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter parDate = new SqlParameter
                {
                    ParameterName = "@pDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = Startdate
                };
                SqlCmd.Parameters.Add(parDate);

                SqlParameter parUp = new SqlParameter
                {
                    ParameterName = "@pUpCommingFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = UpCommingFlag
                };
                SqlCmd.Parameters.Add(parUp);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new UpcommingEvents
                        {
                            EventID = Convert.ToInt32(dr["EventID"]),
                            Title = dr["Title"].ToString(),
                            MinisterID = Convert.ToInt32(dr["MinisterID"]),
                            MinisterName = dr["MinisterName"].ToString(),
                            ScheduledDate = Convert.ToDateTime(dr["ScheduledDate"]),
                            ActiveFlag = Convert.ToBoolean(dr["ACtiveFlag"])
                        };
                        List.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch(Exception ex)
            {
                throw;
            }

            return List;
        }

    }
}
