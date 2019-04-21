window.validate();
//Check unique category
$("#category-name-edit").change(function () {
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

$("#category-edit-form").submit(function (event) {
    if ($("#exist-category-alert .badge-danger").text() !== "") {
        $("#category-name-edit").removeClass("is-valid");
        event.preventDefault();
    }

});
function editCategory() {
    // Log the event object for inspectin'
    var category = {

        "CategoryID": $("#category-edit-modal #category-id-edit").val(),
        "CategoryName": $("#category-edit-modal #category-name-edit").val(),
        "Description": $("#category-edit-modal #category-description-edit").val()
    };
    $.ajax({
        url: "/Admin/Category/Edit",
        type: "POST",
        data: JSON.stringify(category),
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#category-edit-modal").modal('hide');
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