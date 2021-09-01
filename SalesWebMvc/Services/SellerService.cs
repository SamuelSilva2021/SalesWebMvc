using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        //Método para listar vendedores no banco de dados
        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }
        //Método para inserir um novo vendedor no banco de dados
        public void Insert(Seller obj)
        {
            //Adiciona o obj(vendedor/seller)
            _context.Add(obj);
            //Para confirmar usar o método SaveChanges
            _context.SaveChanges();
        }

    }
}
