(function ($) {

    abp.widgets.NotificationsOffcanvasWidget = function ($widget) {

        var maxNotificationTime = null;
        var connection = null;

        function fetchAndShowAlerts() {
            easyAbp.processManagement.notifications.notification.getList({
                fromCreationTime: new Date(new Date() - notificationLifetimeMilliseconds),
                userId: abp.currentUser.id,
                dismissed: false,
                maxResultCount: 10
            }).then(function (res) {
                if (res.items && res.items.length > 0) {
                    const latestNotification = res.items.reduce((latest, current) => {
                        return new Date(latest.creationTime) > new Date(current.creationTime) ? latest : current;
                    });

                    maxNotificationTime = latestNotification.creationTime;
                }

                var alertPlaceholder = $('#alert-placeholder');
                var existingAlerts = alertPlaceholder.find('.alert');

                var existingAlertIds = new Map();
                existingAlerts.each(function () {
                    var id = $(this).attr('id');
                    existingAlertIds.set(id, $(this));
                });

                existingAlertIds.forEach(function (alert, id) {
                    if (!res.items.some(item => item.id === id)) {
                        removeAlert(alert)
                    }
                });

                res.items.reverse().forEach(function (item) {
                    if (!existingAlertIds.has(item.id)) {
                        addNotificationToOffcanvas(item);
                    }
                });
                refreshBaseUiElements();
                refreshToolbarWidget();
            }).catch(function () {
                // Silently ignore network errors for background notification fetch
            });
        }

        function getAlertColorClassName(stateFlag) {
            switch (stateFlag) {
                case 1:
                    return "alert-dark";
                case 2:
                    return "alert-success";
                case 3:
                    return "alert-danger";
                case 4:
                    return "alert-primary";
                case 5:
                    return "alert-purple";
                case 6:
                    return "alert-warning";
                default:
                    break;
            }
        }

        function createAlert(item) {
            var actionBtns = "";
            for (var i in notificationOffcanvasAlertActions) {
                var action = notificationOffcanvasAlertActions[i];
                if (!action.visible(item)) continue;
                var actionBtn = `<button type="button" id="action-btn-${i}-${item.id}" class="process-action-btn btn btn-link btn-sm">${action.text}</button><script>var actionBtn = document.getElementById('action-btn-${i}-${item.id}');actionBtn.addEventListener('click', function () {var alert=new bootstrap.Alert(document.getElementById('${item.id}'));return ${action.action}(${JSON.stringify(item)});});</script>`
                actionBtns += actionBtn;
            }
            return $(`
                <div class="alert ${getAlertColorClassName(item.stateFlag)} alert-dismissible fade-in" role="alert">
                    <div class="alert-content-title d-flex align-items-center mb-1">
                        <img src="/images/process-management/icons/${item.stateFlag}.svg" class="svg-icon" alt=""/>
                        <h6><strong>${item.actionName ? item.actionName : item.stateDisplayName}</strong></h6>
                    </div>
                    <div class="alert-content-area mb-4">
                        <p class="small mb-0">
                            ${item.stateSummaryText}
                        </p>
                        ${actionBtns}
                    </div>
                    <div class="state-update-time">
                        <span class="small"><time class="timeago" datetime="${item.creationTime}">${item.creationTime}</time></span>
                    </div>
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            `).attr('id', item.id);
        }

        function addNotificationToOffcanvas(item) {
            var alertPlaceholder = $('#alert-placeholder');
            var newAlert = createAlert(item);
            alertPlaceholder.prepend(newAlert);
            var newAlertNode = document.getElementById(item.id);
            newAlertNode.addEventListener('close.bs.alert', function () {
                easyAbp.processManagement.notifications.notification.dismiss({
                    notificationIds: [$(this).attr('id')]
                });
            });
            newAlertNode.addEventListener('closed.bs.alert', function () {
                refreshBaseUiElements();
                updateToolbarBadgeCount(Math.max(0, getToolbarBadgeCount() - 1));
            });
        }

        function removeAlert(alert) {
            alert.addClass('fade-out').one('animationend', function () {
                $(this).remove();
                refreshBaseUiElements()
            });
        }

        function refreshBaseUiElements() {
            var alertPlaceholder = $('#alert-placeholder');
            if (alertPlaceholder.find('.alert').length) {
                $('#no-notification-text').hide();
            } else {
                $('#no-notification-text').show();
            }
        }

        function startSignalRConnection() {
            if (connection) {
                return;
            }

            connection = new signalR.HubConnectionBuilder()
                .withUrl("/signalr-hubs/process-management/notification")
                .withAutomaticReconnect()
                .build();

            connection.on("ReceiveNotification", function (notification) {
                // Add to offcanvas if it is currently open
                var offcanvasElement = document.getElementById('notificationsOffcanvas');
                var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasElement);
                if (offcanvas && offcanvasElement.classList.contains('show')) {
                    addNotificationToOffcanvas(notification);
                    refreshBaseUiElements();
                }

                // Update the toolbar badge count directly to avoid stale server cache
                updateToolbarBadgeCount(getToolbarBadgeCount() + 1);
            });

            connection.onreconnected(function () {
                // Refresh the full list after reconnection to catch missed notifications
                var offcanvasElement = document.getElementById('notificationsOffcanvas');
                if (offcanvasElement.classList.contains('show')) {
                    fetchAndShowAlerts();
                }
                refreshToolbarWidget();
            });

            connection.start().catch(function (err) {
                console.error('[ProcessManagement] SignalR connection failed:', err);
                startPollingFallback();
            });
        }

        // Fallback polling when SignalR is unavailable
        var pollingIntervalId = null;

        function startPollingFallback() {
            if (pollingIntervalId) return;
            pollingIntervalId = setInterval(function () {
                var offcanvasElement = document.getElementById('notificationsOffcanvas');
                if (offcanvasElement.classList.contains('show')) {
                    fetchAndShowAlerts();
                }
                refreshToolbarWidget();
            }, 5000);
        }

        function stopPollingFallback() {
            if (pollingIntervalId) {
                clearInterval(pollingIntervalId);
                pollingIntervalId = null;
            }
        }

        function updateToolbarBadgeCount(count) {
            $('.notifications-toolbar-item').each(function () {
                var $icon = $(this).find('i');
                $(this).text(' ' + count).prepend($icon);
            });
        }

        function getToolbarBadgeCount() {
            var $first = $('.notifications-toolbar-item').first();
            if (!$first.length) return 0;
            var text = $first.text().trim();
            return parseInt(text) || 0;
        }

        function refreshToolbarWidget() {
            for (const randomId of (window.toolbarNotificationsWidgetAreaRandomIds || [])) {
                var $wrapper = $('#ToolbarNotificationsWidgetArea-' + randomId);
                var $widgets = $wrapper.find('.abp-widget-wrapper');
                if (!$widgets.length) {
                    if ($wrapper.hasClass('abp-widget-wrapper')) {
                        $widgets = $wrapper;
                    } else {
                        $widgets = $wrapper.findWithSelf('.abp-widget-wrapper');
                    }
                }

                var $firstWidget = $widgets.first();
                var refreshUrl = $firstWidget.attr('data-refresh-url');
                if (!refreshUrl) continue;

                $.ajax({
                    url: refreshUrl,
                    type: 'GET',
                    dataType: 'html',
                    global: false
                }).then(function (result) {
                    var $result = $(result);
                    for (const rid of (window.toolbarNotificationsWidgetAreaRandomIds || [])) {
                        var $w = $('#ToolbarNotificationsWidgetArea-' + rid);
                        var $ww = $w.find('.abp-widget-wrapper');
                        for (const widget of $ww) {
                            widget.replaceWith($result[0].cloneNode(true));
                        }
                    }
                }).catch(function () {
                    // Silently ignore network errors for background toolbar refresh
                });

                break; // Only need one AJAX call
            }
        }

        function init() {
            var offcanvasElement = document.getElementById('notificationsOffcanvas');
            var l = abp.localization.getResource('EasyAbpProcessManagement');

            offcanvasElement.addEventListener('show.bs.offcanvas', function () {
                fetchAndShowAlerts();
            });

            var moreProcessesBtn = document.getElementById('notification-offcanvas-more-processes-btn');
            var dismissAllBtn = document.getElementById('notification-offcanvas-dismiss-all-btn');

            moreProcessesBtn.addEventListener('click', function () {
                var url = abp.appPath + 'ProcessManagement/Processes/Process';
                window.open(url, '_blank');
            });

            dismissAllBtn.addEventListener('click', function () {
                var alertPlaceholder = $('#alert-placeholder');
                var existingAlerts = alertPlaceholder.find('.alert');

                var existingAlertIds = new Map();
                existingAlerts.each(function () {
                    var id = $(this).attr('id');
                    existingAlertIds.set(id, $(this));
                });

                Swal.fire({
                    ...abp.libs.sweetAlert.config.default,
                    ...abp.libs.sweetAlert.config.confirm,
                    text: l('SureToDismissAll'),
                    input: "radio",
                    inputValue: 'DismissDisplayed',
                    inputOptions: {
                        'DismissDisplayed': l('DismissDisplayed'),
                        'DismissAll': l('DismissAll')
                    },
                }).then(function ({value: dismiss}) {
                    if (dismiss) {
                        let dismissOpts = dismiss === 'DismissDisplayed' ?
                            {
                                notificationIds: existingAlertIds.keys().toArray()
                            } :
                            {
                                maxCreationTime: maxNotificationTime
                            };
                        easyAbp.processManagement.notifications.notification.dismiss(dismissOpts).then(function () {
                            var dismissedCount = existingAlertIds.size;
                            existingAlertIds.forEach(function (alert, id) {
                                removeAlert(alert)
                            });
                            if (dismiss === 'DismissAll') {
                                updateToolbarBadgeCount(0);
                            } else {
                                updateToolbarBadgeCount(Math.max(0, getToolbarBadgeCount() - dismissedCount));
                            }
                        });
                    }
                });
            });

            // Start SignalR connection for real-time updates
            if (abp.currentUser.isAuthenticated) {
                startSignalRConnection();
            }
        }

        return {
            init: init
        };
    };
})(jQuery);
