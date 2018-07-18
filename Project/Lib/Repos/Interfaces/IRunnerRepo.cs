using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Repos.Interfaces
{
    public interface IRunnerRepo
    {

        Task<int> InsertAsync(Runner r);
        Task<Runner> GetUserAsync(string id);
        Task SetNameAsync(int runnerId, string name);
        Task SetLastNameAsync(int runnerId, string lastName);
        Task SetSexAsync(int runnerId, int? sex);
        Task SetBirthdayAsync(int runnerId, DateTime birthday);
        Task SetPhotoUriAsync(int runnerId, string photoUri);
        Task GetPhotoUriAsync(string aspNetId);
    }
}
