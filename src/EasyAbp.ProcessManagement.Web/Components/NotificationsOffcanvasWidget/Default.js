(function ($) {

    abp.widgets.NotificationsOffcanvasWidget = function ($widget) {

        var intervalId;
        
        function fetchAndShowAlerts() {
            easyAbp.processManagement.notifications.notification.getList({
                fromCreationTime: new Date(abp.clock.now() - notificationLifetimeMilliseconds),
                userId: abp.currentUser.id,
                dismissed: false,
                maxResultCount: 10
            }).then(function (res) {
                var alertPlaceholder = $('#alert-placeholder');
                var existingAlerts = alertPlaceholder.find('.alert');

                var existingAlertIds = new Map();
                existingAlerts.each(function () {
                    var id = $(this).attr('id');
                    existingAlertIds.set(id, $(this));
                });

                res.items.forEach(function (item) {
                    if (!existingAlertIds.has(item.id)) {
                        var newAlert = createAlert(item);
                        alertPlaceholder.append(newAlert)
                        var newAlertNode = document.getElementById(item.id);
                        newAlertNode.addEventListener('close.bs.alert', function () {
                            tryClearInterval();
                            easyAbp.processManagement.notifications.notification.dismiss({
                                notificationIds: [$(this).attr('id')]
                            }).always(function () {
                                tryCreateInterval();
                            });
                        });
                    }
                });

                existingAlertIds.forEach(function (alert, id) {
                    if (!res.items.some(item => item.id === id)) {
                        alert.addClass('fade-out').one('animationend', function () {
                            $(this).remove();
                        });
                    }
                });
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
            return $(`
                <div class="alert ${getAlertColorClassName(item.stateFlag)} alert-dismissible fade-in" role="alert">
                    <div class="d-flex">
                        <img src="/images/process-management/icons/${item.stateFlag}.svg" class="svg-icon" alt=""/>
                        <div>
                            <strong>${item.actionName ? item.actionName : item.stateDisplayName}</strong>
                            <p class="small mb-4">
                                ${item.stateSummaryText}
                            </p>
                        </div>
                    </div>
                    <div class="state-update-time">
                        <span class="small"><time class="timeago" datetime="${item.creationTime}">${item.creationTime}</time></span>
                    </div>
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            `).attr('id', item.id);
        }

        function tryCreateInterval() {
            intervalId = setInterval(fetchAndShowAlerts, 5000);
        }
        
        function tryClearInterval() {
            if (intervalId) clearInterval(intervalId);
        }
        
        function init() {
            var offcanvasElement = document.getElementById('notificationsOffcanvas');

            offcanvasElement.addEventListener('show.bs.offcanvas', function () {
                fetchAndShowAlerts();
                tryCreateInterval();
            });

            offcanvasElement.addEventListener('hide.bs.offcanvas', function () {
                tryClearInterval();
            });

            var clearAllBtn = document.getElementById('notification-offcanvas-clear-all-btn');

            clearAllBtn.addEventListener('click', function() {
                tryClearInterval();
                var alertPlaceholder = $('#alert-placeholder');
                var existingAlerts = alertPlaceholder.find('.alert');

                var existingAlertIds = new Map();
                existingAlerts.each(function () {
                    var id = $(this).attr('id');
                    existingAlertIds.set(id, $(this));
                });
                
                easyAbp.processManagement.notifications.notification.dismiss({
                    notificationIds: existingAlertIds.keys().toArray()
                }).then(function () {
                    existingAlertIds.forEach(function (alert, id) {
                        alert.addClass('fade-out').one('animationend', function () {
                            $(this).remove();
                        });
                    });
                    tryCreateInterval();
                });
            });
        }

        return {
            init: init
        };
    };
})(jQuery);