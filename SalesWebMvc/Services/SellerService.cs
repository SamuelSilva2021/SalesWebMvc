using System;
using System.Collections.Generic;
using System.Linq;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;
using System.Threading.Tasks;

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
        public async Task <List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }
        //Método para inserir um novo vendedor
        public async Task InsertAsync(Seller obj)
        {
            //Adiciona o obj(vendedor/seller)
            _context.Add(obj);
            //Para confirmar usar o método SaveChanges
           await _context.SaveChangesAsync();
        }
        //Método para buscar um vendedor pelo Id
        public async Task <Seller> FindByIdAsync(int id)
        {
            //Carregar o departamento e o vendedor pelo Id
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);

        }
        //Método para deletar um vendedor
        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }catch(DbUpdateException e)
            {
                throw new IntegrityException("O vendedor possui vendas! Impossivel deletar");
            }
            
        }
        public async Task UpdateAsync(Seller obj)
        {
            //Testar se o Id do obj já existe no banco
            //Se não existir (!) então lançar uma exception
            bool hasAny =await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id não existe");
            }
            try 
            { 
            _context.Update(obj);
            await _context.SaveChangesAsync();
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
