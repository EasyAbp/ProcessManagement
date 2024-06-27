namespace EasyAbp.ProcessManagement.Web.Components.NotificationsWidget;

public class NotificationsWidgetViewModel
{
    /// <summary>
    /// 0~99 unread notifications.
    /// </summary>
    public int UnreadCount { get; set; }

    public NotificationsWidgetViewModel()
    {
    }

    public NotificationsWidgetViewModel(int unreadCount)
    {
        UnreadCount = unreadCount;
    }
}