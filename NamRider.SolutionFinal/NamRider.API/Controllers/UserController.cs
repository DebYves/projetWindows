using NamRider.API.NamRider.Service;
using NamRider.API.NamRiderAPI.Persistence;
using NamRider.API.NamRiderDBAccess;
using NamRider.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace NamRider.API.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        #region Proprities DBAccess and Service Business  methods classes
        private UserDBMethod _userDBMethod = new UserDBMethod();
        #endregion

        /// <summary>
        /// GET: api/User
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Get()
        {
            try
            {
                List<UserModel> listUser = new List<UserModel>();
                var users = _userDBMethod.FindAll();
                if (users != null)
                {
                    foreach (var i in users)
                    {
                        listUser.Add(new UserModel()
                        {
                            UserId = i.Id,
                            UserName = i.UserName,
                            EmailAddress = i.Email,
                        });
                    }
                    return Ok(listUser);
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// GET: api/User/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Get(string id)
        {
            try
            {
                var user = _userDBMethod.FindById(id);
                if (user != null)
                {
                    return Ok(new UserModel()
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        EmailAddress = user.Email,
                    });
                }
                return Ok(user); ;
               
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// GET_BY_USERNAME: api/User/GetByUserName
        /// </summary>
        /// <param name="usernameInput"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/User/GetByUserName")]
        [HttpPost]
        public IHttpActionResult GetByUserName(UsernameModel usernameInput)
        {
            try
            {
                var user = _userDBMethod.FindByUsername(usernameInput.UserName);
                if (user != null)
                {
                    return Ok(new UserModel()
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        EmailAddress = user.Email,
                    });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// GET_BY_EMAIL: api/User/GetByEmail
        /// </summary>
        /// <param name="emailInput"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/User/GetByEmail")]
        [HttpPost]
        public IHttpActionResult GetByEmail(UserEmailModel emailInput)
        {
            try
            {
                var user = _userDBMethod.FindByEmail(emailInput.EmailAddress);
                if (user != null)
                {
                    return Ok(new UserModel()
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        EmailAddress = user.Email,
                    });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
