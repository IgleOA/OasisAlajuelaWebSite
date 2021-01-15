using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;
using ET;

namespace DAL
{
    public class BannnersDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        public List<Banner> Banners(string Location, bool? ActiveFlag)
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

                if (ActiveFlag == true)
                {
                    SqlParameter parStatus = new SqlParameter
                    {
                        ParameterName = "@pActiveFlag",
                        SqlDbType = SqlDbType.Bit,
                        Value = ActiveFlag
                    };
                    SqlCmd.Parameters.Add(parStatus);
                }

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var banner = new Banner
                        {
                            BannerID = Convert.ToInt32(dr["BannerID"]),
                            BannerPath = dr["BannerPath"].ToString(),
                            BannerName = dr["BannerName"].ToString(),
                            LocationID = Convert.ToInt32(dr["LocationID"]),
                            LocationBanner = dr["Location"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ACtiveFlag"]),
                            Slide = Convert.ToInt32(dr["Slide"])
                        };
                        List.Add(banner);
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

        public bool Update(int BannerID, string User)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateBanner]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParInsertUser = new SqlParameter
                {
                    ParameterName = "@User",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = User
                };
                SqlCmd.Parameters.Add(ParInsertUser);

                SqlParameter ParID = new SqlParameter
                {
                    ParameterName = "@BannerID",
                    SqlDbType = SqlDbType.Int,
                    Value = BannerID
                };
                SqlCmd.Parameters.Add(ParID);

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

        public bool AddNew(Banner NewBanner, string InserUser)
        {
            bool rpta = false;

            try
            {
                DynamicParameters Parm = new DynamicParameters();
                Parm.Add("@InsertUser", InserUser);
                Parm.Add("@Banner", NewBanner.BannerPath);
                Parm.Add("@BannerName", NewBanner.BannerName.Trim());
                Parm.Add("@LocationID", NewBanner.LocationID);

                SqlCon.Open();

                SqlCon.Execute("[adm].[uspAddBanner]", Parm, commandType: CommandType.StoredProcedure);

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
