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
                SqlCmd.Parameters.AddWithValue("@WorshipID", Reservation.WorshipID);
                SqlCmd.Parameters.AddWithValue("@Seatlist", Reservation.SeatsReserved);
                SqlCmd.Parameters.AddWithValue("@BookedBy", Reservation.BookedBy);
                SqlCmd.Parameters.AddWithValue("@BookedFor", Reservation.BookedFor);
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
                            WorshipID = Convert.ToInt32(dr["WorshipID"]),
                            SeatID = dr["SeatID"].ToString(),
                            BookedBy = Convert.ToInt32(dr["BookedBy"]),
                            BookedByName = dr["BookedByName"].ToString(),
                            BookedFor = dr["BookedFor"].ToString(),

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
    }
}
