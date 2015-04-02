
$(document).ready(function () {
	var cmd = $('#cmd').val();
	var data = $('#data').val();
	var isClose = $('#close').val();
	
	SEND_MESSAGE_TO_BACKGROUND({
		type: "content-hook",
		cmd: cmd,
		data: data
	});
	if (isClose == '1') {
		window.close();
	}
});