$(document).ready(function () {
    $('.btnSend').off("click").on("click", function () {   
        var content = $("#txtComment").val();
        var id = $(this).attr("id");      
        if (content == "") {
            alert($("#input-your-comment-message").text());
        }           
        else {
            $.ajax({
                url: "/Home/SendComment",
                dataType: "json",
                type: "POST",
                data: { commentContent: content, bookID: id },
                success: function (data) {
                    location.reload();
                }
            });
        }
        
    });
});