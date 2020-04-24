using ET;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GroupsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        public List<Groups> List()
        {
            List<Groups> List = new List<Groups>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadGroups]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Groups
                        {
                            GroupID = Convert.ToInt32(dr["GroupID"]),
                            GroupName = dr["GroupName"].ToString(),
                            Description = dr["Description"].ToString()
                        };
                        List.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return List;
        }

        public List<Groups> ListbyRT(int ResourceTypeID)
        {
            List<Groups> List = new List<Groups>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadGroups]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter pResourceTypeID = new SqlParameter
                {
                    ParameterName = "@ResourceTypeID",
                    SqlDbType = SqlDbType.Int,
                    Value = ResourceTypeID
                };
                SqlCmd.Parameters.Add(pResourceTypeID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Groups
                        {
                            GroupID = Convert.ToInt32(dr["GroupID"]),
                            GroupName = dr["GroupName"].ToString(),
                            Description = dr["Description"].ToString()
                        };
                        List.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return List;
        }

        public List<Groups> ListbyUser(int UserID)
        {
            List<Groups> List = new List<Groups>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadGroups]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter pID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = UserID
                };
                SqlCmd.Parameters.Add(pID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Groups
                        {
                            GroupID = Convert.ToInt32(dr["GroupID"]),
                            GroupName = dr["GroupName"].ToString(),
                            Description = dr["Description"].ToString()
                        };
                        List.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return List;
        }
    }
}
