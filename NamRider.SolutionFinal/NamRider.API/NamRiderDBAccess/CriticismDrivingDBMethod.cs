using NamRider.API.NamRiderAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NamRider.API.NamRiderDBAccess
{
    
    public class CriticismDrivingDBMethod
    {
        #region instance DbContext
        private NamRiderContext contextD = new NamRiderContext();
        #endregion

        #region CRUD Methods

        /// <summary>
        ///  Return all objects of Criticism driving table
        /// </summary>
        /// <returns></returns>
        public List<CriticismDrivingInfo> FindAll()
        {
            try
            {
                return contextD.CriticismDrivingInfoes.ToList();
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Return a object of Criticism driving by idDriving and idUser
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="idInfo"></param>
        /// <returns></returns>
        public CriticismDrivingInfo FindById(string userId,int idInfo)
        {
            try
            {
                var criticism = (from m in contextD.CriticismDrivingInfoes where m.IdDriving.Equals(idInfo) && m.UserId.Equals(userId) select m).Single();
                return criticism;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Add the paramerter object in Criticism driving table
        /// </summary>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        public bool Add(CriticismDrivingInfo aggregate)
        {
            try
            {
                contextD.CriticismDrivingInfoes.Add(aggregate);
                return contextD.SaveChanges() > 0;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Update Criticism driving object in paramerter 
        /// </summary>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        public bool Save(CriticismDrivingInfo aggregate)
        {
            try
            {
                contextD.Entry(aggregate).State = EntityState.Modified;
                return contextD.SaveChanges() > 0;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// delete Criticism driving object in paramerter 
        /// </summary>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        public bool Delete(CriticismDrivingInfo aggregate)
        {
            try
            {
                contextD.Entry(aggregate).State = EntityState.Deleted;
                return contextD.SaveChanges() > 0;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }
        #endregion

        #region Others Methods

        /// <summary>
        /// Return objects of Criticism driving table by drivingId
        /// </summary>
        /// <param name="idInfo"></param>
        /// <returns></returns>
        public List<CriticismDrivingInfo> FindByDriving(int idInfo)
        {
            try
            {
                var list = (from m in contextD.CriticismDrivingInfoes where m.IdDriving.Equals(idInfo) select m).ToList();
                return list;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Delete all Critism driving by Critism driving Id
        /// </summary>
        /// <param name="listAggregate"></param>
        /// <returns></returns>
        public bool DeleteCriticismByInformation(List<CriticismDrivingInfo> listAggregate)
        {
            try
            {
                foreach (CriticismDrivingInfo criticism in listAggregate)
                {
                    contextD.Entry(criticism).State = EntityState.Deleted;
                }
                return contextD.SaveChanges() > 0;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }
       
        #endregion
    }
    
}