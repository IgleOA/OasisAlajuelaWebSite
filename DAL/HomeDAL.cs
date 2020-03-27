﻿using System;
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

                SqlParameter ActiveFlag = new SqlParameter
                {
                    ParameterName = "@ActiveFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = true
                };
                SqlCmd.Parameters.Add(ActiveFlag);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if(dr.HasRows)
                    {
                        Detail.HomePageID = Convert.ToInt32(dr["HomePageID"]);
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

        public bool AddHomePage(HomePage HP, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddHomePage]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter DVerse = new SqlParameter
                {
                    ParameterName = "@DVerse",
                    SqlDbType = SqlDbType.VarChar,
                    Value = HP.DailyVerse.Trim()
                };
                SqlCmd.Parameters.Add(DVerse);

                SqlParameter DVRef = new SqlParameter
                {
                    ParameterName = "@DVRef",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = HP.DailyVerseReference.Trim()
                };
                SqlCmd.Parameters.Add(DVRef);

                SqlParameter SVCTitle = new SqlParameter
                {
                    ParameterName = "@SVCTitle",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = HP.ServicesTitle.Trim()
                };
                SqlCmd.Parameters.Add(SVCTitle);

                SqlParameter SVCDescription = new SqlParameter
                {
                    ParameterName = "@SVCDescription",
                    SqlDbType = SqlDbType.VarChar,
                    Value = HP.ServicesDescription.Trim()
                };
                SqlCmd.Parameters.Add(SVCDescription);

                SqlParameter SerTitle = new SqlParameter
                {
                    ParameterName = "@SerTitle",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = HP.SermonsTitle.Trim()
                };
                SqlCmd.Parameters.Add(SerTitle);

                SqlParameter SerDescription = new SqlParameter
                {
                    ParameterName = "@SerDescription",
                    SqlDbType = SqlDbType.VarChar,
                    Value = HP.SermonsDescription.Trim()
                };
                SqlCmd.Parameters.Add(SerDescription);

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
