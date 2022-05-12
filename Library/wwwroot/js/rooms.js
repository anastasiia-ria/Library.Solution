let maxSize = 0;
let perPage = 8;
let page = 1;

// function showBooks(page) {
//   $.ajax({
//     type: "GET",
//     url: "../../Books/IndexPagination",
//     data: { page: page },
//     success: function (result) {
//       console.log(result);
//       maxSize = result.count;
//       result.books["$values"].forEach(function (book) {
//         $("#books-to-add").append(`<div class="card">
//         <img class="card-img-top" src="https://books.google.com/books/content?id=${book.imgID}&printsec=frontcover&img=1&zoom=5" alt="Book thumbnail" height="240px" object-fit="contain">
//         <div class="card-body">
//           <h5 class="card-title cut-text">${book.title}</h5>
//           <p class="card-text cut-text">${book.authors}</p>
//           @Html.BeginForm("AddLocation", "Books", new { id = ${book.bookId}, shelfId = ViewData["shelfId"], roomId = ViewData["roomId"]},FormMethod.Post )
//           {
//             <button class="btn btn-light" type="submit">Add</button>
//           }
//         </div>
//       </div>`);
//       });
//     },
//   });
// }
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

  // $("#r-search-page-prev").click(function () {
  //   $("#books-to-add").empty();
  //   page -= 8;
  //   if (page === 1) {
  //     $(this).parent().addClass("disabled");
  //   }
  //   if (page <= maxSize / 8) {
  //     $("#r-search-page-next").parent().removeClass("disabled");
  //   }

  //   showBooks(page);
  // });

  // $("#r-search-page-next").click(function () {
  //   $("#books-to-add").empty();
  //   page += 8;
  //   if (page >= maxSize / 8) {
  //     $(this).parent().addClass("disabled");
  //   }
  //   if (page > 1) {
  //     $("#r-search-page-prev").parent().removeClass("disabled");
  //   }
  //   showBooks(page);
  // });
});
