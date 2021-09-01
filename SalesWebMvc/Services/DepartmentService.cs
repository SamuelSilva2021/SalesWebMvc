using SalesWebMvc.Models;
using System.Linq;
using System.Collections.Generic;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMvcContext _context;

        public DepartmentService (SalesWebMvcContext context)
        {
            _context = context;
        }
        //Método para listar todos os departamentos
        public List<Department> FindAll()
        {
            //Usando o método OrderBy para chamar a lista por ordem de Nome
            return _context.Department.OrderBy(x => x.Name).ToList();
        }
    }
}
