$(document).ready(() => {
    loadDataTable();
})

const loadDataTable = () => {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: "/admin/product/getall" },
        "columns": [
            { data: 'title', "width":"25%" },
            { data: 'isbn', "width":"15%" },
            { data: 'price', "width":"10%" },
            { data: 'author', "width":"20%" },
            { data: 'category.name', "width": "15%" },
            {
                data: 'id',
                "render": (data) => {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2> <i class="bi bi-pencil"></i> Edit</a>
                        <a onClick=Delete("/admin/product/delete?id=${data}") class="btn btn-danger mx-2><i class="bi bi-trash-fill></i> Delete </a>
                    </div>`
                }
            }
        ]
    })
}

const Delete = (url) => {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: "btn btn-success",
            cancelButton: "btn btn-danger"
        },
        buttonsStyling: false
    });
    swalWithBootstrapButtons.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel!",
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: (data) => {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire({
                title: "Cancelled",
                text: "Your imaginary file is safe :)",
                icon: "error"
            });
        }
    });
}
