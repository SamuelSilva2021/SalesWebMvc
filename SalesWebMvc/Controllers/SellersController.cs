using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;

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
       
    }
}
