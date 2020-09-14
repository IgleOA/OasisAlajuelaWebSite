using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class EnrollmentsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public bool Add(Enrollments Detail, string InsertUser)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddEnrollments]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pGroupID = new SqlParameter
                {
                    ParameterName = "@GroupID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.GroupID
                };
                SqlCmd.Parameters.Add(pGroupID);

                SqlParameter pStartDate = new SqlParameter
                {
                    ParameterName = "@StartDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = Detail.OpenRegister
                };
                SqlCmd.Parameters.Add(pStartDate);

                SqlParameter pEndDate = new SqlParameter
                {
                    ParameterName = "@EndDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = Detail.CloseRegister
                };
                SqlCmd.Parameters.Add(pEndDate);

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

        public List<Enrollments> List(bool HistoryFlag)
        {
            List<Enrollments> List = new List<Enrollments>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadEnrollments]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (HistoryFlag == true)
                {
                    SqlParameter pHistoryFlag = new SqlParameter
                    {
                        ParameterName = "@HistoryFlag",
                        SqlDbType = SqlDbType.Bit,
                        Value = HistoryFlag
                    };
                    SqlCmd.Parameters.Add(pHistoryFlag);
                }

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Enrollments
                        {
                            EnrollmentID = Convert.ToInt32(dr["EnrollmentID"]),
                            GroupID = Convert.ToInt32(dr["GroupID"]),
                            GroupName = dr["GroupName"].ToString(),
                            OpenRegister = Convert.ToDateTime(dr["OpenRegister"]),
                            CloseRegister = Convert.ToDateTime(dr["CloseRegister"]),
                            ApprovalFlag = Convert.ToBoolean(dr["ApprovalFlag"])
                        };
                        List.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                foreach(var item in List)
                {
                    item.UserList = UserList(item.EnrollmentID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return List;
        }

        public List<EnrolledUsers> UserList(int EnrollmentID)
        {
            List<EnrolledUsers> List = new List<EnrolledUsers>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[ReadEnrolledUsers]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pEnrollmentID = new SqlParameter
                {
                    ParameterName = "@EnrollmentID",
                    SqlDbType = SqlDbType.Int,
                    Value = EnrollmentID
                };
                SqlCmd.Parameters.Add(pEnrollmentID);
                

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new EnrolledUsers
                        {
                            RegisterID = Convert.ToInt32(dr["RegisterID"]),
                            UserID = Convert.ToInt32(dr["UserID"]),
                            EnrollmentID = EnrollmentID,
                            FullName = dr["FullName"].ToString(),
                            PhoneNumber = dr["PhoneNumber"].ToString(),
                            ApprovalFlag = Convert.ToBoolean(dr["ApprovalFlag"])
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

        public bool AddUser(EnrolledUsers Detail, string InsertUser)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddRegistration]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pUserID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.UserID
                };
                SqlCmd.Parameters.Add(pUserID);

                SqlParameter pEnrollmentID = new SqlParameter
                {
                    ParameterName = "@EnrollmentID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.EnrollmentID
                };
                SqlCmd.Parameters.Add(pEnrollmentID);

                SqlParameter pFullName = new SqlParameter
                {
                    ParameterName = "@FullName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = Detail.FullName
                };
                SqlCmd.Parameters.Add(pFullName);

                SqlParameter pPhoneNumber = new SqlParameter
                {
                    ParameterName = "@PhoneNumber",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = Detail.PhoneNumber
                };
                SqlCmd.Parameters.Add(pPhoneNumber);

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

        public bool RemoveEnrollment(int EnrollmentID, string InsertUser)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateEnrollment]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pEnrollmentID = new SqlParameter
                {
                    ParameterName = "@EnrollmentID",
                    SqlDbType = SqlDbType.Int,
                    Value = EnrollmentID
                };
                SqlCmd.Parameters.Add(pEnrollmentID);

                SqlParameter pActiveFlag = new SqlParameter
                {
                    ParameterName = "@ActiveFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = false
                };
                SqlCmd.Parameters.Add(pActiveFlag);

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

        public bool RemoveUser(int RegisterID)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateRegistration]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pRegisterID = new SqlParameter
                {
                    ParameterName = "@RegisterID",
                    SqlDbType = SqlDbType.Int,
                    Value = RegisterID
                };
                SqlCmd.Parameters.Add(pRegisterID);

                SqlParameter pActiveFlag = new SqlParameter
                {
                    ParameterName = "@ActiveFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = false
                };
                SqlCmd.Parameters.Add(pActiveFlag);

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

        public bool ApproveEnrollment(int EnrollmentID)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspApproveEnrollment]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pEnrollmentID = new SqlParameter
                {
                    ParameterName = "@EnrollmentID",
                    SqlDbType = SqlDbType.Int,
                    Value = EnrollmentID
                };
                SqlCmd.Parameters.Add(pEnrollmentID);

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

        public bool Update(Enrollments Detail, string InsertUser)
        {
            bool rpta = false;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateEnrollment]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pEnrollmentID = new SqlParameter
                {
                    ParameterName = "@EnrollmentID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.EnrollmentID
                };
                SqlCmd.Parameters.Add(pEnrollmentID);

                SqlParameter pStartDate = new SqlParameter
                {
                    ParameterName = "@StartDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = Detail.OpenRegister
                };
                SqlCmd.Parameters.Add(pStartDate);

                SqlParameter pEndDate = new SqlParameter
                {
                    ParameterName = "@EndDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = Detail.CloseRegister
                };
                SqlCmd.Parameters.Add(pEndDate);

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

        public Enrollments Details(int EnrollmentID)
        {
            Enrollments ET = new Enrollments();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadEnrollments]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pID = new SqlParameter
                {
                    ParameterName = "@EnrollmentID",
                    SqlDbType = SqlDbType.Int,
                    Value = EnrollmentID
                };
                SqlCmd.Parameters.Add(pID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        ET.EnrollmentID = Convert.ToInt32(dr["EnrollmentID"]);
                        ET.GroupName = dr["GroupName"].ToString();
                        ET.OpenRegister = Convert.ToDateTime(dr["OpenRegister"]);
                        ET.CloseRegister = Convert.ToDateTime(dr["CloseRegister"]);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                ET.UserList = UserList(EnrollmentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return ET;
        }
    }
}
