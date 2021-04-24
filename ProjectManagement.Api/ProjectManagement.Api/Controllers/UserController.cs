using AutoMapper;
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
        private readonly dynamic mapper;

        public UserController(PMContext context)
        {
            _pmContext = context;

            // seed data
            _pmContext.Database.EnsureCreated();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>());
            mapper = new Mapper(config);
        }

        [HttpGet]
        public new IActionResult Get()
        {
            var users = _pmContext.Users.ToList();
            List<UserDto> u = new List<UserDto>();
            users.ForEach(user => u.Add(mapper.Map<UserDto>(user)));
            return Ok(u);
            // throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}")]
        public new IActionResult Get(long id)
        {
            var user = _pmContext.Users.Where(user => id == user.ID).FirstOrDefault();
            return Ok(mapper.Map<UserDto>(user));
            // throw new NotImplementedException();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult GetByEmail(User u)
        {
            var user = _pmContext.Users.Where(user => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            if (user != null)
            {
                return Ok(mapper.Map<UserDto>(user));
            } else
            {
                return NotFound(mapper.Map<UserDto>(user));
            }
            
            // throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            var u = _pmContext.Users.Where(u => u.Email == user.Email).FirstOrDefault();
            if (u == null)
            {
                _pmContext.Users.Add(user);
                _pmContext.SaveChanges();

                return Ok(mapper.Map<UserDto>(user));
            }
            else
            {
                return Conflict(mapper.Map<UserDto>(user));
            }
            
            //throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult Put(User user)
        {
            var u = _pmContext.Users.Where(u => u.Email == user.Email && u.ID != user.ID).FirstOrDefault();
            if (u == null)
            {
                _pmContext.Users.Update(user);
                _pmContext.SaveChanges();
                return Ok(mapper.Map<UserDto>(user));
            } else
            {
                return Conflict(mapper.Map<UserDto>(user));
            }
            
            // throw new NotImplementedException();
        }

        [HttpDelete]
        public new IActionResult Delete()
        {
            var user = _pmContext.Users.ToList();
            List<string> res = new List<string>();
            foreach (var u in user)
            {
                var tasks = _pmContext.Tasks.Where(task => u.ID == task.AssignedToUserID).ToList();
                foreach (var task in tasks)
                {
                    task.AssignedToUserID = -1;
                    try
                    {
                        _pmContext.Update(task);
                        _pmContext.SaveChanges();
                    }
                    catch
                    {
                        Console.WriteLine(String.Format("Error while updating task: id={0}, detail={1}", task.ID, task.Detail));
                    }
                }

                try
                {
                    _pmContext.Remove(u);
                    _pmContext.SaveChanges();
                    res.Add("User Deleted : " + u.FirstName + " " + u.LastName);
                } catch
                {
                    res.Add("Error in deleting user : " + u.FirstName + " " + u.LastName);
                }
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
                return Ok(new List<string> { "User Deleted : " + user.FirstName + " " + user.LastName });
                // throw new NotImplementedException();
            } catch
            {
                return NotFound(id);
            }
        }
    }
}
