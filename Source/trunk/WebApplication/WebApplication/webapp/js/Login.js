
var displayLogin = function (url, title, w, h) {

	var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
	var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
	width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
	height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

	var left = ((width / 2) - (w / 2)) + dualScreenLeft;
	var top = ((height / 2) - (h / 2)) + dualScreenTop;
	var newWindow = window.open(url, title, 'scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

	if (window.focus) {
		newWindow.focus();
	}
}

$(document).ready(function () {
	$('.login-btn').click(function () {
		displayLogin('http://localhost:34003/Login/Login', 'Sioux Login', '400', '520');
	});

	$('.logout-btn').click(function () {
		$.ajax({
			url: config('server') + "/log-out",
			cache: false, sync: false,
			success: function () {
				SEND_MESSAGE_TO_BACKGROUND({
					type: 'account-info',
					cache: false
				}, accountInfoReceiver({
					change: true
				}));
				LOG && log('Logout success');
			},
			error: function () {
				LOG && log('Logout error');
			}
		});
	});
});
