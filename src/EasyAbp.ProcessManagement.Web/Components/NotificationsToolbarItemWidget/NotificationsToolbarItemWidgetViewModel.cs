namespace EasyAbp.ProcessManagement.Web.Components.NotificationsToolbarItemWidget;

public class NotificationsToolbarItemWidgetViewModel
{
    /// <summary>
    /// 0~99 unread notifications.
    /// </summary>
    public int UnreadCount { get; set; }

    public NotificationsToolbarItemWidgetViewModel()
    {
    }

    public NotificationsToolbarItemWidgetViewModel(int unreadCount)
    {
        UnreadCount = unreadCount;
    }
}