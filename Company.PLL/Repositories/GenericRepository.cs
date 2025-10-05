using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly CompanyDBContext _context;

        public GenericRepository(CompanyDBContext context)
        {
            _context = context;
        }
        public IEnumerable<TEntity> GetAll()
        {
            //will be refactoried using a design pattern
            if(typeof(TEntity) == typeof(Employees))
            {
                return (IEnumerable<TEntity>)_context.Employees.Include(E => E.Department).ToList();
            }

            return _context.Set<TEntity>().ToList();
        }

        public TEntity? GetById(int id)
        {
            //will be refactoried using a design pattern
            if (typeof(TEntity) == typeof(Employees))
            {
                return _context.Employees.Include(E => E.Department).FirstOrDefault(e => e.Id == id) as TEntity;
            }

            return _context.Set<TEntity>()
                .FirstOrDefault(e => e.Id == id);
        }

        public int Add(TEntity model)
        {
            _context.Add(model);
            return _context.SaveChanges();
        }

        public int Update(TEntity model)
        {
            _context.Update(model);
            return _context.SaveChanges();
        }

        public int Delete(TEntity model)
        {
            _context.Remove(model);
            return _context.SaveChanges();
        }    
    }
}
