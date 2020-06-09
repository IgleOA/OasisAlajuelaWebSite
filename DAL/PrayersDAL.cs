using ET;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class PrayersDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public bool Add(Prayers Prayer)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddPrayer]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@Requester", Prayer.Requester);
                SqlCmd.Parameters.AddWithValue("@Email", Prayer.Email);
                SqlCmd.Parameters.AddWithValue("@PhoneNumber", Prayer.PhoneNumber);
                SqlCmd.Parameters.AddWithValue("@Reason", Prayer.Reason);
                SqlCmd.Parameters.AddWithValue("@IP", Prayer.IP);
                SqlCmd.Parameters.AddWithValue("@Country", Prayer.Country);
                SqlCmd.Parameters.AddWithValue("@Region", Prayer.Region);
                SqlCmd.Parameters.AddWithValue("@City", Prayer.City);
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

        public List<Prayers> List(bool HistoryFlag)
        {
            List<Prayers> List = new List<Prayers>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadPrayers]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@HistoryFlag", HistoryFlag);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Prayers
                        {
                            PrayerID = Convert.ToInt32(dr["PrayerID"]),
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

        public Prayers Details(int PrayerID)
        {
            var Detail = new Prayers();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadPrayers]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure                    
                };

                SqlCmd.Parameters.AddWithValue("@PrayerID", PrayerID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Detail.PrayerID = Convert.ToInt32(dr["PrayerID"]);
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
