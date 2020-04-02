using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return detail;
        }
    }
}
