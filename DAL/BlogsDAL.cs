using Dapper;
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
    public class BlogsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public List<Blogs> List()
        {
            List<Blogs> List = new List<Blogs>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadBlogs]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };


                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Blogs
                        {
                            BlogID = Convert.ToInt32(dr["BlogID"]),
                            Title = dr["Title"].ToString(),
                            KeyWord = dr["KeyWord"].ToString(),
                            Description = dr["Description"].ToString(),
                            BannerPath = dr["BannerPath"].ToString(),
                            MinisterID = Convert.ToInt32(dr["MinisterID"]),
                            MinisterName = dr["MinisterName"].ToString(),
                            MinisterPhoto = dr["MinisterPhoto"].ToString(),
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

        public List<Blogs> History()
        {
            List<Blogs> List = new List<Blogs>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadBlogs]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@HistoryFlag", true);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Blogs
                        {
                            BlogID = Convert.ToInt32(dr["BlogID"]),
                            Title = dr["Title"].ToString(),
                            KeyWord = dr["KeyWord"].ToString(),
                            Description = dr["Description"].ToString(),
                            BannerPath = dr["BannerPath"].ToString(),
                            MinisterID = Convert.ToInt32(dr["MinisterID"]),
                            MinisterName = dr["MinisterName"].ToString(),
                            MinisterPhoto = dr["MinisterPhoto"].ToString(),
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

        public bool AddNew(Blogs NewPodcast, string InserUser)
        {
            bool rpta;
            try
            {
                DynamicParameters Parm = new DynamicParameters();
                Parm.Add("@InsertUser", InserUser);
                Parm.Add("@Title", NewPodcast.Title.Trim());
                Parm.Add("@KeyWord", NewPodcast.KeyWord.Trim());
                Parm.Add("@Description", NewPodcast.Description.Trim());
                Parm.Add("@Banner", NewPodcast.BannerPath);
                Parm.Add("@MinisterID", NewPodcast.MinisterID);
                Parm.Add("@InsertDate", NewPodcast.InsertDate);

                SqlCon.Open();

                SqlCon.Execute("[adm].[uspAddBlog]", Parm, commandType: CommandType.StoredProcedure);

                rpta = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public bool Update(Blogs NewPodcast, string InsertUser)
        {
            bool rpta;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateBlog]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pNewID = new SqlParameter
                {
                    ParameterName = "@BlogID",
                    SqlDbType = SqlDbType.Int,
                    Value = NewPodcast.BlogID
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
                    Size = 30,
                    Value = NewPodcast.Title
                };
                SqlCmd.Parameters.Add(pTitle);

                SqlParameter pKW = new SqlParameter
                {
                    ParameterName = "@keyWord",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 30,
                    Value = NewPodcast.KeyWord
                };
                SqlCmd.Parameters.Add(pKW);

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

        public Blogs Details(int NewID)
        {
            Blogs details = new Blogs();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadBlogs]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pNewID = new SqlParameter
                {
                    ParameterName = "@BlogID",
                    SqlDbType = SqlDbType.Int,
                    Value = NewID
                };
                SqlCmd.Parameters.Add(pNewID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        details.BlogID = Convert.ToInt32(dr["BlogID"]);
                        details.Title = dr["Title"].ToString();
                        details.KeyWord = dr["KeyWord"].ToString();
                        details.Description = dr["Description"].ToString();
                        details.BannerPath = dr["BannerPath"].ToString();
                        details.MinisterID = Convert.ToInt32(dr["MinisterID"]);
                        details.MinisterName = dr["MinisterName"].ToString();
                        details.MinisterPhoto = dr["MinisterPhoto"].ToString();
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
