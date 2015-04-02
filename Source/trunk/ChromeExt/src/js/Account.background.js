BACKGROUND['account-info'] = function (request, sender, response) {
	if (request.cache === false) {
		var isChanged = false;
		Database.init();
		//LOG && log('Account.background 5 : request.cache false');
		LOG && log('Account.background 6 :');
		LOG && log(LOGGED_USER);
		response({
			type: 'account-info',
			user: LOGGED_USER,
			change: isChanged
		});
	} else {
		LOG && log('Account.background 13 : request.cache !false');
		LOG && log(LOGGED_USER);
		response({
			type: 'account-info',
			user: LOGGED_USER,
			change: false
		});
	}
}