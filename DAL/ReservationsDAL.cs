using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class ReservationsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public List<ReserveDetail> AddReservation(Reservations Reservation, string InsertUser)
        {
            List<ReserveDetail> results = new List<ReserveDetail>();            
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspAddReservation]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@GUID", Reservation.GUID);
                SqlCmd.Parameters.AddWithValue("@EventID", Reservation.EventID);
                SqlCmd.Parameters.AddWithValue("@Seatlist", Reservation.SeatsReserved.Trim());
                SqlCmd.Parameters.AddWithValue("@BookedBy", Reservation.BookedBy);
                SqlCmd.Parameters.AddWithValue("@BookedFor", Reservation.BookedFor.Trim());
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);

                //Exec Command
                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new ReserveDetail
                        {
                            SeatID = dr["SeatID"].ToString(),
                            IsValid = Convert.ToBoolean(dr["IsValid"])
                        };
                        results.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return results;
        }

        public List<Reservations> Details(string GUID)
        {
            List<Reservations> list = new List<Reservations>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspReadReservation]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pNewID = new SqlParameter
                {
                    ParameterName = "@GUID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = GUID
                };
                SqlCmd.Parameters.Add(pNewID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Reservations
                        {
                            ReservationID = Convert.ToInt32(dr["ReservationID"]),
                            GUID = dr["GUID"].ToString(),
                            EventID = Convert.ToInt32(dr["EventID"]),
                            Title = dr["Title"].ToString(),
                            ScheduledDate = Convert.ToDateTime(dr["ScheduledDate"]),
                            SeatID = dr["SeatID"].ToString(),
                            BookedBy = Convert.ToInt32(dr["BookedBy"]),
                            BookedByName = dr["BookedByName"].ToString(),
                            BookedFor = dr["BookedFor"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            ReservationDate = Convert.ToDateTime(dr["ReservationDate"])
                        };
                        list.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return list;
        }

        public string Remove(int ID, string InsertUser)
        {
            string ValidCode = null;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspUpdateReservation]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParGUID = new SqlParameter
                {
                    ParameterName = "@ReservationID",
                    SqlDbType = SqlDbType.Int,
                    Value = ID
                };
                SqlCmd.Parameters.Add(ParGUID);

                SqlParameter pInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(pInsertUser);

                //EXEC Command
                ValidCode = SqlCmd.ExecuteScalar().ToString();
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return ValidCode;
        }

        public bool RemoveGUID(string GUID, string InsertUser)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspUpdateReservation]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParGUID = new SqlParameter
                {
                    ParameterName = "@GUID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = GUID
                };
                SqlCmd.Parameters.Add(ParGUID);

                SqlParameter pInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(pInsertUser);

                //EXEC Command
                rpta = Convert.ToBoolean(SqlCmd.ExecuteScalar());

                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public List<Reservations> ReservationsFullInfo (int EventID, int UserID)
        {
            List<Reservations> list = new List<Reservations>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspReadReservationsByUser]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pUserID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = UserID
                };
                SqlCmd.Parameters.Add(pUserID);

                if(EventID > 0)
                {
                    SqlParameter pEventID = new SqlParameter
                    {
                        ParameterName = "@EventID",
                        SqlDbType = SqlDbType.Int,
                        Value = EventID
                    };
                    SqlCmd.Parameters.Add(pEventID);
                }

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Reservations
                        {
                            ReservationID = Convert.ToInt32(dr["ReservationID"]),
                            GUID = dr["GUID"].ToString(),
                            EventID = Convert.ToInt32(dr["EventID"]),
                            Title = dr["Title"].ToString(),
                            ScheduledDate = Convert.ToDateTime(dr["ScheduledDate"]),
                            SeatID = dr["SeatID"].ToString(),
                            BookedBy = Convert.ToInt32(dr["BookedBy"]),
                            BookedByName = dr["BookedByName"].ToString(),
                            BookedFor = dr["BookedFor"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            ReservationDate = Convert.ToDateTime(dr["ReservationDate"])
                        };
                        list.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return list;
        }

        public List<ReservationLevel1> ReservationsMainInfo(int EventID, int UserID)
        {
            List<ReservationLevel1> list = new List<ReservationLevel1>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspReadMainInfoReservationsbyUser]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pUserID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = UserID
                };
                SqlCmd.Parameters.Add(pUserID);

                if (EventID > 0)
                {
                    SqlParameter pEventID = new SqlParameter
                    {
                        ParameterName = "@EventID",
                        SqlDbType = SqlDbType.Int,
                        Value = EventID
                    };
                    SqlCmd.Parameters.Add(pEventID);
                }

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new ReservationLevel1
                        {
                            GUID = dr["GUID"].ToString(),
                            EventID = Convert.ToInt32(dr["EventID"]),
                            Title = dr["Title"].ToString(),
                            ScheduledDate = Convert.ToDateTime(dr["ScheduledDate"]),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            ReservationDate = Convert.ToDateTime(dr["ReservationDate"])
                        };
                        list.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

                foreach(var item in list)
                {
                    item.Details = Details(item.GUID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return list;
        }

        public List<ReservationLevel1> ReservationsMaster()
        {
            List<ReservationLevel1> list = new List<ReservationLevel1>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspReadMainInfoReservationsbyUser]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new ReservationLevel1
                        {
                            GUID = dr["GUID"].ToString(),
                            EventID = Convert.ToInt32(dr["EventID"]),
                            Title = dr["Title"].ToString(),
                            ScheduledDate = Convert.ToDateTime(dr["ScheduledDate"]),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            ReservationDate = Convert.ToDateTime(dr["ReservationDate"]),
                            BookedByName = dr["BookedByName"].ToString(),
                            BookedFor = dr["BookedFor"].ToString(),
                        };
                        list.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

                foreach (var item in list)
                {
                    item.Details = Details(item.GUID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return list;
        }
    }
}
