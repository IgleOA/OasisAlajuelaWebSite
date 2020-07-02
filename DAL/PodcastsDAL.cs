using Dapper;
using ET;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class PodcastsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public List<Podcasts> List()
        {
            List<Podcasts> List = new List<Podcasts>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadPodcasts]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

               
                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Podcasts
                        {
                            PodcastID = Convert.ToInt32(dr["PodcastID"]),
                            Title = dr["Title"].ToString(),
                            Description = dr["Description"].ToString(),
                            BannerPath = dr["BannerPath"].ToString(),
                            MinisterID = Convert.ToInt32(dr["MinisterID"]),
                            MinisterName = dr["MinisterName"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            InsertDate = Convert.ToDateTime(dr["Date"]),
                            NewYear = dr["Year"].ToString(),
                            NewMonth = dr["Month"].ToString(),
                            NewDay = dr["Day"].ToString(),
                            Slide = Convert.ToInt32(dr["Slide"])
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

        public bool AddNew(Podcasts NewPodcast, string InserUser)
        {
            bool rpta = false;

            try
            {
                DynamicParameters Parm = new DynamicParameters();
                Parm.Add("@InsertUser", InserUser);
                Parm.Add("@Title", NewPodcast.Title.Trim());
                Parm.Add("@Description", NewPodcast.Description.Trim());
                Parm.Add("@Banner", NewPodcast.BannerPath);
                Parm.Add("@MinisterID", NewPodcast.MinisterID);
                Parm.Add("@InsertDate", NewPodcast.InsertDate);

                SqlCon.Open();

                SqlCon.Execute("[adm].[uspAddPodcast]", Parm, commandType: CommandType.StoredProcedure);

                rpta = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public bool Update(Podcasts NewPodcast, string InsertUser)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdatePodcast]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pNewID = new SqlParameter
                {
                    ParameterName = "@PodcastID",
                    SqlDbType = SqlDbType.Int,
                    Value = NewPodcast.PodcastID
                };
                SqlCmd.Parameters.Add(pNewID);

                SqlParameter pInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(pInsertUser);

                SqlParameter pActionType = new SqlParameter
                {
                    ParameterName = "@ActionType",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = NewPodcast.ActionType
                };
                SqlCmd.Parameters.Add(pActionType);

                SqlParameter pInsertDate = new SqlParameter
                {
                    ParameterName = "@InsertDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = NewPodcast.InsertDate
                };
                SqlCmd.Parameters.Add(pInsertDate);

                SqlParameter pTitle = new SqlParameter
                {
                    ParameterName = "@Title",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = NewPodcast.Title
                };
                SqlCmd.Parameters.Add(pTitle);

                SqlParameter pDescription = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = NewPodcast.Description
                };
                SqlCmd.Parameters.Add(pDescription);

                SqlParameter Photo = new SqlParameter
                {
                    ParameterName = "@Banner",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 500,
                    Value = NewPodcast.BannerPath
                };
                SqlCmd.Parameters.Add(Photo);

                SqlParameter pMinisterID = new SqlParameter
                {
                    ParameterName = "@MinisterID",
                    SqlDbType = SqlDbType.Int,
                    Value = NewPodcast.MinisterID
                };
                SqlCmd.Parameters.Add(pMinisterID);

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

        public Podcasts Details(int NewID)
        {
            Podcasts details = new Podcasts();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadPodcasts]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pNewID = new SqlParameter
                {
                    ParameterName = "@PodcastID",
                    SqlDbType = SqlDbType.Int,
                    Value = NewID
                };
                SqlCmd.Parameters.Add(pNewID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        details.PodcastID = Convert.ToInt32(dr["PodcastID"]);
                        details.Title = dr["Title"].ToString();
                        details.Description = dr["Description"].ToString();
                        details.BannerPath = dr["BannerPath"].ToString();
                        details.MinisterID = Convert.ToInt32(dr["MinisterID"]);
                        details.MinisterName = dr["MinisterName"].ToString();
                        details.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        details.InsertDate = Convert.ToDateTime(dr["Date"]);
                        details.NewYear = dr["Year"].ToString();
                        details.NewMonth = dr["Month"].ToString();
                        details.NewDay = dr["Day"].ToString();
                        details.Slide = Convert.ToInt32(dr["Slide"]);
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
