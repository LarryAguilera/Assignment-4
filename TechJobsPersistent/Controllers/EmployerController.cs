using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechJobsPersistent.Data;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechJobsPersistent.Controllers
{
    public class EmployerController : Controller
            {
        private JobDbContext dbContext;
        public EmployerController(JobDbContext DbContext)
        {
            dbContext = DbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Employer> employers = dbContext.Employers.ToList();
            return View(employers);
        }

        public IActionResult Add()
        {
            AddEmployerViewModel newEmployerViewModel = new AddEmployerViewModel();
            return View(newEmployerViewModel);
        }

        public IActionResult ProcessAddEmployerForm(AddEmployerViewModel addEmployerViewModel)
        {
            if (ModelState.IsValid)
            {
                string name = addEmployerViewModel.Name;
                string location = addEmployerViewModel.Location;
                Employer newEmployer = new Employer();
                newEmployer.Name = name;
                newEmployer.Location = location;
                dbContext.Employers.Add(newEmployer);
                dbContext.SaveChanges();
            }
            return View();
        }

        public IActionResult About(int id)
        {
            Employer employer = dbContext.Employers.Find(id);
            return View(employer);
        }
    }
}
