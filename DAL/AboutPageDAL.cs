using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;

namespace DAL
{
    public class AboutPageDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public AboutPage About()
        {
            var Detail = new AboutPage();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadAboutPage]", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Detail.History = dr["History"].ToString();
                        Detail.Mision = dr["Mision"].ToString();
                        Detail.Vision = dr["Vision"].ToString();
                        Detail.Pastors = dr["Pastors"].ToString();
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return Detail;
        }
    }
}
