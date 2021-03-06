﻿using System;
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
                            BannerPath = dr["ImagePath"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),                            
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
        public int AddNew(Sermons NewMS, string InserUser)
        {
            int rpta = 0;

            try
            {
                DynamicParameters Parm = new DynamicParameters();
                Parm.Add("@InsertUser", InserUser);
                Parm.Add("@Title", NewMS.Title.Trim());
                Parm.Add("@Description", NewMS.Description.Trim());
                Parm.Add("@Banner", NewMS.BannerPath);
                Parm.Add("@Tags", NewMS.Tags.Trim());
                Parm.Add("@SermonDate", NewMS.SermonDate);
                Parm.Add("@SermonURL", NewMS.SermonURL.Trim());
                Parm.Add("@MinisterID", NewMS.MinisterID);

                SqlCon.Open();

                //SqlCon.Execute("[adm].[uspAddSermon]", Parm, commandType: CommandType.StoredProcedure);

                rpta = Convert.ToInt32(SqlCon.ExecuteScalar<int>("[adm].[uspAddSermon]", Parm, commandType: CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public bool Update(Sermons NewMS, string InsertUser)
        {
            bool rpta;
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

                if (!string.IsNullOrEmpty(NewMS.Title))
                {
                    SqlParameter pTitle = new SqlParameter
                    {
                        ParameterName = "@Title",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 100,
                        Value = NewMS.Title.Trim()
                    };
                    SqlCmd.Parameters.Add(pTitle);

                    SqlParameter pDescription = new SqlParameter
                    {
                        ParameterName = "@Description",
                        SqlDbType = SqlDbType.VarChar,
                        Value = NewMS.Description.Trim()
                    };
                    SqlCmd.Parameters.Add(pDescription);

                    SqlParameter Photo = new SqlParameter
                    {
                        ParameterName = "@Banner",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 500,
                        Value = NewMS.BannerPath
                    };
                    SqlCmd.Parameters.Add(Photo);

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
                        Value = NewMS.Tags.Trim()
                    };
                    SqlCmd.Parameters.Add(pTags);
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
                        details.BannerPath = dr["ImagePath"].ToString();
                        details.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        details.SermonDate = Convert.ToDateTime(dr["SermonDate"]);
                        details.SermonURL = dr["SermonURL"].ToString();
                        details.MinisterID = Convert.ToInt32(dr["MinisterID"]);
                        details.MinisterName = dr["MinisterName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return details;
        }

        public SermonEmail DetailsForEmail(int SermonID)
        {
            SermonEmail details = new SermonEmail();

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
                    Value = SermonID
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
                        details.SermonDate = Convert.ToDateTime(dr["SermonDate"]);
                        details.ImageURL = dr["ImagePath"].ToString();
                        details.SermonURL = dr["SermonURL"].ToString();
                        details.MinisterName = dr["MinisterName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return details;
        }
    }
}
