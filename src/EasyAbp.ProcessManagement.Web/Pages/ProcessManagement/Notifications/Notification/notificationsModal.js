$(function () {

    $('#go-to-management-page').click(function () {
        document.location = abp.appPath + 'ProcessManagement/Processes/Process';
    });

    var l = abp.localization.getResource('ProcessManagement');

    var notificationService = easyAbp.processManagement.notifications.notification;
    var processService = easyAbp.processManagement.processes.process;
    var widgetManager = new abp.WidgetManager({wrapper: '#ToolbarNotificationsWidgetArea'});

    var dataTable = $('#NotificationsTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: false,
        lengthChange: false,
        pageLength: 10,
        searching: false, // disable default searchbox
        autoWidth: false,
        scrollCollapse: true,
        ordering: false,
        ajax: abp.libs.datatables.createAjax(notificationService.getList, {
            fromCreationTime: abp.clock.normalize(new Date(abp.clock.now() - notificationLifetime)),
            userId: abp.currentUser.id,
            dismissed: false
        }),
        columnDefs: [
            {
                data: "stateName",
                render: function (data, type, row, meta) {
                    return `<div class="stage-flag-name-container">` + getStateFlagIcon(row) + ' ' + renderRow(row, row.actionName ? row.actionName : data) + `</div>`;
                }
            },
            {
                data: "creationTime",
                render: function (data, type, row, meta) {
                    return renderRow(row, toTimeAgo(data))
                }
            }
        ]
    }));

    function renderRow(row, data) {
        return row.readTime == null ? '<span class="row-text-bold">' + data + '</span>' : data;
    }

    function toTimeAgo(time) {
        var timeTag = `<time class="timeago" datetime="` + time + `">` + time + `</time>`;
        return `<span data-bs-toggle="popover" data-bs-content="` + time + `" data-bs-trigger="hover focus">`
            + timeTag
            + `</span>`
    }

    function formatChildRow(data) {
        return data;
    }

    function getStateFlagIcon(row) {
        // https://commons.wikimedia.org/wiki/File:Emojione_BW_23F8.svg
        if (row.stateFlag === 0) {
            return `<img src="` + abp.appPath + `images/process-management/icons/0.svg` + `" class="svg-icon" alt=""/>`;
        }
        if (row.stateFlag === 1) {
            return `<img src="` + abp.appPath + `images/process-management/icons/1.svg` + `" class="svg-icon" alt=""/>`;
        }
        if (row.stateFlag === 2) {
            return `<img src="` + abp.appPath + `images/process-management/icons/2.svg` + `" class="svg-icon" alt=""/>`;
        }
        if (row.stateFlag === 3) {
            return `<img src="` + abp.appPath + `images/process-management/icons/3.svg` + `" class="svg-icon" alt=""/>`;
        }
        if (row.stateFlag === 4) {
            return `<img src="` + abp.appPath + `images/process-management/icons/4.svg` + `" class="svg-icon" alt=""/>`;
        }
        if (row.stateFlag === 5) {
            return `<img src="` + abp.appPath + `images/process-management/icons/5.svg` + `" class="svg-icon" alt=""/>`;
        }
        return ''
    }

    // Add event listener for opening and closing details
    $('#NotificationsTable tbody').on('click', 'td', function () {
        var tr = $(this).closest('tr');
        var row = dataTable.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        } else {
            var rowData = row.data();
            if (!rowData) return;
            // Hide other rows' child
            dataTable.rows().every(function () {
                var otherRow = this;
                if (otherRow.child.isShown()) {
                    otherRow.child.hide();
                }
            });
            tr.removeClass('shown');
            // Open this row
            row.child(formatChildRow(rowData.stateSummaryText)).show();
            tr.addClass('shown');
            if (!rowData.readTime) {
                notificationService.read(rowData.id).then(function () {
                    widgetManager.refresh();
                    tr.find('span.row-text-bold').removeClass('row-text-bold');
                });
            }
        }
    });
});