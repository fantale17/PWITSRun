using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibSharedRun.Repos
{
    public interface IActivityRepo
    {
        Task RegisterRacePartecipant(string uriGara, int runnerId);
        Task<Activity> GetRaceDetailsAsync(string uriGara);
        Task<IEnumerable<Activity>> GetAllRacesAsync();
        Task InsertAsync(Activity act);
    }
}
