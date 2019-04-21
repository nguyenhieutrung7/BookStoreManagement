$(function () {
    //$("#author-list").DataTable({
    //        "order": []
    //});
    $('#author-list').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/" + $("#current-language").text() + ".json"
        },
        "bDestroy": true
    });
});

window.validate();

$("#add-new-author").on("click", function (e) {
    $("#add-author-modal").modal();
});

function createOrEditAuthor(url) {
    var author = {
        authorName: $("#author-name").val(),
        authorPhone: $("#author-phone").val(),
        isActive: $("#is-active").is(":checked")
    };
    //send object to server
    $.ajax({
        url: url,
        type: "POST",
        data: author,
        success: function (result) {
            if (result) {
                $("#add-author-modal").modal('hide');
                alert("Successfully!");
                $("#author-management").click();
            }
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}
//create author
function createAuthor() {
    createOrEditAuthor("/Admin/Author/Create");
}

//show edit author modal
$(".edit-author").on("click", function (e) {
    //set data for modal
    var id = $(this).closest("tr").attr("id");
    var authorName = $(this).closest("tr").children("td:eq(1)").html();
    var authorPhone = $(this).closest("tr").children("td:eq(2)").html();
    $("#edit-author-modal #author-id").val(id);
    $("#edit-author-modal #author-name").val(authorName);
    $("#edit-author-modal #author-phone").val(authorPhone);
    $("#edit-author-modal").modal();
});

//remove validated class when click cancel
$("#cancel-edit").on("click", function () {
    $("#edit-author-modal form").removeClass('was-validated');
});

//Edit author
function editAuthor() {
    var author = {
        authorID: $("#edit-author-modal #author-id").val(),
        authorName: $("#edit-author-modal #author-name").val(),
        authorPhone: $("#edit-author-modal #author-phone").val(),
        isActive: $("#edit-author-modal #is-active").is(":checked")
    };
    //send object to server
    $.ajax({
        url: "/Admin/Author/Edit",
        type: "POST",
        data: author,
        success: function (result) {
            if (result) {
                $("#edit-author-modal").modal('hide');
                alert("Successfully!");
                $("#author-management").click();
            }
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}
//call ajax delete
function deleteAjax(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/Author/Delete?id="+id,
        success: function (result) {
            if (result === true) {
                $("#author-management").click();
            }
        }
    });
}
//delete author
$(".delete-author").off("click").on("click", function (e) {
    e.preventDefault();
    var id = $(this).closest("tr").attr("id");
    var authorName = $(this).closest("tr").children("td:eq(1)").html();
    bootbox.confirm({
        title: "Delete Author",
        message: "Are you sure to delete: " + authorName,
        buttons: {
            confirm: {
                label: "Yes",
                className: "btn-success"
            },
            cancel: {
                label: "No",
                className: "btn-danger"
            }
        },
        callback: function (res) {
            if (res) {
                deleteAjax(id);
            }
        }
    });
});

//detail author
$(".detail-author").off("click").on("click", function (e) {
    e.preventDefault();
    var id = $(this).closest("tr").attr("id");
    $.ajax({
        type: "GET",
        url: "/Admin/Author/Detail?id=" + id,
        dataType:'JSON',
        success: function (data) {
            $("#detail-author-modal").modal();
            ////set data
            $("#detail-author-modal #author-id").val(data.AuthorID);
            $("#detail-author-modal #author-name").val(data.AuthorName);
            $("#detail-author-modal #author-phone").val(data.AuthorPhone);
            if (data.IsActive === true) {
                var active = $("#is-active-label").text();
                $("#detail-author-modal #is-active").val(active);
            }
            else {
                $("#detail-author-modal #is-active").val("Block");
            }
            var st = "";
            $.each(data.ListBooksTitle, function (index, value) {
                st+='<option value="'+index+'">'+value+'</option>';
            });
            $("#list-books").html(st);
        },
        error: function () {
            alert("Somethings were wrong!");
        }
    });
});