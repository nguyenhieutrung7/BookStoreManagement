var bookStocks = {
    getData: function () {
        $.ajax({
            type: "GET",
            url: "https://localhost:44356/bookstocks",
            cache: "false",
            success: function (data) {
                $('#bookstocks-tbody').empty();
                $.each(data, function (key, item) {
                    var tr = $("<tr></tr>");
                    tr.append($("<td></td>").append(item.title));
                    tr.append($("<td></td>").append(item.description));
                    tr.append($("<td></td>").append(item.categoryName));
                    tr.append($("<td></td>").append(item.authorName));
                    tr.append($("<td></td>").append(item.isbn));
                    tr.append($("<td></td>").append(item.price));
                    tr.append($("<td></td>").append(item.publisherName));
                    tr.append($("<td></td>").append(item.quantity));
                    $('#bookstocks-tbody').append(tr);
                });
            }
        });
    }
};