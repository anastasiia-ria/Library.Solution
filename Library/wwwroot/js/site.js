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

  $(".button-search").on("click", function (event) {
    event.preventDefault();

    $("#search").prev().hide();
    $("#search-results").empty();
    let general = $("#general").val();
    let title = $("#title").val();
    let authors = $("#authors").val();
    let publisher = $("#publisher").val();
    let isbn = $("#isbn").val();
    console.log(general);
    $.ajax({
      type: "GET",
      url: "../../Books/Search",
      data: { general: general, title: title, authors: authors, publisher: publisher, isbn: isbn },
      success: function (result) {
        result.books.forEach(function (book) {
          $("#search-results").append(`<div class="card">
            <img class="card-img-top" src="https://books.google.com/books/content?id=${book.imgID}&printsec=frontcover&img=1&zoom=5" alt="Book thumbnail">
            <div class="card-body">
              <h5 class="card-title">${book.title}</h5>
              <p class="card-text">${book.authors}</p>
              <button type="button" id="${book.imgID}" class="btn btn-light add-book-from-search">Add</button>
            </div>
          </div>`);
        });
      },
      error: function () {
        var search = document.getElementById("search");
        search.setCustomValidity("Your search did not match any books. Please, try different keywords.");
        search.reportValidity();
        $("#general").val("");
      },
    });
  });
  $(document).on("click", ".add-book-from-search", function (event) {
    event.preventDefault();
    $("#add-book-form").show();
    $("#search-results").hide();
    let id = $(this).attr("id");
    console.log("here id is " + id);
    $.ajax({
      type: "GET",
      url: "../../Books/Create",
      data: { id: id },
      success: function (result) {
        console.log(result.book);
        $("input[name='Title']").val(result.book.title);
        $("input[name='Authors']").val(result.book.authors);
        $("input[name='Description']").val(result.book.description);
        $("input[name='Publisher']").val(result.book.publisher);
        $("input[name='PublishedDate']").val(result.book.publishedDate);
        $("input[name='ISBN_10']").val(result.book.isbN_10);
        $("input[name='ISBN_13']").val(result.book.isbN_13);
        $("input[name='PageCount']").val(result.book.pageCount);
        $("input[name='ImgID']").val(result.book.imgID);
      },
    });
  });
  $(document).on("click", "#advanced-search", function (event) {
    event.preventDefault();
    $("#advanced-search-form").slideDown();
  });
  $(document).on("click", "#start-search", function (event) {
    event.preventDefault();
    $("#search").prev().hide();
    $.ajax({
      type: "GET",
      url: "../../Books/Search",
      data: { general: "Programming" },
      success: function (result) {
        result.books.forEach(function (book) {
          $("#search-results").append(`<div class="card">
            <img class="card-img-top" src="https://books.google.com/books/content?id=${book.imgID}&printsec=frontcover&img=1&zoom=5" alt="Book thumbnail">
            <div class="card-body">
              <h5 class="card-title">${book.title}</h5>
              <p class="card-text">${book.authors}</p>
              <button type="button" id="${book.imgID}" class="btn btn-light add-book-from-search">Add</button>
            </div>
          </div>`);
        });
      },
    });
  });
});
