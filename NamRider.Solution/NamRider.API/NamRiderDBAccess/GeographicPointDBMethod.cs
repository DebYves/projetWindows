using NamRider.API.NamRiderAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NamRider.API.NamRiderDBAccess
{
    public class GeographicPointDBMethod
    {
        #region instance of DbContext
        private NamRiderContext context = new NamRiderContext();
        #endregion

        #region CRUD Methods

        /// <summary>
        ///  Return all objects of GeographicPoint table
        /// </summary>
        /// <returns></returns>
        public List<GeographicPoint> FindAll()
        {
            try
            {
                return context.GeographicPoints.ToList();
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Return the object of GeographicPoint by id (latitude and longitude)
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public GeographicPoint FindById(decimal latitude, decimal longitude)
        {
            try
            {
                var geographiePoint = (from m in context.GeographicPoints where m.Latitude.Equals(latitude) && m.Longitude.Equals(longitude) select m).Single();
                return geographiePoint;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Add GeographicPoint object in paramerter to GeographicPoint table
        /// </summary>
        /// <param name="aggregate">GeographicPoint root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Add(GeographicPoint aggregate)
        {
            try
            {
                context.GeographicPoints.Add(aggregate);
                return context.SaveChanges() > 0;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }

        /// <summary>
        ///  Update GeographicPoint object in paramerter 
        /// </summary>
        /// <param name="aggregate">GeographicPoint root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Save(GeographicPoint aggregate)
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
        ///  delete GeographicPoint object in paramerter 
        /// </summary>
        /// <param name="aggregate">GeographicPoint root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Delete(GeographicPoint aggregate)
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

        #region Others Mehods
        /// <summary>
        /// delete GeographicPoint object by idParking 
        /// </summary>
        /// <param name="idParking"></param>
        /// <returns></returns>
        public GeographicPoint FindByIdParkng(int idParking)
        {
            try
            {
                var geographiquePoint = (from m in context.GeographicPoints where m.IdParking.Equals(idParking) select m).Single();
                return geographiquePoint;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }
        #endregion
    }
}