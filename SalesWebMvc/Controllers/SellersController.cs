using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;

        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }
        public IActionResult Create()
        {
            return View();
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
