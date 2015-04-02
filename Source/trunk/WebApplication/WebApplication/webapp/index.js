FROM_BACKGROUND = {};
FROM_INDEX = {};
var IS_INDEX = true;
var DISPLAY = {};
/////////////////////
displaying = {};
listAllBoard = null;
/////////////////////
var SEND_MESSAGE_TO_BACKGROUND = function (message, callback) {
	if (typeof BACKGROUND[message.type] != 'undefined') {
		BACKGROUND[message.type](message, callback);
	}
};