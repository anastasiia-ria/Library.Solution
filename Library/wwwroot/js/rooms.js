$(document).ready(function () {
  $("#rooms").children().first().removeClass("hidden");
  $("#rooms-nav > a").first().addClass("active");
  $("#rooms-nav > a").on("click", function () {
    $("#rooms > div").addClass("hidden");
    $("#rooms-nav > a").removeClass("active");
    $(this).addClass("active");
    let id = $(this).attr("id").slice(10);
    $(`#room-${id}`).removeClass("hidden");
  });
  let roomId = $(".getRoomId").attr("id");
  if (roomId != 0) {
    $("#rooms > div").addClass("hidden");
    $(`#room-${roomId}`).removeClass("hidden");
  }
  $(".books > img").click(function () {
    let id = parseInt($(this).attr("id").slice(5));
    $.ajax({
      type: "GET",
      url: "../../Books/Details",
      data: { id: id },
      success: function (result) {
        var book = result.thisBook;
        $("#book-title").text(book.title);
        $("#book-description").html(book.description);
        $("#book-authors").text(book.authors);
        $("#book-publisher").text(book.publisher);
        $("#book-publishedDate").text(book.publishedDate);
        $("#book-pageCount").text(book.pageCount);
        $("#book-isbn10").text(book.isbN_10);
        $("#book-isbn13").text(book.isbN_13);
        $("#book-img").attr("src", `https://books.google.com/books/content?id=${book.imgID}&printsec=frontcover&img=1&zoom=5`);
      },
    });
  });
  $("#edit-room").click(function () {
    $(".delete-shelf").toggle();
  });
});
