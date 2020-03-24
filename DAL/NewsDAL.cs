using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;


namespace DAL
{
    public class NewsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public List<News> List(bool ActiveFlag)
        {
            List<News> List = new List<News>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadNews]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter parStatus = new SqlParameter
                {
                    ParameterName = "@pActiveFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = ActiveFlag
                };
                SqlCmd.Parameters.Add(parStatus);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new News
                        {
                            NewID = Convert.ToInt32(dr["NewID"]),
                            Title = dr["Title"].ToString(),
                            Description = dr["Description"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"])
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
