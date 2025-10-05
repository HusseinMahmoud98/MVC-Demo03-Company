using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.Demo03.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.Extensions.Options;

namespace Company.Demo03.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            // Dictionary : 3 Properties
            // 1.ViewData : Transfer extra information from controller(Action) to View
            //ViewData["Message"] = "Hello";

            // 2. ViewBag : Transfer extra information from controller(Action) to View


            IEnumerable<Employees> employees;

            if (string.IsNullOrEmpty(SearchInput))
            { 
                employees = _employeeRepository.GetAll();
            }

            else
            {
                employees = _employeeRepository.GetByName(SearchInput);
            }

                return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departemnt = _departmentRepository.GetAll();
            ViewData["Department"] = departemnt;
            return View();
        }

        [HttpPost]
        public IActionResult Create(DtoEmployee model)
        {
            if (ModelState.IsValid)
            {
                ////Manual Mapping
                //var employee = new Employees()
                //{
                //    Name = model.Name,
                //    Age = model.Age,
                //    Email = model.Email,
                //    Address = model.Address,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    HiringDate = model.HiringDate,
                //    CreateAt = model.CreateAt,
                //    DepartmentId = model.DepartmentId
                //};

                //Using mapper
                var employee = _mapper.Map<Employees>(model);

                var count = _employeeRepository.Add(employee);

                if (count > 0)
                {
                    TempData["Message"] = "Employee is created";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) 
                return BadRequest("Invalid Id");

            var employee = _employeeRepository.GetById(id.Value);

            if (employee is null) 
                return NotFound(new { statusCode = 404, message = $"Empolyee with Id :{id} is not found" });
            
            //var dtoEmployee = _mapper.Map<DtoEmployee>(employee);

            return View(viewName , employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var departemnt = _departmentRepository.GetAll();
            ViewData["Department"] = departemnt;

            if (id is null)
                return BadRequest("Invalid Id");

            var employee = _employeeRepository.GetById(id.Value);

            if (employee is null)
                return NotFound(new { statusCode = 404, message = $"Empolyee with Id :{id} is not found" });


            ////Manual Mapping
            //var employeeDto = new DtoEmployee()
            //{
            //    Name = employee.Name,
            //    Age = employee.Age,
            //    Email = employee.Email,
            //    Address = employee.Address,
            //    Phone = employee.Phone,
            //    Salary = employee.Salary,
            //    IsActive = employee.IsActive,
            //    IsDeleted = employee.IsDeleted,
            //    HiringDate = employee.HiringDate,
            //    CreateAt = employee.CreateAt,
            //    DepartmentId = employee.DepartmentId
            //};

            //mapping using AytoMapper
            var employeeDto = _mapper.Map<DtoEmployee>(employee);

            return View(employeeDto);

            //return Details(id, nameof(Edit));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, DtoEmployee model)
        {
            if (ModelState.IsValid)
            {
                //if (id != model.Id)
                //    return BadRequest();

                var employee = new Employees()
                {
                    Id = id,
                    Name = model.Name,
                    Age = model.Age,
                    Email = model.Email,
                    Address = model.Address,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    HiringDate = model.HiringDate,
                    CreateAt = model.CreateAt,
                    DepartmentId = model.DepartmentId
                };

                var count = _employeeRepository.Update(employee);

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return Details(id, nameof(Delete));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employees model)
        {
            if(id != model.Id)
                return BadRequest();

            var count = _employeeRepository.Delete(model);

            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        
    }
}