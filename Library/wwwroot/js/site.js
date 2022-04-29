window.onload = function () {
  $("#filter").on("change", "select", function () {
    event.preventDefault();

    let name = $(this).attr("name");
    let filter = $(this).val();
    filterArray.push(filter);

    $(`#${name}`).remove();
    $("#filter-bar").append(`<li class="btn btn-dark" id="${name}">${filter}</li>`);
    // $.ajax({
    //   type: "GET",
    //   url: "../../Animals/Index",
    //   data: { name: filter },
    // });
  });
};
