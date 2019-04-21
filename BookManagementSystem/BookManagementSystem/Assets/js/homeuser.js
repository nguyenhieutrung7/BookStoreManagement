function LoadData(response) {
    $("#main-content").html(response);
}
$(document).ready(function () {
    function removeActive() {
        $("#list-authors a li.active").removeClass("active");
        $("#list-categories a li.active").removeClass("active");
        $("#main-menu li a.font-italic").removeClass('font-italic');
    }
    $(".row .product a").on("click", function (event) {
        event.preventDefault();
        var obj = $(event.target);
        var id = obj.attr("id");
        detailAjax(id);
        
    });
    function detailAjax(id) {
        $.ajax({
            url: "/Home/Detail?id=" + id,
            type: "GET",
            dataType: "html",
            success: function (response) {
                $("#main-content").html(response);
                window.history.pushState({},"Book Detail", "/Home/DetailBookPage/" + id);
            }
        });
    }
    $("#list-authors a").off("click").on("click", function (event) {
        event.preventDefault();
        removeActive();
        $(this).children().addClass("active");
        //get id a tag
        var obj = $(event.target);
        //alert();
        loadAjax(obj.attr("id"), 0);
    });
    $("#list-categories a").off("click").on("click", function (event) {
        event.preventDefault();
        removeActive();
        $(this).children().addClass("active");
        //get id a tag
        var obj = $(event.target);
        loadAjax(0, obj.attr("id"));
    });
    function loadAjax(authorID, categoryID) {
        var url = "/Home/PartialIndex?authorID=" + authorID + "&&categoryID=" + categoryID;
        $.ajax({
            cache: false,
            async:true,
            url: url,
            type: "GET",
            dataType: "html",
            success: function (response) {
                $("#main-content").html(response);
            }
        });
        
    }
    //return home page
    $("#logo").on("click", function (e) {
        e.preventDefault();
        removeActive();
        $("#home-page").addClass('font-italic');
        returnHomePage();
    });
    $("#home-page").off("click").on("click", function (e) {
        removeActive();
        $(this).addClass('font-italic');
        e.preventDefault();
        returnHomePage();
    });
    window.returnHomePage=function() {
        $.ajax({
            url: "/Home/PartialIndex?authorID=0&&categoryID=0",
            type: "GET",
            dataType: "html",
            success: function (response) {
                console.log(response);
                $("#main-content").html(response);
            }
        });
    }
    //view shopping cart
    $("#view-shopping-cart").off("click").on("click", function (e) {
        e.preventDefault();
        removeActive();
        $.ajax({
            url: "/Cart/Index",
            type: "GET",
            dataType: "html",
            success: function (response) {
                $("#main-content").html(response);
                window.history.pushState({},"Shopping cart","/");
            }
        });
    });
    //buy now click
    $(".buy-now").off("click").on("click", function () {
        //get id of img tag 
        var obj = $(this).parent().siblings().first().children("a").children("img");
        //var price = $(this).siblings().children('.price').text();
        var id = obj.attr("id");
        var quantityObj = $(this).siblings('.quantity');
        if (parseInt(quantityObj.val()) > 0) {
            $.ajax({
                url: "/Cart/AddToCart?bookID=" + id + "&&quantity=" + 1,
                type: "GET",
                dataType: "html",
                success: function (respone) {
                    if ($("#cart-index") != undefined) {
                        $("#cart-index").remove();
                    }
                    $("#main-content").append(respone);
                    $("#alert-success").removeClass("d-none");
                    $("#total-price-in-cart").text($("#cart-index #total-price").text());
                    $("#number-items-in-cart").text(parseInt($("#number-items-in-cart").text()) + 1);
                    $("#main-content #cart-index").addClass("d-none");
                    //$("#alert-success").removeClass("d-none");
                    setTimeout(function () { $("#alert-success").addClass("d-none"); }, 1500);
                    //update quantity in cart icon
                    //var number = $("#number-items-in-cart");
                    //number.text(parseInt(number.text()) + parseInt(1));
                    ////update money
                    ////var money = $("#total-price-in-cart");
                    ////money.text(parseInt(money.text()) + parseInt(price) * parseInt(1));
                    //$("#quantity-in-stock").attr("max", parseInt($("#quantity-in-stock").attr("max")) - parseInt(1));
                   


                    //var money = $("#total-price-in-cart");
                    //if ($("#user-current-langauge").text() == "vi") {
                    //    money.text(numeral((parseFloat(money.text()) + parseFloat(price))).format('0.000') + " đ");
                    //}
                    //else {
                    //    var num = parseFloat(money.text().substr(1)) + parseFloat(price.substr(1));
                    //    money.text(numeral(num).format('$0,0'));
                    //}
                    quantityObj.val(parseInt(quantityObj.val()) - 1);
                }
            });
        }
        else {
            alert($("#out-of-stock").text());
        }
    });

    //view profile
    $("#view-profile").off("click").on("click", function (e) {
        e.preventDefault();
        $.ajax({
            url: "/Profile/Index",
            type: "GET",
            dataType: "html",
            success: function (response) {
                if ($(response).find("#login-form").length > 0) {
                    var rs = confirm($("#login-confirm").text());
                    if (rs) {
                        window.location.href = "/Account/Login";
                    }
                   
                }
                else {
                    removeActive();
                    $("#view-profile").addClass('font-italic');
                    $("#main-content").html(response);
                }
                
            },
            error: function (err) {
                alert(err);
            }
        });
    });
});