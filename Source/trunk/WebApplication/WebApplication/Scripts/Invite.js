
var lang = {
	emailEmptyMessage: "Please enter your friend email.",
	emailValidMessage: "Please enter a valid email address.",
	emailAvailabeMessage: "The email is ready in our system",
	errorMessage: "Sorry, you can not invite friend now."
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


var inviteSubmit = function()
{
	var firstName = $('#first-name').val();
	var lastName = $('#last-name').val();
	var email = $('#email-input').val();

	var checkEmail = emailValidate(email);

	$('#invite_error').empty();

	if (checkEmail) {
		$('#submit').prop("disabled", true);
		$.ajax({
			url: "/Invite/invite",
			data: { email: email, firstName: firstName, lastName: lastName, message: "Hi, i would like invite join to system." },
			type: "POST", dataType: 'json', cache: false,
			success: function (json) {
				if(json.message === 'successful')
				{
					window.location.href = ThankyoupageRedirect;
				} else if (json.message === 'emailavailable')
				{
					$('#invite_error').text(lang.emailAvailabeMessage);
				} else {
					$('#invite_error').text(lang.errorMessage);
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
	$('#submit').click(inviteSubmit);
});
