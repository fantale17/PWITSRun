using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Lib;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ITSSelfRunning.Services
{
    /// <summary>
    /// Class to communicate with the SharedRuns APIs
    /// </summary>
    public class SharedRunService : ISharedRunService
    {
        private readonly HttpClient client;
        private readonly string _sharedRunBaseUrl;

        public SharedRunService(string sharedRunBaseUrl)
        {
            client = new HttpClient();
            _sharedRunBaseUrl = sharedRunBaseUrl;
        }


        /// <summary>
        /// Returns the list of all races in SharedRuns
        /// </summary>
        /// <returns>A list of all races</returns>
        public async Task<IEnumerable<Race>> GetAllRaces()
        {
            HttpResponseMessage response = await client.GetAsync(_sharedRunBaseUrl + "List");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Race>>(json);
        }



        /// <summary>
        /// View a specific race details by uriGara
        /// </summary>
        /// <param name="uriGara"></param>
        /// <returns>The full race with all details</returns>
        public async Task<Race> GetRaceDetails(string uriGara)
        {
            HttpResponseMessage response = await client.GetAsync(_sharedRunBaseUrl  + uriGara);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Race>(json);
        }


        /// <summary>
        /// Sends the request to register to a race current Runner in session
        /// </summary>
        /// <param name="uriGara"> the identifier of the race</param>
        /// <param name="runnerId"> the identifier of the runner</param>
        /// <returns>Response statusCode</returns>
        public async Task<HttpStatusCode> JoinRace(string uriGara, int runnerId)
        {
            var json = JsonConvert.SerializeObject(runnerId);
            var response = await client.PostAsync(_sharedRunBaseUrl + "join/" + uriGara, new StringContent(json, Encoding.UTF8, "application/json"));
            return response.StatusCode;
        }

        /// <summary>
        /// When an User registers on the SelfRunning it registers it's Runner's data on the SharedRun
        /// </summary>
        /// <param name="runner"></param>
        /// <returns>response status code</returns>
        public async Task<HttpStatusCode> RegisterRunner(Runner runner)
        {
            var json = JsonConvert.SerializeObject(runner);
            var response = await client.PostAsync(_sharedRunBaseUrl + "subRunner", new StringContent(json, Encoding.UTF8, "application/json"));
            return response.StatusCode;
        }

    }
}
