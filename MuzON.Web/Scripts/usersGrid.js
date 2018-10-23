﻿$(document).ready(function () {
    $("#tableUsersGrid").DataTable({
        "ajax": {
            "url": "/Account/GetUsers",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "Email" },
            {
                "data": "Id",
                "searchable": false,
                "sortable": false,
                "orderable": false,
                "render": function (Id) {
                    return `<div class="btn-group" role="group">
                                <button type="button" class="btn btn-info btn-md" data-toggle="modal" data-url="/Account/UserEdit/` + Id + `" id="btnEditUser">
                                    <span class="fa fa-pencil" aria-hidden="true"></span> Edit
                                </button>
                                <button type="button" class="btn btn-warning btn-md" data-toggle="modal" data-url="/Account/UserDetails/` + Id + `" id="btnDetailsUser">
                                    <span class="fa fa-eye" aria-hidden="true"></span> Details
                                </button>
                                <button type="button" class="btn btn-danger btn-md" data-toggle="modal" data-url="/Account/Delete/` + Id + `" id="btnDeleteUser">
                                    <span class="fa fa-trash" aria-hidden="true"></span> Delete
                                </button>
                            </div>`;
                }
            }
        ]
    });
});

$("#tableUsersGrid").on("click", "#btnDeleteUser", function () {

    var url = $(this).data("url");

    $.get(url, function (data) {
        $('#deleteUserContainer').html(data);

        $('#deleteUserModal').modal('show');
    });

});

function DeleteUserSuccess(data) {

    if (data.data != "success") {
        $('#deleteUserContainer').html(data.data);
        return;
    }
    $.notify({
        // options
        icon: 'fa fa-check-circle',
        title: '<strong>Success</strong>: ',
        message: 'Deleted!'
    }, {
            type: 'success',
            z_index: 1051,
            animate: {
                enter: 'animated bounceIn',
                exit: 'animated bounceOut'
            }
        });
    $('#deleteUserModal').modal('hide');
    $('#deleteUserContainer').html("");
    $('#tableUsersGrid').DataTable().ajax.reload();
}