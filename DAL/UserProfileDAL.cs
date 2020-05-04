using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using ET;
namespace DAL
{
    public class UserProfileDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        public UserProfile Detail(int UserID)
        {
            UserProfile detail = new UserProfile();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspReadUsersProfile]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter parID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = UserID
                };
                SqlCmd.Parameters.Add(parID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if(dr.HasRows)
                    {
                        detail.UserID = Convert.ToInt32(dr["UserID"]);
                        detail.RoleID = Convert.ToInt32(dr["RoleID"]);
                        detail.FullName = dr["FullName"].ToString();
                        detail.UserName = dr["UserName"].ToString();
                        detail.Email = dr["Email"].ToString();
                        detail.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        detail.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                        detail.RoleName = dr["RoleName"].ToString();
                        detail.PhotoExt = dr["PhotoExt"].ToString();
                        detail.Phone = dr["Phone"].ToString();
                        detail.Mobile = dr["Mobile"].ToString();
                        detail.Facebook = dr["Facebook"].ToString();
                        detail.Twitter = dr["Twitter"].ToString();
                        detail.Snapchat = dr["Snapchat"].ToString();
                        detail.Instragram = dr["Instragram"].ToString();
                        detail.Country = dr["Country"].ToString();
                        detail.State = dr["State"].ToString();
                        detail.City = dr["City"].ToString();
                        detail.LastActivityDate = Convert.ToDateTime(dr["LastActivityDate"]);
                        
                        if (!Convert.IsDBNull(dr["Photo"]))
                        {
                            detail.PhotoData = (byte[])dr["Photo"];
                        }
                        else
                        {
                            detail.PhotoData = null;
                        }
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

        public bool Update(UserProfile UP, string InsertUser)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateUserProfile]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pActionType = new SqlParameter
                {
                    ParameterName = "@ActionType",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = UP.ActionType
                };
                SqlCmd.Parameters.Add(pActionType);

                SqlParameter pInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(pInsertUser);

                SqlParameter UserID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = UP.UserID
                };
                SqlCmd.Parameters.Add(UserID);

                SqlParameter Photo = new SqlParameter
                {
                    ParameterName = "@Photo",
                    SqlDbType = SqlDbType.VarBinary,
                    Value = UP.PhotoData
                };
                SqlCmd.Parameters.Add(Photo);

                SqlParameter pPhotoExt = new SqlParameter
                {
                    ParameterName = "@PhotoExt",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = UP.PhotoExt
                };
                SqlCmd.Parameters.Add(pPhotoExt);

                SqlParameter pPhone = new SqlParameter
                {
                    ParameterName = "@Phone",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = UP.Phone.Trim()
                };
                SqlCmd.Parameters.Add(pPhone);

                SqlParameter pMobile = new SqlParameter
                {
                    ParameterName = "@Mobile",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = UP.Mobile.Trim()
                };
                SqlCmd.Parameters.Add(pMobile);

                SqlParameter Country = new SqlParameter
                {
                    ParameterName = "@Country",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = UP.Country.Trim()
                };
                SqlCmd.Parameters.Add(Country);

                SqlParameter State = new SqlParameter
                {
                    ParameterName = "@State",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = UP.State.Trim()
                };
                SqlCmd.Parameters.Add(State);

                SqlParameter City = new SqlParameter
                {
                    ParameterName = "@City",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = UP.City.Trim()
                };
                SqlCmd.Parameters.Add(City);

                SqlParameter Facebook = new SqlParameter
                {
                    ParameterName = "@Facebook",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = UP.Facebook.Trim()
                };
                SqlCmd.Parameters.Add(Facebook);

                SqlParameter Twitter = new SqlParameter
                {
                    ParameterName = "@Twitter",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = UP.Twitter.Trim()
                };
                SqlCmd.Parameters.Add(Twitter);

                SqlParameter Snapchat = new SqlParameter
                {
                    ParameterName = "@Snapchat",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = UP.Snapchat.Trim()
                };
                SqlCmd.Parameters.Add(Snapchat);

                SqlParameter Instragram = new SqlParameter
                {
                    ParameterName = "@Instragram",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = UP.Instragram.Trim()
                };
                SqlCmd.Parameters.Add(Instragram);

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
