
window.validate();
//submit edit book
function editBook() {
    window.createOrEditBook("/Admin/Book/Edit");
}
//back to list book on click
$("#back-to-list-book").on("click", function (e) {
    window.loadPage(e, "/Admin/Book/Index");
});

