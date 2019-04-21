window.validate();
//back to list book on click
$("#back-to-list-book").on("click", function (e) {
    window.loadPage(e, "/Admin/Book/Index");
});

function createNewBook() {
    window.createOrEditBook("/Admin/Book/Create");
}
