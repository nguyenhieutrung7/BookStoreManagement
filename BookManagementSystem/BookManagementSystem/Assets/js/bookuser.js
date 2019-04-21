var invalidChars = [
    "-",
    "e",
    "+"
];
//add to cart function
window.addToCart = function (bookID, quantity, price) {
    $.ajax({
        url: "/Cart/AddToCart?bookID=" + bookID + "&&quantity=" + quantity,
        type: "GET",
        dataType: "html",
        success: function (respone) {
            if ($("#cart-index") != undefined) {
                $("#cart-index").remove();
            }
            $("#main-content").append(respone);
            $("#main-content #cart-index").addClass("d-none");
            $("#alert-success").removeClass("d-none");
            $("#total-price-in-cart").text($("#total-price").text());
            $("#number-items-in-cart").text(parseInt($("#number-items-in-cart").text()) + parseInt(quantity));
            setTimeout(function () { $("#alert-success").addClass("d-none"); }, 1500);
            ////update quantity in cart icon
            //var number = $("#number-items-in-cart");
            //number.text(parseInt(number.text()) + parseInt(quantity));
            ////update money
            //var money = $("#total-price-in-cart");
            //if ($("#user-current-langauge").text() == "vi") {
            //    money.text(numeral((parseFloat(money.text()) + parseFloat(price) * parseFloat(quantity))).format('0.0,00') + " đ");
            //}
            //else {
            //    var num = parseFloat(money.text().substr(1)) + parseFloat(price.substr(1)) * parseFloat(quantity);
            //    money.text(numeral(num).format('$0,0'));
            //}
            
            $("#quantity-in-stock").attr("max", parseInt($("#quantity-in-stock").attr("max")) - parseInt(quantity));
        }
    });
}
$(document).ready(function () {
    $("#add-to-cart").off("click").on("click", function (e) {
        e.preventDefault();
        var bookID = $("#BookID").val();
        var quantity = $("#quantity").val();
        var price = $("#price").text();
        //check quantity greater than quantity in stock
        if (parseInt(quantity) > parseInt($("#quantity-in-stock").attr("max"))) {
            var message = $("#alert-not-enough-head").text() + $("#quantity-in-stock").attr("max") + $("#alert-not-enough-tail").text();
            alert(message);
            $("#quantity").attr("max", $("#quantity-in-stock").attr("max"));
            $("#quantity").val($("#quantity-in-stock").attr("max"));
            if ($("#quantity").attr("max") === "0") {
                $("#add-to-cart").addClass("disabled");
                $("#status").text("Out of stock");
            }
            return;
        }
        if (parseInt($("#quantity").val()) === 0 || $("#quantity").val() === "") {
            alert($("#enter-quantity-book-message").text());
            $("#quantity").val("1");
            return;
        }
        addToCart(bookID, quantity, price);
    });
    
    window.checkInvalidChars = function (e) {
        if (invalidChars.filter(m => m === e.key).length > 0) {
            e.preventDefault();
        }
    }
    //prevent enter + - e characters
    $("#quantity").off("keydown").on("keydown", function (e) {
        checkInvalidChars(e);
    });
    $("#quantity").off("keyup").on("keyup", function (e) {
        if (parseInt($(this).val()) > parseInt($("#quantity-in-stock").attr("max"))) {
            var message = $("#alert-not-enough-head").text() + $("#quantity-in-stock").attr("max") + $("#alert-not-enough-tail").text();
            alert(message);
            $(this).val($("#quantity-in-stock").attr("max"));
        }
    });
});