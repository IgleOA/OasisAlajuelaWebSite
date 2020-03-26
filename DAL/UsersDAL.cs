using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;

namespace DAL
{
    public class UsersDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public bool CheckAvailability(string TxtValidate)
        {
            bool rpta = true;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspCheckAvailability]", SqlCon)
                {
                        CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParUserName = new SqlParameter
                {
                    ParameterName = "@TxtValidate",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = TxtValidate
                };
                SqlCmd.Parameters.Add(ParUserName);

                rpta = Convert.ToBoolean(SqlCmd.ExecuteScalar());

                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();                

            }
            catch (Exception ex)
            {
                throw;
            }

            return rpta;
        }
               
        public bool AddUser(Users User, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddUser]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = InsertUser.Trim()
                };
                SqlCmd.Parameters.Add(ParInsertUser);

                SqlParameter ParFullName = new SqlParameter
                {
                    ParameterName = "@FullName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = User.FullName.Trim()
                };
                SqlCmd.Parameters.Add(ParFullName);

                SqlParameter ParUserName = new SqlParameter
                {
                    ParameterName = "@UserName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = User.UserName.Trim()
                };
                SqlCmd.Parameters.Add(ParUserName);

                SqlParameter ParEmail = new SqlParameter
                {
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.VarChar,
                    Value = User.Email.Trim()
                };
                SqlCmd.Parameters.Add(ParEmail);

                SqlParameter ParPassword = new SqlParameter
                {
                    ParameterName = "@Password",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = User.Password.Trim()
                };
                SqlCmd.Parameters.Add(ParPassword);

                SqlParameter ParRoleID = new SqlParameter
                {
                    ParameterName = "@RoleID",
                    SqlDbType = SqlDbType.Int,
                    Value = User.RoleID
                };
                SqlCmd.Parameters.Add(ParRoleID);

                //EXEC Command
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

        public int Login(Login User)
        {
            int userid = 0;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspLogin]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParUserName = new SqlParameter
                {
                    ParameterName = "@TxtName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = User.UserName
                };
                SqlCmd.Parameters.Add(ParUserName);

                SqlParameter ParPassword = new SqlParameter
                {
                    ParameterName = "@Password",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = User.Password
                };
                SqlCmd.Parameters.Add(ParPassword);

                //EXEC Command
                userid = Convert.ToInt32(SqlCmd.ExecuteScalar());

                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();                
            }
            catch (Exception ex)
            {
                throw;
            }
            return userid;
        }

        public AuthorizationCode AuthCode(string Email)
        {
            AuthorizationCode code = new AuthorizationCode();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspGenerateGUIDResetPassword]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParEmail = new SqlParameter
                {
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Email
                };
                SqlCmd.Parameters.Add(ParEmail);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        code.GUID = dr["GUID"].ToString();
                        code.UserID = Convert.ToInt32(dr["UserID"]);
                        code.FullName = dr["FullName"].ToString();
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();                
            }
            catch (Exception ex)
            {
                throw;
            }
            return code;
        }

        public int ValidateGUID(string GUID)
        {
            int ValidCode = 0;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspValidateGUIDResetPassword]", SqlCon)
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

                //EXEC Command
                ValidCode = Convert.ToInt32(SqlCmd.ExecuteScalar());

                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return ValidCode;
        }

        public bool ResetPassword(ResetPasswordModel Model)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspResetPassword]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParGUID = new SqlParameter
                {
                    ParameterName = "@GUID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Model.GUID
                };
                SqlCmd.Parameters.Add(ParGUID);

                SqlParameter ParPassword = new SqlParameter
                {
                    ParameterName = "@Password",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Model.Password
                };
                SqlCmd.Parameters.Add(ParPassword);

                //EXEC Command
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