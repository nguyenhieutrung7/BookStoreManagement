
    window.validate();

//    $('#categories-list').DataTable({
//        "aaSorting": []
//});
$('#categories-list').DataTable({
    "language": {
        "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/" + $("#current-language").text() + ".json"
    },
    "bDestroy": true
   
});
//detail category
$(".category-detail-button").on("click", function (e) {
    var categoryid = $(this).data("categoryid");
    $.ajax({
        url: "/Admin/Category/ShowDetail",
        type: "GET",
        data: { categoryID: categoryid },
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#category-detail-modal").modal("show");
            $("#category-detail-modal-container").empty();
            $("#category-detail-modal-container").html(data);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});

//edit account
$(".category-edit-button").on("click", function (e) {
    var categoryid = $(this).data("categoryid");
    $.ajax({
        url: "/Admin/Category/ShowEdit",
        type: "GET",
        data: { categoryID: categoryid },
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#category-edit-modal").modal("show");
            $("#category-edit-modal-container").empty();
            $("#category-edit-modal-container").html(data);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});

//Check unique category
$("#category-name-create").change(function () {
    var categoryname = $(this).val();
    $.ajax({
        url: "/Admin/Category/CheckUniqueCategoryName",
        type: "GET",
        data: { categoryName: categoryname },
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (data) {
            if (data === false) {
                $("#notexist-category-alert .badge-success").text("");
                $("#exist-category-alert .badge-danger").text("Category name exist!!!");
            }
            else {
                $("#exist-category-alert .badge-danger").text("");
                $("#notexist-category-alert .badge-success").text("Can use this name.");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});

$("#category-create-form").submit(function (event) {
    if ($("#exist-category-alert .badge-danger").text() !== "") {
        $("#category-name-create").removeClass("is-valid");
        event.preventDefault();
    }

});


//create category
function createCategory() {
    // Log the event object for inspectin'
    var category = {
        "CategoryName": $("#category-create-modal #category-name-create").val(),
        "Description": $("#category-create-modal #category-description-create").val()
    };
    $.ajax({
        url: "/Admin/Category/Create",
        type: "POST",
        data: JSON.stringify(category),
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#category-create-modal").modal('hide');
            setTimeout(function () {
                $("#page-content-wrapper").empty();
                $("#page-content-wrapper").html(data);
            }, 300);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}