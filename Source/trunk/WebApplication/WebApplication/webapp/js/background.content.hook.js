BACKGROUND["content-hook"] = function (request, response) {
	LOG && log("Background retrieve content hook");
	switch (request.cmd) {
		case "logged":
		case "logout":
		case "upgraded":
			Database.init();
			SEND_MESSAGE_TO_INDEX({
				type: 'account-info',
				user: LOGGED_USER,
				change: true
			});
			break;
		default:
			break;
	}
}