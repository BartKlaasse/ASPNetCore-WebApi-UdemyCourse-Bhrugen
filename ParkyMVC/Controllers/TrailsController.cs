using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyMVC.Models;
using ParkyMVC.Models.ViewModels;
using ParkyMVC.Repository.IRepository;
using ParkyMVC.Utility;

namespace ParkyMVC.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;

        public TrailsController(INationalParkRepository npRepo, ITrailRepository trailRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(Static.NationalParkAPIPath);
            TrailsViewModel objVM = new TrailsViewModel()
            {
                NationalParkList = npList.Select(i => new SelectListItem
                {
                Text = i.Name,
                Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                //FLOW: True for create
                return View(obj);
            }

            //FLOW: Update method from here
            objVM.Trail = await _trailRepo.GetAsync(Static.TrailAPIPath, id.GetValueOrDefault());
            if (objVM.Trail == null)
            {
                return NotFound();
            }
            else
            {
                return View(objVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsViewModel obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Trail.Id == 0)
                {
                    //FLOW: Create
                    await _trailRepo.CreateAsync(Static.TrailAPIPath, obj.Trail);
                }
                else
                {
                    await _trailRepo.UpdateAsync(Static.TrailAPIPath + obj.Trail.Id, obj.Trail);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepo.GetAllAsync(Static.TrailAPIPath) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(Static.TrailAPIPath, id);
            if (status)
            {
                return Json(new { succes = true, message = "Delete succesfull" });
            }
            return Json(new { succes = true, message = "Delete not succesfull" });
        }
    }
}