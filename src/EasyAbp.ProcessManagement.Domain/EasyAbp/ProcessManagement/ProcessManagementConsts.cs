using EasyAbp.ProcessManagement.Localization;
using Volo.Abp.Localization;

namespace EasyAbp.ProcessManagement;

public static class ProcessManagementConsts
{
    public static class InstantNotificationProcess
    {
        public static string ProcessName { get; set; } = "InstantNotification";

        public static ILocalizableString ProcessDisplayName { get; set; } =
            new LocalizableString(typeof(ProcessManagementResource), "Process:InstantNotification");

        public static string TheOnlyStateName { get; set; } = "Notification";

        public static ILocalizableString TheOnlyStateDisplayName { get; set; } =
            new LocalizableString(typeof(ProcessManagementResource), "ProcessState:Notification");
    }
}