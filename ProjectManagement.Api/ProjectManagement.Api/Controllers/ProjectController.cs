using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Entities;
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
        private readonly PMDbContext _pmDbContext;

        public ProjectController(PMDbContext context)
        {
            _pmDbContext = context;
        }

        [HttpGet]
        public new IActionResult Get()
        {
            var projects = _pmDbContext.Projects.ToList();
            foreach (var project in projects)
            {
                project.Tasks = _pmDbContext.Tasks.Where(task => task.ProjectID == project.ID).ToList();
                foreach (var task in project.Tasks)
                {
                    task.AssignedToUser = task.AssignedToUserID >= 0 ? _pmDbContext.Users.Single(user => user.ID == task.AssignedToUserID) : null;
                }
            }
            return Ok(projects);
            // throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id}")]
        public new IActionResult Get(long id)
        {
            var project = _pmDbContext.Projects.Where(project => id == project.ID).FirstOrDefault();
            project.Tasks = _pmDbContext.Tasks.Where(task => task.ProjectID == project.ID).ToList();
            foreach (var task in project.Tasks)
            {
                task.AssignedToUser = task.AssignedToUserID >= 0 ? _pmDbContext.Users.Single(user => user.ID == task.AssignedToUserID) : null;
            }
            return Ok(project);
            // throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post(Project project)
        {
            _pmDbContext.Projects.Add(project);
            _pmDbContext.SaveChanges();

            return Ok("Project Added : " + project.Name);
            // throw new NotImplementedException();
        }

        [HttpPut]
        public IActionResult Put(Project project)
        {
            var projectInDb = _pmDbContext.Projects.Single(p => project.ID == p.ID);
            projectInDb.Name = project.Name;
            projectInDb.Detail = project.Detail;
            _pmDbContext.Update(projectInDb);
            _pmDbContext.SaveChanges();
            return Ok(project);
            // throw new NotImplementedException();
        }

        [HttpDelete]
        public new IActionResult Delete()
        {
            var projects = _pmDbContext.Projects.ToList();
            var res = "";
            foreach (var p in projects)
            {
                var tasks = _pmDbContext.Tasks.Where(task => p.ID == task.ProjectID).ToList();
                foreach (var task in tasks)
                {
                    task.ProjectID = -1;
                    _pmDbContext.Update(task);
                    _pmDbContext.SaveChanges();
                }
                _pmDbContext.Remove(p);
                _pmDbContext.SaveChanges();
                res += "Project Deleted : " + p.Name + "\n";
            }
            return Ok(res);
            // throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteById(long id)
        {
            var project = _pmDbContext.Projects.Single(project => project.ID == id);
            var tasks = _pmDbContext.Tasks.Where(task => project.ID == task.ProjectID).ToList();
            foreach (var task in tasks)
            {
                task.ProjectID = -1;
                _pmDbContext.Update(task);
                _pmDbContext.SaveChanges();
            }
            _pmDbContext.Remove(project);
            _pmDbContext.SaveChanges();
            return Ok("Project Deleted : " + project.Name);
            // throw new NotImplementedException();
        }
    }
}
