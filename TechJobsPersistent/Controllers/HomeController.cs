using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet("/Add")]
        public IActionResult AddJob()
        {
            AddJobViewModel newJobViewModel = new AddJobViewModel();
            //Check employer list as a SelectListItem
            List<Employer> employers = context.Employers.ToList();
            newJobViewModel.Employers = new List<SelectListItem>();
                foreach (var employer in  employers)
            {
                SelectListItem item = new SelectListItem();
                item.Text = employer.Name;
                item.Value = employer.Id.ToString();
                newJobViewModel.Employers.Add(item);
            }
            return View(newJobViewModel);
        }

        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel)
        {
            if (ModelState.IsValid)
            {
                string name = addJobViewModel.Name;
                int employerId = addJobViewModel.EmployerId;
                Job newJob = new Job();
                newJob.Name = name;
                newJob.EmployerId = employerId;
                context.Jobs.Add(newJob);
                context.SaveChanges();
            }
            return View();
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
