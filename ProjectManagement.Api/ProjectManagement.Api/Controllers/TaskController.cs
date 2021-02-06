using Microsoft.AspNetCore.Mvc;
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
        private readonly PMDbContext _pmDbContext;

        public TaskController(PMDbContext context)
        {
            _pmDbContext = context;
        }

        [HttpGet]
        public new IActionResult Get()
        {
            var tasks = _pmDbContext.Tasks.ToList();
            foreach (var task in tasks)
            {
                if (task != null && task.AssignedToUserID >= 0)
                { 
                    task.AssignedToUser = _pmDbContext.Users.Single(user => task.AssignedToUserID == user.ID); 
                }
            }
            return Ok(tasks);
            // throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}")]
        public new IActionResult Get(long id)
        {
            var task = _pmDbContext.Tasks.Where(task => id == task.ID).FirstOrDefault();
            if (task != null && task.AssignedToUserID >= 0)
            {
                task.AssignedToUser = _pmDbContext.Users.Single(user => task.AssignedToUserID == user.ID);
            }
            return Ok(task);
            // throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post(Entities.Task task)
        {
            task.AssignedToUser = _pmDbContext.Users.Where(user => task.AssignedToUserID == user.ID).FirstOrDefault();
            task.CreatedOn = DateTime.UtcNow;
            // var project = _pmDbContext.Projects.Where(project => task.ProjectID == project.ID).FirstOrDefault();
            _pmDbContext.Tasks.Add(task);
            // project.Tasks.Append(task);
            // _pmDbContext.Update(project);
            _pmDbContext.SaveChanges();
            return Ok(task);
            // throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult Put(Entities.Task task)
        {
            var taskInDb = _pmDbContext.Tasks.Single(t => t.ID == task.ID);
            taskInDb.Detail = task.Detail;
            taskInDb.Status = task.Status;
            taskInDb.AssignedToUserID = task.AssignedToUserID;
            taskInDb.AssignedToUser = _pmDbContext.Users.Single(user => task.AssignedToUserID == user.ID);
            taskInDb.ProjectID = task.ProjectID;
            _pmDbContext.Update(taskInDb);
            _pmDbContext.SaveChanges();
            return Ok(taskInDb);
            // throw new NotImplementedException();
        }

        [HttpDelete]
        public new IActionResult Delete()
        {
            var tasks = _pmDbContext.Tasks.ToList();
            string res = "";
            foreach (var task in tasks)
            {
                res += "Task Deleted : " + task.Detail + "\n";
            }
            _pmDbContext.RemoveRange(tasks);
            _pmDbContext.SaveChanges();
            
            /*foreach (var project in _pmDbContext.Projects.ToList())
            {
                project.Tasks = new List<Entities.Task>();
                _pmDbContext.Update(project);
                
            }*/
            return Ok(res);
            // throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteById(long id)
        {
            var task = _pmDbContext.Tasks.Single(task => task.ID == id);
            // var project = _pmDbContext.Projects.Single(p => p.ID == task.ProjectID);
            // project.Tasks = project.Tasks.Where(t => t.ID != task.ID).FirstOrDefault();

            _pmDbContext.Remove(task);
            // _pmDbContext.Update(project);

            _pmDbContext.SaveChanges();
            return Ok("Task Deleted : " + task.Detail);
            // throw new NotImplementedException();
        }
    }
}
