$(document).ready(function () {
    $('.cancel-order').off("click").on("click", function () {
        var id_order = $(this).parent().siblings().children('p').children('span.order-id').text();
        bootbox.confirm({
            title: $("#confirm-title").text(),
            message: $("#confirm-message").text(),
            buttons: {
                confirm: {
                    label: $("#yes-result").text(),
                    className: "btn-success"
                },
                cancel: {
                    label: $("#no-result").text(),
                    className: "btn-danger"
                }
            },
            callback: function (res) {
                if (res) {                   
                    $.ajax({
                        url: "/Home/UpdateStatusOrder_Customer",
                        type: "POST",
                        data: { orderID: id_order},
                        dataType: "json",
                        success: function (data) {
                            $("#view-profile").trigger("click");
                        }
                    });
                }
            }
        });
    });
    
});