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

    var l = abp.localization.getResource('ProcessManagement');

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
                            }
                        ]
                }
            },
            {
                title: l('ProcessStateFlag'),
                data: "stateFlag"
            },
            {
                title: l('ProcessCreationTime'),
                data: "creationTime"
            },
            {
                title: l('ProcessProcessName'),
                data: "processName"
            },
            {
                title: l('ProcessStateName'),
                data: "processDisplayName",
                render: function (data, type, row) {
                    return row.subStateName ? data + ' (' + row.subStateName + ')' : data;
                }
            },
            {
                title: l('ProcessStateSummaryText'),
                data: "stateSummaryText"
            },
        ]
    }));
});
