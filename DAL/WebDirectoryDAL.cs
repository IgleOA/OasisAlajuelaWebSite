﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;

namespace DAL
{
    public class WebDirectoryDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        public List<WebDirectory> List()
        {
            List<WebDirectory> List = new List<WebDirectory>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspReadWebDirectory]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new WebDirectory
                        {
                            WebID = Convert.ToInt32(dr["WebID"]),
                            Controller = dr["Controller"].ToString(),
                            Action = dr["Action"].ToString(),
                            PublicMenu = Convert.ToBoolean(dr["PublicMenu"]),
                            AdminMenu = Convert.ToBoolean(dr["AdminMenu"]),
                            DisplayName = dr["DisplayName"].ToString(),
                            Parameter = dr["Parameter"].ToString(),
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

        public bool AddNew(WebDirectory Detail, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddWebDirectory]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter AppID = new SqlParameter
                {
                    ParameterName = "@AppID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.AppID
                };
                SqlCmd.Parameters.Add(AppID);

                SqlParameter Controller = new SqlParameter
                {
                    ParameterName = "@Controller",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.Controller
                };
                SqlCmd.Parameters.Add(Controller);

                SqlParameter Action = new SqlParameter
                {
                    ParameterName = "@Action",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.Action
                };
                SqlCmd.Parameters.Add(Action);

                SqlParameter PublicMenu = new SqlParameter
                {
                    ParameterName = "@PublicMenu",
                    SqlDbType = SqlDbType.Bit,
                    Value = Detail.PublicMenu
                };
                SqlCmd.Parameters.Add(PublicMenu);

                SqlParameter AdminMenu = new SqlParameter
                {
                    ParameterName = "@AdminMenu",
                    SqlDbType = SqlDbType.Bit,
                    Value = Detail.AdminMenu
                };
                SqlCmd.Parameters.Add(AdminMenu);

                SqlParameter DisplayName = new SqlParameter
                {
                    ParameterName = "@DisplayName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.DisplayName
                };
                SqlCmd.Parameters.Add(DisplayName);

                SqlParameter Parameter = new SqlParameter
                {
                    ParameterName = "@Parameter",
                    SqlDbType = SqlDbType.VarChar,
                    Size= 50,
                    Value = Detail.Parameter
                };
                SqlCmd.Parameters.Add(Parameter);

                SqlParameter Order = new SqlParameter
                {
                    ParameterName = "@Order",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.Order
                };
                SqlCmd.Parameters.Add(Order);

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

        public List<WebDirectory> WDByUser(string UserName)
        {
            List<WebDirectory> List = new List<WebDirectory>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspWebDirectorybyUser]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pUserName = new SqlParameter
                {
                    ParameterName = "@UserName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = UserName
                };
                SqlCmd.Parameters.Add(pUserName);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new WebDirectory
                        {
                            WebID = Convert.ToInt32(dr["WebID"]),
                            Controller = dr["Controller"].ToString(),
                            Action = dr["Action"].ToString(),
                            DisplayName = dr["DisplayName"].ToString(),
                            Parameter = dr["Parameter"].ToString(),
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