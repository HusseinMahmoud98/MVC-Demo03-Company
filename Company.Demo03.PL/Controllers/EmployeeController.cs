using AutoMapper;
using Company.BLL;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.Demo03.PL.Dtos;
using Company.Demo03.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Company.Demo03.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;

        private readonly IMapper _mapper;

        public IUnitOfWork _unitOfWork { get; }

        public EmployeeController(
               //IEmployeeRepository employeeRepository,
               //IDepartmentRepository departmentRepository,
               IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;

            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            // Dictionary : 3 Properties
            // 1.ViewData : Transfer extra information from controller(Action) to View
            //ViewData["Message"] = "Hello";

            // 2. ViewBag : Transfer extra information from controller(Action) to View


            IEnumerable<Employees> employees;

            if (string.IsNullOrEmpty(SearchInput))
            { 
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }

            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }

                return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departemnt = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["Department"] = departemnt;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DtoEmployee model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                }
                                 
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

                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                var count = await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    TempData["Message"] = "Employee is created";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) 
                return BadRequest("Invalid Id");

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);

            if (employee is null) 
                return NotFound(new { statusCode = 404, message = $"Empolyee with Id :{id} is not found" });
            
            var dtoEmployee = _mapper.Map<DtoEmployee>(employee);

            return View(viewName , dtoEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var departemnt = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["Department"] = departemnt;

            if (id is null)
                return BadRequest("Invalid Id");

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);

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
        public async Task<IActionResult> Edit([FromRoute] int id, DtoEmployee model)
        {
            if (ModelState.IsValid)
            {
                if(model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }

                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }
                 
                //if (id != model.Id)
                //    return BadRequest();

                //var employee = new Employees()
                //{
                //    Id = id,
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

                var employee = _mapper.Map<Employees>(model);
                employee.Id = id;


                _unitOfWork.EmployeeRepository.Update(employee);
                var count = await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, nameof(Delete));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, DtoEmployee model)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employees>(model);
                employee.Id = id;

                _unitOfWork.EmployeeRepository.Delete(employee);

                var count = await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    if(employee.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(employee.ImageName, "images");
                    }

                    return RedirectToAction(nameof(Index));
                }
                
            }

            return View(model);
        }

        
    }
}