$(document).ready(function () {
	var firstnameValidate = function (firstName) {
		if (firstName == "") {
			$("#firstname_error").text("Please enter your firstname");
			return false;
		}
		$("#firstname_error").empty();
		return true;
	};

	var lastnameValidate = function (lastName) {
		if (lastName == "") {
			$("#lastname_error").text("Please enter your lastname");
			return false;
		}
		$("#lastname_error").empty();
		return true;
	};

	var emailValidate = function (email) {
		var emailReg = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;;
		if (email == "") {
			$('#email_error').text("Please enter your email.");
			return false;
		}
		if (!emailReg.test(email)) {
			$('#email_error').text("Please enter a valid email address.")
			return false;
		}
		$('#email_error').empty();
		return true;
	}

	var passwordValidate = function (password) {
		if (password == "") {
			$('#password_error').text("Please provide a password.");
			return false;
		}
		if (password.length < 6) {
			$('#password_error').text("Your password must be at least 6 characters long.")
			return false;
		}
		$('#password_error').empty();
		return true;
	}

	var repasswordValidate = function (repassword, password) {
		if (repassword == "") {
			$('#repassword_error').text("Please provide a password");
			return false;
		}
		if (repassword != password) {
			$('#repassword_error').text("Please enter the same password as above");
			return false;
		}
		$("#repassword_error").empty();
		return true;
	}

	$('#first-name').change(function () {
		firstnameValidate($(this).val());
		lastnameValidate($('#last-name').val());
		emailValidate($('#email-input').val());
		passwordValidate($('#password-input').val());
		repasswordValidate($('#repassword-input').val());
	});

	$('#last-name').change(function () {
		firstnameValidate($('#first-name').val());
		lastnameValidate($(this).val());
		emailValidate($('#email-input').val());
		passwordValidate($('#password-input').val());
		repasswordValidate($('#repassword-input').val());
	});

	$('#email-input').change(function () {
		firstnameValidate($('#first-name').val());
		lastnameValidate($('#last-name').val());
		emailValidate($(this).val());
		passwordValidate($('#password-input').val());
		repasswordValidate($('#repassword-input').val());
	});

	$('#password-input').change(function () {
		firstnameValidate($('#first-name').val());
		lastnameValidate($('#last-name').val());
		emailValidate($('#email-input').val());
		passwordValidate($(this).val());
		repasswordValidate($('#repassword-input').val());
	});

	$('#repassword-input').change(function () {
		firstnameValidate($('#first-name').val());
		lastnameValidate($('#last-name').val());
		emailValidate($('#email-input').val());
		passwordValidate($('#password-input').val());
		repasswordValidate($(this).val());
	});

	$('#submit').click(function () {
		var firstName = $('#first-name').val();
		var lastName = $('#last-name').val();
		var email = $('#email-input').val();
		var password = $('#password-input').val();
		var repassword = $('#repassword-input').val();

		var checkFirstName = firstnameValidate(firstName);
		var checkLastName = lastnameValidate(lastName);
		var checkEmail = emailValidate(email);
		var checkPassword = passwordValidate(password);
		var checkRePassword = repasswordValidate(repassword, password);

		if (checkFirstName && checkLastName && checkEmail && checkPassword && checkRePassword) {
			$.ajax({
				type: "POST",
				async: false,
				url: "/Login/Register",
				data: { Email: email, Password: password, FirstName: firstName, LastName: lastName },
				success: function (result) {
					var url = "/home/thankyoupage";
					window.location.href = url;
				},
				error: function () {
					alert('loi');
				}
			});
		}
	});
	$('#loginSubmit').click(function () {
		document.location = 'Login';
	});
});
