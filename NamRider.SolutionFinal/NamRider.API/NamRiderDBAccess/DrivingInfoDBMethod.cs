using NamRider.API.NamRiderAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Infrastructure;

namespace NamRider.API.NamRiderDBAccess
{
    public class DrivingInfoDBMethod
    {
        #region instance of Dbcontext
        private NamRiderContext context = new NamRiderContext();
        #endregion

        #region CRUD Methods

        /// <summary>
        ///  Return all objects of DrivingInfo table
        /// </summary>
        /// <returns></returns>
        public List<DrivingInfo> FindAll()
        {
            try
            {
                return context.DrivingInfoes.ToList();
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Return the object of DrivingInfo with id at parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DrivingInfo FindById(int id)
        {
            try
            {
                var dr = (from m in context.DrivingInfoes where m.Id.Equals(id) select m).Include(i => i.AspNetUsers).Single();
                return dr;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Add DrivingInfo object in paramerter to DrivingInfo  table
        /// </summary>
        /// <param name="aggregate">DrivingInfo root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Add(DrivingInfo aggregate)
        {
            try
            {
                context.DrivingInfoes.Add(aggregate);
                return context.SaveChanges() > 0;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Update DrivingInfo object in paramerter 
        /// </summary>
        /// <param name="aggregate">DrivingInfo root object</param>
        /// <returns>True if successful; False otherwise</returns>
       // [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public bool Save(DrivingInfo aggregate)
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
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
                return context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// delete DrivingInfo object in paramerter 
        /// </summary>
        /// <param name="aggregate">DrivingInfo root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Delete(DrivingInfo aggregate)
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
            catch (DbUpdateConcurrencyException ex)
            {

                ex.Entries.Single().Reload();
                return context.SaveChanges() > 0;
            }
        }
        #endregion

        #region Others methods
        /// <summary>
        /// Get alert users
        /// </summary>
        /// <param name="idDriving"></param>
        /// <returns></returns>
        public AspNetUser GetAlertUser(int idDriving, string userId)
        {
            var driving = (from m in context.DrivingInfoes where m.Id.Equals(idDriving) select m).Single();
            return  (from m in driving.AspNetUsers where m.Id.Equals(userId) select m).Single();
        }

        /// <summary>
        /// Get number of alerts for DrivingInfo
        /// </summary>
        /// <param name="idDriving"></param>
        /// <returns></returns>
        public int GetNumberAlert(int idDriving)
        {
            var driving = (from m in context.DrivingInfoes where m.Id.Equals(idDriving) select m).Single();
            return driving.AspNetUsers.Count;
        }
        #endregion
    }
}