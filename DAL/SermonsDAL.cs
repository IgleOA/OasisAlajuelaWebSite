using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;

namespace DAL
{
    public class SermonsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public List<Sermons> List()
        {
            List<Sermons> List = new List<Sermons>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadSermons]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

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
                            BackgroundImage = (byte[])dr["BackgroundImage"],
                            BackgroundExt = dr["BackgroundExt"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),                            
                        };
                        List.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return List;
        }
    }
}
