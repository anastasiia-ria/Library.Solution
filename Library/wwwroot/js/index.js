let Search = function () {
  this.general = "";
  this.title = "";
  this.authors = "";
  this.publisher = "";
  this.isbn = "";
  this.startIndex = 0;
  this.size = 0;
};

let Pagination = function () {
  this.skip = 0;
  this.size = 0;
  this.use = "index";
  this.shelf = 0;
  this.room = 0;
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
Pagination.prototype.clear = function () {
  this.skip = 0;
  this.size = 0;
  this.use = "index";
  this.shelf = 0;
  this.room = 0;
};

let search = new Search();
let pagination = new Pagination();
let grid = false;
function showRating(rating) {
  let stars = "";
  for (let i = 1; i <= 5; i++) {
    if (rating > 0) {
      stars += '<i class="fa fa-star"></i>';
    } else {
      stars += '<i class = "fa fa-star-o"></i>';
    }
    rating--;
  }
  return stars;
}

function setScale() {
  console.log("scale");
  let id = parseInt($(".room").attr("id").slice(5));
  $.ajax({
    type: "GET",
    url: "../../Rooms/SetScale",
    data: { id: id },
    success: function (result) {
      let oldWidth = result.width;
      let newWidth = $(".room").width();
      let scale = newWidth / oldWidth;
      $(".room").css("transform", `scale(${scale})`);
    },
  });
}

function showBookDetails(id) {
  $("#bookDetails").modal("show");
  $("#edit-details").hide();
  $("#show-details").show();
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
      $("#book-language").text(book.language);
      $("#book-status").text(book.status);
      $("#book-categories").text(book.categories);
      $("#book-img").attr("src", `https://books.google.com/books/content?id=${book.imgID}&printsec=frontcover&img=1&zoom=1`);
      $(".rating").html(`${showRating(book.rating)}`);
      $(".remove-from-shelf").attr("data-book-id", book.bookId);
      $(".start-edit").attr("data-book-id", book.bookId);
    },
  });
}
function searchAPI() {
  $.ajax({
    type: "GET",
    url: "../../Books/Search",
    data: { general: search.general, title: search.title, authors: search.authors, publisher: search.publisher, isbn: search.isbn, startIndex: search.startIndex },
    success: function (result) {
      search.size = result.size;
      if (result.size <= 8) {
        $("#search-page-next").parent().addClass("disabled");
      }
      result.books.forEach(function (book) {
        $("#search-results").append(`<div class="card add-book-from-search" id="add-${book.imgID}">
          <img class="card-img-top" src="https://books.google.com/books/content?id=${book.imgID}&printsec=frontcover&img=1&zoom=1" alt="Book thumbnail">
          <div class="card-body">
            <h5 class="card-title cut-text">${book.title}</h5>
            <div class="cut-text">${book.authors}</div>
            <div class="rating">${showRating(book.rating)}</div>
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

function paginate() {
  $.ajax({
    type: "GET",
    url: "../../Books/Pagination",
    data: { skip: pagination.skip, shelfId: pagination.shelf },
    success: function (result) {
      pagination.size = result.size;
      if (result.size <= 8) {
        $("#books-page-next").parent().addClass("disabled");
      }
      if (pagination.use === "index") {
        result.books.forEach(function (book) {
          $("#books-page").append(`<div class="card show-book-details" id="book-${book.bookId}" data-bs-toggle="modal" data-bs-target="#bookDetails">
          <button type="submit" class="btn btn-light delete-book corner-icon"><span class="fa fa-close"></span></button>
          <img class="card-img-top" src="https://books.google.com/books/content?id=${book.imgID}&printsec=frontcover&img=1&zoom=1" alt="Book thumbnail" height="240px" object-fit="contain">
          <div class="card-body">
            <h5 class="card-title cut-text">${book.title}</h5>
            <p class="card-text cut-text">${book.authors}</p>
          </div>
        </div>`);
        });
      } else {
        if (result.books.length === 0) {
          $(".no-books").show();
          $("#r-pagination").hide();
        } else {
          $(".no-books").hide();
          $("#r-pagination").show();
          result.books.forEach(function (book) {
            $("#books-to-add").append(`<div class="card assign-location" id="book-${book.bookId}">
              <img class="card-img-top" src="https://books.google.com/books/content?id=${book.imgID}&printsec=frontcover&img=1&zoom=1" alt="Book thumbnail" height="240px" object-fit="contain">
              <div class="card-body">
                <h5 class="card-title cut-text">${book.title}</h5>
                <div class="cut-text">${book.authors}</div>
                <div class="rating">${showRating(book.rating)}</div>
              </div>
            </div>`);
          });
        }
      }
    },
    error: function () {},
  });
}

function scaleFunc(direction) {
  let str = $(".room").css("transform");
  let number = str.substring(str.indexOf("(") + 1, str.indexOf(","));
  let current = parseFloat(number);
  let scale = current;
  if (direction === "minus") {
    scale = current - 0.1;
  } else if (direction === "plus") {
    scale = current + 0.1;
  }
  $(".room").css("transform", `scale(${scale})`);
}

$(document).ready(function () {
  $(".rooms").each(function () {
    setScale();
  });
  $("#edit-books").click(function () {
    $(".delete-book").toggle();
  });
  $(document).on("click", "#cancel-edit", function () {
    $("#edit-details").hide();
    $("#show-details").show();
  });
  $(document).on("click", ".delete-book", function (event) {
    event.preventDefault();
    let id = parseInt($(this).closest(".card").attr("id").slice(5));
    $.ajax({
      type: "POST",
      url: "../../Books/Delete",
      data: { id: id },
      success: function () {
        pagination.size--;
        $(`#book-${id}`).remove();
        if (pagination.size <= pagination.skip && pagination.skip >= 8) {
          pagination.skip -= 8;
        }
        $("#books-page").empty();
        paginate();
      },
    });
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
    searchAPI();
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
        tinymce.init({
          selector: "#myTextarea",
        });
        $("input[name='Title']").val(book.title);
        $("input[name='Subtitle']").val(book.subtitle);
        $("input[name='Authors']").val(book.authors);
        $("textarea[name='Description']").val(book.description);
        $("input[name='Publisher']").val(book.publisher);
        $("input[name='PublishedDate']").val(book.publishedDate);
        $("input[name='ISBN_10']").val(book.isbN_10);
        $("input[name='ISBN_13']").val(book.isbN_13);
        $("input[name='Categories']").val(book.categories);
        $("input[name='Language']").val(book.language);
        $("input[name='PageCount']").val(book.pageCount);
        $("input[name='ImgID']").val(book.imgID);
        $("input[name='Rating']").val(book.rating);
      },
    });
  });

  $(document).on("click", ".start-edit", function (event) {
    event.preventDefault();
    $(".edit-details").show();
    $(".show-details").hide();
    let id = parseInt($(this).attr("data-book-id"));
    $.ajax({
      type: "GET",
      url: "../../Books/Details",
      data: { id: id },
      success: function (result) {
        var book = result.thisBook;
        console.log(book);
        tinymce.init({
          selector: "#myTextarea",
        });
        $("input[name='Title']").val(book.title);
        $("input[name='Subtitle']").val(book.subtitle);
        $("input[name='Authors']").val(book.authors);
        $("textarea[name='Description']").val(book.description);
        $("input[name='Publisher']").val(book.publisher);
        $("input[name='PublishedDate']").val(book.publishedDate);
        $("input[name='ISBN_10']").val(book.isbN_10);
        $("input[name='ISBN_13']").val(book.isbN_13);
        $("input[name='Categories']").val(book.categories);
        $("input[name='Language']").val(book.language);
        $("input[name='PageCount']").val(book.pageCount);
        $("input[name='ImgID']").val(book.imgID);
        $("input[name='Rating']").val(book.rating);
        $("input[name='BookId']").val(book.bookId);
      },
    });
  });

  $("#filter").on("submit", function (event) {
    event.preventDefault();

    let title = $("#title").val().toLowerCase();
    let authors = $("#authors").val().toLowerCase();
    let publisher = $("#publisher").val().toLowerCase();
    let isbn = $("#isbn").val().toLowerCase();
    let status = $("#status").val();
    $.ajax({
      type: "GET",
      url: "../../Rooms/Filter",
      data: { title: title, authors: authors, publisher: publisher, isbn: isbn, status: status },
      success: function (result) {
        result.books.forEach(function (book) {
          $(`#book-${book.bookId}`).addClass("highlight");
        });
      },
      error: function () {},
    });
  });

  $(document).on("click", "#advanced-search", function (event) {
    event.preventDefault();
    $("#advanced-search-form").slideToggle();
  });

  $(document).on("click", "#clear", function (event) {
    event.preventDefault();
    $(".book").removeClass("highlight");
    $("#filter input").val("");
  });

  $(document).on("click", "#open-filter > i", function (event) {
    event.preventDefault();
    $("#filter").slideToggle();
    $("#clear").toggle();
    $("#open-filter > i").toggleClass("fa-search");
    $("#open-filter > i").toggleClass("fa-close");
  });

  $(document).on("click", "#start-search", function (event) {
    event.preventDefault();
    $("#search-results").empty();
    $("#search-results").show();
    $("#add-book-form").hide();
    $("#search").prev().hide();
    $("#pagination").show();
    search.clear();
    search.general = "programming";
    searchAPI();
  });

  $(document).on("click", ".add-books-to-room", function (event) {
    event.preventDefault();
    let shelf = parseInt($(this).parent().children("input[name='shelf']").val());
    let room = parseInt($(this).parent().children("input[name='room']").val());
    $("#books-to-add").empty();
    pagination.clear();
    pagination.use = "rooms";
    pagination.shelf = shelf;
    pagination.room = room;
    paginate();
  });

  $(document).on("click", ".shelf", function () {
    let id = $(this).attr("id").slice(6);
    let top = $(this).css("top");
    let left = $(this).css("left");
    let bgWidth = $(".room-background").width();
    let width = $(this).css("width");
    let height = $(this).css("height");
    $.ajax({
      type: "POST",
      url: "../../Shelves/Drag",
      data: { id: id, top: top, left: left, width: width, height: height, roomWidth: bgWidth },
      success: function () {},
    });
  });

  $(document).on("click", ".add-shelf", function (event) {
    event.preventDefault();
    let room = parseInt($(".room").attr("id").slice(5));
    $.ajax({
      type: "POST",
      url: "../../Shelves/Create",
      data: { id: room },
      success: function (result) {
        let shelf = result.shelf;
        $(".room").append(`<div id="shelf-${shelf.shelfId}" class="shelf draggable" style="touch-action: none; left: ${shelf.left}; top: ${shelf.top};">
        <div class="books resize select-book" style="width:${shelf.width}; height:${shelf.height}"></div>
        <span class="delete-shelf btn btn-light corner-icon"><span class="fa fa-close"></span></span>
        <span class="btn btn-light add-books-to-room corner-icon" data-bs-toggle="modal" data-bs-target="#addBookToRoom"><i class="fa fa-plus" aria-hidden="true"></i></span>
        <span class="handle btn btn-light corner-icon"><i class="fa-solid fa-arrows-up-down-left-right"></i></span>
        <input type="hidden" name="shelf" value="${shelf.shelfId}">
        <input type="hidden" name="room" value="${shelf.roomId}">
      </div>`);
        $(".delete-shelf").show();
        $(".add-books-to-room").show();
        $(".handle").show();
        var $draggables = $(".draggable").draggabilly({
          handle: ".handle",
          containment: true,
          grid: [0, 0],
        });
      },
    });
  });

  $(document).on("click", ".change-background", function (event) {
    event.preventDefault();
    let room = parseInt($(".room").attr("id").slice(5));
    $("input[name='RoomId']").val(room);
  });

  $(document).on("click", ".assign-location", function () {
    let room = pagination.room;
    let shelf = pagination.shelf;
    let id = parseInt($(this).attr("id").slice(5));
    console.log("assignFirst");
    $.ajax({
      type: "POST",
      url: "../../Books/AddLocation",
      data: { id: id, shelfId: shelf, roomId: room },
      success: function (result) {
        $("#books-to-add").empty();
        $(`#book-${id}`).remove();
        paginate();
        console.log("assign");
        $(`#shelf-${shelf} > .books`).append(`<div style="background-image: url('https://books.google.com/books/content?id=${result.img}&printsec=frontcover&img=1&zoom=1');" class="book back" id="book-${id}"></div>`);
      },
    });
  });

  $(".show-book-details").click(function () {
    let id = parseInt($(this).attr("id").slice(5));
    showBookDetails(id);
  });

  $("#search-page-prev").click(function () {
    $("#search-results").empty();
    search.startIndex -= 8;
    if (search.startIndex === 0) {
      $(this).parent().addClass("disabled");
    }
    if (search.startIndex <= search.size - 8) {
      $("#search-page-next").parent().removeClass("disabled");
    }
    searchAPI();
  });

  $("#search-page-next").click(function () {
    $("#search-results").empty();
    search.startIndex += 8;
    if (search.startIndex >= search.size - 8) {
      $(this).parent().addClass("disabled");
    }
    if (search.startIndex >= 8) {
      $("#search-page-prev").parent().removeClass("disabled");
    }
    searchAPI();
  });

  $("#books-page-prev").click(function () {
    $("#books-to-add").empty();
    $("#books-page").empty();
    pagination.skip -= 8;
    if (pagination.skip === 0) {
      $(this).parent().addClass("disabled");
    }
    if (pagination.skip <= pagination.size - 8) {
      $("#books-page-next").parent().removeClass("disabled");
    }

    paginate();
  });

  $("#books-page-next").click(function () {
    $("#books-to-add").empty();
    $("#books-page").empty();
    pagination.skip += 8;
    if (pagination.skip >= pagination.size - 8) {
      $(this).parent().addClass("disabled");
    }
    if (pagination.skip >= 8) {
      $("#books-page-prev").parent().removeClass("disabled");
    }
    paginate();
  });

  $("#back-to-search").click(function () {
    $("#add-book-form").hide();
    $("#search-results").show();
    $("#pagination").show();
    $("#search-results").empty();
    $("#add-book-form input").val("");
    searchAPI();
  });

  $(".grid").click(function () {
    $(".room-overlay").toggleClass("dotted");
    grid = !grid;
    console.log(grid);
    if (grid) {
      var $draggables = $(".draggable").draggabilly({
        handle: ".handle",
        containment: true,
        grid: [20, 20],
      });
    } else {
      var $draggables = $(".draggable").draggabilly({
        handle: ".handle",
        containment: true,
        grid: [0, 0],
      });
    }
  });

  $("#edit-room").click(function () {
    $(".delete-shelf").toggle();
    $(".delete-room").toggle();
    $(".add-shelf").toggle();
    $(".add-room").toggle();
    $(".handle").toggle();
    $(".add-books-to-room").toggle();
    $(".change-background").toggle();
    $(".scale").toggleClass("hidden");
    $(".book").toggleClass("back");
    $(".books").toggleClass("resize");
    $(".room-overlay").removeClass("dotted");
    grid = false;
    var $draggables = $(".draggable").draggabilly({
      handle: ".handle",
      containment: true,
      grid: [0, 0],
    });
  });
  $(document).on("click", ".delete-shelf", function (event) {
    event.preventDefault();
    let id = $(this).parent().attr("id").slice(6);
    $.ajax({
      type: "POST",
      url: "../../Shelves/Delete",
      data: { id: id },
      success: function () {
        $(`#shelf-${id}`).remove();
      },
    });
  });

  window.addEventListener(
    "resize",
    function (event) {
      $(".rooms").each(function () {
        let bgWidth = $(".room-background").width();
        $(".rooms").css("width", bgWidth + "px");
        setScale();
      });
    },
    true
  );

  let bgWidth = $(".room-background").width();
  $(".rooms").css("width", bgWidth + "px");

  $(document).on("click", "#minus", function () {
    scaleFunc("minus");
  });

  $(document).on("click", "#plus", function () {
    scaleFunc("plus");
  });

  $(document).on("dblclick", ".room", function () {
    grid = false;
    $(".delete-shelf").toggle();
    $(".delete-room").toggle();
    $(".add-shelf").toggle();
    $(".add-room").toggle();
    $(".handle").toggle();
    $(".add-books-to-room").toggle();
    $(".change-background").toggle();
    $(".scale").toggleClass("hidden");
    $(".book").toggleClass("back");
    $(".books").toggleClass("resize");
    $(".room-overlay").removeClass("dotted");
    var $draggables = $(".draggable").draggabilly({
      handle: ".handle",
      containment: true,
      grid: [0, 0],
    });
  });

  $(document).on("change", "#addBackground input[type='file']", function () {
    if ($(this)[0].files && $(this)[0].files[0]) {
      var reader = new FileReader();
      reader.onload = function (e) {
        $("#imagepreview").attr("src", e.target.result);
      };
      reader.readAsDataURL($(this)[0].files[0]);
    }
  });
  $(document).on("submit", "#addBackground form", function () {
    $("#imagepreview").attr("src", "");
  });
});

$(document).on("click", ".select-book > .book", function () {
  let id = parseInt($(this).attr("id").slice(5));
  showBookDetails(id);
});

$(document).on("click", ".remove-from-shelf", function () {
  let id = parseInt($(this).attr("data-book-id"));
  $.ajax({
    type: "POST",
    url: "../../Books/RemoveLocation",
    data: { id: id },
    success: function () {
      $(`#book-${id}`).remove();
      $("#bookDetails").modal("hide");
    },
  });
});
