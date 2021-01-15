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

                if (ActiveFlag == true)
                {
                    SqlParameter pActiveFlag = new SqlParameter
                    {
                        ParameterName = "@pActiveFlag",
                        SqlDbType = SqlDbType.Bit,
                        Value = ActiveFlag
                    };
                    SqlCmd.Parameters.Add(pActiveFlag);
                }

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
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            EventMonth = dr["Month"].ToString(),
                            EventDay = dr["Day"].ToString(),
                            EventTime = dr["Time"].ToString(),
                            ReservationFlag = Convert.ToBoolean(dr["ReservationFlag"]),
                            Capacity = Convert.ToInt32(dr["Capacity"]),
                            SocialDistance = Convert.ToInt32(dr["SocialDistance"]),
                            Available = Convert.ToInt32(dr["Available"])
                        };

                        List.Add(detail);
                    }
                }                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
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
                    Value = Event.Title.Trim()
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
                    Value = Event.Description.Trim()
                };
                SqlCmd.Parameters.Add(Description);

                SqlParameter ScheduleDate = new SqlParameter
                {
                    ParameterName = "@ScheduleDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = Event.ScheduledDate
                };
                SqlCmd.Parameters.Add(ScheduleDate);

                SqlParameter pRF = new SqlParameter
                {
                    ParameterName = "@ReservationFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = Event.ReservationFlag
                };
                SqlCmd.Parameters.Add(pRF);

                if (Event.ReservationFlag == true)
                {
                    SqlParameter pCapacity = new SqlParameter
                    {
                        ParameterName = "@Capacity",
                        SqlDbType = SqlDbType.Int,
                        Value = Event.Capacity
                    };
                    SqlCmd.Parameters.Add(pCapacity);

                    SqlParameter pDistance = new SqlParameter
                    {
                        ParameterName = "@SocialDistance",
                        SqlDbType = SqlDbType.Int,
                        Value = Event.SocialDistance
                    };
                    SqlCmd.Parameters.Add(pDistance);
                }

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public UpcommingEvents Details(int EventID)
        {
            UpcommingEvents ET = new UpcommingEvents();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadUpcommingEvents]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter parDate = new SqlParameter
                {
                    ParameterName = "@pDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ServerHourAdjust"]))
                };
                SqlCmd.Parameters.Add(parDate);

                SqlParameter parUp = new SqlParameter
                {
                    ParameterName = "@pUpCommingFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = 1
                };
                SqlCmd.Parameters.Add(parUp);

                SqlParameter pEventID = new SqlParameter
                {
                    ParameterName = "@pEventID",
                    SqlDbType = SqlDbType.Int,
                    Value = EventID
                };
                SqlCmd.Parameters.Add(pEventID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        ET.EventID = Convert.ToInt32(dr["EventID"]);
                        ET.Title = dr["Title"].ToString();
                        ET.MinisterID = Convert.ToInt32(dr["MinisterID"]);
                        ET.MinisterName = dr["MinisterName"].ToString();
                        ET.Description = dr["Description"].ToString();
                        ET.ScheduledDate = Convert.ToDateTime(dr["ScheduledDate"]);
                        ET.ScheduledTime = (TimeSpan)dr["ScheduledTime"];
                        ET.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        ET.EventMonth = dr["Month"].ToString();
                        ET.EventDay = dr["Day"].ToString();
                        ET.EventTime = dr["Time"].ToString();
                        ET.ReservationFlag = Convert.ToBoolean(dr["ReservationFlag"]);
                        ET.Capacity = Convert.ToInt32(dr["Capacity"]);
                        ET.SocialDistance = Convert.ToInt32(dr["SocialDistance"]);
                        ET.Available = Convert.ToInt32(dr["Available"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return ET;
        }

        public bool Update(UpcommingEvents Event, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateUpcommingEvent]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter EventID = new SqlParameter
                {
                    ParameterName = "@EventID",
                    SqlDbType = SqlDbType.Int,
                    Value = Event.EventID
                };
                SqlCmd.Parameters.Add(EventID);

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

                SqlParameter pRF = new SqlParameter
                {
                    ParameterName = "@ReservationFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = Event.ReservationFlag
                };
                SqlCmd.Parameters.Add(pRF);

                if (Event.ReservationFlag == true)
                {
                    SqlParameter pCapacity = new SqlParameter
                    {
                        ParameterName = "@Capacity",
                        SqlDbType = SqlDbType.Int,
                        Value = Event.Capacity
                    };
                    SqlCmd.Parameters.Add(pCapacity);

                    SqlParameter pDistance = new SqlParameter
                    {
                        ParameterName = "@SocialDistance",
                        SqlDbType = SqlDbType.Int,
                        Value = Event.SocialDistance
                    };
                    SqlCmd.Parameters.Add(pDistance);
                }

                SqlParameter ParInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(ParInsertUser);

                SqlParameter UpdateType = new SqlParameter
                {
                    ParameterName = "@UpdateType",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = Event.ActionType
                };
                SqlCmd.Parameters.Add(UpdateType);

                //Exec Command
                SqlCmd.ExecuteNonQuery();

                rpta = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

    }
}
