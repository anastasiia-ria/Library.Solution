let passwordBool = false;
let confirmBool = false;
let emailBool = false;

function validateAll() {
  if (confirmBool && passwordBool && emailBool) {
    console.log(confirmBool + passwordBool + emailBool);
    $("#register-form button").removeAttr("disabled");
  } else {
    $("#register-form button").attr("disabled", "disabled");
  }
  console.log(confirmBool + "n  " + passwordBool + "  " + emailBool);
}
function validatePassword() {
  let password = $("#register-p").val();
  count = 0;
  if (password.length >= 8) {
    count++;
    $("#length").addClass("green-t");
    $("#length span").removeClass("fa-close");
    $("#length span").addClass("fa-check");
  } else {
    count--;
    $("#length").removeClass("green-t");
    $("#length span").addClass("fa-close");
    $("#length span").removeClass("fa-check");
  }
  if (/[A-Z]/.test(password)) {
    count++;
    $("#capital").addClass("green-t");
    $("#capital span").removeClass("fa-close");
    $("#capital span").addClass("fa-check");
  } else {
    count--;
    $("#capital").removeClass("green-t");
    $("#capital span").addClass("fa-close");
    $("#capital span").removeClass("fa-check");
  }
  if (/[a-zA-Z]/.test(password)) {
    count++;
    $("#letter").addClass("green-t");
    $("#letter span").removeClass("fa-close");
    $("#letter span").addClass("fa-check");
  } else {
    count--;
    $("#letter").removeClass("green-t");
    $("#letter span").addClass("fa-close");
    $("#letter span").removeClass("fa-check");
  }
  if (/[1-9]/.test(password)) {
    count++;
    $("#number").addClass("green-t");
    $("#number span").removeClass("fa-close");
    $("#number span").addClass("fa-check");
  } else {
    count--;
    $("#number").removeClass("green-t");
    $("#number span").addClass("fa-close");
    $("#number span").removeClass("fa-check");
  }
  if (count === 4) {
    passwordBool = true;
    $(".password-validation").addClass("green-b green-t");
    $("#register-p").addClass("green");
    $("#register-p").removeClass("red");
  } else {
    passwordBool = false;
    $(".password-validation").removeClass("green-b green-t");
    $("#register-p").removeClass("green");
    $("#register-p").addClass("red");
  }
}

function validateEmail() {
  let email = $("#register-e").val() || "email";
  $.ajax({
    type: "GET",
    url: "../../Account/Validate",
    data: { email: email },
    success: function (result) {
      console.log(result);
      $("#valid").addClass("hidden");
      $("#used").addClass("hidden");
      if (result.account != null) {
        emailBool = false;
        $("#register-e").addClass("red");
        $("#register-e").removeClass("green");
        $(".email-validation").show();
        $("#used").removeClass("hidden");
      } else if (!/^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(email)) {
        emailBool = false;
        $("#register-e").addClass("red");
        $("#register-e").removeClass("green");
        $(".email-validation").show();
        $("#valid").removeClass("hidden");
      } else {
        emailBool = true;
        $("#register-e").removeClass("red");
        $("#register-e").addClass("green");
        $(".email-validation").hide();
        $("#valid").addClass("hidden");
        $("#used").addClass("hidden");
      }
      validateAll();
    },
  });
}
function validateConfirm() {
  let password = $("#register-p").val();
  let confirm = $("#register-p-c").val();
  if (password === confirm && passwordBool) {
    $("#register-p-c").removeClass("red");
    $("#register-p-c").addClass("green");
    confirmBool = true;
  } else {
    $("#register-p-c").removeClass("green");
    $("#register-p-c").addClass("red");
    confirmBool = false;
  }
}
$(document).ready(function () {
  $("#register-form input").on("click", function () {
    $(".password-validation").hide();
    $(".email-validation").hide();
  });

  $("#register-p").on("input", function () {
    $(".password-validation").show();
    validatePassword();
    validateConfirm();
    validateAll();
  });

  $("#register-p-c").on("input", function () {
    validateConfirm();
    validateAll();
  });

  $("#register-e").on("input", function () {
    validateEmail();
  });
});
