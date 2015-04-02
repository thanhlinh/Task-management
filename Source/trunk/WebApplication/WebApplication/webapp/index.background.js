var IS_BACKGROUND = true;
var BACKGROUND = {};

var SEND_MESSAGE_TO_INDEX = function (message, callback) {
	if (typeof FROM_BACKGROUND[message.type] != 'undefined') {
		LOG && log('receiver from index: ' + message.type);
		FROM_BACKGROUND[message.type](message, callback);
	}
};

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