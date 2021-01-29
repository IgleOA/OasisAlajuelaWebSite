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

        public bool AddNew(ReservationRequest Model, string InsertUser)
        {
            bool rpta = false;
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
