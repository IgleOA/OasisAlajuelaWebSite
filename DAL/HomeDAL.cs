using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;

namespace DAL
{
    public class HomeDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public HomePage Home()
        {
            var Detail = new HomePage();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadHomePage]", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if(dr.HasRows)
                    {
                        Detail.DailyVerse = dr["DailyVerse"].ToString();
                        Detail.DailyVerseReference = dr["DailyVerseReference"].ToString();
                        Detail.ServicesTitle = dr["ServicesTitle"].ToString();
                        Detail.ServicesDescription = dr["ServicesDescription"].ToString();
                        Detail.SermonsTitle = dr["SermonsTitle"].ToString();
                        Detail.SermonsDescription = dr["SermonsDescription"].ToString();
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch(Exception ex)
            {
                throw;
            }

            return Detail;
        }
    }
}
