using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System.Diagnostics;

namespace Project.Controllers
{
    public class StudentController : Controller
    {
        public StudentDbContext _context;

        public StudentController(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //ViewBag.Message = HttpContext.Session.GetString("Email");
            return View(await _context.Set<Student>().ToListAsync());
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddEdit(int id)
        {
            Student student = new Student();
            if(id != 0)
            {
                student = await _context.Set<Student>().FindAsync(id);
            }
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(Student student)
        {
            if (ModelState.IsValid)
            {
                if (student.Id == 0)
                {
                    await _context.AddAsync(student);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Student Added Successfully";
                }
                else
                {
                    Student new_student = await _context.Set<Student>().FindAsync(student.Id);
                    new_student.Name = student.Name;
                    new_student.Address = student.Address;
                    new_student.PhoneNumber = student.PhoneNumber;
                    _context.Update(new_student);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Student Edited Successfully";


                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Message"] = "Please input data again";
                return RedirectToAction(nameof(AddEdit));
            }
        }


    }
}
