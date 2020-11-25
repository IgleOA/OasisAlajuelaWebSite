using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;
using System.Collections.Generic;

namespace DAL
{
    public class HomePageDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        //private UpcommingEventsDAL UDAL = new UpcommingEventsDAL();
        //private BlogsDAL BDLA = new BlogsDAL();
        //private ServicesDAL SDAL = new ServicesDAL();
        //private BannnersDAL BNDAL = new BannnersDAL();
        //private SermonsDAL SEDAL = new SermonsDAL();

        public List<HomePage> HomePage(bool ActiveFlag)
        {
            List<HomePage> List = new List<HomePage>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadHomePage]", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (ActiveFlag)
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
                        var detail = new HomePage
                        {
                            SectionID = Convert.ToInt32(dr["SectionID"]),
                            Title = dr["Title"].ToString(),
                            Description = dr["Description"].ToString(),
                            RouterLink = dr["RouterLink"].ToString(),
                            Image = dr["Image"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"])
                        };
                        List.Add(detail);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return List;
        }

        //public InitalHomePage HomePage(DateTime id)
        //{
        //    var Detail = new InitalHomePage();

        //    try
        //    {
        //        SqlCon.Open();
        //        var SqlCmd = new SqlCommand("[config].[uspReadHomePage]", SqlCon);
        //        SqlCmd.CommandType = CommandType.StoredProcedure;

        //        SqlParameter ActiveFlag = new SqlParameter
        //        {
        //            ParameterName = "@ActiveFlag",
        //            SqlDbType = SqlDbType.Bit,
        //            Value = true
        //        };
        //        SqlCmd.Parameters.Add(ActiveFlag);

        //        using (var dr = SqlCmd.ExecuteReader())
        //        {
        //            dr.Read();
        //            if (dr.HasRows)
        //            {
        //                Detail.DailyVerse = dr["DailyVerse"].ToString();
        //                Detail.DailyVerseReference = dr["DailyVerseReference"].ToString();
        //                Detail.ServicesTitle = dr["ServicesTitle"].ToString();
        //                Detail.ServicesDescription = dr["ServicesDescription"].ToString();
        //                Detail.PodcastTitle = dr["PodcastTitle"].ToString();
        //                Detail.PodcastDescription = dr["PodcastDescription"].ToString();
        //                Detail.SermonsTitle = dr["SermonsTitle"].ToString();
        //                Detail.SermonsDescription = dr["SermonsDescription"].ToString();
        //            }
        //        }
        //        if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                
        //        Detail.NextEvent = UDAL.List(id, false, true).Take(1).FirstOrDefault();
        //        Detail.HPBlogs = BDLA.List().Take(5).ToList();
        //        Detail.HPServices = SDAL.List(true);
        //        Detail.HPBanners = BNDAL.Banners("HomePage", true);
        //        Detail.HPSermons = SEDAL.List(true).Take(3).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
        //    return Detail;
        //}



        public bool AddHomePage(HomePage HP, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddHomePage]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter DVerse = new SqlParameter
                {
                    ParameterName = "@Title",
                    SqlDbType = SqlDbType.VarChar,
                    Value = HP.Title.Trim()
                };
                SqlCmd.Parameters.Add(DVerse);

                SqlParameter DVRef = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = HP.Description.Trim()
                };
                SqlCmd.Parameters.Add(DVRef);

                SqlParameter pRouterLink = new SqlParameter
                {
                    ParameterName = "@RouterLink",
                    SqlDbType = SqlDbType.VarChar,
                    Value = HP.RouterLink
                };
                SqlCmd.Parameters.Add(pRouterLink);

                SqlParameter pImage = new SqlParameter
                {
                    ParameterName = "@Image",
                    SqlDbType = SqlDbType.VarChar,
                    Value = HP.Image
                };
                SqlCmd.Parameters.Add(pImage);

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

        public bool UpdateHomePage(HomePage HP, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateHomePage]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pSectionID = new SqlParameter
                {
                    ParameterName = "@SectionID",
                    SqlDbType = SqlDbType.Int,
                    Value = HP.SectionID
                };
                SqlCmd.Parameters.Add(pSectionID);

                SqlParameter DVerse = new SqlParameter
                {
                    ParameterName = "@Title",
                    SqlDbType = SqlDbType.VarChar,
                    Value = HP.Title.Trim()
                };
                SqlCmd.Parameters.Add(DVerse);

                SqlParameter DVRef = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = HP.Description.Trim()
                };
                SqlCmd.Parameters.Add(DVRef);

                SqlParameter pRouterLink = new SqlParameter
                {
                    ParameterName = "@RouterLink",
                    SqlDbType = SqlDbType.VarChar,
                    Value = HP.RouterLink
                };
                SqlCmd.Parameters.Add(pRouterLink);

                SqlParameter pImage = new SqlParameter
                {
                    ParameterName = "@Image",
                    SqlDbType = SqlDbType.VarChar,
                    Value = HP.Image
                };
                SqlCmd.Parameters.Add(pImage);

                SqlParameter pActive = new SqlParameter
                {
                    ParameterName = "@ActiveFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = HP.ActiveFlag
                };
                SqlCmd.Parameters.Add(pActive);

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
    }
}
