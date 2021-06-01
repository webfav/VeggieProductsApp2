using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeggieProductsApp2.Data;
using VeggieProductsApp2.Models.ViewModels;
using VeggieProductsApp2.Models;
using VeggieProductsApp2.ManagedUsers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace VeggieProductsApp2.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]
        public ProductVM ProductVM { get; set; }

        public ProductController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;

            ProductVM = new ProductVM()
            {
                Categories = _context.Category,
                Product = new Models.Product()
            };

        }

        public class AuthorizeAdminOrEndUser : AuthorizeAttribute
        {
            public AuthorizeAdminOrEndUser()
            {
                //taking both roles to authorize
                Roles = Users.ManagerUser + "," + Users.EndUser;
            }
        }

        [AllowAnonymous]
        // GET: ProductController
        public async Task<IActionResult> Index(string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var productsContext = from s in _context.Product.Include(p => p.Category)
                                  select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                productsContext = productsContext.Where(s => s.ProductName.Contains(searchString)
                                       || s.Category.CategoryName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    productsContext = productsContext.OrderByDescending(s => s.ProductName);
                    break;
                case "Brand":
                    productsContext = productsContext.OrderBy(s => s.Category.CategoryName);
                    break;
                default:
                    productsContext = productsContext.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Product>.CreateAsync(productsContext.Include(p => p.Category).AsNoTracking(), pageNumber ?? 1, pageSize));


            //var productItems = await _context.Product.Include(m => m.Category).ToListAsync();
            //return View(productItems);
        }


        // GET: ProductController/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            ProductVM.Product = await _context.Product.Include(m => m.Category).SingleOrDefaultAsync(m => m.Id == id);

            if (ProductVM.Product == null)
            {
                return NotFound();
            }

            return View(ProductVM);
        }


        [AuthorizeAdminOrEndUser]
        // GET: ProductController/Create
        public ActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", ProductVM.Product.CategoryId);
            return View(ProductVM);
        }


        // POST: ProductController/Create
        [AuthorizeAdminOrEndUser]
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", ProductVM.Product.CategoryId);

            try
            {
                if (ModelState.IsValid)
                {                   
                    _context.Product.Add(ProductVM.Product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index)); 
                }
              
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Kunne ikke gemme. " +
                    "Prøv igen, " + "Ellers kontakt din administrator.");                
            }
            return View(ProductVM);
        }

        [AuthorizeAdminOrEndUser]
        // GET: ProductController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", ProductVM.Product.CategoryId);

            ProductVM.Product = await _context.Product.Include(m => m.Category).SingleOrDefaultAsync(m => m.Id == id);

            if (ProductVM.Product == null)
            {
                return NotFound();
            }
            return View(ProductVM);
        }


        //TODO

        // POST: ProductController/Edit/5
        [AuthorizeAdminOrEndUser]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(ProductVM);
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", ProductVM.Product.CategoryId);

            var productFromDb = await _context.Product.FindAsync(ProductVM.Product.Id);
            productFromDb.ProductName = ProductVM.Product.ProductName;
            productFromDb.Description = ProductVM.Product.Description;
            productFromDb.BrandName = ProductVM.Product.BrandName;
            productFromDb.Price = ProductVM.Product.Price;
            productFromDb.Date = ProductVM.Product.Date;
            productFromDb.Date = ProductVM.Product.Date;
            productFromDb.ShopName = ProductVM.Product.ShopName;
            productFromDb.Adress = ProductVM.Product.Adress;
            productFromDb.Zipcode = ProductVM.Product.Zipcode;
            productFromDb.City = ProductVM.Product.City;
            productFromDb.CategoryId = ProductVM.Product.CategoryId;
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        // GET: ProductController/Delete/5
        [Authorize(Roles = Users.ManagerUser)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductVM.Product = await _context.Product.Include(m => m.Category).SingleOrDefaultAsync(m => m.Id == id);

            if (ProductVM.Product == null)
            {
                return NotFound();
            }

            return View(ProductVM);
        }

        // POST: ProductController/Delete/5
        [Authorize(Roles = Users.ManagerUser)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.Product.FindAsync(id);
            try
            {
                if (product != null)
                {
                    _context.Product.Remove(product);
                    await _context.SaveChangesAsync();

                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
