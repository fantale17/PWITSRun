using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Repos.Interfaces
{
    public interface IActivityRepo
    {
        Task InsertAsync(Activity act);
        Task<IEnumerable<Activity>> GetTrainingsAsync(int idRunner);
        Task OpenTrainingAsync(int idActivity);
        Task CloseTrainingAsync(int idActivity);
    }
}
