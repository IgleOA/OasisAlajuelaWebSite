using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;
using Dapper;

namespace DAL
{
    public class SermonsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public List<Sermons> List(bool ActiveFlag)
        {
            List<Sermons> List = new List<Sermons>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadSermons]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (ActiveFlag == true)
                {
                    SqlParameter pActiveFlag = new SqlParameter
                    {
                        ParameterName = "@ActiveFlag",
                        SqlDbType = SqlDbType.Bit,
                        Value = ActiveFlag
                    };
                    SqlCmd.Parameters.Add(pActiveFlag);
                }

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Sermons
                        {
                            SermonID = Convert.ToInt32(dr["SermonID"]),
                            Title = dr["Title"].ToString(),
                            Description = dr["Description"].ToString(),
                            Tags = dr["Tags"].ToString(),
                            MinisterID = Convert.ToInt32(dr["MinisterID"]),
                            MinisterName = dr["MinisterName"].ToString(),
                            SermonDate = Convert.ToDateTime(dr["SermonDate"]),
                            SermonURL = dr["SermonURL"].ToString(),
                            BannerData = (byte[])dr["BackgroundImage"],
                            BannerExt = dr["BackgroundExt"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),                            
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
        public bool AddNew(Sermons NewMS, string InserUser)
        {
            bool rpta = false;

            try
            {
                DynamicParameters Parm = new DynamicParameters();
                Parm.Add("@InsertUser", InserUser);
                Parm.Add("@Title", NewMS.Title);
                Parm.Add("@Description", NewMS.Description);
                Parm.Add("@BannerData", NewMS.BannerData);
                Parm.Add("@BannerExt", NewMS.BannerExt);
                Parm.Add("@Tags", NewMS.Tags);
                Parm.Add("@SermonDate", NewMS.SermonDate);
                Parm.Add("@SermonURL", NewMS.SermonURL);
                Parm.Add("@MinisterID", NewMS.MinisterID);

                SqlCon.Open();

                SqlCon.Execute("[adm].[uspAddSermon]", Parm, commandType: CommandType.StoredProcedure);

                rpta = true;

                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            }
            catch (Exception ex)
            {
                throw;
            }

            return rpta;
        }

        public bool Update(Sermons NewMS, string InsertUser)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateSermon]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pSermonID = new SqlParameter
                {
                    ParameterName = "@SermonID",
                    SqlDbType = SqlDbType.Int,
                    Value = NewMS.SermonID
                };
                SqlCmd.Parameters.Add(pSermonID);

                SqlParameter pInsertUser = new SqlParameter
                {
                    ParameterName = "@User",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(pInsertUser);

                SqlParameter pSermonDate = new SqlParameter
                {
                    ParameterName = "@SermonDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = NewMS.SermonDate
                };
                SqlCmd.Parameters.Add(pSermonDate);

                SqlParameter pTitle = new SqlParameter
                {
                    ParameterName = "@Title",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = NewMS.Title
                };
                SqlCmd.Parameters.Add(pTitle);

                SqlParameter pDescription = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = NewMS.Description
                };
                SqlCmd.Parameters.Add(pDescription);

                SqlParameter Photo = new SqlParameter
                {
                    ParameterName = "@BannerData",
                    SqlDbType = SqlDbType.VarBinary,
                    Value = NewMS.BannerData
                };
                SqlCmd.Parameters.Add(Photo);

                SqlParameter pPhotoExt = new SqlParameter
                {
                    ParameterName = "@BannerExt",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = NewMS.BannerExt
                };
                SqlCmd.Parameters.Add(pPhotoExt);

                SqlParameter pMinisterID = new SqlParameter
                {
                    ParameterName = "@MinisterID",
                    SqlDbType = SqlDbType.Int,
                    Value = NewMS.MinisterID
                };
                SqlCmd.Parameters.Add(pMinisterID);

                SqlParameter pTags = new SqlParameter
                {
                    ParameterName = "@Tags",
                    SqlDbType = SqlDbType.VarChar,
                    Value = NewMS.Tags
                };
                SqlCmd.Parameters.Add(pTags);

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

        public Sermons Details(int NewID)
        {
            Sermons details = new Sermons();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadSermons]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pNewID = new SqlParameter
                {
                    ParameterName = "@SermonID",
                    SqlDbType = SqlDbType.Int,
                    Value = NewID
                };
                SqlCmd.Parameters.Add(pNewID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        details.SermonID = Convert.ToInt32(dr["SermonID"]);
                        details.Title = dr["Title"].ToString();
                        details.Description = dr["Description"].ToString();
                        details.Tags = dr["Tags"].ToString();
                        details.BannerData = (byte[])dr["BackgroundImage"];
                        details.BannerExt = dr["BackgroundExt"].ToString();
                        details.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        details.SermonDate = Convert.ToDateTime(dr["SermonDate"]);
                        details.SermonURL = dr["SermonURL"].ToString();
                        details.MinisterID = Convert.ToInt32(dr["MinisterID"]);
                        details.MinisterName = dr["MinisterName"].ToString();
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            }
            catch (Exception ex)
            {
                throw;
            }

            return details;
        }
    }
}
