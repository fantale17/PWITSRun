using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibSharedRun.Repos
{
    public interface IRunnerRepo
    {
        Task InsertRunner(Runner runner);
    }
}
