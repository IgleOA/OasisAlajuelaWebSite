using ET;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ContactsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public bool Add(Contacts Model)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddContact]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@Requester", Model.Requester);
                SqlCmd.Parameters.AddWithValue("@Email", Model.Email);
                SqlCmd.Parameters.AddWithValue("@PhoneNumber", Model.PhoneNumber);
                SqlCmd.Parameters.AddWithValue("@ContactTypeID", Model.ContactTypeID);
                SqlCmd.Parameters.AddWithValue("@Reason", Model.Reason);
                SqlCmd.Parameters.AddWithValue("@IP", Model.IP);
                SqlCmd.Parameters.AddWithValue("@Country", Model.Country);
                SqlCmd.Parameters.AddWithValue("@Region", Model.Region);
                SqlCmd.Parameters.AddWithValue("@City", Model.City);
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

        public List<Contacts> ContactList(ContactListRequest Model)
        {
            List<Contacts> List = new List<Contacts>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadContacts]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@HistoryFlag", Model.HistoryFlag);
                SqlCmd.Parameters.AddWithValue("@ContactTypeID", Model.ContactTypeID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Contacts
                        {
                            ContactID = Convert.ToInt32(dr["ContactID"]),
                            Requester = dr["Requester"].ToString(),
                            Email = dr["Email"].ToString(),
                            PhoneNumber = dr["PhoneNumber"].ToString(),
                            ContactTypeID = Convert.ToInt32(dr["ContactTypeID"]),
                            Reason = dr["Reason"].ToString(),
                            InsertDate = Convert.ToDateTime(dr["InsertDate"]),
                            IP = dr["IP"].ToString(),
                            Country = dr["Country"].ToString(),
                            Region = dr["Region"].ToString(),
                            City = dr["City"].ToString()
                        };
                        List.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return List;
        }

        public List<ContactType> ContactTypeList()
        {
            List<ContactType> List = new List<ContactType>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadContactTypes]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new ContactType
                        {
                            ContactTypeID = Convert.ToInt32(dr["ContactTypeID"]),
                            Type = dr["Type"].ToString(),
                            TypeName = dr["TypeName"].ToString(),
                            TypeTitle = dr["TypeTitle"].ToString(),
                            TypeSubtitle = dr["TypeSubtitle"].ToString(),
                            Order = Convert.ToInt32(dr["Order"]),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"])
                    };
                        List.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return List;
        }

        public ContactType Details(int ContactTypeID)
        {
            var Detail = new ContactType();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadContactTypes]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@ContactTypeID", ContactTypeID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Detail.ContactTypeID = Convert.ToInt32(dr["ContactTypeID"]);
                        Detail.Type = dr["Type"].ToString();
                        Detail.TypeName = dr["TypeName"].ToString();
                        Detail.TypeTitle = dr["TypeTitle"].ToString();
                        Detail.TypeSubtitle = dr["TypeSubtitle"].ToString();
                        Detail.Order = Convert.ToInt32(dr["Order"]);
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
    }
}
