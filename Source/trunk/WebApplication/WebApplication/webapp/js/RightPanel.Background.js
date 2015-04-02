

BACKGROUND['change-theme'] = function (request, sender, sendResponse) {

	chrome.tabs.query({}, function (tabs) {
		SEND_MESSAGE_TO_INDEX(tabs, request);
	});

}