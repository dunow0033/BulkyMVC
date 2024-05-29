var dataTable;

$(document).ready(function () {
    loadDataTable();
});

/*function loadDataTable() {
    $.ajax({
        url: '/Admin/Product/GetAll',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            dataTable = $('#tblData').DataTable({
                data: data,
                columns: [
                    { data: 'title', "width": "25%" },
                    { data: 'isbn', "width": "15%" },
                    { data: 'listPrice', "width": "10%" },
                    { data: 'author', "width": "20%" },
                    { data: 'category.name', "width": "15%" },
                ]
            });
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}*/

/*$(document).ready(function () {
    loadDataTable();
});*/

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Order/GetAll' },
        "columns": [
            { data: 'id', "width": "25%" },
            { data: 'name', "width": "15%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'applicationUser.email', "width": "15%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/Admin/Order/Upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
					</div>`
                },
                "width": "25%"
            }
        ]
    });
}