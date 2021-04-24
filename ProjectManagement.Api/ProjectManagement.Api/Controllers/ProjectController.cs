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
    [Route("api/Project")]
    public class ProjectController : BaseController<Project>
    {
        private readonly PMContext _pmContext;

        public ProjectController(PMContext context)
        {
            _pmContext = context;

            // seed data
            _pmContext.Database.EnsureCreated();
        }

        [HttpGet]
        public new IActionResult Get()
        {
            var projects = _pmContext.Projects.ToList();
            foreach (var project in projects)
            {
                project.Tasks = _pmContext.Tasks.Where(task => task.ProjectID == project.ID).ToList();
                foreach (var task in project.Tasks)
                {
                    task.AssignedToUser = task.AssignedToUserID >= 0 ? _pmContext.Users.Single(user => user.ID == task.AssignedToUserID) : null;
                }
            }
            return Ok(projects);
            // throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}")]
        public new IActionResult Get(long id)
        {
            var project = _pmContext.Projects.Where(project => id == project.ID).FirstOrDefault();
            project.Tasks = _pmContext.Tasks.Where(task => task.ProjectID == project.ID).ToList();
            foreach (var task in project.Tasks)
            {
                task.AssignedToUser = task.AssignedToUserID >= 0 ? _pmContext.Users.Single(user => user.ID == task.AssignedToUserID) : null;
            }
            return Ok(project);
            // throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post(Project project)
        {
            project.CreatedOn = DateTime.UtcNow;
            _pmContext.Projects.Add(project);
            _pmContext.SaveChanges();

            return Ok(project);
            // throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult Put(Project project)
        {
            var projectInDb = _pmContext.Projects.Single(p => project.ID == p.ID);
            projectInDb.Name = project.Name;
            projectInDb.Detail = project.Detail;
            _pmContext.Update(projectInDb);
            _pmContext.SaveChanges();
            return Ok(project);
            // throw new NotImplementedException();
        }

        [HttpDelete]
        public new IActionResult Delete()
        {
            var projects = _pmContext.Projects.ToList();
            List<string> res = new List<string>();
            foreach (var p in projects)
            {
                var tasks = _pmContext.Tasks.Where(task => p.ID == task.ProjectID).ToList();
                foreach (var task in tasks)
                {
                    task.ProjectID = -1;
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
                    _pmContext.Remove(p);
                    _pmContext.SaveChanges();
                    res.Add("Project Deleted : " + p.Name);
                }
                catch
                {
                    res.Add("Error in deleting project : " + p.Name);
                }
            }
            return Ok(res);
            // throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteById(long id)
        {
            var project = _pmContext.Projects.Single(project => project.ID == id);
            var tasks = _pmContext.Tasks.Where(task => project.ID == task.ProjectID).ToList();
            foreach (var task in tasks)
            {
                task.ProjectID = -1;
                _pmContext.Update(task);
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
                _pmContext.Remove(project);
                _pmContext.SaveChanges();
                return Ok(new List<string> { "Project Deleted : " + project.Name });
            }
            catch
            {
                return Ok(new List<string> { "Error in deleting project : " + project.Name });
            }
            
            
            // throw new NotImplementedException();
        }
    }
}
