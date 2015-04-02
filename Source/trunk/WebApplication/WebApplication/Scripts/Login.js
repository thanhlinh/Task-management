var lang = {
	emailEmptyMessage: "Please enter your email.",
	emailValidMessage: "Please enter a valid email address.",
	passwordEmptyMessage: "Please enter your password.",
	passwordShortMessage: "Your password must be at least 6 characters long.",
	unconfirmedMessage: 'You need go to mail for confirm befor use this account.',
	wrongEmailPassMessage: "Please check your email or password."
};

var emailValidate = function (email) {
	var emailReg = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
	$('#cannot-login').empty();
	if (email == "") {
		$('#email-error').text(lang.emailEmptyMessage);
		return false;
	}
	if (!emailReg.test(email)) {
		$('#email-error').text(lang.emailValidMessage)
		return false;
	}
	$('#email-error').empty();
	return true;
};

var passwordValidate = function (password) {
	$('#cannot-login').empty();
	if (password == "") {
		$('#password-error').text(lang.passwordEmptyMessage);
		return false;
	}
	if (password.length < 6) {
		$('#password-error').text(lang.passwordShortMessage)
		return false;
	}
	$('#password-error').empty();
	return true;
};

var loginClick = function () {
	var email = $('#email-input').val();
	var password = $('#password-input').val();
	var checkEmail = emailValidate(email);
	var checkPass = passwordValidate(password);
	if (checkEmail && checkPass) {
		$('#submit').prop("disabled", true);
		$.ajax({
			url: "/Login/Login",
			data: { Email: email, Password: password },
			type: "POST", dataType: 'json', cache: false,
			success: function (json) {
				if (json.success && json.message == 'popup') {
					window.location.href = LoginSuccessRedirect;
				}
				else if (json.success && json.message == 'nopopup')
				{
					window.location.href = '/webapp/';
				}
				else if (json.data === 'unconfirmed') {
					$('#cannot-login').text(lang.unconfirmedMessage);
				}
				else {
					$('#cannot-login').text(lang.wrongEmailPassMessage);
				}
				$('#submit').prop("disabled", false);
			},
			error: function () {
				$('#submit').prop("disabled", false);
			}
		});
	}
};


$(document).ready(function () {
	$('#email-input').keypress(function () {
		$('#password-input').val("");
	});

	$('#password-input').change(function (event) {
			loginClick();
	});
	
	$('#submit').click(loginClick);
	$('#registerSubmit').click(function () {
		document.location = 'Register';
	});

	$('#oauthgoogle').click(function () {
		window.location.href = GoogleLoginExternalRedirect;
	});
	$('#oauthfacebook').click(function () {
		window.location.href = FacebookLoginExternalRedirect;
	});
});
