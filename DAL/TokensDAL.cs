using ET;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace DAL
{
    public class TokensDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public bool AddNew(Token Detail)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspInsertUserToken]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter TokenID = new SqlParameter
                {
                    ParameterName = "@Token",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 5000,
                    Value = Detail.TokenID
                };
                SqlCmd.Parameters.Add(TokenID);

                SqlParameter pUserID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.UserID
                };
                SqlCmd.Parameters.Add(pUserID);

                SqlParameter pExpDate = new SqlParameter
                {
                    ParameterName = "@ExpiresDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = Detail.ExpiresDate
                };
                SqlCmd.Parameters.Add(pExpDate);

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
        
        public Token ValidateToken(string TokenID)
        {
            Token LoginUser = new Token();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspValidateUserToken]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@Token", TokenID);                

                //EXEC Command
                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        LoginUser.TokenID = dr["Token"].ToString();
                        LoginUser.UserID = Convert.ToInt32(dr["UserID"]);
                        LoginUser.ExpiresDate = Convert.ToDateTime(dr["ExpiresDate"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return LoginUser;
        }
    }
}
