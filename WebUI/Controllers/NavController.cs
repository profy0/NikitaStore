using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {

        private IProductRepository productRepository;

        public NavController(IProductRepository productRepository)
        {
            this.productRepository = productRepository; 
        }

        public PartialViewResult Menu(string category = null)
        {

            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = productRepository.Products
                .Select(product => product.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}