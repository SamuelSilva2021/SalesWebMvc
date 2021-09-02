using SalesWebMvc.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<Department>> FindAllAsync()
        {
            //Usando o método OrderBy para chamar a lista por ordem de Nome
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
