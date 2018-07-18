using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LibSharedRun;
using LibSharedRun.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharedRuns.Controllers
{
    /// <summary>
    /// API to supply race's data and more
    /// </summary>
    [Produces("application/json")]
    [Route("api/SharedRace")]
    public class SharedRaceController : Controller
    {

        private readonly IActivityRepo _activityRepo;

        private readonly IRunnerRepo _runnerRepo;

        public SharedRaceController(IActivityRepo activityRepo, IRunnerRepo runnerRepo)
        {
            _activityRepo = activityRepo;
            _runnerRepo = runnerRepo;
        }

        /// <summary>
        /// Returns the json with all the races
        /// </summary>
        /// <returns></returns>
        [Route("List")]
        [HttpGet]
        public async Task<IEnumerable<Activity>> GetRaceList()
        {       
            return await _activityRepo.GetAllRacesAsync();
        }

        /// <summary>
        /// Returns the json with a race in details
        /// </summary>
        /// <param name="uriGara"></param>
        /// <returns></returns>
        [Route("{uriGara}")]
        [HttpGet]
        public async Task<Activity> GetRaceDetails([FromRoute] string uriGara)
        {
            return await _activityRepo.GetRaceDetailsAsync(uriGara);
        }

        /// <summary>
        /// API Method to call to register to a race
        /// </summary>
        /// <param name="uriGara"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Route("join/{uriGara}")]
        [HttpPost]
        public async Task<IActionResult> JoinRace([FromRoute] string uriGara, [FromBody]string msg)
        {
            try
            {
                  int idRunner = JsonConvert.DeserializeObject<int>(msg);
                  await _activityRepo.RegisterRacePartecipant(uriGara, idRunner);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }

            return Ok();

        }

        /// <summary>
        /// API call which permits to ITSSelfRunning to send its newly created Runner to SharedRuns for also registration on its DB.
        /// It's called on user registration on ITSSelfRunning
        /// </summary>
        /// <param name="runner">The newly created Runner in ITSSelfRunning</param>
        /// <returns></returns>
        [Route("subRunner")]
        [HttpPost]
        public async Task<IActionResult> SubscribeRunner([FromBody]Runner runner)
        {
            try
            {
                await _runnerRepo.InsertRunner(runner);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok();
        }


        
    }
}