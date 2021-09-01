using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;
using System.Collections.Generic;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        //Criar depedencias
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        //Construtor com as depedencias
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        //Método que abre pra cadastrar um vendedor
        public IActionResult Create()
        {
            //Carregar os departamentos
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        //Anotation do tipo de requisição
        [HttpPost]
        //Validação para previnir contra ataque CSRF
        [ValidateAntiForgeryToken]
        //Criar um POST
        public IActionResult Create(Seller seller)
        {
            //chama o método _sellerService.Insert
            _sellerService.Insert(seller);
            //Redirecionar a ação para pagina Index
            return RedirectToAction(nameof(Index));
        }
        //Criar a confirmação para o Delete (?)-> quer dizer opcional, só para não dar erro. Mais o id é obrigatório
        //Esse método trás a tela de confirmação para o delete, porém ainda não deleta. Abaixo está implementado o delete com o POST
        public IActionResult Delete(int? id)
        {
            //Veririficar se o Id é null
            if (id == null)
            {
                return NotFound();
            }
            //Testar se o id existe no banco de dados e armazenar em obj
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            //Retornar o obj
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if(id != seller.Id)
            {
                return BadRequest();
            }
            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }catch(NotFoundException)
            {
                return NotFound();
            }
            catch (DbConcurrencyException)
            {
                return BadRequest();
            }
            
        }
    }
}
