
    window.validate();

    //$('#accounts-list').DataTable({
    //    "aaSorting": []
    //});
$('#accounts-list').DataTable({
    "language": {
        "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/" + $("#current-language").text() + ".json"
    },
    "bDestroy": true
});
//detail account
$(".account-detail-button").on("click", function (e) {
    var siteownerid = $(this).data("siteownerid");
    $.ajax({
        url: "/Admin/Account/ShowDetail",
        type: "GET",
        data: { siteownerID: siteownerid },
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#account-detail-modal").modal("show");
            $("#account-detail-modal-container").empty();
            $("#account-detail-modal-container").html(data);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});

//edit account
$(".account-edit-button").on("click", function (e) {
    var siteownerid = $(this).data("siteownerid");
    $.ajax({
        url: "/Admin/Account/ShowEdit",
        type: "GET",
        data: { siteownerID: siteownerid },
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#account-edit-modal").modal("show");
            $("#account-edit-modal-container").empty();
            $("#account-edit-modal-container").html(data);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});

//Check unique account
$("#account-name-create").change(function () {
    var accountname = $(this).val();
    var accountexisttext = $('#get-value #account-exist-text').text();
    var accountnoexisttext = $('#get-value #account-noexist-text').text();
    $.ajax({
        url: "/Admin/Account/CheckUniqueAccountName",
        type: "GET",
        data: { accountName: accountname },
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (data) {
            if (data === false) {
                $("#notexist-account-alert .badge-success").text("");
                $("#exist-account-alert .badge-danger").text(accountexisttext);
            }
            else {
                $("#exist-account-alert .badge-danger").text("");
                $("#notexist-account-alert .badge-success").text(accountnoexisttext);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});

$("#account-create-form").submit(function (event) {
    if ($("#exist-account-alert .badge-danger").text() !== "") {
        $("#account-name-create").removeClass("is-valid");
        event.preventDefault();
    }

});

//create account
function createAccount() {
    // Log the event object for inspectin'
    var accountInfoVM = {
        "AccountName": $("#account-create-modal #account-name-create").val(),
        "Password": $("#account-create-modal #password-create").val(),
        "SiteOwnerName": $("#account-create-modal #siteowner-name-create").val(),
        "SiteOwnerAddress": $("#account-create-modal #siteowner-address-create").val(),
        "SiteOwnerEmail": $("#account-create-modal #siteowner-email-create").val(),
        "SiteOwnerPhoneNumber": $("#account-create-modal #siteowner-phonenumber-create").val(),
        "GenderID": $("#account-create-modal #gender-id-create option:selected").val(),
        "SiteOwnerID": null
    };
    $.ajax({
        url: "/Admin/Account/Create",
        type: "POST",
        data: JSON.stringify(accountInfoVM),
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#account-create-modal").modal('hide');
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
$(".account-submit-delete").on("click", function (e) {
    var accountid = $(this).attr("data-id");
    var delete_modal = "account" + accountid + "-delete-modal";
    $.ajax({
        url: "/Admin/Account/Delete",
        type: "GET",
        data: { accountID: accountid },
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

$(document).ready(function () {
    var listOptions = $("#gender-id-create option");
    var male = $("#male").text();
    var female = $("#female").text();
    var unknown = $("#unknown").text();
    $.each(listOptions, function (key, value) {
        if (($(value).text()) == "Male") {
            $(value).text(male);
        }
        else if (($(value).text()) == "Female") {
            $(value).text(female);
        }
        else {
            $(value).text(unknown);
        }
    });

});