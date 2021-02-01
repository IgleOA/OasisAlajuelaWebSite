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

        public List<ReservationResult> AddNew(ReservationRequest Model, string InsertUser)
        {
            List<ReservationResult> Results = new List<ReservationResult>();
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspAddReservation]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pGUID = new SqlParameter
                {
                    ParameterName = "@GUID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Model.GUID
                };
                SqlCmd.Parameters.Add(pGUID);

                SqlParameter pEventID = new SqlParameter
                {
                    ParameterName = "@EventID",
                    SqlDbType = SqlDbType.Int,
                    Value = Model.EventID
                };
                SqlCmd.Parameters.Add(pEventID);

                SqlParameter pBookedBy = new SqlParameter
                {
                    ParameterName = "@BookedBy",
                    SqlDbType = SqlDbType.Int,
                    Value = Model.BookedBy
                };
                SqlCmd.Parameters.Add(pBookedBy);

                SqlParameter pJSON = new SqlParameter
                {
                    ParameterName = "@JSONBookedFor",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Model.JSONBookedFor
                };
                SqlCmd.Parameters.Add(pJSON);

                SqlParameter ParInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(ParInsertUser);

                //Exec Command
                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new ReservationResult
                        {
                            GUID = dr["GUID"].ToString(),
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            IdentityID = dr["IdentityID"].ToString(),
                            Status = dr["Status"].ToString()
                        };
                        Results.Add(detail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return Results;
        }

        public List<Reservations> List(ReservationListRequest Model)
        {
            List<Reservations> Results = new List<Reservations>();
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspReadReservation]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pGUID = new SqlParameter
                {
                    ParameterName = "@GUID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Model.GUID
                };
                SqlCmd.Parameters.Add(pGUID);

                if (Model.EventID > 0)
                {
                    SqlParameter pEventID = new SqlParameter
                    {
                        ParameterName = "@EventID",
                        SqlDbType = SqlDbType.Int,
                        Value = Model.EventID
                    };
                    SqlCmd.Parameters.Add(pEventID);
                }
                
                //Exec Command
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
                            BookedBy = Convert.ToInt32(dr["BookedBy"]),
                            BookedByName = dr["BookedByName"].ToString(),
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            IdentityID = dr["IdentityID"].ToString(),
                            ReservationDate = Convert.ToDateTime(dr["ReservationDate"])
                        };
                        Results.Add(detail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return Results;
        }

        public Reservations Details(int ReservationID)
        {
            Reservations detail = new Reservations();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspReadReservation]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@ReservationID", ReservationID);

                //EXEC Command
                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        detail.ReservationID = Convert.ToInt32(dr["ReservationID"]);
                        detail.GUID = dr["GUID"].ToString();
                        detail.EventID = Convert.ToInt32(dr["EventID"]);
                        detail.Title = dr["Title"].ToString();
                        detail.ScheduledDate = Convert.ToDateTime(dr["ScheduledDate"]);
                        detail.BookedBy = Convert.ToInt32(dr["BookedBy"]);
                        detail.BookedByName = dr["BookedByName"].ToString();
                        detail.FirstName = dr["FirstName"].ToString();
                        detail.LastName = dr["LastName"].ToString();
                        detail.IdentityID = dr["IdentityID"].ToString();
                        detail.ReservationDate = Convert.ToDateTime(dr["ReservationDate"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return detail;
        }

        public List<Reserver> Reservers()
        {
            List<Reserver> Results = new List<Reserver>();
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspReadReservers]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };               

                //Exec Command
                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Reserver
                        {
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            IdentityID = dr["IdentityID"].ToString()                            
                        };
                        Results.Add(detail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return Results;
        }

        public bool Update(Reservations Detail, string InsertUser)
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
                SqlParameter pReservationID = new SqlParameter
                {
                    ParameterName = "@ReservationID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.ReservationID
                };
                SqlCmd.Parameters.Add(pReservationID);

                SqlParameter ParInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(ParInsertUser);

                SqlParameter pActionType = new SqlParameter
                {
                    ParameterName = "@ActionType",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = Detail.ActionType
                };
                SqlCmd.Parameters.Add(pActionType);

                SqlParameter pFN = new SqlParameter
                {
                    ParameterName = "@FirstName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = Detail.FirstName
                };
                SqlCmd.Parameters.Add(pFN);

                SqlParameter pLN = new SqlParameter
                {
                    ParameterName = "@LastName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = Detail.LastName
                };
                SqlCmd.Parameters.Add(pLN);

                SqlParameter pID = new SqlParameter
                {
                    ParameterName = "@IdentityID",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = Detail.IdentityID
                };
                SqlCmd.Parameters.Add(pID);

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
