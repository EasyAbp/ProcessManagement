(function ($) {

    let modal = new abp.ModalManager(abp.appPath + 'ProcessManagement/Notifications/Notification/NotificationsModal');

    abp.widgets.NotificationsWidget = function ($widget) {

        let widgetManager = $widget.data('abp-widget-manager');

        function init() {
            $('.notifications-toolbar-item').click(function () {
                modal.open();
            })
        }

        return {
            init: init
        };
    };
})(jQuery);