using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;
using System.Drawing;
using System.IO;

namespace DAL
{
    public class HomeDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        public List<Banner> Banners(string Location)
        {
            List<Banner> List = new List<Banner>();

            try 
            { 
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadBanners]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter parLocation = new SqlParameter
                {
                    ParameterName = "@pLocation",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = Location
                };
                SqlCmd.Parameters.Add(parLocation);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var banner = new Banner
                        {
                            BannerID = Convert.ToInt32(dr["BannerID"]),
                            BannerData = (byte[])dr["BannerData"],
                            BannerExt = dr["BannerExt"].ToString(),
                            BannerName = dr["BannerName"].ToString(),
                            LocationBanner = dr["Location"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ACtiveFlag"]),
                            Order = Convert.ToInt32(dr["Order"]),
                            Slide = Convert.ToInt32(dr["Slide"])
                        };
                        List.Add(banner);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch(Exception ex)
            {
                throw;
            }

            return List;
        }
    }
}
