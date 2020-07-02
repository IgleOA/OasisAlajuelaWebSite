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

                if(ActiveFlag == true)
                {
                    SqlParameter pActiveFlag = new SqlParameter
                    {
                        ParameterName = "@ActiveFlag",
                        SqlDbType = SqlDbType.Bit,
                        Value = ActiveFlag
                    };
                    SqlCmd.Parameters.Add(pActiveFlag);
                }
                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new News
                        {
                            NewID = Convert.ToInt32(dr["NewID"]),
                            Title = dr["Title"].ToString(),
                            Description = dr["Description"].ToString(),
                            BannerPath = dr["BannerPath"].ToString(),
                            ShowFlag = Convert.ToBoolean(dr["ShowFlag"]),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            InsertDate = Convert.ToDateTime(dr["Date"]),
                            NewYear = dr["Year"].ToString(),
                            NewMonth = dr["Month"].ToString(),
                            NewDay = dr["Day"].ToString()
                        };
                        List.Add(detail);
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

        public bool AddNew(News NewMS, string InserUser)
        {
            bool rpta = false;

            try
            {
                DynamicParameters Parm = new DynamicParameters();
                Parm.Add("@InsertUser", InserUser);
                Parm.Add("@Title", NewMS.Title.Trim());
                Parm.Add("@Description", NewMS.Description.Trim());
                Parm.Add("@Banner", NewMS.BannerPath);
                Parm.Add("@InsertDate", NewMS.InsertDate);

                SqlCon.Open();

                SqlCon.Execute("[adm].[uspAddNew]", Parm, commandType: CommandType.StoredProcedure);

                rpta = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public bool Update(News NewMS, string InsertUser)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateNew]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pNewID = new SqlParameter
                {
                    ParameterName = "@NewID",
                    SqlDbType = SqlDbType.Int,
                    Value = NewMS.NewID
                };
                SqlCmd.Parameters.Add(pNewID);

                SqlParameter pInsertUser = new SqlParameter
                {
                    ParameterName = "@User",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(pInsertUser);

                SqlParameter pActionType = new SqlParameter
                {
                    ParameterName = "@ActionType",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = NewMS.ActionType
                };
                SqlCmd.Parameters.Add(pActionType);

                SqlParameter pInsertDate = new SqlParameter
                {
                    ParameterName = "@InsertDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = NewMS.InsertDate
                };
                SqlCmd.Parameters.Add(pInsertDate);

                SqlParameter pTitle = new SqlParameter
                {
                    ParameterName = "@Title",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = NewMS.Title
                };
                SqlCmd.Parameters.Add(pTitle);

                SqlParameter pDescription = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = NewMS.Description
                };
                SqlCmd.Parameters.Add(pDescription);

                SqlParameter Photo = new SqlParameter
                {
                    ParameterName = "@Banner",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 500,
                    Value = NewMS.BannerPath
                };
                SqlCmd.Parameters.Add(Photo);

                SqlParameter ShowFlag = new SqlParameter
                {
                    ParameterName = "@ShowFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = NewMS.ShowFlag
                };
                SqlCmd.Parameters.Add(ShowFlag);

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

        public News Details(int NewID)
        {
            News details = new News();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadNews]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pNewID = new SqlParameter
                {
                    ParameterName = "@NewID",
                    SqlDbType = SqlDbType.Int,
                    Value = NewID
                };
                SqlCmd.Parameters.Add(pNewID);
                
                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if(dr.HasRows)
                    {
                        details.NewID = Convert.ToInt32(dr["NewID"]);
                        details.Title = dr["Title"].ToString();
                        details.Description = dr["Description"].ToString();
                        details.BannerPath = dr["BannerPath"].ToString();
                        details.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        details.InsertDate = Convert.ToDateTime(dr["Date"]);
                        details.NewYear = dr["Year"].ToString();
                        details.NewMonth = dr["Month"].ToString();
                        details.NewDay = dr["Day"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return details;
        }
    }
}
