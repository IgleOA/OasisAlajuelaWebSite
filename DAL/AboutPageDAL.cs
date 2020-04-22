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

        public bool UpdateAbout(AboutPage Details, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddAboutPage]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pHistory = new SqlParameter
                {
                    ParameterName = "@History",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Details.History.Trim()
                };
                SqlCmd.Parameters.Add(pHistory);

                SqlParameter pMision = new SqlParameter
                {
                    ParameterName = "@Mision",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Details.Mision.Trim()
                };
                SqlCmd.Parameters.Add(pMision);

                SqlParameter pVision = new SqlParameter
                {
                    ParameterName = "@Vision",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Details.Vision.Trim()
                };
                SqlCmd.Parameters.Add(pVision);

                SqlParameter pPastors = new SqlParameter
                {
                    ParameterName = "@Pastors",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Details.Pastors.Trim()
                };
                SqlCmd.Parameters.Add(pPastors);

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

                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return rpta;
        }
    }
}
