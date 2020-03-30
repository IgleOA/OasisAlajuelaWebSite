using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;
using Dapper;

namespace DAL
{
    public class NewsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public List<News> List()
        {
            List<News> List = new List<News>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadNews]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new News
                        {
                            NewID = Convert.ToInt32(dr["NewID"]),
                            Title = dr["Title"].ToString(),
                            Description = dr["Description"].ToString(),
                            BannerData = (byte[])dr["BannerData"],
                            BannerExt = dr["BannerExt"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            NewYear = dr["Year"].ToString(),
                            NewMonth = dr["Month"].ToString(),
                            NewDay = dr["Day"].ToString()
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

        public bool AddNew(News NewMS, string InserUser)
        {
            bool rpta = false;

            try
            {
                DynamicParameters Parm = new DynamicParameters();
                Parm.Add("@InsertUser", InserUser);
                Parm.Add("@Title", NewMS.Title);
                Parm.Add("@Description", NewMS.Description);
                Parm.Add("@BannerData", NewMS.BannerData);
                Parm.Add("@BannerExt", NewMS.BannerExt);
                Parm.Add("@InsertDate", NewMS.InsertDate);

                SqlCon.Open();

                SqlCon.Execute("[adm].[uspAddNew]", Parm, commandType: CommandType.StoredProcedure);

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
