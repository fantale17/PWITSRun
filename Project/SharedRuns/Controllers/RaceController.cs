using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibSharedRun;
using LibSharedRun.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedRuns.Models;
using SharedRuns.Models.RaceViewModels;

namespace SharedRuns.Controllers
{
    public class RaceController : Controller
    {

        private readonly IActivityRepo _activityRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public RaceController(IActivityRepo activityRepo, UserManager<ApplicationUser> userManager)
        {
            _activityRepo = activityRepo;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _activityRepo.GetAllRacesAsync());
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(RaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                Activity act = new Activity
                {
                    Name = raceVM.Name,
                    Place = raceVM.Place,
                    Type = 2, //1= training, 2 = gara
                    CreationDate = DateTime.Now,
                    IdOrganizer = _userManager.GetUserId(User),
                    UriGara = raceVM.UriGara
                };
                await _activityRepo.InsertAsync(act);
            }

            return RedirectToAction("Index");
        }



        [Authorize]
        public async Task<IActionResult> Details(string uriGara)
        {
            Activity act = await _activityRepo.GetRaceDetailsAsync(uriGara);
            RaceViewModel rvm = new RaceViewModel()
            {
                Place = act.Place,
                CreationDate = act.CreationDate,
                Name = act.Name,
                UriGara = act.UriGara,
                IdOrganizer = act.IdOrganizer
            };
            return View(rvm);
        }
    }
}