
var lang = {
	firstnameEmptyMessage: "Please enter your firstname",
	lastnameEmptyMessage: "Please enter your lastname",
	passwordEmptyMessage: "Please provide a password.",
	passwordShortMessage: "Your password must be at least 6 characters long.",
	repasswordEmptyMessage: "Please provide a password",
	repasswordNotSameMessage: "Please enter the same password as above",
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

var compelteSubmit = function()
{
	var firstName = $('#first-name').val().trim();
	var lastName = $('#last-name').val().trim();
	var email = $('#email-input').val().trim();
	var password = $('#password-input').val();
	var repassword = $('#repassword-input').val();

	var checkFirstName = firstnameValidate(firstName);
	var checkLastName = lastnameValidate(lastName);
	var checkPassword = passwordValidate(password);
	var checkRePassword = repasswordValidate(repassword, password);

	$('#create_error').empty();

	if (checkFirstName && checkLastName && checkPassword && checkRePassword) {
		$('#submit').prop("disabled", true);
		$('#inviteconfirm_error').empty();
		$.ajax({
			url: "/Invite/InviteConfirmation",
			data: { email: email, password: password, firstName: firstName, lastName: lastName },
			type: "POST", dataType: 'json', cache: false,
			success: function (json) {
				console.log('Success invite confirm ajax');
				if(json.message === 'successful')
				{
					window.location.href = ThankyoupageRedirect;
				} else {
					$('#inviteconfirm_error').text(lang.registerMessage);
				}
				$('#submit').prop("disabled", false);
			},
			error: function () {
				console.log('Error invite confirm ajax');
				$('#submit').prop("disabled", false);
			}
		});
	}
}
$(document).ready(function () {
	$('#submit').click(compelteSubmit);
	$('#loginSubmit').click(function () {
		window.location.href = LoginRedirect;
	});
});
