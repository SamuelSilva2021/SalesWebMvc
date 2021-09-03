using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;
using System.Collections.Generic;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        //Método que abre pra cadastrar um vendedor
        public async Task <IActionResult> Create()
        {
            //Carregar os departamentos
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        //Anotation do tipo de requisição
        [HttpPost]
        //Validação para previnir contra ataque CSRF
        [ValidateAntiForgeryToken]
        //Criar um POST
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            //chama o método _sellerService.Insert
            await _sellerService.InsertAsync(seller);
            //Redirecionar a ação para pagina Index
            return RedirectToAction(nameof(Index));
        }
        //Criar a confirmação para o Delete (?)-> quer dizer opcional, só para não dar erro. Mais o id é obrigatório
        //Esse método trás a tela de confirmação para o delete, porém ainda não deleta. Abaixo está implementado o delete com o POST
        public async Task <IActionResult> Delete(int? id)
        {
            //Veririficar se o Id é null
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provaided"});
            }
            //Testar se o id existe no banco de dados e armazenar em obj
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            //Retornar o obj
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {

                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            
        }
        public async Task <IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }
        public async Task<IActionResult> Edit(int? id)
        {

            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj =await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            //Comparar se o id do departamento é o mesmo id departamento que contém no vendedor selecionado
            //Se for diferente retorna um BadRequest
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
            try
            {
               await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }catch(ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            

        }
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                //pegar o Id interno da requisição
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
