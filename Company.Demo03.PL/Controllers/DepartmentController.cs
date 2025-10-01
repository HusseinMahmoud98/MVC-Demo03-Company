using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.Demo03.PL.Dtos;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace Company.Demo03.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        //Ask CLR To Create Object From DepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet] // GET://Department/Index 
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                var department = new Department()
                { 
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };

                var count = _departmentRepository.Add(department);

                if (count>0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        //[HttpGet]
        //public IActionResult Details(int? id )
        //{
        //    if(id is null)
        //    {
        //        return BadRequest("Invalid Id"); //Status Code: 400
        //    }

        //    var department = _departmentRepository.GetById(id.Value);

        //    if (department is null)
        //        return NotFound(new { statusCode = 404, message = $"Department with Id :{id} is not found" });

        //    return View(department);
        //}

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var department = _departmentRepository.GetById(id.Value);

            if(department is null)
            {
                return NotFound(new { statusCode = 404, message = $"Department with Id :{id} is not found" });
            }

            return View(viewName, department); 
        }



        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest("Invalid Id"); //Status Code: 400
            }

            var department = _departmentRepository.GetById(id.Value);

            if (department is null)
            {
                return NotFound(new { satusCode = 404, message = $"Department with Id :{id} is not found" });
            }

            var departmentDto = new CreateDepartmentDto()
            {
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt
            };


            return View(departmentDto);

            //return Details(id, nameof(Edit));
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Prevent any external tool from sending a request or call endpoint
        public IActionResult Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != model.Id)
                //    return BadRequest();

                var department = new Department()
                {
                    Id = id,
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                
                    var count = _departmentRepository.Update(department);

                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    } 
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id"); //400

            //var department = _departmentRepository.GetById(id.Value);

            //if (department is null) return NotFound(new { StatusCode = 404, message = $"Department with Id :{id} is not found" });

            //return View(department);

            return Details(id, nameof(Delete));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid) {
                var department = new Department()
                {
                    Id = id,
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };

                var count = _departmentRepository.Delete(department);

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);            
        }
         
    }
}
