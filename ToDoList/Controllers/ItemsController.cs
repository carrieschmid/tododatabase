using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BasicAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Controllers {

    [Authorize]
    public class ItemsController : Controller {
        private readonly ToDoListContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemsController (UserManager<ApplicationUser> userManager, ToDoListContext db) {
            _userManager = userManager;
            _db = db;
        }

        public async Task<ActionResult> Index () 
        {
            var userId = this.User.FindFirst (ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync (userId);
            var userItems = _db.Items.Where (entry => entry.User.Id == currentUser.Id);
            return View (userItems);
        }

        public ActionResult Create () {
            ViewBag.CategoryId = new SelectList (_db.Categories, "CategoryId", "Name");
            return View ();
        }

        [HttpPost]
        public ActionResult Create (Item item, int CategoryId) {
            // item.Date=DateTime.Now;--this would set every entry to current date
            _db.Items.Add (item);
            if (CategoryId != 0) {
                _db.CategoryItem.Add (new CategoryItem () { CategoryId = CategoryId, ItemId = item.ItemId });
            }
            _db.SaveChanges ();
            return RedirectToAction ("Index");
        }

        public ActionResult Details (int id) {

            var thisItem = _db.Items
                .Include (item => item.Categories)
                //Loads Categories to each Item, a collection of join entities
                .ThenInclude (join => join.Category)
                //Loads the Category objects of each CategoryItem, which includes id of Category and Item
                .FirstOrDefault (item => item.ItemId == id);
            //Finds the item from the database you're working with
            // var dateAndTime = thisItem.Date.Now;
            // var date = dateAndTime.Date;

            return View (thisItem);
        }
        public ActionResult Edit (int id) {
            var thisItem = _db.Items.FirstOrDefault (items => items.ItemId == id);
            ViewBag.CategoryId = new SelectList (_db.Categories, "CategoryId", "Name");
            return View (thisItem);
        }

        [HttpPost]
        public ActionResult Edit (Item item, int CategoryId) {
            if (CategoryId != 0) {
                _db.CategoryItem.Add (new CategoryItem () { CategoryId = CategoryId, ItemId = item.ItemId });
            }
            _db.Entry (item).State = EntityState.Modified;
            _db.SaveChanges ();
            return RedirectToAction ("Index");
        }

        public ActionResult AddCategory (int id) {
            var thisItem = _db.Items.FirstOrDefault (items => items.ItemId == id);
            ViewBag.CategoryId = new SelectList (_db.Categories, "CategoryId", "Name");
            return View (thisItem);
        }

        [HttpPost]
        public ActionResult AddCategory (Item item, int CategoryId) {
            if (CategoryId != 0) {
                _db.CategoryItem.Add (new CategoryItem () { CategoryId = CategoryId, ItemId = item.ItemId });
            }
            _db.SaveChanges ();
            return RedirectToAction ("Index");
        }
        public ActionResult Delete (int id) {
            var thisItem = _db.Items.FirstOrDefault (items => items.ItemId == id);
            return View (thisItem);
        }

        [HttpPost, ActionName ("Delete")]
        public ActionResult DeleteConfirmed (int id) {
            var thisItem = _db.Items.FirstOrDefault (items => items.ItemId == id);
            _db.Items.Remove (thisItem);
            _db.SaveChanges ();
            return RedirectToAction ("Index");
        }

        [HttpPost]
        public ActionResult DeleteCategory (int joinId) {
            var joinEntry = _db.CategoryItem.FirstOrDefault (entry => entry.CategoryItemId == joinId);
            //We use the name joinId id becuase .NET automatically utilizes the value in the URL query in the Startup.cs if we name the variable id. For example, if we named the parameter id instead of joinId and the details URL was something like /Items/Details/6, then the value of id would be 6, which is the ItemId and not the CategoryItemId that we wanted from our Hidden() method
            _db.CategoryItem.Remove (joinEntry);
            _db.SaveChanges ();
            return RedirectToAction ("Index");
        }

        [HttpGet, ActionName ("ClosestDate")]
        public ActionResult ClosestDate () {
            List<Item> model = _db.Items.OrderBy (item => item.Date).ToList ();
            Console.WriteLine (model);
            return View ("Index", model);
        }

    }
}