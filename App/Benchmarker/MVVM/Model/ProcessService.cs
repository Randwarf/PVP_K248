using Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Benchmarker.MVVM.Model
{
    //Process.GetProcessesByName("Discord").ToList().ForEach(x => Debug.WriteLine(ParentProcessUtilities.GetParentProcess(x.Id).Id));
    //Process.GetProcesses().Where(x => (long)x.MainWindowHandle != 0).ToList().ForEach(x => Debug.WriteLine(x.ProcessName));
    //Process.GetProcesses().Where(x => x.MainWindowTitle != "").ToList().ForEach(x => Debug.WriteLine(x.MainWindowTitle));
    //Process.GetProcesses().Where(x => x.ProcessName == "explorer").ToList().ForEach(x => Debug.WriteLine(x.ProcessName));

    public class ProcessService
    {
        private List<Process> userProcesses;
        private List<Process> topLevelUserProcesses;
        private Dictionary<Process, Process> parentProcesses;

        public ProcessService()
        {
            userProcesses = new List<Process>();
            topLevelUserProcesses = new List<Process>();
            parentProcesses = new Dictionary<Process, Process>();
        }

        // TODO: Parallel?
        public Dictionary<Process, List<Process>> GetTopLevelProcesses()
        {
            int explorerId = Process.GetProcessesByName("explorer")[0].Id;

            parentProcesses = new Dictionary<Process, Process>();
            topLevelUserProcesses = new List<Process>();
            userProcesses = new List<Process>();

            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    // If process has ended
                    if (process.HasExited || process == null)
                    {
                        continue;
                    }

                    Process parentProcess = ParentProcessUtilities.GetParentProcess(process.Id);

                    if (parentProcess == null)
                    {
                        topLevelUserProcesses.Add(process);
                        continue;
                    }

                    userProcesses.Add(process);
                    parentProcesses.Add(process, parentProcess);

                    int parentId = parentProcess.Id;
                    if (parentId == explorerId)
                    {
                        topLevelUserProcesses.Add(process);
                    }
                }
                catch
                {
                    continue;
                }
            }

            var processes = new Dictionary<Process, List<Process>>();

            foreach (Process process in topLevelUserProcesses)
            {
                // If process has ended
                if (process.HasExited || process == null)
                {
                    continue;
                }

                List<Process> children = GetChildProcesses(process);

                processes.Add(process, children);
            }

            return processes;
        }

        public List<Process> GetChildProcesses(Process process)
        {
            return userProcesses
                .Where(x =>
                {
                    try
                    {
                        // If process has ended
                        if (x.HasExited || x == null)
                        {
                            return false;
                        }

                        Process parentProcess = parentProcesses[x];

                        if (parentProcess == null)
                        {
                            return false;
                        }

                        return parentProcess.Id == process.Id;
                    }
                    catch
                    {
                        return false;
                    }
                })
                .ToList();
        }
    }
}
