using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api/Task")]
    public class TaskController : BaseController<Task>
    {
        private readonly PMContext _pmContext;

        public TaskController(PMContext context)
        {
            _pmContext = context;

            // seed data
            _pmContext.Database.EnsureCreated();
        }

        [HttpGet]
        public new IActionResult Get()
        {
            var tasks = _pmContext.Tasks.ToList();
            foreach (var task in tasks)
            {
                if (task != null && task.AssignedToUserID >= 0)
                { 
                    task.AssignedToUser = _pmContext.Users.Single(user => task.AssignedToUserID == user.ID); 
                }
            }
            return Ok(tasks);
            // throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}")]
        public new IActionResult Get(long id)
        {
            var task = _pmContext.Tasks.Where(task => id == task.ID).FirstOrDefault();
            if (task != null && task.AssignedToUserID >= 0)
            {
                task.AssignedToUser = _pmContext.Users.Single(user => task.AssignedToUserID == user.ID);
            }
            return Ok(task);
            // throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post(Entities.Task task)
        {
            task.AssignedToUser = _pmContext.Users.Where(user => task.AssignedToUserID == user.ID).FirstOrDefault();
            if (task.AssignedToUser == null)
            {
                task.AssignedToUserID = -1;
            }
            task.CreatedOn = DateTime.UtcNow;
            var project = _pmContext.Projects.Where(project => task.ProjectID == project.ID).FirstOrDefault();
            if (project == null)
            {
                task.ProjectID = -1;
            }
            _pmContext.Tasks.Add(task);
            // project.Tasks.Append(task);
            // _pmContext.Update(project);
            _pmContext.SaveChanges();
            return Ok(task);
            // throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult Put(Entities.Task task)
        {
            var taskInDb = _pmContext.Tasks.Single(t => t.ID == task.ID);
            taskInDb.Detail = task.Detail;
            taskInDb.Status = task.Status;
            taskInDb.AssignedToUserID = task.AssignedToUserID;
            taskInDb.AssignedToUser = _pmContext.Users.Single(user => task.AssignedToUserID == user.ID);
            taskInDb.ProjectID = task.ProjectID;
            _pmContext.Update(taskInDb);
            _pmContext.SaveChanges();
            return Ok(taskInDb);
            // throw new NotImplementedException();
        }

        [HttpDelete]
        public new IActionResult Delete()
        {
            var tasks = _pmContext.Tasks.ToList();
            string res = "";
            foreach (var task in tasks)
            {
                res += "Task Deleted : " + task.Detail + "\n";
            }
            _pmContext.RemoveRange(tasks);
            _pmContext.SaveChanges();
            
            /*foreach (var project in _pmContext.Projects.ToList())
            {
                project.Tasks = new List<Entities.Task>();
                _pmContext.Update(project);
                
            }*/
            return Ok(res);
            // throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteById(long id)
        {
            var task = _pmContext.Tasks.Single(task => task.ID == id);
            // var project = _pmContext.Projects.Single(p => p.ID == task.ProjectID);
            // project.Tasks = project.Tasks.Where(t => t.ID != task.ID).FirstOrDefault();

            _pmContext.Remove(task);
            // _pmContext.Update(project);

            _pmContext.SaveChanges();
            return Ok("Task Deleted : " + task.Detail);
            // throw new NotImplementedException();
        }
    }
}
