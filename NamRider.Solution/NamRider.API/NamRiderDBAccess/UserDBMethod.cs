using NamRider.API.NamRiderAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NamRider.API.NamRiderDBAccess
{
    public class UserDBMethod
    {
        #region instance of DbContext
        private NamRiderContext context = new NamRiderContext();
        #endregion

        #region CRUD Methods

        /// <summary>
        ///  Return all objects of User table
        /// </summary>
        /// <returns></returns>
        public List<AspNetUser> FindAll()
        {
            try
            {
                return context.AspNetUsers.ToList();
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Return the object of User with id equals id parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AspNetUser FindById(string userId)
        {
            try
            {
                var user = (from m in context.AspNetUsers where m.Id.Equals(userId) select m).Single();
                return user;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        ///  Update User object in paramerter 
        /// </summary>
        /// <param name="aggregate">User root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Save(AspNetUser aggregate)
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
        /// delete User object in paramerter 
        /// </summary>
        /// <param name="aggregate">User root object</param>
        /// <returns>True if successful; False otherwise</returns>
        public bool Delete(AspNetUser aggregate)
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
        /// Get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public AspNetUser FindByUsername(string username)
        {
            try
            {
                return (from m in context.AspNetUsers where m.UserName.Equals(username) select m).Single();
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public AspNetUser FindByEmail(string email)
        {
            try
            {
                return (from m in context.AspNetUsers where m.Email.Equals(email) select m).Single();
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }
        #endregion

    }
}