using Volo.Abp;

namespace EasyAbp.ProcessManagement.Options;

public class UndefinedProcessStateException : AbpException
{
    public UndefinedProcessStateException(string stateName, string processName) : base(
        $"State `{stateName}` is undefined for the process `{processName}`")
    {
    }
}