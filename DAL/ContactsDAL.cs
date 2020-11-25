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

        public List<Contacts> List(bool HistoryFlag)
        {
            List<Contacts> List = new List<Contacts>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadContacts]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@HistoryFlag", HistoryFlag);

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

        public Contacts Details(int ContactID)
        {
            var Detail = new Contacts();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadContacts]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@ContactID", ContactID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Detail.ContactID = Convert.ToInt32(dr["ContactID"]);
                        Detail.Requester = dr["Requester"].ToString();
                        Detail.Email = dr["Email"].ToString();
                        Detail.PhoneNumber = dr["PhoneNumber"].ToString();
                        Detail.Reason = dr["Reason"].ToString();
                        Detail.InsertDate = Convert.ToDateTime(dr["InsertDate"]);
                        Detail.IP = dr["IP"].ToString();
                        Detail.Country = dr["Country"].ToString();
                        Detail.Region = dr["Region"].ToString();
                        Detail.City = dr["City"].ToString();
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
