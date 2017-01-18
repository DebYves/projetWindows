using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NamRider.API.NamRiderAPI.Persistence;
using System.Data.Entity;

namespace NamRider.API.NamRiderDBAccess
{
    public class CriticismParkingDBMethod
    {
        #region instance of DbContext
        public NamRiderContext context = new NamRiderContext();
        #endregion

        #region CRUD Methods

        /// <summary>
        ///  Return all objects of Criticism Park table
        /// </summary>
        /// <returns></returns>
        public List<CriticismParkingInfo> FindAll()
        {
            try
            {
                return context.CriticismParkingInfoes.ToList();
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Return the object of Criticism Park by IdPark and IdUser
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="idInfo"></param>
        /// <returns></returns>
        public CriticismParkingInfo FindById(string userId, int idInfo)
        {
            try
            {
                var criticism = (from m in context.CriticismParkingInfoes where m.IdParking.Equals(idInfo) && m.UserId.Equals(userId) select m).Single();
                return criticism;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        ///  Add Criticism Park object in paramerter to Criticism Park table
        /// </summary>
        /// <param name="aggregate">Criticism root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Add(CriticismParkingInfo aggregate)
        {
            try
            {
                context.CriticismParkingInfoes.Add(aggregate);
                return context.SaveChanges() > 0;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }

        /// <summary>
        ///  Update Criticism Park object in paramerter 
        /// </summary>
        /// <param name="aggregate">Criticism root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Save(CriticismParkingInfo aggregate)
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
        /// delete Criticism Park object in paramerter 
        /// </summary>
        /// <param name="aggregate">Criticism root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Delete(CriticismParkingInfo aggregate)
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
        /// Return  objects of Criticism Park table by ParkId
        /// </summary>
        /// <param name="idInfo"></param>
        /// <returns></returns>
        public List<CriticismParkingInfo> FindByParking(int idInfo)
        {
            try
            {
                var list = (from m in context.CriticismParkingInfoes where m.IdParking.Equals(idInfo) select m).ToList();
                return list;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Delete all Critism Park by ParkId
        /// </summary>
        /// <param name="listAggregate"></param>
        /// <returns></returns>
        public bool DeleteCriticismByParking(List<CriticismParkingInfo> listAggregate)
        {
            try
            {
                foreach (CriticismParkingInfo criticism in listAggregate)
                {
                    context.Entry(criticism).State = EntityState.Deleted;
                }
                return context.SaveChanges() > 0;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }
        #endregion
    }
}