using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ET;

namespace DAL
{
    public class ResourcesDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());
        private GroupsDAL GDAL = new GroupsDAL();

        public bool AddNewResourceType(ResourceTypes RT, string InsertUser)
        {
            bool rpta = false;

            try
            {
                DynamicParameters Parm = new DynamicParameters();
                Parm.Add("@InsertUser", InsertUser);
                Parm.Add("@TypeName", RT.TypeName.Trim());
                Parm.Add("@TypeImage", RT.TypeImage);
                Parm.Add("@TypeImageExt", RT.TypeImageExt);
                Parm.Add("@Description", RT.Description.Trim());
                Parm.Add("@IsPublic", RT.IsPublic);

                SqlCon.Open();

                SqlCon.Execute("[adm].[uspAddResourceType]", Parm, commandType: CommandType.StoredProcedure);

                rpta = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public int AddNewResource(Resources RT, string InsertUser)
        {
            int rpta = 0;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddResource]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(ParInsertUser);

                SqlParameter pResourceTypeID = new SqlParameter
                {
                    ParameterName = "@ResourceTypeID",
                    SqlDbType = SqlDbType.Int,
                    Value = RT.ResourceTypeID
                };
                SqlCmd.Parameters.Add(pResourceTypeID);

                SqlParameter pDescription = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = RT.Description
                };
                SqlCmd.Parameters.Add(pDescription);

                SqlParameter pFileType = new SqlParameter
                {
                    ParameterName = "@FileType",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = RT.FileType
                };
                SqlCmd.Parameters.Add(pFileType);

                SqlParameter pFileData = new SqlParameter
                {
                    ParameterName = "@FileData",
                    SqlDbType = SqlDbType.VarBinary,
                    Value = RT.FileData
                };
                SqlCmd.Parameters.Add(pFileData);

                SqlParameter pFileExt = new SqlParameter
                {
                    ParameterName = "@FileExt",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = RT.FileExt
                };
                SqlCmd.Parameters.Add(pFileExt);

                SqlParameter pFileName = new SqlParameter
                {
                    ParameterName = "@FileName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 500,
                    Value = RT.FileName
                };
                SqlCmd.Parameters.Add(pFileName);

                SqlParameter pFileURL = new SqlParameter
                {
                    ParameterName = "@FileURL",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 500,
                    Value = RT.FileURL
                };
                SqlCmd.Parameters.Add(pFileURL);

                //EXEC Command
                rpta = Convert.ToInt32(SqlCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return rpta;
        }

        public List<ResourceTypes> TypeList(string UserName)
        {
            List<ResourceTypes> List = new List<ResourceTypes>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadResourceTypes]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (UserName.Length >= 1)
                {
                    SqlParameter pUserName = new SqlParameter
                    {
                        ParameterName = "@UserName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 100,
                        Value = UserName
                    };
                    SqlCmd.Parameters.Add(pUserName);
                }
                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new ResourceTypes
                        {
                            ResourceTypeID = Convert.ToInt32(dr["ResourceTypeID"]),
                            TypeName = dr["TypeName"].ToString(),
                            Description = dr["Description"].ToString(),
                            TypeImage = (byte[])dr["TypeImage"],
                            TypeImageExt = dr["TypeImageExt"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            IsPublic = Convert.ToBoolean(dr["IsPublic"])
                        };
                        List.Add(detail);
                    }
                }         
                foreach(var item in List)
                {
                    item.GroupList = GDAL.ListbyRT(item.ResourceTypeID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return List;
        }

        public List<Resources> ResourceList(int ResourceTypeID, Boolean ActiveFlag)
        {
            List<Resources> List = new List<Resources>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadResources]", SqlCon)
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

                if (ActiveFlag == true)
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
                        var detail = new Resources
                        {
                            ResourceID = Convert.ToInt32(dr["ResourceID"]),
                            ResourceTypeID = Convert.ToInt32(dr["ResourceTypeID"]),
                            TypeName = dr["TypeName"].ToString(),
                            FileType = dr["FileType"].ToString(),
                            FileExt = dr["FileExt"].ToString(),
                            FileName = dr["FileName"].ToString(),
                            FileURL = dr["FileURL"].ToString(),
                            Description = dr["Description"].ToString(),
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

        public Resources ResourceDetails(int ResourceID)
        {
            Resources details = new Resources();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadResources]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pResourceID = new SqlParameter
                {
                    ParameterName = "@ResourceID",
                    SqlDbType = SqlDbType.Int,
                    Value = ResourceID
                };
                SqlCmd.Parameters.Add(pResourceID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        details.ResourceID = Convert.ToInt32(dr["ResourceID"]);
                        details.ResourceTypeID = Convert.ToInt32(dr["ResourceTypeID"]);
                        details.TypeName = dr["TypeName"].ToString();
                        details.FileType = dr["FileType"].ToString();
                        details.FileExt = dr["FileExt"].ToString();
                        details.FileName = dr["FileName"].ToString();
                        details.FileURL = dr["FileURL"].ToString();
                        details.Description = dr["Description"].ToString();
                        details.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        if(!Convert.IsDBNull(dr["FileData"]))
                        {
                            details.FileData = (byte[])dr["FileData"];
                        }                       
                    }
                }

                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

                details.TypeData = TypeList("").Where(x => x.ResourceTypeID == details.ResourceTypeID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return details;
        }

        public bool Update(Resources RT, string InsertUser)
        {
            bool rpta = false;

            try
            {
                DynamicParameters Parm = new DynamicParameters();
                Parm.Add("@InsertUser", InsertUser);
                Parm.Add("@ResourceID", RT.ResourceID);
                Parm.Add("@ActionType", RT.ActionType);
                Parm.Add("@ResourceTypeID", RT.ResourceTypeID);
                Parm.Add("@FileName", RT.FileName.Trim());
                Parm.Add("@Description", RT.Description.Trim()); 
                Parm.Add("@FileURL", RT.FileURL.Trim());

                SqlCon.Open();

                SqlCon.Execute("[adm].[uspUpdateResource]", Parm, commandType: CommandType.StoredProcedure);

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
