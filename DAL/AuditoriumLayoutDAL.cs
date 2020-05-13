using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class AuditoriumLayoutDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MAIN_CR_OA_Connection"].ToString());

        public AuditoriumLayout Layout(int EventID)
        {
            List<Block> blocks = new List<Block>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspReadAuditoriumLayout]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };
                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Block
                        {
                            BlockID = dr["BlockID"].ToString(),
                            Rows = Convert.ToInt32(dr["Rows"])                            
                        };
                        blocks.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

                foreach(var block in blocks)
                {
                    block.RowData = RowDetails(block.BlockID, EventID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            AuditoriumLayout layout = new AuditoriumLayout()
            {
                Blocks = blocks
            };

            return layout;
        }

        private List<Row> RowDetails(string BlockID, int EventID)
        {
            List<Row> list = new List<Row>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspReadAuditoriumRows]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter parBlockID = new SqlParameter
                {
                    ParameterName = "@BlockID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = BlockID
                };
                SqlCmd.Parameters.Add(parBlockID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Row
                        {
                            RowID = dr["RowID"].ToString(),
                            BlockID = dr["BlockID"].ToString(),
                            Label = Convert.ToInt32(dr["Label"]),
                            SeatsNbr = Convert.ToInt32(dr["Seats"])
                        };
                        list.Add(detail);
                    }
                }
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

                foreach (var d in list)
                {
                    d.SeatsData = SeatDetails(d.RowID, EventID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return list;
        }

        private List<Seats> SeatDetails(string RowID, int EventID)
        {
            List<Seats> list = new List<Seats>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[book].[uspReadAuditoriumSeats]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pRowID = new SqlParameter
                {
                    ParameterName = "@RowID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = RowID
                };
                SqlCmd.Parameters.Add(pRowID);

                SqlParameter pEventID = new SqlParameter
                {
                    ParameterName = "@EventID",
                    SqlDbType = SqlDbType.Int,
                    Value = EventID
                };
                SqlCmd.Parameters.Add(pEventID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Seats
                        {
                            SeatID = dr["SeatID"].ToString(),
                            RowID = dr["RowID"].ToString(),
                            Label = Convert.ToInt32(dr["Label"]),
                            Reserved = Convert.ToBoolean(dr["Reserved"]),
                            Booked = Convert.ToInt32(dr["Booked"]),
                            BookedFor = dr["BookedFor"].ToString()
                        };
                        list.Add(detail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return list;
        }
    }
}
