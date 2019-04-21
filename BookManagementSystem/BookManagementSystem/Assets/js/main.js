/*--------------Bootstrap Validation----------------------*/
// Example starter JavaScript for disabling form submissions if there are invalid fields
window.validate = function () {
    if (document.forms.length > 0) {
        (function () {
            'use strict';
            for (var i = 0; i < document.forms.length; i++) {
                document.forms[i].addEventListener('click', function () {
                    // Get the forms we want to add validation styles to
                    var forms = $('.needs-validation');
                    // Loop over them and prevent submission
                    var validation = Array.prototype.filter.call(forms, function (form) {
                        form.addEventListener('submit', function (event) {
                            if (form.checkValidity() === false) {
                                event.preventDefault();
                                event.stopPropagation();
                            }
                            form.classList.add('was-validated');
                        }, false);
                    });
                }, false);
            }

        })();
    }
}
window.appendContainer = function (html) {
    //$(".list-group > list-group-item").removeClass("active");
    $("#page-content-wrapper").html(html);
}

//load all books
window.loadPage = function (e, url) {
    e.preventDefault();
    $.ajax({
        type: "GET",
        url: url,
        dataType: "html",
        success: function (obj) {
            //$.when().done(function () {
            appendContainer(obj);
            //$(".list-group-item.active").removeClass("active");
            //$("#book-management").addClass("active");
            //});
        },
        error: function () {
            alert("Error");
        }
    });
}

//view book category
$("#book-category-management").on("click", function (e) {
    $.ajax({
        url: "/Admin/Category/Index",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#page-content-wrapper").html(data);
            $(".list-group > .list-group-item.active").removeClass("active");
            $("#book-category-management").addClass("active");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});

//view book management
$("#book-management").on("click", function (e) {
    $(".list-group > .list-group-item").addClass("disabled");
    window.loadPage(e, "/Admin/Book/Index");
    $(".list-group > .list-group-item.active").removeClass("active");
    $("#book-management").addClass("active");
    $(".list-group > .list-group-item.disabled").removeClass("disabled");
});

//home admin click
$("#home-admin").on("click", function (e) {
    $(".list-group > .list-group-item").addClass("disabled");
    $("#page-content-wrapper").empty();
    $(".list-group > .list-group-item.active").removeClass("active");
    $(this).addClass("active");
    $(".list-group > .list-group-item.disabled").removeClass("disabled");
});

//author management click
$("#author-management").on("click", function (e) {
    $(".list-group > .list-group-item").addClass("disabled");
    window.loadPage(e, "/Admin/Author/Index");
    $(".list-group > .list-group-item.active").removeClass("active");
    $("#author-management").addClass("active");
    $(".list-group > .list-group-item.disabled").removeClass("disabled");
});

//view account
$("#account-management").on("click", function (e) {
    $(".list-group > .list-group-item").addClass("disabled");
    $.ajax({
        url: "/Admin/Account/Index",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#page-content-wrapper").html(data);
            $(".list-group > .list-group-item.active").removeClass("active");
            $("#account-management").addClass("active");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $(".list-group > .list-group-item.disabled").removeClass("disabled");
});

//view account
$("#publisher-management").on("click", function (e) {
    $(".list-group > .list-group-item").addClass("disabled");
    $.ajax({
        url: "/Admin/Publisher/Index",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "html",
        success: function (data) {
            $("#page-content-wrapper").html(data);
            $(".list-group > .list-group-item.active").removeClass("active");
            $("#publisher-management").addClass("active");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $(".list-group > .list-group-item.disabled").removeClass("disabled");

});
//comment management click
$("#comment-management").on("click", function () {
    $(".list-group > .list-group-item").addClass("disabled");
    //alert("vao ham");
    $("#sidebar-wrapper").show();

    $.ajax({
        url: "/Admin/Comment/CommentManagement",
        type: "GET",
        dataType: "html",
        success: function (data) {
            $(".list-group > .list-group-item.active").removeClass("active");
            $("#comment-management").addClass("active");
            $("#page-content-wrapper").html(data);
        }
    });
    $(".list-group > .list-group-item.disabled").removeClass("disabled");
});

//order management click
$("#order-management").on("click", function () {
    $(".list-group > .list-group-item").addClass("disabled");
    $("#sidebar-wrapper").show();
    
    $.ajax({
        url: "/Admin/Order/OrderManagement",
        type: "GET",
        dataType: "html",
        success: function (data) {
            $(".list-group > .list-group-item.active").removeClass("active");
            $("#order-management").addClass("active");        
            $("#page-content-wrapper").html(data);
           
            
        }
    });
    $(".list-group > .list-group-item.disabled").removeClass("disabled");
});