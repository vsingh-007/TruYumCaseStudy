using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using truYum.Models;

namespace truYum.Controllers
{
    public class MenuItemsController : Controller
    {
        // GET: MenuItems

        readonly TruYumContext db = new TruYumContext();

        [HttpGet]
        [Authorize]
        public ActionResult Index(bool? isAdmin=false)
        {
            if(User.Identity.IsAuthenticated)
            {
                if (isAdmin == null)
                {
                    ViewBag.isAdmin = false;
                }
                else
                {
                    ViewBag.isAdmin = true;
                }
            }
            return View(db.MenuItems.ToList()); 
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Category = new SelectList(db.Categories.Select(x=>x.Name).ToList(), "Main Course");
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include ="Name,Price,Active,DateOfLaunch,FreeDelivery")] MenuItem menuItem)
        {
            if(menuItem!=null)
            {
                ViewBag.Category = db.Categories.Select(x => x.Name).ToList();
                if (ModelState.IsValid)
                {
                    //TruYumContext db = new TruYumContext();
                    db.MenuItems.Add(menuItem);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new NullReferenceException("Some Error Occured !!");
                }
            }
            else
            {
                throw new NullReferenceException("Some Error Occured !!");
            }
            
        }

        [HttpGet]
        public ActionResult Edit(int? ID)
        {
            ViewBag.Category = new SelectList(db.Categories.Select(x => x.Name).ToList(), "Main Course");
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MenuItem menuItem = db.MenuItems.Find(ID);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Name,Price,Active,DateOfLaunch,FreeDelivery")] MenuItem menuItem)
        {
            if (menuItem != null)
            {
                ViewBag.Category = db.Categories.Select(x => x.Name).ToList();
                if (ModelState.IsValid)
                {
                    //TruYumContext db = new TruYumContext();
                    db.Entry(menuItem).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new NullReferenceException("Some Error Occured !!");
                }
            }
            else
            {
                throw new NullReferenceException("Some Error Occured !!");
            }

        }
        //public ActionResult Edit([Bind(Include = "Name,FreeDelivery,Price,DateOfLaunch,Active")] MenuItem m)
        //{
        //    ViewBag.Category = db.Categories.Select(x => x.Name).ToList();

        //    if (ModelState.IsValid)
        //    {
        //        //menuItem.Categories = null;
        //        if(m!=null)
        //        {
        //            db.Entry(m).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            throw new Exception("Database not Updated...!");
        //        }

        //    }
        //    return View(m);
        //}

        [HttpGet]
        public ActionResult Delete(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(ID);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            db.Entry(menuItem).State = EntityState.Deleted;
            return RedirectToAction("Index","MenuItems");
        }

        [HttpGet]
        public ActionResult Cart(int? id)
        {
            if (id != null)
            {
                MenuItem menuItem = db.MenuItems.Find(id);
                Cart cart = new Cart()
                {
                    MenuItems = menuItem
                };
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("ViewCart");
            }
            else
            {
                return View("Cart");
            }
        }

        public class CartList
        {
            public int CartId { get; set; }
            public string Name { get; set; }
            public bool FreeDelivery { get; set; }
            public double Price { get; set; }
        }

        public ActionResult ViewCart()
        {
            var list = (from cart in db.Carts
                        join menu in db.MenuItems on cart.MenuItems.ID equals menu.ID
                        orderby cart.Id
                        select new CartList()
                        {
                            CartId = cart.Id,
                            Name = menu.Name,
                            FreeDelivery = menu.FreeDelivery,
                            Price = menu.Price,
                        }).ToList();

            if(list.Count<=0)
            {
                ViewBag.Empty = true;
            }
            else
            {
                ViewBag.Empty = false;
            }
            return View(list);
        }

    }
}