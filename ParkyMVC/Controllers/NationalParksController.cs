using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkyMVC.Models;
using ParkyMVC.Repository.IRepository;
using ParkyMVC.Utility;

namespace ParkyMVC.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;

        public NationalParksController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark obj = new NationalPark();
            if (id == null)
            {
                //FLOW: True for create
                return View(obj);
            }
            //FLOW: Update method from here
            obj = await _npRepo.GetAsync(Static.NationalParkAPIPath, id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                return View(obj);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    //FLOW: If file has been uploaded
                    byte[] p1 = null;
                    using(var fs1 = files[0].OpenReadStream())
                    {
                        using(var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    obj.Picture = p1;
                }
                else
                {
                    var objFromDb = await _npRepo.GetAsync(Static.NationalParkAPIPath, obj.Id);
                    obj.Picture = objFromDb.Picture;
                }
                if (obj.Id == 0)
                {
                    //FLOW: Create
                    await _npRepo.CreateAsync(Static.NationalParkAPIPath, obj);
                }
                else
                {
                    await _npRepo.UpdateAsync(Static.NationalParkAPIPath + obj.Id, obj);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { data = await _npRepo.GetAllAsync(Static.NationalParkAPIPath) });
        }
    }
}