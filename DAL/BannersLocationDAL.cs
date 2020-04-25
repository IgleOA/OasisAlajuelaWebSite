using ET;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BannersLocationDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        public List<BannersLocation> List()
        {
            List<BannersLocation> List = new List<BannersLocation>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadBannersLocation]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };                              

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var banner = new BannersLocation
                        {
                            LocationID = Convert.ToInt32(dr["LocationID"]),
                            LocationName = dr["LocationName"].ToString()
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
    }
}
