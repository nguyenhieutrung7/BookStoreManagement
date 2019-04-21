
    window.validate();

    //Datatable
    //$('#publishers-list').DataTable({
    //    "aaSorting": []
    //});

        $('#publishers-list').DataTable({
            "language": {
                "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/" + $("#current-language").text() + ".json"
            },
            "bDestroy": true
        });
    //show add book form

//detail publisher
$(".publisher-detail-button").on("click", function (e) {
    var publisherid = $(this).data("publisherid");
    $.ajax({
        url: "/Admin/Publisher/ShowDetail",
        type: "GET",
        data: { publisherID: publisherid },
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#publisher-detail-modal").modal("show");
            $("#publisher-detail-modal-container").empty();
            $("#publisher-detail-modal-container").html(data);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});

//edit account
$(".publisher-edit-button").on("click", function (e) {
    var publisherid = $(this).data("publisherid");
    $.ajax({
        url: "/Admin/Publisher/ShowEdit",
        type: "GET",
        data: { publisherID: publisherid },
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#publisher-edit-modal").modal("show");
            $("#publisher-edit-modal-container").empty();
            $("#publisher-edit-modal-container").html(data);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});

//Check unique account


//create publisher
function createPublisher() {
    // Log the event object for inspectin'
    var publisher = {
        "PublisherName": $("#publisher-create-modal #publisher-name-create").val(),
        "PublisherPhone": $("#publisher-create-modal #publisher-phone-create").val()
    };
    $.ajax({
        url: "/Admin/Publisher/Create",
        type: "POST",
        data: JSON.stringify(publisher),
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#publisher-create-modal").modal('hide');
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

//delete account
$(".publisher-submit-delete").on("click", function (e) {
    var publisherID = $(this).attr("data-id");
    var delete_modal = "publisher" + publisherID + "-delete-modal";
    $.ajax({
        url: "/Admin/Publisher/Delete",
        type: "GET",
        data: { publisherID: publisherID },
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $(delete_modal).modal('hide');
            setTimeout(function () {
                $("#page-content-wrapper").empty();
                $("#page-content-wrapper").html(data);
            }, 300);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});