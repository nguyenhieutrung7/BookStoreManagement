//Send mail
var sendMail = {
    sendOrderedForm: function (content, email) {
        $.ajax({
            url: "/Admin/Mail/SendOrderedMail",
            type: "POST",
            data: { content: content, email: email },
            dataType: "json",
            success: function (data) {
                $('#modalSpinner').modal('hide');
                alert($("#success-message").text());
                //return home page
                setTimeout(function () { window.location.href = "/Home/Index"; }, 500);
            }
        });
    },
    orderedForm: function (id, email) {
        $.ajax({
            url: "/Admin/Mail/OrderedMail",
            type: "GET",
            data: { orderId: id },
            dataType: "html",
            success: function (content) {
                sendMail.sendOrderedForm(content, email);
            }
        });
    },
    sendDeliveringForm: function (content, email) {
        $.ajax({
            url: "/Admin/Mail/SendDeliveringMail",
            type: "POST",
            data: { content: content, email: email },
            dataType: "json",
            success: function (data) {
                setTimeout(function () { $('#alert-delivering-mail').addClass('d-none'); }, 2000);
                $("#order-management").click();
            }
        });
    },
    deliveringForm: function (id, email) {
        $.ajax({
            url: "/Admin/Mail/DeliveringMail",
            type: "GET",
            data: { orderId: id },
            dataType: "html",
            success: function (content) {
                sendMail.sendDeliveringForm(content, email);
            }
        });
    },
    sendCancelledForm: function (content, email) {
        $.ajax({
            url: "/Admin/Mail/SendCancelledMail",
            type: "POST",
            data: { content: content, email: email },
            dataType: "json",
            success: function (data) {
                setTimeout(function () { $('#alert-cancelled-mail').addClass('d-none'); }, 2000);
                $("#order-management").click();
            }
        });
    },
    cancelledForm: function (id, email) {
        $.ajax({
            url: "/Admin/Mail/CancelledMail",
            type: "GET",
            data: { orderId: id },
            dataType: "html",
            success: function (content) {
                sendMail.sendCancelledForm(content, email);
            }
        });
    },
    sendCompletedForm: function (content, email) {
        $.ajax({
            url: "/Admin/Mail/SendCompletedMail",
            type: "POST",
            data: { content: content, email: email },
            dataType: "json",
            success: function (data) {
                setTimeout(function () { $('#alert-delivering-mail').addClass('d-none'); }, 2000);
                $("#order-management").click();
            }
        });
    },
    completedForm: function (id, email) {
        $.ajax({
            url: "/Admin/Mail/CompletedMail",
            type: "GET",
            data: { orderId: id },
            dataType: "html",
            success: function (content) {
                sendMail.sendCompletedForm(content, email);
            }
        });
    }
};