BACKGROUND["content-hook"] = function (request, sender, response) {
	LOG && log("Background retrieve content hook");
	switch (request.cmd) {
		case "logged":
		case "logout":
		case "upgraded":
			Database.init();
			chrome.tabs.query({}, function (tabs) {
				SEND_MESSAGE_TO_INDEX(tabs, {
					type: 'account-info',
					user: LOGGED_USER,
					change: true
				});
			});
			break;
		default:
			break;
	}
}