 using Company.BLL.Interfaces;
using Company.DAL.Data.Contexts;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employees>, IEmployeeRepository
    {
        private readonly CompanyDBContext _context;

        //Ask CKR to create an object from companyDbContext
        public EmployeeRepository(CompanyDBContext context) :base(context) 
        {
            _context = context;
        }

        public async Task<List<Employees>> GetByNameAsync(string name)
        {
            return await _context.Employees.Include(e => e.Department).Where(e => e.Name.ToLower() == name.ToLower()).ToListAsync();
        }
        //private readonly CompanyDBContext _context;

        //public EmployeeRepository(CompanyDBContext context)
        //{
        //    _context = context;
        //}

        //public IEnumerable<Employees> GetAll()
        //{
        //    return _context.Employees.ToList();
        //}

        //public Employees GetById(int id)
        //{
        //   return _context.Employees.FirstOrDefault(e=>e.Id == id);
        //}

        //public int Add(Employees model)
        //{
        //   _context.Employees.Add(model);
        //    return _context.SaveChanges();
        //}

        //public int Update(Employees model)
        //{
        //    _context.Employees.Update(model);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Employees model)
        //{
        //    _context.Employees.Remove(model);
        //    return _context.SaveChanges();
        //}



    }
}
