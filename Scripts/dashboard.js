function deleteListItem(itemId, itemName) {
    $("#deleteWarning").text("Are you sure you want to delete " + itemName + "?");
    $("#delBtn").attr("href", "/list/DeleteListItem/" + itemId);
}

function deleteListHeader(headerId, listName) {
    $("#deleteWarning").text("Are you sure you want to delete " + listName + "?");
    $("#delBtn").attr("href", "/list/DeleteListHeader/" + headerId);
}

function showAjaxModal() {
    $("#ajaxModal").modal("show");
}