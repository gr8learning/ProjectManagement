using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Entities;
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
        private readonly PMDbContext _pmDbContext;

        public UserController(PMDbContext context)
        {
            _pmDbContext = context;
        }

        [HttpGet]
        public new IActionResult Get()
        {
            var users = _pmDbContext.Users.ToList();
            return Ok(users);
            // throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}")]
        public new IActionResult Get(long id)
        {
            var user = _pmDbContext.Users.Where(user => id == user.ID).FirstOrDefault();
            return Ok(user);
            // throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            _pmDbContext.Users.Add(user);
            _pmDbContext.SaveChanges();

            return Ok("User Added : " + user.FirstName + " " + user.LastName);
            //throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult Put(User user)
        {
            _pmDbContext.Users.Update(user);
            _pmDbContext.SaveChanges();
            return Ok(user);
            // throw new NotImplementedException();
        }

        [HttpDelete]
        public new IActionResult Delete()
        {
            var user = _pmDbContext.Users.ToList();
            var res = "";
            foreach (var u in user)
            {
                var tasks = _pmDbContext.Tasks.Where(task => u.ID == task.AssignedToUserID).ToList();
                foreach (var task in tasks)
                {
                    task.AssignedToUserID = -1;
                    _pmDbContext.Update(task);
                    _pmDbContext.SaveChanges();
                }
                _pmDbContext.Remove(u);
                _pmDbContext.SaveChanges();
                res += "User Deleted : " + u.FirstName + " " + u.LastName + "\n";
            }
            return Ok(res);
            // throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteById(long id)
        {
            var user = _pmDbContext.Users.Single(user => user.ID == id);
            var tasks = _pmDbContext.Tasks.Where(task => user.ID == task.AssignedToUserID).ToList();
            foreach (var task in tasks)
            {
                task.AssignedToUserID = -1;
                _pmDbContext.Update(task);
                _pmDbContext.SaveChanges();
            }
            _pmDbContext.Remove(user);
            _pmDbContext.SaveChanges();
            return Ok("User Deleted : " + user.FirstName + " " + user.LastName);
            // throw new NotImplementedException();
        }
    }
}
