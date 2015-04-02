var lang = {
	firstnameEmptyMessage: "Please enter your firstname",
	lastnameEmptyMessage: "Please enter your lastname",
	emailEmptyMessage: "Please enter your email.",
	emailValidMessage: "Please enter a valid email address.",
	passwordEmptyMessage: "Please provide a password.",
	passwordShortMessage: "Your password must be at least 6 characters long.",
	repasswordEmptyMessage: "Please provide a password",
	repasswordNotSameMessage: "Please enter the same password as above",
	emailAvailableMessage: "Unable to create local account. An account with the email may already exist.",
	registerMessage: "Sorrry, you can not register an account now."
};

var firstnameValidate = function (firstName) {
	if (firstName == "") {
		$("#firstname_error").text(lang.firstnameEmptyMessage);
		return false;
	}
	$("#firstname_error").empty();
	return true;
};

var lastnameValidate = function (lastName) {
	if (lastName == "") {
		$("#lastname_error").text(lang.lastnameEmptyMessage);
		return false;
	}
	$("#lastname_error").empty();
	return true;
};

var emailValidate = function (email) {
	var emailReg = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
	if (email == "") {
		$('#email_error').text(lang.emailEmptyMessage);
		return false;
	}
	if (!emailReg.test(email)) {
		$('#email_error').text(lang.emailValidMessage)
		return false;
	}
	$('#email_error').empty();
	return true;
}

var passwordValidate = function (password) {
	if (password == "") {
		$('#password_error').text(lang.passwordEmptyMessage);
		return false;
	}
	if (password.length < 6) {
		$('#password_error').text()
		return false;
	}
	$('#password_error').empty();
	return true;
}

var repasswordValidate = function (repassword, password) {
	if (repassword == "") {
		$('#repassword_error').text(lang.repasswordEmptyMessage);
		return false;
	}
	if (repassword != password) {
		$('#repassword_error').text(lang.repasswordNotSameMessage);
		return false;
	}
	$("#repassword_error").empty();
	return true;
}

var register = function()
{
	var firstName = $('#first-name').val().trim();
	var lastName = $('#last-name').val().trim();
	var email = $('#email-input').val().trim();
	var password = $('#password-input').val();
	var repassword = $('#repassword-input').val();

	var checkFirstName = firstnameValidate(firstName);
	var checkLastName = lastnameValidate(lastName);
	var checkEmail = emailValidate(email);
	var checkPassword = passwordValidate(password);
	var checkRePassword = repasswordValidate(repassword, password);

	$('#create_error').empty();

	if (checkFirstName && checkLastName && checkEmail && checkPassword && checkRePassword) {
		$('#submit').prop("disabled", true);
		$.ajax({
			url: "/Login/Register",
			data: { Email: email, Password: password, FirstName: firstName, LastName: lastName },
			type: "POST", dataType: 'json', cache: false,
			success: function (json) {
				if (json.message === 'successful') {
					window.location.href = ThankyoupageRedirect;
				} else if (json.message === 'emailavailable') {
					$('#create_error').text(lang.emailAvailableMessage);
				} else {
					$('#create_error').text(lang.registerMessage);
				}
				$('#submit').prop("disabled", false);
			},
			error: function () {
				$('#submit').prop("disabled", false);
			}
		});
	}
}


$(document).ready(function () {
	$('#submit').click(register);

	$('#loginSubmit').click(function () {
		document.location = 'Login';
	});
});
