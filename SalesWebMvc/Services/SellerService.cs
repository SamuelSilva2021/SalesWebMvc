using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

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
        //Método para inserir um novo vendedor
        public void Insert(Seller obj)
        {
            //Adiciona o obj(vendedor/seller)
            _context.Add(obj);
            //Para confirmar usar o método SaveChanges
            _context.SaveChanges();
        }
        //Método para buscar um vendedor pelo Id
        public Seller FindById(int id)
        {
            //Carregar o departamento e o vendedor pelo Id
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);

        }
        //Método para deletar um vendedor
        public void Remove(int id)
        {
            var obj =_context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }
        public void Update(Seller obj)
        {
            //Testar se o Id do obj já existe no banco
            //Se não existir (!) então lançar uma exception
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id não existe");
            }
            try 
            { 
            _context.Update(obj);
            _context.SaveChanges();
            }
            //Interceptar uma exceção de acesso a dados
            catch(DbConcurrencyException e)
            {
                //Relaçar a excessão em nível de serviço
                throw new DbConcurrencyException(e.Message);
            }
        }

    }
}
