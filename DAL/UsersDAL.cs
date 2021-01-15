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
        private GroupsDAL GDAL = new GroupsDAL();

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
                    Value = TxtValidate.Trim()
                };
                SqlCmd.Parameters.Add(ParUserName);

                rpta = Convert.ToBoolean(SqlCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public Users Login(Login User)
        {
            Users LoginUser = new Users();

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
                    Value = User.UserName.Trim()
                };
                SqlCmd.Parameters.Add(ParUserName);

                SqlParameter ParPassword = new SqlParameter
                {
                    ParameterName = "@Password",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = User.Password.Trim()
                };
                SqlCmd.Parameters.Add(ParPassword);

                //EXEC Command
                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        LoginUser.UserID = Convert.ToInt32(dr["UserID"]);
                        LoginUser.NeedResetPwd = Convert.ToBoolean(dr["NeedResetPwd"]);
                        LoginUser.UserName = dr["UserName"].ToString();
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
                    Value = Email.Trim()
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return ValidCode;
        }

        public bool AdminResetPassword(int UserID, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAdminResetPassword]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pUserID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = UserID
                };
                SqlCmd.Parameters.Add(pUserID);

                SqlParameter pInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(pInsertUser);

                //EXEC Command
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

        public bool ResetPassword(ResetPasswordModel Model)
        {
            bool rpta;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspResetPassword]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParPassword = new SqlParameter
                {
                    ParameterName = "@Password",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Model.Password.Trim()
                };
                SqlCmd.Parameters.Add(ParPassword);

                if (Model.UserID > 0)
                {
                    SqlParameter pUserID = new SqlParameter
                    {
                        ParameterName = "@UserID",
                        SqlDbType = SqlDbType.Int,
                        Value = Model.UserID
                    };
                    SqlCmd.Parameters.Add(pUserID);
                }
                else
                {
                    SqlParameter ParGUID = new SqlParameter
                    {
                        ParameterName = "@GUID",
                        SqlDbType = SqlDbType.VarChar,
                        Value = Model.GUID
                    };
                    SqlCmd.Parameters.Add(ParGUID);
                }                

                //EXEC Command
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

        public List<Users> List()
        {
            List<Users> List = new List<Users>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspReadUsers]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Users
                        {
                            UserID = Convert.ToInt32(dr["UserID"]),
                            RoleID = Convert.ToInt32(dr["RoleID"]),
                            FullName = dr["FullName"].ToString(),
                            UserName = dr["UserName"].ToString(),
                            Email = dr["Email"].ToString(),
                            Subscriber = Convert.ToBoolean(dr["Subscriber"]),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            CreationDate = Convert.ToDateTime(dr["CreationDate"]),
                            RoleName = dr["RoleName"].ToString()                            
                        };
                        if(!Convert.IsDBNull(dr["LastActivityDate"]))
                        {
                            detail.LastActivityDate = Convert.ToDateTime(dr["LastActivityDate"]);
                        }
                        else
                        {
                            detail.LastActivityDate = null;
                        }
                        List.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();                
            }
            catch (Exception ex)
            {
                throw ex;
            }            
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return List;
        }

        public List<Users> Subscribers(int ResourceID, bool IsPublic)
        {
            List<Users> List = new List<Users>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadSubscribers]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (ResourceID > 0)
                {
                    SqlCmd.Parameters.AddWithValue("@ResourceID", ResourceID);
                }
                if(IsPublic == true)
                {
                    SqlCmd.Parameters.AddWithValue("@IsPublic", IsPublic);
                }

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Users
                        {
                            UserID = Convert.ToInt32(dr["UserID"]),
                            Email = dr["Email"].ToString()                            
                        };
                        
                        List.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return List;
        }

        public bool Update(Users User, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateUser]", SqlCon)
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

                SqlParameter pActionType = new SqlParameter
                {
                    ParameterName = "@ActionType",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = User.ActionType
                };
                SqlCmd.Parameters.Add(pActionType);

                SqlParameter pUserID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = User.UserID
                };
                SqlCmd.Parameters.Add(pUserID);

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


                SqlParameter pActiveFlag = new SqlParameter
                {
                    ParameterName = "@ActiveFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = User.ActiveFlag
                };
                SqlCmd.Parameters.Add(pActiveFlag);

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public Users Details(int UserID)
        {
            var Detail = new Users();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspReadUsers]", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

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
                        Detail.UserID = Convert.ToInt32(dr["UserID"]);
                        Detail.RoleID = Convert.ToInt32(dr["RoleID"]);
                        Detail.FullName = dr["FullName"].ToString();
                        Detail.UserName = dr["UserName"].ToString();
                        Detail.Email = dr["Email"].ToString();
                        Detail.Subscriber = Convert.ToBoolean(dr["Subscriber"]);
                        Detail.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        Detail.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                        Detail.RoleName = dr["RoleName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return Detail;
        }

        public Users DetailsbyEmail(string Email)
        {
            var Detail = new Users();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspReadUsers]", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pEmail = new SqlParameter
                {
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = Email
                };
                SqlCmd.Parameters.Add(pEmail);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Detail.UserID = Convert.ToInt32(dr["UserID"]);
                        Detail.RoleID = Convert.ToInt32(dr["RoleID"]);
                        Detail.FullName = dr["FullName"].ToString();
                        Detail.UserName = dr["UserName"].ToString();
                        Detail.Email = dr["Email"].ToString();
                        Detail.Subscriber = Convert.ToBoolean(dr["Subscriber"]);
                        Detail.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        Detail.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                        Detail.RoleName = dr["RoleName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return Detail;
        }

        public bool InsertActivity(string UserName, string Controller, string Action, DateTime ActivityDate)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddActivity]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParInsertUser = new SqlParameter
                {
                    ParameterName = "@User",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = UserName
                };
                SqlCmd.Parameters.Add(ParInsertUser);

                SqlParameter pController = new SqlParameter
                {
                    ParameterName = "@Controller",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Controller
                };
                SqlCmd.Parameters.Add(pController);

                SqlParameter ParAction = new SqlParameter
                {
                    ParameterName = "@Action",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Action
                };
                SqlCmd.Parameters.Add(ParAction);
                               

                SqlParameter ParActivityDate = new SqlParameter
                {
                    ParameterName = "@ActivityDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = ActivityDate
                };
                SqlCmd.Parameters.Add(ParActivityDate);

                //EXEC Command
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

        public bool AddLogin(LoginRecord Login)
        {
            bool rpta = true;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddLogin]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@UserID", Login.UserID);
                SqlCmd.Parameters.AddWithValue("@IP", Login.IP);
                SqlCmd.Parameters.AddWithValue("@Country", Login.Country);
                SqlCmd.Parameters.AddWithValue("@Region", Login.Region);
                SqlCmd.Parameters.AddWithValue("@City", Login.City);

                rpta = Convert.ToBoolean(SqlCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public bool RemoveSubscriber(string Email)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspRemoveSubscriber]", SqlCon)
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

                //EXEC Command
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