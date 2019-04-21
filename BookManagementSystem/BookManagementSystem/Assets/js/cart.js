$(document).ready(function () {
    //continue shopping click
    $(".row #continue-shopping").on("click", function (e) {
        e.preventDefault();
        window.returnHomePage();
    });
    //create move item from cart
    $(".delete-item").on("click", function (e) {
        e.preventDefault();
       
        var id = $(this).closest("tr").attr("id");
        //var deletedQuantity = $(this).closest("tr").find("td:eq(3) input[type='number']").val();
        //var price = $(this).closest("tr").find("td:eq(5)").text();
        $.ajax({
            url: "/Cart/RemoveFromCart?bookID=" + id,
            type: "GET",
            dataType: "html",
            success: function (respone) {
                $("#main-content").html(respone);
                $("#total-price-in-cart").text($("#total-price").text());
                var sum = 0;
                if ($("#cart-table-body tr#empty-data").length == 0) {
                    $("#cart-table-body tr").each(function () {
                        sum = parseFloat(sum) + parseInt($(this).find("td:eq(3) input[type='number']").val());
                    });
                }
                $("#number-items-in-cart").text(sum);
                //var money = $("#total-price-in-cart");
                //if ($("#cart-table-body tr#empty-data").length ==0) {
                //    alert("lon hon 1");
                //    if (deletedQuantity != "") {
                //        $("#number-items-in-cart").text(parseInt($("#number-items-in-cart").text()) - parseInt(deletedQuantity));
                //    }
                //    //update money
                    
                //    if ($("#user-current-langauge").text() == "vi") {
                //        money.text(numeral(parseFloat(money.text()) - parseFloat(price)).format('0.0,00') + " đ");
                //    }
                //    else {
                //        var num = parseFloat(money.text().substr(1)) - parseFloat(price.substr(1));
                //        money.text(numeral(num).format('$0,0'));
                //    }
                //}
                //else {
                //    alert("Nho");
                //    $("#number-items-in-cart").text("0");
                //    money.text(numeral(0).format('0.0,00') + " đ");
                //}

                
            }
        });
    });

    //prevent e,+,- keys
    $(".quantity-book").off("keydown").on("keydown", function (e) {
        window.checkInvalidChars(e);
        $(".delete-item").addClass("disabled");
    });
    function updateCartAjax(bookID, newQuantity) {
        $.ajax({
            url: "/Cart/UpdateFromCart?bookID=" + bookID + "&&newQuantity=" + newQuantity,
            type: "GET",
            dataType: "html",
            success: function (respone) {
                console.log(respone);
                $("#main-content").html(respone);
                $("#total-price-in-cart").text($("#total-price").text());
                var sum = 0;
                $("#cart-table-body tr").each(function () {
                    sum = parseFloat(sum) + parseInt($(this).find("td:eq(3) input[type='number']").val());
                });
                $("#number-items-in-cart").text(sum);
                //var rate = $("#currency-rate").text();
                //var totalPriceDollar = respone.sumTotal * parseFloat(rate);
                //var totalQuantityInCart = respone.sumQuantity;
                //var sum = 0;
                //$("#cart-table-body tr").each(function () {
                //    sum = parseFloat(sum) + parseFloat($(this).find("td:eq(5)").text());
                //});
                //var totalPrice = sum.toFixed(3);
                //console.log(totalPrice);
                //var totalPrice = 0;
                //var totalQuantityInCart = 0;
                //
                //$.each(respone.ListCartItems, function (key, value) {
                //    totalQuantityInCart += parseInt(value.Quantity);
                //    totalPrice = parseFloat(totalPrice) + parseFloat(value.Book.Price * parseFloat(rate)) * value.Quantity;
                //    //}
                //    //else {
                //    //    totalPrice = parseFloat(totalPrice) + parseFloat(value.Book.Price) * value.Quantity;
                //    //}
                //    //if ($("#user-current-langauge").text() == "vi") {
                    
                //});
                
                //$("#number-items-in-cart").text(totalQuantityInCart);
                //if ($("#user-current-langauge").text() == "vi") {
                //    //alert(totalPrice.toString().split('.').pop());
                //    //if (totalPrice.toString().split('').pop().length == 2) {
                //        $("#total-price-in-cart").text(totalPrice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + " đ");
                //        $("#total-price").text(totalPrice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + " đ");
                //    //}
                //    //else {
                //    //    $("#total-price-in-cart").text(totalPrice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + "0 đ");
                //    //    $("#total-price").text(totalPrice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + "0 đ");
                //    //}
                    
                //}
                //else {
                //    $("#total-price-in-cart").text(numeral(totalPriceDollar).format('$0,0'));
                //    $("#total-price").text(numeral(totalPriceDollar).format('$0,0'));
                //}
            }
        });
    }
    //check valid quantity and update
    $(".quantity-book").off("keyup").on("keyup", function (e) {
        if (parseInt($(this).val()) > parseInt($(this).attr("max"))) {
            $(this).val($(this).attr("max"));
        }
        $(".delete-item").removeClass("disabled");
    });
    $(".quantity-book").change(function () {
        var newQuantity = $(this).val();
        if (newQuantity.trim().length <= 0 || parseInt(newQuantity.trim()) == 0) {
            newQuantity = "0";
        }
        var bookID = $(this).closest("tr").attr("id");
        updateCartAjax(bookID, newQuantity);
        //var totalObj = $(this).closest("tr").find("td:eq(5)");
        //var price = $(this).closest("tr").find("td:eq(4)").text();
        //if ($("#user-current-langauge").text() == "vi") {
        //    totalObj.text(numeral(parseInt(newQuantity) * parseFloat(price)).format('0.000') + ' đ');
        //}
        //else {
        //    totalObj.text(numeral(parseInt(newQuantity) * parseFloat(price.substr(1))).format('$0,0'));
        //}
        if (parseInt(newQuantity.trim()) == 0) {
            $(this).closest("tr").find("td:eq(6)").children().trigger("click");
        }
    });

    //check out
    $("#check-out").off("click").on("click", function (e) {
        e.preventDefault();
        //check number items correct?
        var items = $("#cart-table-body tr");

        $.each(items, function (key, value) {
            var quantity = $(value).find("td:eq(3) input[type='number']").val();
            if (quantity == "" || quantity == "0") {
                $(value).find("td:eq(3) input[type='number']").focus();
                alert($("#incorrect-quantity-message").text());
                return;
            }
        });
        //call ajax
        $.ajax({
            url: "/Cart/ConfirmOrder",
            type: "GET",
            dataType: "html",
            success: function (response) {
                if ($(response).find("#login-form").length > 0) {
                    window.location.href = "/Account/Login";
                }
                else {
                    $("#main-content").html(response);
                }
            }
        });
    });

});