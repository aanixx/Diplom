using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Data
{
    public class ShafaStoreDBContext : DbContext
    {
        // GET: ShafaStoreDBContext
        public ActionResult Index()
        {
            return View();
        }

        // GET: ShafaStoreDBContext/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ShafaStoreDBContext/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShafaStoreDBContext/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShafaStoreDBContext/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShafaStoreDBContext/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShafaStoreDBContext/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShafaStoreDBContext/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
