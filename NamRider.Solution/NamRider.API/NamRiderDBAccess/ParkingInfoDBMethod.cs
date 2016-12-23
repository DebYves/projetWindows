using NamRider.API.NamRiderAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NamRider.API.NamRiderDBAccess
{
    public class ParkingInfoDBMethod
    {
        #region instance of DbContext
        private NamRiderContext context = new NamRiderContext();
        #endregion

        public static SqlConnection GetDatabaseConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["NamRiderContext"].ConnectionString);
        }

        #region CRUD Methods
        /// <summary>
        ///  Return all objects of ParkingInfo table
        /// </summary>
        /// <returns></returns>
        public List<ParkingInfo> FindAll()
        {
            try
            {
                return context.ParkingInfoes.ToList();
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        ///  Return the object of ParkingInfo with id equals id parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ParkingInfo FindById(int id)
        {
            try
            {
                return (from m in context.ParkingInfoes where m.Id.Equals(id) select m).Single();
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Add ParkingInfo object in paramerter to DrivingInfo  table
        /// </summary>
        /// <param name="aggregate">ParkingInfo root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Add(ParkingInfo aggregate)
        {
            try
            {
                context.ParkingInfoes.Add(aggregate);
                return context.SaveChanges() > 0;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Update ParkingInfo object in paramerter 
        /// </summary>
        /// <param name="aggregate">ParkingInfo root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Save(ParkingInfo aggregate)
        {
            try
            {
                context.Entry(aggregate).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }

        /// <summary>
        ///  delete ParkingInfo object in paramerter 
        /// </summary>
        /// <param name="aggregate">ParkingInfo root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Delete(ParkingInfo aggregate)
        {
            try
            {
                context.Entry(aggregate).State = EntityState.Deleted;
                return context.SaveChanges() > 0;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }
        #endregion

        #region Others Methods
        /// <summary>
        /// Find ParkingInfo By Type
        /// </summary>
        /// <param name="lisType"></param>
        /// <returns></returns>
        public List<ParkingInfo> FindByType(List<string> lisType)
        {
            try
            {
                var listParkingReturn = new List<ParkingInfo>();
                var listParking = context.ParkingInfoes.ToList();

                foreach (string type in lisType)
                {
                    foreach (ParkingInfo park in listParking)
                    {
                        if (park.Type.Equals(type))
                        {
                            listParkingReturn.Add(park);
                        }
                    }
                }
                return listParkingReturn;
                //return (from m in context.ParkingInfoes where m.Type.Equals(type) select m).ToList();
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        ///  Get number of alerts for parkingInfo
        /// </summary>
        /// <param name="idPark"></param>
        /// <returns></returns>
        public int GetNumberAlert(int idPark)
        {
            var park = (from m in context.ParkingInfoes where m.Id.Equals(idPark) select m).Single();
            return park.AspNetUsers.Count;
        }

        #endregion


    }
}