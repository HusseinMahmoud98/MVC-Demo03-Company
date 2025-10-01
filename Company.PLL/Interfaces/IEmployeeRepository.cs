using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employees>
    {
        //IEnumerable<Employees> GetAll();
        //Employees GetById(int id);
        //int Add(Employees model);
        //int Update(Employees model);
        //int Delete(Employees model);
    }
}
