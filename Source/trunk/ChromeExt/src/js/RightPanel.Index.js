
FROM_BACKGROUND['change-theme'] = function (request, sender, sendResponse) {
	//$('link[rel=stylesheet]').attr('href', request.href);
	LOG && log(request);
	$('.themesPanel .theme-selected').removeClass('theme-selected').css('opacity', '0.5');
	$('.themesPanel').find(request.name).addClass('theme-selected');
	/*SEND_MESSAGE_TO_BACKGROUND({
		type: 'display-board',
		boardId: displaying['boardid'],
		boardName: displaying['boardname']
	})*/
}