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
        $("#book-title").text(result.thisBook[0].title);
        $("#book-authors").text(result.thisBook[0].authors);
      },
    });
  });
  $("#edit-room").click(function () {
    $(".delete-shelf").toggle();
  });

  $("#search").on("click", function (event) {
    event.preventDefault();
    $("#search-results").empty();
    let search = $("#general").val();
    $.ajax({
      type: "GET",
      url: "../../Books/Search",
      data: { search: search },
      success: function (result) {
        result.books.forEach(function (book) {
          $("#search-results").append(`<div class="card" style="width: 15rem;">
            <img class="card-img-top" src="https://books.google.com/books/content?id=${book.imgID}&printsec=frontcover&img=1&zoom=5" alt="Book thumbnail" height="240px" width="auto">
            <div class="card-body">
              <h5 class="card-title">${book.title}</h5>
              <p class="card-text">${book.authors}</p>
              <buttton id="${book.imgID}" class="btn btn-light add-book-from-search">Add</button>
            </div>
          </div>`);
        });
      },
    });
  });
  $(".add-book-from-search").on("click", function (event) {
    event.preventDefault();
    let id = $(this).attr("id");
    console.log("here id is " + id);
    // $.ajax({
    //   type: "GET",
    //   url: "../../Books/Create",
    //   data: { id: id },
    //   success: function (result) {
    //     $("#title-input").val() = result.book.title;
    //     $("#authors-input").val() = result.book.authors;
    //     $("#publisher-input").val() = result.book.publisher;
    //     $("#published-date-input").val() = result.book.published-date;
    //   },
    // });
  });
});
