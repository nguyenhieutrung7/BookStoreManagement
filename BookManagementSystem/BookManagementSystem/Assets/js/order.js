
   $(".show-detail").off("click").on("click", function () {
        //alert("vao ham");
        var id = $(this).closest("tr").attr("id");      
        LoadCustomer(id);
        $.ajax({
            url: "/Admin/Order/LoadOrderDetail",
            type: "GET",
            data: { orderID: id },
            dataType: "json",
            success: function (data) {
                $("#load-detail").empty();
                $.each(data, function (i, item) {
                    var rows = '<tr>';
                    rows += '<td>' + item.BookName + '</td>';
                    rows += '<td>' + item.Quantity + '</td>';     
                    rows += '<td>' + item.Money + '</td>';
                    rows += '</tr>';
                    $("#load-detail").append(rows);
                });
                
                $("#detail-modal").modal();
            }
        });
    });

    // update status order
    $(".click-confirm").off("click").on("click", function () {
        //alert("vao ham");
        
        var id = $(this).closest("tr").attr("id");
        var status = $(this).closest("button").attr("id");
        //alert(Status);
        //alert(id);
        $.ajax({
            url: "/Admin/Order/UpdateOrderStatus",
            type: "POST",
            dataType: "json",
            data: { orderID: id, idStatus:status },
            success: function (data) { 
                switch (status) {
                    case "1":
                            $('#alert-delivering-mail').removeClass('d-none');
                            sendMail.deliveringForm(id, data.email);
                        break;
                    case "2":
                            $('#alert-completed-mail').removeClass('d-none');
                            sendMail.completedForm(id, data.email);
                        break;
                    default:
                        break;
                    // code block
                }
                $("#order-management").click();
                
            }
        });
    });

    $("#check-all").click(function () {
        $('#tbData > tbody  > tr').each(function () {
            if (($(this).find('input').is(":disabled")) === false) {
                //alert($(this).is(':checked'));
                var flag = $("#check-all").is(':checked');             
                $(this).find('input').attr('checked', flag);
            }
            
        });
    });

$(".check-confirm").off("click").on("click", function () {   
    //alert("vao ham");
        var flag = 0;
        var dem_1 = 0;
        var dem_2 = 0;

        $('#tbData > tbody  > tr').each(function () {
            if (($(this).find('input').is(":checked")) === true) {              
                var id = $(this).closest("tr").attr("id");
                var status = $(this).find(".click-confirm").attr("id");
                //alert(id);
                $.ajax({
                    url: "/Admin/Order/UpdateOrderStatus",
                    type: "POST",
                    dataType: "json",
                    data: { orderID: id, idStatus: status },
                    success: function (data) {
                        switch (status) {
                            case "1":
                                    $('#alert-delivering-mail').removeClass('d-none');
                                    sendMail.deliveringForm(id, data.email);
                                break;
                            case "2":
                                    $('#alert-completed-mail').removeClass('d-none');
                                    sendMail.completedForm(id, data.email);
                                break;
                            default:
                            // code block
                        }
                    }
                });   
                flag = 1;
                if (status ==1)
                    dem_1 += 1; //status ordered
                else
                    dem_2 += 1; // status delivering
            }           
        });
        if (flag === 0) {
            $("#warning-alert").removeClass("d-none");
            setTimeout(function () {
                $("#warning-alert").addClass("d-none");              
            }, 2000);
            
            //$("#warning-alert").addClass("d-none");
        }          
        else {
            var slcf = dem_1;
            var slcp = dem_2;           
            var row = $("#have").text() + slcf + $("#slconfirm").text() + slcp + $("#slcomplete").text();
            $("#success-alert").append(row);
            $("#success-alert").removeClass("d-none");
            $("#order-management").click();
          
        }      
    });

    $(".click-cancel").off("click").on("click", function () {
        //alert("vao ham");
        var id = $(this).closest("tr").attr("id");       
        //alert(Status);
        //alert(id);
        $.ajax({
            url: "/Admin/Order/CancelOrder",
            type: "POST",
            dataType: "json",
            data: { orderID: id},
            success: function (data) {
                $('#alert-cancelled-mail').removeClass('d-none');
                sendMail.cancelledForm(id, data.email);
                
            }
        });
    });

    $(".btn-refresh").off("click").on("click", function () {
        $("#order-management").click();
    });
function LoadCustomer(id) {   
    $.ajax({
        url: "/Admin/Order/LoadInfoCustomer",
        type: "GET",
        data: { orderID: id },
        dataType: "json",
        success: function (data) {
            $("#accountName").val(data.CustomerName);
            $("#email").val(data.Email);
            $("#address").val(data.Address);
            $("#phone").val(data.Phone);
            
        }
    });
}



