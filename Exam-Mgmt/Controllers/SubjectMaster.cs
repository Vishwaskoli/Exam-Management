using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Mgmt.Controllers
{
    public class SubjectMaster : Controller
    {
        // GET: SubjectMaster
        public ActionResult Index()
        {
            return View();
        }

        // GET: SubjectMaster/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubjectMaster/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubjectMaster/Create
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

        // GET: SubjectMaster/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SubjectMaster/Edit/5
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

        // GET: SubjectMaster/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubjectMaster/Delete/5
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
