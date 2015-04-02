var accountInfoReceiver = function(request) {
	LOG && log('Account.index 2: ');
	LOG && log(request);
	LOGGED_USER = request.user;
	if (request.change){
		LOG && log(request.user);
		localStorage.removeItem('boardIdDisplaying');
		localStorage.removeItem('boardNameDisplaying');
		location.reload();
	}
	else if (request.user === false) {
		LOG && log('Account.index - request.user false');
		$('.uavatar').hide();
	} else {
		LOG && log('Account.index - request.user true');
		$('.login-btn').hide();
		$('.uname').text(request.user.firstname + ' ' + request.user.lastname);
		$('.umail').text(request.user.email);
		$('.account-info').addClass('loged');
		$('.uavatar').attr('src', request.user.avatar);
		$('.account-icon').hide();
		if (request.user.role.toLowerCase() != 'user') $('.upgrade-btn').hide();
	}
};

SEND_MESSAGE_TO_BACKGROUND({
	type: 'account-info'
}, accountInfoReceiver);

FROM_BACKGROUND['account-info'] = function (request, sender, response) {
	LOG && log('recive account-info from background');
	accountInfoReceiver(request);
}