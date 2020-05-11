using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;

namespace DAL
{
    public class MinistriesDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        public List<Ministries> List()
        {
            List<Ministries> List = new List<Ministries>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadMinistries]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Ministries
                        {
                            MinistryID = Convert.ToInt32(dr["MinistryID"]),
                            Name = dr["Name"].ToString(),
                            Description = dr["Description"].ToString(),
                            Image = (byte[])dr["Image"],
                            ImageExt = dr["ImageExt"].ToString(),
                            ActionLink = dr["ActionLink"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"])
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

        public bool AddNew(Ministries Ministry, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddMinistry]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter Title = new SqlParameter
                {
                    ParameterName = "@Name",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Ministry.Name.Trim()
                };
                SqlCmd.Parameters.Add(Title);

                SqlParameter Description = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Ministry.Description.Trim()
                };
                SqlCmd.Parameters.Add(Description);

                SqlParameter pImage = new SqlParameter
                {
                    ParameterName = "@Image",
                    SqlDbType = SqlDbType.VarBinary,
                    Value = Ministry.Image
                };
                SqlCmd.Parameters.Add(pImage);

                SqlParameter pImageExt = new SqlParameter
                {
                    ParameterName = "@ImageExt",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Ministry.ImageExt
                };
                SqlCmd.Parameters.Add(pImageExt);

                SqlParameter Link = new SqlParameter
                {
                    ParameterName = "@ActionLink",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Ministry.ActionLink
                };
                SqlCmd.Parameters.Add(Link);

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

        public Ministries Details(int MinistryID)
        {
            Ministries ET = new Ministries();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadMinistries]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pMinistryID = new SqlParameter
                {
                    ParameterName = "@MinistryID",
                    SqlDbType = SqlDbType.Int,
                    Value = MinistryID
                };
                SqlCmd.Parameters.Add(pMinistryID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        ET.MinistryID = Convert.ToInt32(dr["MinistryID"]);
                        ET.Name = dr["Name"].ToString();
                        ET.Description = dr["Description"].ToString();
                        ET.Image = (byte[])dr["Image"];
                        ET.ImageExt = dr["ImageExt"].ToString();
                        ET.ActionLink = dr["ActionLink"].ToString();
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

        public bool Update(Ministries Event, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateMinistry]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter EventID = new SqlParameter
                {
                    ParameterName = "@MinistryID",
                    SqlDbType = SqlDbType.Int,
                    Value = Event.MinistryID
                };
                SqlCmd.Parameters.Add(EventID);

                SqlParameter Name = new SqlParameter
                {
                    ParameterName = "@Name",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Event.Name.Trim()
                };
                SqlCmd.Parameters.Add(Name);

                SqlParameter Description = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Event.Description.Trim()
                };
                SqlCmd.Parameters.Add(Description);

                SqlParameter pImage = new SqlParameter
                {
                    ParameterName = "@Image",
                    SqlDbType = SqlDbType.VarBinary,
                    Value = Event.Image
                };
                SqlCmd.Parameters.Add(pImage);

                SqlParameter pImageExt = new SqlParameter
                {
                    ParameterName = "@ImageExt",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Event.ImageExt
                };
                SqlCmd.Parameters.Add(pImageExt);

                SqlParameter Actionlink = new SqlParameter
                {
                    ParameterName = "@ActionLink",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Event.ActionLink
                };
                SqlCmd.Parameters.Add(Actionlink);

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
                    Value = Event.ActionType
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
