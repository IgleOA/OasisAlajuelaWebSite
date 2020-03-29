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

        public List<UpcommingEvents> List(DateTime Startdate, bool UpCommingFlag, bool? ActiveFlag)
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

                SqlParameter pActiveFlag = new SqlParameter
                {
                    ParameterName = "@pActiveFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = ActiveFlag
                };
                SqlCmd.Parameters.Add(pActiveFlag);

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
                            Description = dr["Description"].ToString(),
                            ScheduledDate = Convert.ToDateTime(dr["ScheduledDate"]),
                            ScheduledTime = (TimeSpan)dr["ScheduledTime"],
                            ActiveFlag = Convert.ToBoolean(dr["ACtiveFlag"]),
                            EventMonth = dr["Month"].ToString(),
                            EventDay = dr["Day"].ToString()
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

        public bool AddNew(UpcommingEvents Event, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddUpcommingEvent]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter Title = new SqlParameter
                {
                    ParameterName = "@Title",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Event.Title
                };
                SqlCmd.Parameters.Add(Title);

                SqlParameter MinisterID = new SqlParameter
                {
                    ParameterName = "@MinisterID",
                    SqlDbType = SqlDbType.Int,
                    Value = Event.MinisterID
                };
                SqlCmd.Parameters.Add(MinisterID);

                SqlParameter Description = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Event.Description
                };
                SqlCmd.Parameters.Add(Description);

                SqlParameter ScheduleDate = new SqlParameter
                {
                    ParameterName = "@ScheduleDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = Event.ScheduledDate
                };
                SqlCmd.Parameters.Add(ScheduleDate);

                SqlParameter ParInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(ParInsertUser);

                //Exec Command
                SqlCmd.ExecuteNonQuery();

                rpta = true;

                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return rpta;
        }

    }
}
