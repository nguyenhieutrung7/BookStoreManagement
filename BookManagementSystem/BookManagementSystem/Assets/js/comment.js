
$(document).ready(function () { 

    //Show modal comment
    $('.showComment').off("click").on("click", function () {
        var id = $(this).closest("tr").attr("id");
        Reply(id);
    });    
    //Show modal delete comment 
    $('.showDelete').off("click").on("click", function () {
        var id = $(this).closest("tr").attr("id");
        $("#delete-comment").empty();
        var row = '<button class="btn btn-success" onclick="Delete(' + id + ')">' + $("#txtDelete").text() + '</button><button class="btn btn-danger" data-dismiss="modal" >' + $("#txtClose").text()+'</button >';
        $("#delete-comment").append(row);
        $("#modelDel").modal();
    });
    //Even delete all comment
    $('.delete-all').off("click").on("click", function () {
        //alert("vao ham");      
        var child = $('tbody tr').not(':has(.dataTables_empty)').length;
        var notdata = $("#not-data").text();
        if (child == 0) {
            alert(notdata);         
        }           
        else {
            $("#deleteAll-comment").empty();
            var row = '<button class="btn btn-success" onclick="DeleteAll()">' + $("#txtDelete").text() + '</button><button class="btn btn-danger" data-dismiss="modal" >' + $("#txtClose").text() +'</button >';
            $("#deleteAll-comment").append(row);
            $("#modelDelAll").modal();
        }
        
    });
    
    
   
});
// Convert DateTime on json
function ToJavaScriptDate(value) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
}
// Show Reply content of that comment
function Reply(id) {
    //alert("vào hàm");
    var myModal = $("#modelRep");   
    //alert(id);
    $.ajax({
        url: "/Admin/Comment/LoadReply",
        type: "GET",
        data: { idComment: id },
        dataType: "json",
        success: function (data) {
            //alert("yeah");
            $("#loadModal").empty();
            $.each(data, function (i, item) {

                var rows = '<div>';
                rows += '<h5>' + item.AccountName + ' <small><i>' + $("#txtPost").text() + ToJavaScriptDate(item.ReplyDate) + '</i></small></h5>';
                rows += '<p class="offset-lg-1">' + item.ReplyContent + '</p>';
                rows += '</div>';
                rows += '<hr/>';
                $("#loadModal").append(rows);
            });
            var html = '<form class="mt-1" novalidate><textarea class="form-control" rows="3" id="comment" placeholder="' + $("#txtEnter").text() +'..." required></textarea></form>';
            html += '<div class="modal-footer" ><button type="button" class="btn btn-success" onclick="Add(' + id + ')">' + $("#txtReply").text() +'</button><button type="button" class="btn btn-danger" data-dismiss="modal">'+ $("#txtClose").text() +'</button></div>';
            $("#loadModal").append(html);
            myModal.modal();
        }
    });

}
// Add reply
function Add(id) {
    //alert("vao ham");
    if ($("#comment").val() == "")
        alert("Please enter reply!!");
    else {
        $.ajax({
            url: "/Admin/Comment/AddReply",
            data: {
                replyContent: $("#comment").val(),               
                idComment: id
            },
            type: "POST",
            dataType: "json",
            success: function (result) { 
                Reply(id);           
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
    
}
// Delete comment
function Delete(id) {
    //alert("vao ham");
    $.ajax({
        url: "/Admin/Comment/DeleteComment",
        data: { idComment: id },
        dataType: "json",
        type: "POST",
        success: function (data) {
            //alert("Success");
            $('.showDelete').off("click");
            $("#modelDel").modal('hide');
            setTimeout(function () {
                $("#comment-management").click();
            }, 300);
                
           
        }

    });
}
// Delete all comment
function DeleteAll() {
    $('#tbData > tbody  > tr').each(function () {
        var id = $(this).closest("tr").attr("id");
        $.ajax({
            url: "/Admin/Comment/DeleteComment",
            data: { idComment: id },
            dataType: "json",
            type: "POST",
            success: function (data) {
                $('.delete-all').off("click");
                $("#modelDelAll").modal('hide');
                setTimeout(function () {
                    $("#comment-management").click();
                }, 300);
            }

        });
    });
}
