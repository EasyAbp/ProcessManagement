$(function () {

    $("#ProcessFilter :input").on('input', function () {
        dataTable.ajax.reload();
    });

    //After abp v7.2 use dynamicForm 'column-size' instead of the following settings
    //$('#ProcessCollapse div').addClass('col-sm-3').parent().addClass('row');

    var getFilter = function () {
        var input = {};
        $("#ProcessFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/ProcessFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('EasyAbpProcessManagement');

    var service = easyAbp.processManagement.processes.process;
    var detailsModal = new abp.ModalManager(abp.appPath + 'ProcessManagement/Processes/Process/DetailsModal');

    var dataTable = $('#ProcessTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,//disable default searchbox
        autoWidth: false,
        scrollCollapse: true,
        order: [[0, "asc"]],
        ajax: abp.libs.datatables.createAjax(service.getList, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l('Details'),
                                action: function (data) {
                                    detailsModal.open({id: data.record.id});
                                }
                            },
                            ...customActions
                        ]
                }
            },
            {
                title: l('ProcessStateName'),
                data: "stateName",
                render: function (data, type, row, meta) {
                    return `<div class="stage-flag-name-container">` + getStateFlagIcon(row) + ' ' + (row.actionName ? row.actionName : row.stateDisplayName) + `</div>`;
                }
            },
            {
                title: l('ProcessStateSummaryText'),
                data: "stateSummaryText"
            },
            {
                title: l('ProcessCreationTime'),
                data: "creationTime",
                render: function (data, type, row, meta) {
                    return toTimeAgo(data)
                }
            },
            {
                title: l('ProcessProcessName'),
                data: "processName",
                render: function (data, type, row, meta) {
                    return row.processDisplayName;
                }
            },
        ]
    }));

    function toTimeAgo(time) {
        var timeTag = `<time class="timeago" datetime="` + time + `">` + time + `</time>`;
        return `<span data-bs-toggle="popover" data-bs-content="` + time + `" data-bs-trigger="hover focus">`
            + timeTag
            + `</span>`
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
});
