﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    /// <summary>
    /// Christian Lopez
    /// Created 
    /// 2017/03/03
    /// 
    /// Handles accessing the DB for warehouse information
    /// </summary>
    public class WarehouseAccessor
    {
        /// <summary>
        /// Chrsitian Lopez
        /// Created 
        /// 2017/03/03
        /// 
        /// Gets a list of all warehouses in the DB
        /// </summary>
        /// <returns></returns>
        public static List<Warehouse> RetrieveAllWarehouses()
        {
            List<Warehouse> warehouses = new List<Warehouse>();

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_retrieve_warehouse_list";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Warehouse warehouse = new Warehouse
                        {
                            WarehouseID = reader.GetInt32(0),
                            AddressOne = reader.GetString(1),
                            AddressTwo = reader.GetString(2),
                            City = reader.GetString(3),
                            State = reader.GetString(4),
                            Zip = reader.GetString(5)
                        };
                        warehouses.Add(warehouse);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                conn.Close();
            }

            return warehouses;
        }

        /// <summary>
        /// Mason Allen
        /// Created:
        /// 2017/03/30
        /// 
        /// Creates a new warehouse
        /// 
        /// <remarks>
        /// Mason Allen
        /// Updated:
        /// 2017/04/13
        /// 
        /// 
        /// Updated return int to sql generated warehouseId instead of numrows
        /// 
        /// Ariel Sigo
        /// Updated: 
        /// 2017/04/29
        /// 
        /// Standardized Comment
        /// </remarks>
        /// 
        /// 
        /// </summary>
        /// <param name="newWarehouse">Warehouse Object to be Created</param>
        /// <returns>Returns an int of 1 if successful, 0 if not</returns>
        public static int CreateWarehouse(Warehouse newWarehouse)
        {
            var conn = DBConnection.GetConnection();
            const string cmdText = @"sp_create_warehouse";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ADDRESS_1", newWarehouse.AddressOne);
            cmd.Parameters.AddWithValue("@ADDRESS_2", newWarehouse.AddressTwo);
            cmd.Parameters.AddWithValue("@CITY", newWarehouse.City);
            cmd.Parameters.AddWithValue("@STATE", newWarehouse.State);
            cmd.Parameters.AddWithValue("@ZIP", newWarehouse.Zip);

            int warehouseId = 0;

            try
            {
                conn.Open();
                warehouseId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {

                throw new ApplicationException("There was an error saving to the DB: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return warehouseId;
        }

        /// <summary>
        /// Ariel Sigo
        /// Updated:
        /// 2017/04/29
        /// 
        /// Added Comment,
        /// 
        /// Calls stored procedure to make a warehouse no longer active.
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns>rows that were changed in the Warehouse table</returns>
        public static int DeleteWarehouse(int warehouseId)
        {
            var conn = DBConnection.GetConnection();
            const string cmdText = @"sp_delete_warehouse";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@WAREHOUSE_ID", warehouseId);

            int rows = 0;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }
    }
}
