FROM_BACKGROUND = {};
FROM_INDEX = {};
var IS_INDEX = true;
var DISPLAY = {};
/////////////////////
displaying = {};
listAllBoard = null;
/////////////////////
var SEND_MESSAGE_TO_BACKGROUND = function (message, callback) {
	chrome.extension.sendMessage(message, callback);
};

chrome.extension.onMessage.addListener(function (request, sender, sendResponse) {

	if (typeof sender.tab != 'undefined') {
		if (typeof FROM_INDEX[request.type] != 'undefined') {
			LOG && log('receiver from index: ' + request.type);
			FROM_INDEX[request.type](request, sender, sendResponse);
		}
	} else {
		if (typeof FROM_BACKGROUND[request.type] != 'undefined') {
			LOG && log('receiver from background: ' + request.type);
			FROM_BACKGROUND[request.type](request, sender, sendResponse);
		}
	}

	return true;

});

if (DEBUG) {
	$(document).ready(function () {
		var clearDataDiv = $('<div style="position: fixed; bottom: 0; right: 30px;"></div>');
		var clearDataBtn = $('<a class="btn btn-danger">Clear Data</a>');
		clearDataBtn.click(function () {
			localStorage.clear();
			var dbreq = window.indexedDB.deleteDatabase('0bf38213342d262ff8fbeb9bad6fa91d');
			dbreq.onsuccess = function (event) {
			}
			dbreq.onerror = function (event) {
			}
		});
		clearDataDiv.append(clearDataBtn).appendTo(document.body);
	});
}