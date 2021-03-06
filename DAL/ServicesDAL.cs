﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;
using Dapper;

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

                if (ActiveFlag == true)
                {
                    SqlParameter parStatus = new SqlParameter
                    {
                        ParameterName = "@pActiveFlag",
                        SqlDbType = SqlDbType.Bit,
                        Value = ActiveFlag
                    };
                    SqlCmd.Parameters.Add(parStatus);
                }
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
                            Order = Convert.ToInt32(dr["Order"]),
                            ControllerLink = dr["ControllerLink"].ToString(),
                            ActionLink = dr["ActionLink"].ToString(),
                            Parameter = dr["Parameter"].ToString()
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


        public Services Details(int ServiceID)
        {
            Services detail = new Services();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadServices]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

               SqlCmd.Parameters.AddWithValue("@ServiceID", ServiceID);

                //EXEC Command
                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        detail.ServiceID = Convert.ToInt32(dr["ServiceID"]);
                        detail.ServiceName = dr["ServiceName"].ToString();
                        detail.ServiceDescription = dr["ServiceDescription"].ToString();
                        detail.ServiceIcon = dr["ServiceIcon"].ToString();
                        detail.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        detail.Order = Convert.ToInt32(dr["Order"]);
                        detail.ControllerLink = dr["ControllerLink"].ToString();
                        detail.ActionLink = dr["ActionLink"].ToString();
                        detail.Parameter = dr["Parameter"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return detail;
        }

        public bool Update(Services Service, string User)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateServices]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParInsertUser = new SqlParameter
                {
                    ParameterName = "@User",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = User
                };
                SqlCmd.Parameters.Add(ParInsertUser);

                SqlParameter ParID = new SqlParameter
                {
                    ParameterName = "@ServiceID",
                    SqlDbType = SqlDbType.Int,
                    Value = Service.ServiceID
                };
                SqlCmd.Parameters.Add(ParID);

                SqlParameter SVCIcon = new SqlParameter
                {
                    ParameterName = "@SVCIcon",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Service.ServiceIcon
                };
                SqlCmd.Parameters.Add(SVCIcon);

                SqlParameter SVCName = new SqlParameter
                {
                    ParameterName = "@SVCName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Service.ServiceName
                };
                SqlCmd.Parameters.Add(SVCName);

                SqlParameter SVCDescription = new SqlParameter
                {
                    ParameterName = "@SVCDescription",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Service.ServiceDescription
                };
                SqlCmd.Parameters.Add(SVCDescription);

                SqlParameter SVCOrder = new SqlParameter
                {
                    ParameterName = "@SVCOrder",
                    SqlDbType = SqlDbType.Int,
                    Value = Service.Order
                };
                SqlCmd.Parameters.Add(SVCOrder);

                SqlParameter ActiveFlag = new SqlParameter
                {
                    ParameterName = "@ActiveFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = Service.ActiveFlag
                };
                SqlCmd.Parameters.Add(ActiveFlag);

                SqlParameter SVCController = new SqlParameter
                {
                    ParameterName = "@ControllerLink",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Service.ControllerLink
                };
                SqlCmd.Parameters.Add(SVCController);


                SqlParameter SVActionLink = new SqlParameter
                {
                    ParameterName = "@ActionLink",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Service.ActionLink
                };
                SqlCmd.Parameters.Add(SVActionLink);

                SqlParameter pParameter = new SqlParameter
                {
                    ParameterName = "@Parameter",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Service.Parameter
                };
                SqlCmd.Parameters.Add(pParameter);

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

        public bool AddNew(Services Service, string InserUser)
        {
            bool rpta = false;

            try
            {
                DynamicParameters Parm = new DynamicParameters();
                Parm.Add("@InsertUser", InserUser);
                Parm.Add("@SVCIcon", Service.ServiceIcon.Trim());
                Parm.Add("@SVCName", Service.ServiceName.Trim());
                Parm.Add("@SVCDescription", Service.ServiceDescription.Trim());
                Parm.Add("@SVCOrder", Service.Order);
                Parm.Add("@ControllerLink", Service.ControllerLink);
                Parm.Add("@ActionLink", Service.ActionLink);
                Parm.Add("@Parameter", Service.Parameter);
                SqlCon.Open();

                SqlCon.Execute("[adm].[uspAddServices]", Parm, commandType: CommandType.StoredProcedure);

                rpta = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }
    }
}
