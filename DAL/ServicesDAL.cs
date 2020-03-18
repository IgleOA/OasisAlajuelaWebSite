using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;

namespace DAL
{
    public class ServicesDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public List<Services> List(bool ActiveFlag)
        {
            List<Services> List = new List<Services>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadServices]", SqlCon)
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
                        var detail = new Services
                        {
                            ServiceID = Convert.ToInt32(dr["ServiceID"]),
                            ServiceName = dr["ServiceName"].ToString(),
                            ServiceDescription = dr["ServiceDescription"].ToString(),
                            ServiceIcon = dr["ServiceIcon"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            Order = Convert.ToInt32(dr["Order"])
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
