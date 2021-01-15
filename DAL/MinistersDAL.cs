using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ET;

namespace DAL
{
    public class MinistersDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        public List<Ministers> List(bool ActiveFlag)
        {
            List<Ministers> List = new List<Ministers>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadMinisters]", SqlCon)
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
                        var detail = new Ministers
                        {
                            MinisterID = Convert.ToInt32(dr["MinisterID"]),
                            Title = dr["Title"].ToString(),
                            FullName = dr["FullName"].ToString(),
                            Photo = dr["Photo"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"])
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

        public bool AddNew(Ministers Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddMinister]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter Title = new SqlParameter
                {
                    ParameterName = "@Title",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.Title.Trim()
                };
                SqlCmd.Parameters.Add(Title);

                SqlParameter pFullName = new SqlParameter
                {
                    ParameterName = "@FullName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = Detail.FullName.Trim()
                };
                SqlCmd.Parameters.Add(pFullName);

                SqlParameter pPhoto = new SqlParameter
                {
                    ParameterName = "@Photo",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 500,
                    Value = Detail.Photo
                };
                SqlCmd.Parameters.Add(pPhoto);

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public Ministers Details(int MiniterID)
        {
            var Detail = new Ministers();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadMinisters]", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pID = new SqlParameter
                {
                    ParameterName = "@MinisterID",
                    SqlDbType = SqlDbType.Int,
                    Value = MiniterID
                };
                SqlCmd.Parameters.Add(pID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Detail.MinisterID = Convert.ToInt32(dr["MinisterID"]);
                        Detail.FullName = dr["FullName"].ToString();
                        Detail.Title = dr["Title"].ToString();
                        Detail.Photo = dr["Photo"].ToString();
                        Detail.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return Detail;
        }

        public bool Update(Ministers Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateMinister]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pID = new SqlParameter
                {
                    ParameterName = "@MinisterID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.MinisterID
                };
                SqlCmd.Parameters.Add(pID);

                SqlParameter Title = new SqlParameter
                {
                    ParameterName = "@Title",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.Title.Trim()
                };
                SqlCmd.Parameters.Add(Title);

                SqlParameter pFullName = new SqlParameter
                {
                    ParameterName = "@FullName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = Detail.FullName.Trim()
                };
                SqlCmd.Parameters.Add(pFullName);

                SqlParameter pPhoto = new SqlParameter
                {
                    ParameterName = "@Photo",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 500,
                    Value = Detail.Photo
                };
                SqlCmd.Parameters.Add(pPhoto);

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
