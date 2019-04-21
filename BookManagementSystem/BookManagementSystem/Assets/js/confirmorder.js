$(document).ready(function () {
    //edit phone number click show input
    $("#edit-phone-number").off("click").on("click", function (e) {
        e.preventDefault();
        var spanTag = $("#customer-phone-number");
        window.oldPhoneNumber = spanTag.text().trim();
        var input = $('<input>', {
            id: "new-phone-number",
            class: 'col-lg-4 form-control',
            type: 'text',
            value: oldPhoneNumber,
            required:'required'
        });
        spanTag.replaceWith(input);
        $(this).addClass("d-none");
        $("#save-phone-number").removeClass("d-none");
        $("#cancel-phone-number").removeClass("d-none");
    });
    $("#save-phone-number").off("click").on("click", function () {
        var newPhoneNumber = $("#new-phone-number").val();
        if (newPhoneNumber.trim().length < 10 || newPhoneNumber.trim().length > 15) {
            $("#message-phone-number").text("New phone number is incorrect. It must contain from 10 to 15 digits!");
            $("#message-phone-number").removeClass("d-none");
            return;
        }
        for (var i = 0; i < newPhoneNumber.length; i++) {
            if (!$.isNumeric(newPhoneNumber.charAt(i))) {
                $("#message-phone-number").text("New phone number is incorrect.It doesn't contain any letter!");
                $("#message-phone-number").removeClass("d-none");
                return;
            }
        }
        $("#message-phone-number").addClass("d-none");
        var spanTag = $('<span>', {
            id: "customer-phone-number",
            text: newPhoneNumber.trim()
        });
        $("#new-phone-number").replaceWith(spanTag);
        addClassDisplayNone();
    });
    $("#cancel-phone-number").off("click").on("click", function () {
        var spanTag = $('<span>', {
            id: "customer-phone-number",
            text: oldPhoneNumber
        });
        $("#message-phone-number").addClass("d-none");
        $("#new-phone-number").replaceWith(spanTag);
        addClassDisplayNone();
    });
    function addClassDisplayNone() {
        $("#save-phone-number").addClass("d-none");
        $("#cancel-phone-number").addClass("d-none");
        $("#edit-phone-number").removeClass("d-none");
    }
    $("#edit-address").off("click").on("click", function (e) {
        e.preventDefault();
        var spanTag = $("#customer-address");
        window.oldAddress = spanTag.text().trim();
        var input = $('<input>', {
            id: "new-address",
            class: 'form-control',
            type: 'text',
            value: oldAddress,
            required: 'required'
        });
        spanTag.replaceWith(input);
        $(this).addClass("d-none");
        $("#save-address").removeClass("d-none");
        $("#cancel-address").removeClass("d-none");
    });
    $("#save-address").off("click").on("click", function () {
        var newAddress = $("#new-address").val().trim();
        if (newAddress.trim().length < 10 || newAddress.trim().length > 100) {
            $("#message-address").text("New address is incorrect. It must contain from 10 to 100 letters!");
            $("#message-address").removeClass("d-none");
            return;
        }
        $("#message-address").addClass("d-none");
        var spanTag = $('<span>', {
            id: "customer-address",
            text: newAddress
        });
        $("#new-address").replaceWith(spanTag);
        $("#edit-address").removeClass("d-none");
        $("#save-address").addClass("d-none");
        $("#cancel-address").addClass("d-none");
    });
    $("#cancel-address").off("click").on("click", function () {
        $("#message-address").addClass("d-none");
        var spanTag = $('<span>', {
            id: "customer-address",
            text: window.oldAddress
        });
        $("#new-address").replaceWith(spanTag);
        $("#edit-address").removeClass("d-none");
        $("#save-address").addClass("d-none");
        $("#cancel-address").addClass("d-none");
    });

    $("#confirm-order").off("click").on("click", function () {
        var phoneNumber = $("#customer-phone-number").text();
        var address = $("#customer-address").text();
        $.ajax({
            url: "/Cart/CreateOrder?phoneNumber=" + phoneNumber + "&&address=" + address,
            type: "GET",
            dataType: "json",
            success: function (response) {
                $('#modalSpinner').modal({ backdrop: 'static', keyboard: false });
                if (response.id > 0) {
                    sendMail.orderedForm(response.id, response.email);
                }
                else {
                    window.location.href = "/Account/Login";
                }
            }
        });
    });
    $("#cancel-order").off("click").on("click", function () {
        $("#view-shopping-cart").trigger("click");
    });
});