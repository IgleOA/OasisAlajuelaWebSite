using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;

namespace DAL
{
    public class LeadershipDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        public List<Leadership> List()
        {
            List<Leadership> List = new List<Leadership>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadLeadership]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Leadership
                        {
                            LeaderID = Convert.ToInt32(dr["LeaderID"]),
                            FullName = dr["FullName"].ToString(),
                            Description = dr["Description"].ToString(),
                            ImagePath = dr["ImagePath"].ToString(),
                            Order = Convert.ToInt32(dr["Order"]),
                            ActionLink = dr["ActionLink"].ToString()
                        };
                        List.Add(detail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return List;
        }

        public bool AddNew(Leadership Detail, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddLeadership]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter Title = new SqlParameter
                {
                    ParameterName = "@FullName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = Detail.FullName.Trim()
                };
                SqlCmd.Parameters.Add(Title);

                SqlParameter Description = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Detail.Description.Trim()
                };
                SqlCmd.Parameters.Add(Description);

                SqlParameter pImage = new SqlParameter
                {
                    ParameterName = "@Image",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 500,
                    Value = Detail.ImagePath
                };
                SqlCmd.Parameters.Add(pImage);

                SqlParameter Link = new SqlParameter
                {
                    ParameterName = "@ActionLink",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.ActionLink
                };
                SqlCmd.Parameters.Add(Link);

                SqlParameter pOrder = new SqlParameter
                {
                    ParameterName = "@Order",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.Order
                };
                SqlCmd.Parameters.Add(pOrder);

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

        public Leadership Details(int LeaderID)
        {
            Leadership ET = new Leadership();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadLeadership]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pMinistryID = new SqlParameter
                {
                    ParameterName = "@LeaderID",
                    SqlDbType = SqlDbType.Int,
                    Value = LeaderID
                };
                SqlCmd.Parameters.Add(pMinistryID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        ET.LeaderID = Convert.ToInt32(dr["LeaderID"]);
                        ET.FullName = dr["FullName"].ToString();
                        ET.Description = dr["Description"].ToString();
                        ET.ImagePath = dr["ImagePath"].ToString();
                        ET.ActionLink = dr["ActionLink"].ToString();
                        ET.Order = Convert.ToInt32(dr["Order"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return ET;
        }

        public bool Update(Leadership Detail, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateLeadership]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter EventID = new SqlParameter
                {
                    ParameterName = "@LeaderID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.LeaderID
                };
                SqlCmd.Parameters.Add(EventID);

                SqlParameter Name = new SqlParameter
                {
                    ParameterName = "@FullName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.FullName
                };
                SqlCmd.Parameters.Add(Name);

                SqlParameter Description = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Detail.Description
                };
                SqlCmd.Parameters.Add(Description);

                SqlParameter pImage = new SqlParameter
                {
                    ParameterName = "@Image",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 500,
                    Value = Detail.ImagePath
                };
                SqlCmd.Parameters.Add(pImage);

                SqlParameter Actionlink = new SqlParameter
                {
                    ParameterName = "@ActionLink",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Detail.ActionLink
                };
                SqlCmd.Parameters.Add(Actionlink);

                SqlParameter pOrder = new SqlParameter
                {
                    ParameterName = "@Order",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.Order
                };
                SqlCmd.Parameters.Add(pOrder);

                SqlParameter ParInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(ParInsertUser);

                SqlParameter UpdateType = new SqlParameter
                {
                    ParameterName = "@UpdateType",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = Detail.ActionType
                };
                SqlCmd.Parameters.Add(UpdateType);

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
