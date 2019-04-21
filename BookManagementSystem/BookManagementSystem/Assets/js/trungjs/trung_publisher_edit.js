window.validate();

//Edit publisher
function editPublisher() {
    // Log the event object for inspectin'
    var publisher = {

        "PublisherID": $("#publisher-edit-modal #publisher-id-edit").val(),
        "PublisherName": $("#publisher-edit-modal #publisher-name-edit").val(),
        "PublisherPhone": $("#publisher-edit-modal #publisher-phone-edit").val()
    };
    $.ajax({
        url: "/Admin/Publisher/Edit",
        type: "POST",
        data: JSON.stringify(publisher),
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#publisher-edit-modal").modal('hide');
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