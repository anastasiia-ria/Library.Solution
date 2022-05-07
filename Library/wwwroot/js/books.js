let Search = function () {
  this.general = "";
  this.title = "";
  this.authors = "";
  this.publisher = "";
  this.isbn = "";
  this.startIndex = 0;
  this.size = 0;
};

Search.prototype.clear = function () {
  this.general = "";
  this.title = "";
  this.authors = "";
  this.publisher = "";
  this.isbn = "";
  this.startIndex = 0;
  this.size = 0;
};

let search = new Search();

function searchAPI(search) {
  $.ajax({
    type: "GET",
    url: "../../Books/Search",
    data: { general: search.general, title: search.title, authors: search.authors, publisher: search.publisher, isbn: search.isbn, startIndex: search.startIndex },
    success: function (result) {
      console.log(result);
      search.size = result.size;
      result.books["$values"].forEach(function (book) {
        $("#search-results").append(`<div class="card">
          <img class="card-img-top" src="https://books.google.com/books/content?id=${book.imgID}&printsec=frontcover&img=1&zoom=5" alt="Book thumbnail">
          <div class="card-body">
            <h5 class="card-title cut-text">${book.title}</h5>
            <p class="card-text cut-text">${book.authors}</p>
            <button type="button" id="add-${book.imgID}" class="btn btn-light add-book-from-search">Add</button>
          </div>
        </div>`);
      });
    },
    error: function () {
      var search = document.getElementById("search");
      $("#advanced-search-form input").val("");
      $(".search-title input").val("");
      $("#pagination").hide();
      search.setCustomValidity("Your search did not match any books. Please, try different keywords.");
      search.reportValidity();
    },
  });
}

$(document).ready(function () {
  $("#edit-books").click(function () {
    $(".delete-book").toggle();
  });

  $(".button-search").on("click", function (event) {
    event.preventDefault();
    search.clear();
    $("#search-results").empty();
    $("#add-book-form").hide();
    $("#search-results").show();
    $("#pagination").show();
    $("#search").prev().hide();
    let general = $("#general").val();
    let title = $("#title").val();
    let authors = $("#authors").val();
    let publisher = $("#publisher").val();
    let isbn = $("#isbn").val();

    search.general = general;
    search.authors = authors;
    search.title = title;
    search.publisher = publisher;
    search.isbn = isbn;
    searchAPI(search);
  });

  $(document).on("click", ".add-book-from-search", function (event) {
    event.preventDefault();
    $("#add-book-form").show();
    $("#search-results").hide();
    $("#advanced-search-form").hide();
    $("#pagination").hide();
    let id = $(this).attr("id").slice(4);
    $.ajax({
      type: "GET",
      url: "../../Books/Create",
      data: { id: id },
      success: function (result) {
        var book = result.book;
        $("input[name='Title']").val(book.title);
        $("input[name='Authors']").val(book.authors);
        $("textarea[name='Description']").val(book.description);
        $("input[name='Publisher']").val(book.publisher);
        $("input[name='PublishedDate']").val(book.publishedDate);
        $("input[name='ISBN_10']").val(book.isbN_10);
        $("input[name='ISBN_13']").val(book.isbN_13);
        $("input[name='PageCount']").val(book.pageCount);
        $("input[name='ImgID']").val(book.imgID);
      },
    });
  });

  $(document).on("click", "#advanced-search", function (event) {
    event.preventDefault();
    $("#advanced-search-form").slideToggle();
  });

  $(document).on("click", "#start-search", function (event) {
    event.preventDefault();
    $("#search-results").empty();
    $("#search").prev().hide();
    $("#pagination").show();
    search.clear();
    search.general = "programming";
    searchAPI(search);
  });

  $(".show-book-details").click(function () {
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

  $("#page-prev").click(function () {
    $("#search-results").empty();
    search.startIndex -= 8;
    if (search.startIndex === 0) {
      $(this).parent().addClass("disabled");
    }
    if (search.startIndex <= search.size - 8) {
      $("#page-next").parent().removeClass("disabled");
    }

    searchAPI(search);
  });

  $("#page-next").click(function () {
    $("#search-results").empty();
    search.startIndex += 8;
    if (search.startIndex >= search.size - 8) {
      $(this).parent().addClass("disabled");
    }
    if (search.startIndex >= 8) {
      $("#page-prev").parent().removeClass("disabled");
    }
    searchAPI(search);
  });
  $("#back-to-search").click(function () {
    $("#add-book-form").hide();
    $("#search-results").show();
    $("#pagination").show();
    $("#search-results").empty();
    $("#add-book-form input").val("");
    searchAPI(search);
  });
});
