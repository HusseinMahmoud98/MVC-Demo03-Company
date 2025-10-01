using Company.BLL.Interfaces;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T? GetById(int id)
        {
            return _context.Set<T>()
                .FirstOrDefault(e => e.Id == id);
        }

        public int Add(T model)
        {
            _context.Add(model);
            return _context.SaveChanges();
        }

        public int Update(T model)
        {
            _context.Update(model);
            return _context.SaveChanges();
        }

        public int Delete(T model)
        {
            _context.Remove(model);
            return _context.SaveChanges();
        }    
    }
}
