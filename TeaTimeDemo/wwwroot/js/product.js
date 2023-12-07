var dataTable;
$(document).ready( function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/product/getall'
        },
        "columns": [
            { data: 'id', "width": "10%" },
            {
                data: 'imageUrl',
                "width": "10%",
                "render": function (data) {
                    if (data == '') {
                        return `<a>${data}</><img src="https://126285492.cdn6.editmysite.com/uploads/1/2/6/2/126285492/s348742077488039169_p392_i2_w900.jpeg" width="60" height="60" class="img-thumbnail" />`
                    }
                    return `<img src="${data}" width="60" height="60" class="img-thumbnail" />`
                }
            },
            { data: 'name', "width": "20%" },
            { data: 'category.name', "width": "10%" },
            { data: 'size', "width": "10%" },
            { data: 'price', "width": "5%" },
            { data: 'description', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-150 btn-group" role="group">
                            <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> 編輯
                            </a>
                            <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2" width="120">
                                <i class="bi bi-trash-fill"></i> 刪除
                            </a>
                        </div>`
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "您確定要刪除嗎？",
        text: "一但執行此動作將無法復原",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#dd3333',
        confirmButtonText: '確認執行刪除'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}

