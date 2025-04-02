(function ($) {

    abp.widgets.NotificationsOffcanvasWidget = function ($widget) {

        var intervalId;

        function fetchAndShowAlerts() {
            easyAbp.processManagement.notifications.notification.getList({
                fromCreationTime: new Date(new Date() - notificationLifetimeMilliseconds),
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

                existingAlertIds.forEach(function (alert, id) {
                    if (!res.items.some(item => item.id === id)) {
                        removeAlert(alert)
                    }
                });

                res.items.reverse().forEach(function (item) {
                    if (!existingAlertIds.has(item.id)) {
                        var newAlert = createAlert(item);
                        alertPlaceholder.prepend(newAlert)
                        var newAlertNode = document.getElementById(item.id);
                        newAlertNode.addEventListener('close.bs.alert', function () {
                            tryClearInterval();
                            easyAbp.processManagement.notifications.notification.dismiss({
                                notificationIds: [$(this).attr('id')]
                            }).always(function () {
                                tryCreateInterval();
                            });
                        });
                        newAlertNode.addEventListener('closed.bs.alert', function () {
                            refreshBaseUiElements()
                        });
                    }
                });
                refreshBaseUiElements()
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

        function tryCreateInterval() {
            intervalId = setInterval(fetchAndShowAlerts, 5000);
        }

        function tryClearInterval() {
            if (intervalId) clearInterval(intervalId);
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

        function init() {
            var offcanvasElement = document.getElementById('notificationsOffcanvas');
            var l = abp.localization.getResource('EasyAbpProcessManagement');

            offcanvasElement.addEventListener('show.bs.offcanvas', function () {
                fetchAndShowAlerts();
                tryCreateInterval();
            });

            offcanvasElement.addEventListener('hide.bs.offcanvas', function () {
                tryClearInterval();
            });

            var moreProcessesBtn = document.getElementById('notification-offcanvas-more-processes-btn');
            var clearAllBtn = document.getElementById('notification-offcanvas-clear-all-btn');

            moreProcessesBtn.addEventListener('click', function () {
                var url = abp.appPath + 'ProcessManagement/Processes/Process';
                window.open(url, '_blank');
            });

            clearAllBtn.addEventListener('click', function () {
                tryClearInterval();
                var alertPlaceholder = $('#alert-placeholder');
                var existingAlerts = alertPlaceholder.find('.alert');

                var existingAlertIds = new Map();
                existingAlerts.each(function () {
                    var id = $(this).attr('id');
                    existingAlertIds.set(id, $(this));
                });

                abp.message.confirm(l('SureToClearAll')).then(function (confirmed) {
                    if (confirmed) {
                        easyAbp.processManagement.notifications.notification.dismiss({
                            notificationIds: existingAlertIds.keys().toArray()
                        }).then(function () {
                            existingAlertIds.forEach(function (alert, id) {
                                removeAlert(alert)
                            });
                        });
                    }
                }).always(function () {
                    tryCreateInterval();
                });
            });
        }

        return {
            init: init
        };
    };
})(jQuery);