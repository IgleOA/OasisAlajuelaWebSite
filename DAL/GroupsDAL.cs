using ET;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GroupsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        public List<Groups> List()
        {
            List<Groups> List = new List<Groups>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadGroups]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Groups
                        {
                            GroupID = Convert.ToInt32(dr["GroupID"]),
                            GroupName = dr["GroupName"].ToString(),
                            Description = dr["Description"].ToString()
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

        public List<Groups> FullList()
        {
            List<Groups> List = new List<Groups>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadGroups]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Groups
                        {
                            GroupID = Convert.ToInt32(dr["GroupID"]),
                            GroupName = dr["GroupName"].ToString(),
                            Description = dr["Description"].ToString()
                        };
                        List.Add(detail);
                    }
                }

                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

                //foreach (var item in List)
                //{
                //    item.UserList = UserList(item.GroupID);
                //    item.RTypesList = RTList(item.GroupID);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return List;
        }

        public List<UsersGroups> UserList(int GroupID)
        {
            List<UsersGroups> List = new List<UsersGroups>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadUsersGroups]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter pID = new SqlParameter
                {
                    ParameterName = "@GroupID",
                    SqlDbType = SqlDbType.Int,
                    Value = GroupID
                };
                SqlCmd.Parameters.Add(pID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new UsersGroups
                        {
                            UserGroupID = Convert.ToInt32(dr["UserGroupID"]),
                            GroupID = Convert.ToInt32(dr["GroupID"]),
                            UserID = Convert.ToInt32(dr["UserID"]),
                            FullName = dr["FullName"].ToString()
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

        public List<ResourcesGroups> RTList(int GroupID)
        {
            List<ResourcesGroups> List = new List<ResourcesGroups>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadResourcesGroups]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter pID = new SqlParameter
                {
                    ParameterName = "@GroupID",
                    SqlDbType = SqlDbType.Int,
                    Value = GroupID
                };
                SqlCmd.Parameters.Add(pID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new ResourcesGroups
                        {
                            ResourceGroupID = Convert.ToInt32(dr["ResourceGroupID"]),
                            GroupID = Convert.ToInt32(dr["GroupID"]),
                            ResourceTypeID = Convert.ToInt32(dr["ResourceTypeID"]),
                            TypeName = dr["TypeName"].ToString()
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

        public List<Groups> ListbyRT(int ResourceTypeID)
        {
            List<Groups> List = new List<Groups>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadGroups]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter pResourceTypeID = new SqlParameter
                {
                    ParameterName = "@ResourceTypeID",
                    SqlDbType = SqlDbType.Int,
                    Value = ResourceTypeID
                };
                SqlCmd.Parameters.Add(pResourceTypeID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Groups
                        {
                            GroupID = Convert.ToInt32(dr["GroupID"]),
                            GroupName = dr["GroupName"].ToString(),
                            Description = dr["Description"].ToString()
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

        public List<Groups> ListbyUser(int UserID)
        {
            List<Groups> List = new List<Groups>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadGroups]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter pID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = UserID
                };
                SqlCmd.Parameters.Add(pID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Groups
                        {
                            GroupID = Convert.ToInt32(dr["GroupID"]),
                            GroupName = dr["GroupName"].ToString(),
                            Description = dr["Description"].ToString()
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

        public bool AddUserGroup(UsersGroups UG, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddUserGroup]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pUserID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = UG.UserID
                };
                SqlCmd.Parameters.Add(pUserID);

                SqlParameter pGroupID = new SqlParameter
                {
                    ParameterName = "@GroupID",
                    SqlDbType = SqlDbType.Int,
                    Value = UG.GroupID
                };
                SqlCmd.Parameters.Add(pGroupID);

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

        public bool AddNew(Groups UG, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddGroup]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters

                SqlParameter pName = new SqlParameter
                {
                    ParameterName = "@GroupName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = UG.GroupName.Trim()
                };
                SqlCmd.Parameters.Add(pName);

                SqlParameter pDescription = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = UG.Description.Trim()
                };
                SqlCmd.Parameters.Add(pDescription);

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

        public bool Udpdate(Groups UG, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateGroup]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pRTID = new SqlParameter
                {
                    ParameterName = "@GroupID",
                    SqlDbType = SqlDbType.Int,
                    Value = UG.GroupID
                };
                SqlCmd.Parameters.Add(pRTID);

                SqlParameter pName = new SqlParameter
                {
                    ParameterName = "@GroupName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = UG.GroupName.Trim()
                };
                SqlCmd.Parameters.Add(pName);

                SqlParameter pDescription = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = UG.Description.Trim()
                };
                SqlCmd.Parameters.Add(pDescription);

                SqlParameter pAT = new SqlParameter
                {
                    ParameterName = "@ActionType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = UG.ActionType
                };
                SqlCmd.Parameters.Add(pAT);

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

        public bool AddRTGroup(ResourcesGroups UG, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddResourceGroup]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pRTID = new SqlParameter
                {
                    ParameterName = "@ResourceTypeID",
                    SqlDbType = SqlDbType.Int,
                    Value = UG.ResourceTypeID
                };
                SqlCmd.Parameters.Add(pRTID);

                SqlParameter pGroupID = new SqlParameter
                {
                    ParameterName = "@GroupID",
                    SqlDbType = SqlDbType.Int,
                    Value = UG.GroupID
                };
                SqlCmd.Parameters.Add(pGroupID);

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

        public bool RemoveUG(UsersGroups UG, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateUserGroup]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };
                if (UG.UserGroupID >= 1)
                {
                    SqlParameter pUGID = new SqlParameter
                    {
                        ParameterName = "@UserGroupID",
                        SqlDbType = SqlDbType.Int,
                        Value = UG.UserGroupID
                    };
                    SqlCmd.Parameters.Add(pUGID);
                }
                else
                {
                    //Insert Parameters
                    SqlParameter pUserID = new SqlParameter
                    {
                        ParameterName = "@UserID",
                        SqlDbType = SqlDbType.Int,
                        Value = UG.UserID
                    };
                    SqlCmd.Parameters.Add(pUserID);

                    SqlParameter pGroupID = new SqlParameter
                    {
                        ParameterName = "@GroupID",
                        SqlDbType = SqlDbType.Int,
                        Value = UG.GroupID
                    };
                    SqlCmd.Parameters.Add(pGroupID);
                }
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

        public bool RemoveRG(ResourcesGroups UG, string InsertUser)
        {
            bool rpta = false;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateResourceGroup]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };
                if (UG.ResourceGroupID >= 1)
                {
                    SqlParameter pUGID = new SqlParameter
                    {
                        ParameterName = "@ResourceGroupID",
                        SqlDbType = SqlDbType.Int,
                        Value = UG.ResourceGroupID
                    };
                    SqlCmd.Parameters.Add(pUGID);
                }
                else
                {
                    //Insert Parameters
                    SqlParameter pRTID = new SqlParameter
                    {
                        ParameterName = "@ResourceTypeID",
                        SqlDbType = SqlDbType.Int,
                        Value = UG.ResourceTypeID
                    };
                    SqlCmd.Parameters.Add(pRTID);

                    SqlParameter pGroupID = new SqlParameter
                    {
                        ParameterName = "@GroupID",
                        SqlDbType = SqlDbType.Int,
                        Value = UG.GroupID
                    };
                    SqlCmd.Parameters.Add(pGroupID);
                }
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

        public Groups Details(int GroupID)
        {
            var Detail = new Groups();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadGroups]", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pID = new SqlParameter
                {
                    ParameterName = "@GroupID",
                    SqlDbType = SqlDbType.Int,
                    Value = GroupID
                };
                SqlCmd.Parameters.Add(pID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Detail.GroupID = Convert.ToInt32(dr["GroupID"]);
                        Detail.GroupName = dr["GroupName"].ToString();
                        Detail.Description = dr["Description"].ToString();
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
    }
}
