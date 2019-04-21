$(function () {
    //$("#books-list").DataTable({
    //    "order": []
    //});
    $('#books-list').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/" + $("#current-language").text() + ".json"
        },
        "bDestroy": true
    });
});
function saveImage() {
    if (window.FormData !== null) {

        var fileUpload = $("#Picture").get(0);
        var files = fileUpload.files;

        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        $.ajax({
            url: '/Admin/Book/UploadFiles',
            type: "POST",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: fileData,
            success: function (result) {
                //alert(result);
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    } else {
        alert("FormData is not supported.");
    }
}

//create new or update book
window.createOrEditBook = function (url) {
    saveImage();
    var st = $("#Picture").val().toString().split("\\");
    console.log(st[st.length - 1]);
    var bookID = 0;
    var createdDate = undefined;
    //case update book
    if ($("#BookID").val() !== undefined) {
        bookID = $("#BookID").val();
        createdDate = $("#CreatedDate").val();
    }
    var book = {
        bookID: bookID > 0 ? bookID : undefined,
        createdDate: createdDate === undefined ? new Date() : createdDate,
        title: $("#Title").val(),
        description: $("#Description").val(),
        categoryID: $("#CategoryID >option:selected").val(),
        authorID: $("#AuthorID > option:selected").val(),
        price: $("#Price").val(),
        quantity: $("#Quantity").val(),
        isbn: $("#ISBN").val(),
        publisherID: $("#PublisherID > option:selected").val(),
        isActive: true,
        picture: st[st.length - 1]
    };

    //send object to server
    $.ajax({
        url: url,
        type: "POST",
        data: book,
        success: function (result) {
            if (result) {
                $("#book-management").click();
            }
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}

//create new book click
$("#add-new-book").on("click", function (e) {
    window.loadPage(e, "/Admin/Book/Create");
});

//edit book button click
$(".edit-button").on("click", function (e) {
    e.preventDefault();
    var id = $(this).closest("tr").attr("id");
    $.ajax({
        type: "GET",
        url: "/Admin/Book/Edit?id=" + id,
        dataType: "html",
        success: function (obj) {
            $.when(window.appendContainer(obj)).done(function () {
                $("#book-management").addClass("active");
            });
        },
        error: function () {
            alert("Error");
        }
    });
});

function deleteAjax(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/Book/Delete?id=" + id,
        success: function (result) {
            if (result === true) {
                $("#book-management").click();
            }
            else {
                alert("Cannot delete this book because it exists in order!");
            }
        }
    });
}
//parse date
function parseDate(input) {
    theDate = new Date(parseInt(input.substring(6, 19)));
    return theDate.toLocaleDateString();
}

//view detail book modal
$(".detail-book").on("click", function (e) {
    e.preventDefault();
    var id = $(this).closest("tr").attr("id");
    $.ajax({
        type: "GET",
        url: "/Admin/Book/Detail?id=" + id,
        dataType:'JSON',
        success: function (data) {
          
            //set data
            $("#BookID").val(data.BookID);
            $("#Title").text(data.Title);
            $("#AuthorName").val(data.AuthorName);
            $("#ISBN").val(data.ISBN);
            $("#Description").text(data.Description);
            $("#Category").text(data.CategoryName);
            $("#Price").val(data.Price);
            $("#Quantity").val(data.Quantity);
            $("#Publisher").val(data.PublisherName);
            $("#ModifiedDate").val(parseDate(data.ModifiedDate));
            $("#CreatedDate").val(parseDate(data.CreatedDate));
            $("#IsActive").text("Active");
            $("#Picture").attr("src", data.Picture);
            $("#detail-book-modal").modal();
        },
        error: function (e) {
            alert("Somethings were wrong!" + e.err);
        }
    });

});

//delete confirm by boot box
$(".delete-book").off("click").on("click", function (e) {
    e.preventDefault();
    var id = $(this).closest("tr").attr("id");
    var bookName = $(this).closest("tr").children("td:eq(1)").html();
    bootbox.confirm({
        title: $("#deletebook").text()+" ",
        message: $("#ans").text() + bookName,
        buttons: {
            confirm: {
                label: $("#yes").text(),
                className: "btn-success"
            },
            cancel: {
                label: $("#no").text(),
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
