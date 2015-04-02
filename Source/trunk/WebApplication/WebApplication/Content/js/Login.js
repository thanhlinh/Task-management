$(document).ready(function () {
	var emailValidate = function (email) {
		var emailReg = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;;
		$('#cannot-login').empty();
		if (email == "") {
			$('#email-error').text("Please enter your email.");
			return false;
		}
		if (!emailReg.test(email)) {
			$('#email-error').text("Please enter a valid email address.")
			return false;
		}
		$('#email-error').empty();
		return true;
	}

	var passwordValidate = function (password) {
		$('#cannot-login').empty();
		if (password == "") {
			$('#password-error').text("Please enter your password.");
			return false;
		}
		if (password.length < 6) {
			$('#password-error').text("Your password must be at least 6 characters long.")
			return false;
		}
		$('#password-error').empty();
		return true;
	}

	$('#email-input').change(function () {
		emailValidate($(this).val());
		var password = $('#password-input').val();
		passwordValidate(password);
	});

	$('#password-input').change(function () {
		var email = $('#email-input').val();
		emailValidate(email);
		passwordValidate($(this).val());
	});

	$('#submit').click(function () {
		var email = $('#email-input').val();
		var password = $('#password-input').val();
		var checkEmail = emailValidate(email);
		var checkPass = passwordValidate(password);
		if (checkEmail && checkPass) {
			$.ajax({
				type: "POST",
				async: false,
				url: "/Login/Login",
				data: { Email: email, Password: password },
				success: function (result) {
					if (result == 'True') {
						window.location.href = LoginSuccessRedirect;
					}
					else {
						$('#cannot-login').text("Please check your email or password.");
					}
				},
				error: function () {
				}
			});
		}
	});
	$('#registerSubmit').click(function () {
		document.location = 'Register';
	});
});
