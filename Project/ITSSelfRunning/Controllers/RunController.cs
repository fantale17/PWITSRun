using Dapper;
using ITSSelfRunning.Models;
using ITSSelfRunning.Models.Run;
using ITSSelfRunning.Models.RunViewModels;
using Lib.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ITSSelfRunning.Services;
using Lib;
using Lib.Repos.Interfaces;
using Activity = Lib.Activity;
using Point = Lib.Point;

namespace ITSSelfRunning.Controllers
{
    public class RunController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IActivityRepo _activityRepo;
        private readonly IEventHubService _eventHubService;
        private readonly IPointRepo _pointRepo;
        private readonly IRunnerRepo _runnerRepo;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly ISharedRunService _sharedRunService;




        public RunController(UserManager<ApplicationUser> userManager,
                                IActivityRepo activityRepo, IEventHubService eventHubService,
                                IPointRepo pointRepo, IRunnerRepo runnerRepo, ICloudStorageService cloudStorageService,
                                ISharedRunService sharedRunService)
        {
            _userManager = userManager;
            _activityRepo = activityRepo;
            _eventHubService = eventHubService;
            _pointRepo = pointRepo;
            _runnerRepo = runnerRepo;
            _cloudStorageService = cloudStorageService;
            _sharedRunService = sharedRunService;
        }




        /// <summary>
        /// View list with all runner's activities
        /// </summary>
        /// <returns>the view list</returns>
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var runner = await _runnerRepo.GetUserAsync(_userManager.GetUserId(User));
            // ViewData["ProfilePhotoUri"] = runner.PhotoUri;
            IEnumerable<Activity> list = await _activityRepo.GetTrainingsAsync(runner.Id);
            return View(list);
        }



        /// <summary>
        /// Starts the training
        /// </summary>
        /// <param name="activityId">the id of the activity to start</param>
        /// <param name="isToOpen">checks if the activity is not already open</param>
        /// <returns>the view with the running activity</returns>
        [Authorize]
        public async Task<IActionResult> RunTraining(int activityId, int activityType, string uriGara, bool isToOpen)
        {
            var runner = await _runnerRepo.GetUserAsync(_userManager.GetUserId(User));
            if (isToOpen)
            {
                await _activityRepo.OpenTrainingAsync(activityId);
            }

            return View(new ActivityRunner
            {
                IdRunner = runner.Id,
                IdActivity = activityId,
                Type = activityType,
                UriGara = uriGara
            });
        }


        /// <summary>
        /// Returns the view with the data of a closed training
        /// </summary>
        /// <param name="act">the closed activity to analyze</param>
        /// <returns>Returns the view with the data of a closed training</returns>
        [Authorize]
        public async Task<IActionResult> ViewTrainingData(Activity act)
        {
            IEnumerable<Point> list = await _pointRepo.GetActivityPointsAsync(act.Id);
            ViewData["Telemetry"] = list;
            ViewData["Activity"] = act;
            list.AsList().RemoveAt(0);
            return View();
        }

        /// <summary>
        /// Close a training
        /// </summary>
        /// <param name="activityId">the id of the activity to close</param>
        /// <returns>redirect to Index View</returns>
        [Authorize]
        public async Task<IActionResult> EndTraining(int activityId)
        {
            await _activityRepo.CloseTrainingAsync(activityId);
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Returns view to create a training
        /// </summary>
        /// <returns> Returns view to create a training</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateTraining()
        {
            return View();
        }




        /// <summary>
        /// Create a training
        /// </summary>
        /// <param name="activity">User submitted parameters</param>
        /// <returns>Index view</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTraining(ActivityViewModel activity)
        {
            var runner = await _runnerRepo.GetUserAsync(_userManager.GetUserId(User));
            Activity obj = new Activity
            {
                IdRunner = runner.Id,
                Name = activity.Name,
                Place = activity.Place,
                Type = 1, //1= training, 2 = gara
            };
            await _activityRepo.InsertAsync(obj);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Sends the telemetry point to eventHub
        /// </summary>
        /// <param name="point">the given point from the client</param>
        /// <returns>Status code 200</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendPoint([FromBody] Point point)
        {

            var msg = JsonConvert.SerializeObject(new
            {
                Type = "Point",
                Msg = JsonConvert.SerializeObject(point)
            });
            await _eventHubService.SendMsg(msg);
            return Json(new { result = "ok" });
        }

        /// <summary>
        /// Returns view summary of activity to start
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>View with the activity to start</returns>
        [Authorize]
        public IActionResult StartTraining(Activity activity)
        {
            return View(activity);
        }

        /// <summary>
        /// Uploads to Azure Blob a given photo as b64 string and sends uri and time taken to eventHub
        /// </summary>
        /// <param name="photo">string Photo = b64 img, DateTime Time = time taken</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task UploadSelfie([FromBody] SelfieTime photo)
        {
            var blobUri = await _cloudStorageService.UploadPhotoFromB64(photo.Photo);
            var msg = JsonConvert.SerializeObject(new
            {
                Type = "Selfie",
                Msg = JsonConvert.SerializeObject(new
                {
                    SelfieUri = blobUri,
                    Time = photo.Time,
                    IdActivity = photo.IdActivity
                })
            });
            await _eventHubService.SendMsg(msg);

        }







        /// <summary>
        /// Returns the view of all the races in SharedRuns WebApp
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetRaceList()
        {
            return View(await _sharedRunService.GetAllRaces());
        }


        /// <summary>
        /// View of the details of a race
        /// </summary>
        /// <param name="uriGara">identifier of the race</param>
        /// <returns></returns>
        public async Task<IActionResult> RaceDetails(string uriGara)
        {
            return View(await _sharedRunService.GetRaceDetails(uriGara));
        }

        /// <summary>
        /// Registers the current user to a race on SharedRuns and creates the current istance-activity
        /// </summary>
        /// <param name="race">the race to join</param>
        /// <returns></returns>
        public async Task<IActionResult> JoinRace(Race race)
        {
            var runner = await _runnerRepo.GetUserAsync(_userManager.GetUserId(User));
            await _sharedRunService.JoinRace(race.UriGara, runner.Id);

            Activity personalRace = new Activity
            {
                IdRunner = runner.Id,
                Name = race.Name,
                Place = race.Place,
                UriGara = race.UriGara,
                Type = 2,
                CreationDate = DateTime.Now,
                Status = 0
            };

            await _activityRepo.InsertAsync(personalRace);

            return RedirectToAction("Index");
        }

    }
}