using System;

namespace EasyAbp.ProcessManagement.Processes;

[Serializable]
public class ProcessStateChangedEto
{
    public ProcessStateInfoModel OldState { get; set; }

    public Process Process { get; set; }

    public ProcessStateChangedEto()
    {
    }

    public ProcessStateChangedEto(ProcessStateInfoModel oldState, Process process)
    {
        OldState = oldState;
        Process = process;
    }
}