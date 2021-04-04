using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Entities;
using ProjectManagement.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : BaseController<User>
    {
        private readonly PMContext _pmContext;

        public UserController(PMContext context)
        {
            _pmContext = context;

            // seed data
            _pmContext.Database.EnsureCreated();
        }

        [HttpGet]
        public new IActionResult Get()
        {
            var users = _pmContext.Users.ToList();
            return Ok(users);
            // throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}")]
        public new IActionResult Get(long id)
        {
            var user = _pmContext.Users.Where(user => id == user.ID).FirstOrDefault();
            return Ok(user);
            // throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            _pmContext.Users.Add(user);
            _pmContext.SaveChanges();

            return Ok("User Added : " + user.FirstName + " " + user.LastName);
            //throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult Put(User user)
        {
            _pmContext.Users.Update(user);
            _pmContext.SaveChanges();
            return Ok(user);
            // throw new NotImplementedException();
        }

        [HttpDelete]
        public new IActionResult Delete()
        {
            var user = _pmContext.Users.ToList();
            var res = "";
            foreach (var u in user)
            {
                var tasks = _pmContext.Tasks.Where(task => u.ID == task.AssignedToUserID).ToList();
                foreach (var task in tasks)
                {
                    task.AssignedToUserID = -1;
                    _pmContext.Update(task);
                    _pmContext.SaveChanges();
                }
                _pmContext.Remove(u);
                _pmContext.SaveChanges();
                res += "User Deleted : " + u.FirstName + " " + u.LastName + "\n";
            }
            return Ok(res);
            // throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteById(long id)
        {
            try { 
                var user = _pmContext.Users.Single(user => user.ID == id);
                var tasks = _pmContext.Tasks.Where(task => user.ID == task.AssignedToUserID).ToList();
                foreach (var task in tasks)
                {
                    task.AssignedToUserID = -1;
                    _pmContext.Update(task);
                    _pmContext.SaveChanges();
                }
                _pmContext.Remove(user);
                _pmContext.SaveChanges();
                return Ok("User Deleted : " + user.FirstName + " " + user.LastName);
                // throw new NotImplementedException();
            } catch
            {
                return NotFound(id);
            }
        }
    }
}
