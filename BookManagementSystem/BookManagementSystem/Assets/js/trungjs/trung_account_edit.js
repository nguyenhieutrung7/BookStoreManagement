window.validate();


//Check unique account
$("#account-name-edit").change(function () {
    var accountname = $(this).val();
    var accountexisttext = $('#get-value #account-exist-text').text();
    var accountnoexisttext = $('#get-value #account-noexist-text').text();
    var currentaccountname = $('#edit-get-value #current-account-name').text();
    if (accountname !== currentaccountname) {
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
    }
    else {
        $("#notexist-account-alert .badge-success").text("");
        $("#exist-account-alert .badge-danger").text("");
    }
});

$("#account-edit-form").submit(function (event) {
    if ($("#exist-account-alert .badge-danger").text() !== "") {
        $("#account-name-create").removeClass("is-valid");
        event.preventDefault();
    }

});

function editAccount() {
    // Log the event object for inspectin'
    var accountInfoVM = {
        "AccountName": $("#account-edit-modal #account-name-edit").val(),
        "Password": $("#account-edit-modal #password-edit").val(),
        "SiteOwnerName": $("#account-edit-modal #siteowner-name-edit").val(),
        "SiteOwnerAddress": $("#account-edit-modal #siteowner-address-edit").val(),
        "SiteOwnerEmail": $("#account-edit-modal #siteowner-email-edit").val(),
        "SiteOwnerPhoneNumber": $("#account-edit-modal #siteowner-phonenumber-edit").val(),
        "GenderID": $("#account-edit-modal #gender-id-edit option:selected").val(),
        "SiteOwnerID": $("#account-edit-modal #siteowner-id-edit").val()
    };
    $.ajax({
        url: "/Admin/Account/Edit",
        type: "POST",
        data: JSON.stringify(accountInfoVM),
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#account-edit-modal").modal('hide');
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
$(document).ready(function () {
    var listOptions = $("#gender-id-edit option");
    var male = $("#male").text();
    var female = $("#female").text();
    var unknown = $("#unknown").text();
    $.each(listOptions, function (key, value) {
        if (($(value).text()) === "Male") {
            $(value).text(male);
        }
        else if (($(value).text()) === "Female") {
            $(value).text(female);
        }
        else {
            $(value).text(unknown);
        }
    });

});