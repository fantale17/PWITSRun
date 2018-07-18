using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lib;

namespace ITSSelfRunning.Services
{
    public interface ISharedRunService
    {
        Task<IEnumerable<Race>> GetAllRaces();
        Task<Race> GetRaceDetails(string uriGara);
        Task<HttpStatusCode> JoinRace(string uriGara, int runnerId);
        Task<HttpStatusCode> RegisterRunner(Runner runner);
    }
}
