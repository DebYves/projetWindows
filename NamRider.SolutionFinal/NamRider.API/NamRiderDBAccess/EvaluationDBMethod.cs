using NamRider.API.NamRiderAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NamRider.API.NamRiderDBAccess
{
    public class EvaluationDBMethod
    {
        #region instance of DbContext
        private NamRiderContext context = new NamRiderContext();
        #endregion

        #region CRUD Methods

        /// <summary>
        ///  Return all objects of Evaluation table
        /// </summary>
        /// <returns></returns>
        public List<Evaluation> FindAll()
        {
            try
            {
                return context.Evaluations.ToList();
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Return the object of Evaluation by idDriving and idUser
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="idDrivingInfos"></param>
        /// <returns></returns>
        public Evaluation FindById(string userId, int idDrivingInfos)
        {
            try
            {
                return (from m in context.Evaluations where m.IdDriving.Equals(idDrivingInfos) && m.UserId.Equals(userId) select m).Single();
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        ///     Add Evaluation object in paramerter to Evaluation table
        /// </summary>
        /// <param name="aggregate">Evaluation root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Add(Evaluation aggregate)
        {
            try
            {
                context.Evaluations.Add(aggregate);
                return context.SaveChanges() > 0;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Update Evaluation object in paramerter 
        /// </summary>
        /// <param name="aggregate">Evaluation root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Save(Evaluation aggregate)
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
        ///     delete Evaluation object in paramerter 
        /// </summary>
        /// <param name="aggregate">Evaluation root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Delete(Evaluation aggregate)
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
        /// Find all evaluation by a idDriving
        /// </summary>
        /// <param name="idInfoDriving"></param>
        /// <returns></returns>
        public List<Evaluation> FindByDrivingId(int idInfoDriving)
        {
            return (from m in context.Evaluations where m.IdDriving.Equals(idInfoDriving) select m).ToList();
        }

        /// <summary>
        /// Delete all evaluation by idDriving 
        /// </summary>
        /// <param name="listAggregate"></param>
        /// <returns></returns>
        public bool DeleteEvaluationByDrivingId(List<Evaluation> listAggregate)
        {
            try
            {
                foreach(Evaluation evaluation in listAggregate)
                {
                    context.Entry(evaluation).State = EntityState.Deleted;
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