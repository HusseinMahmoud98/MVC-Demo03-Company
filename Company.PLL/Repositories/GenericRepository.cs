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
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            //will be refactoried using a design pattern
            if(typeof(TEntity) == typeof(Employees))
            {
                return (IEnumerable<TEntity>) await _context.Employees.Include(E => E.Department).ToListAsync();
            }

            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            //will be refactoried using a design pattern
            if (typeof(TEntity) == typeof(Employees))
            {
                return await _context.Employees.Include(E => E.Department).FirstOrDefaultAsync(e => e.Id == id) as TEntity;
            }

            return await _context.Set<TEntity>()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(TEntity model)
        {
            await _context.AddAsync(model);
            //return _context.SaveChanges();
        }

        public void Update(TEntity model)
        {
            _context.Update(model);
            //return _context.SaveChanges();
        }

        public void Delete(TEntity model)
        {
            _context.Remove(model);
            //return _context.SaveChanges();
        }    
    }
}
