using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;

namespace DAL
{
    public class UserNotesDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public List<UserNotes> List(string UserName, bool HistoryFlag)
        {
            List<UserNotes> List = new List<UserNotes>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadUserNotes]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@UserName", UserName);
                SqlCmd.Parameters.AddWithValue("@HistoryFlag", HistoryFlag);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new UserNotes
                        {
                            NoteID = Convert.ToInt32(dr["NoteID"]),
                            UserID = Convert.ToInt32(dr["UserID"]),
                            RequestNote = dr["RequestNote"].ToString(),
                            NoteDate = Convert.ToDateTime(dr["NoteDate"]),
                            InsertedBy = dr["InsertedBy"].ToString(),
                            ResponseRequired = Convert.ToBoolean(dr["ResponseRequired"]),
                            ResponseNote = dr["ResponseNote"].ToString(),
                            ReadFlag = Convert.ToBoolean(dr["ReadFlag"])
                        };
                        if (!Convert.IsDBNull(dr["ResponseDate"]))
                        {
                            detail.ResponseDate = Convert.ToDateTime(dr["ResponseDate"]);
                        }
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
        public bool AddNote(UserNotes Note, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddUserNote]", SqlCon)
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

                SqlParameter pUserID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = Note.UserID
                };
                SqlCmd.Parameters.Add(pUserID);

                SqlParameter pNote = new SqlParameter
                {
                    ParameterName = "@RequestNote",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Note.RequestNote.Trim()
                };
                SqlCmd.Parameters.Add(pNote);

                SqlParameter pResponseRequired = new SqlParameter
                {
                    ParameterName = "@ResponseRequired",
                    SqlDbType = SqlDbType.Bit,
                    Value = Note.ResponseRequired
                };
                SqlCmd.Parameters.Add(pResponseRequired);

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

        public bool UpdateNote(ResponseUserNote Note, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateUserNote]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@NoteID",Note.NoteID);
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);
                SqlCmd.Parameters.AddWithValue("@UpdateType", Note.ActionType);

                if (Note.ResponseNote.Length > 0)
                {
                    SqlCmd.Parameters.AddWithValue("@ResponseNote", Note.ResponseNote);
                }
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


        public UserNotes Details (int NoteID)
        {
            UserNotes Note = new UserNotes();
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadUserNotes]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@NoteID", NoteID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Note.NoteID = Convert.ToInt32(dr["NoteID"]);
                        Note.UserID = Convert.ToInt32(dr["UserID"]);
                        Note.RequestNote = dr["RequestNote"].ToString();
                        Note.NoteDate = Convert.ToDateTime(dr["NoteDate"]);
                        Note.ResponseRequired = Convert.ToBoolean(dr["ResponseRequired"]);
                        Note.InsertedBy = dr["InsertedBy"].ToString();
                        Note.ResponseNote = dr["ResponseNote"].ToString();
                        
                        Note.ReadFlag = Convert.ToBoolean(dr["ReadFlag"]);

                        if (!Convert.IsDBNull(dr["ResponseDate"]))
                        {
                            Note.ResponseDate = Convert.ToDateTime(dr["ResponseDate"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return Note;

        }
    }
}
