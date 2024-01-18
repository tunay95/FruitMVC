using FruitMVC.Areas.Admin.ViewModel;
using FruitMVC.DAL;
using FruitMVC.Helpers;
using FruitMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

namespace FruitMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class FruitController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public FruitController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Fruit>fruists = await _dbContext.Fruits.ToListAsync();
            return View(fruists);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateFruitVM createFruitVM)
        {
            if(!ModelState.IsValid)
            {
                return View(createFruitVM);
            }

            if (!createFruitVM.Image.CheckContent("image/"))
            {
                ModelState.AddModelError("Image", "Enter the correct format");
                return View();
            }
            if (!createFruitVM.Image.CheckLength(3000000))
            {
                ModelState.AddModelError("Image", "you can/t upload more than 2 mb ");
                return View();
            }

            Fruit fruits = new Fruit()
            {
                Title=createFruitVM.Title,
                Category=createFruitVM.Category,
                ImgUrl=createFruitVM.Image.Upload(_env.WebRootPath,@"/Upload/") 
            };
            await _dbContext.Fruits.AddAsync(fruits);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Update(int id)
        {
            Fruit fruit = await _dbContext.Fruits.FindAsync(id);
            UpdateFruitVM updateFruitVM = new UpdateFruitVM()
            {
                Title = fruit.Title,
                Category = fruit.Category,
                ImgUrl = fruit.ImgUrl
            };
            return View(updateFruitVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateFruitVM updateFruitVM)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            if(updateFruitVM.Id<=0) throw new Exception("Id can't be less than zero");
            var fruit = _dbContext.Fruits.FirstOrDefault<Fruit>(x=>x.Id==updateFruitVM.Id);
            if (fruit == null) throw new Exception("Fruit can't be null");


            fruit.Title = updateFruitVM.Title;
            fruit.Category = updateFruitVM.Category;
            fruit.ImgUrl = updateFruitVM.Image.Upload(_env.WebRootPath,@"/Upload/");
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Fruit fruit =await _dbContext.Fruits.FindAsync(id);
            _dbContext.Fruits.Remove(fruit);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
