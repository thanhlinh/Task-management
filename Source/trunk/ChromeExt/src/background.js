var IS_BACKGROUND = true;
var BACKGROUND = {};

var SEND_MESSAGE_TO_INDEX = function (tabs, message, exclude) {
	if ($.isArray(exclude)) {
		var excludeLength = exclude.length;
		for (var i = 0, l = tabs.length; i < l; i++) {
			if (tabs[i].url.indexOf('chrome://newtab/') !== 0)
				continue;
			var cnt = false;
			for (var j = 0; j < excludeLength; j++) {
				if (tabs[i].id === exclude[j])
					cnt = true;
			}
			if (cnt)
				continue;
			chrome.tabs.sendMessage(tabs[i].id, message);
		}
	} else {
		var sendMessage = message;
		for (var i = 0, l = tabs.length; i < l; i++) {
			if (tabs[i].url.indexOf('chrome://newtab/') !== 0 || tabs[i].id === exclude)
				continue;
			chrome.tabs.sendMessage(tabs[i].id, sendMessage);
		}
	}
};

var SEND_MESSAGE_TO_TAB = function (tab, message) {
	chrome.tabs.sendMessage(tab.id, message);
};

chrome.extension.onMessage.addListener(function (request, sender, sendResponse) {
	if (!Enable.backgroundListener)
		return true;

	if (typeof BACKGROUND[request.type] != 'undefined') {
		BACKGROUND[request.type](request, sender, sendResponse);
	}
	return true;
});


// Sync to server
SYNC_TO_SERVER = {};
SYNC_FROM_SERVER = {};
var syncToServerIntervalId = setInterval(function () {
	if (!Enable.backgroundSync)
		return;
	for (var name in SYNC_TO_SERVER) {
		SYNC_TO_SERVER[name]();
	}
}, config('syncToServerInterval'));

var syncFromServerIntervalId = setInterval(function () {
	if (!Enable.backgroundSync)
		return;

}, config('syncFromServerInterval'));