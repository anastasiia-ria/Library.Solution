$(document).ready(function () {
  $("#edit-room").click(function () {
    $(".delete-shelf").toggle();
    console.log("delete");
  });
  $(document).on("click", ".delete-shelf", function (event) {
    event.preventDefault();
    console.log($(this).parent().attr("id"));
    let id = $(this).parent().attr("id").slice(6);
    console.log(id);
    $.ajax({
      type: "POST",
      url: "../../Shelves/Delete",
      data: { id: id },
      success: function () {
        console.log("removing shelf ", id);
        $(`#shelf-${id}`).remove();
      },
    });
  });
});
