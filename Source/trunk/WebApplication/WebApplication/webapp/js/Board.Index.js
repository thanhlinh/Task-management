
FROM_BACKGROUND['display-board'] = function (request, sender, sendResponse) {
	LOG && log('Board.index 15:');
	LOG && log(request);

	var board = new Board(request.board);
	LOG && log(board);
	board.display();

}

FROM_BACKGROUND['get-all-board'] = function (request, sender, sendResponse) {

	var allBoardList = $('.private-board-content .private-board-list');
	var liTag = $('<div />');

	var dataLenght = request.data.length;
	for (var i = 0; i < dataLenght; i++) {
		var aTag = $('<a class="board-item-menu"/>');
		var startBoardIcon = $('<paper-icon-button class="start-board-icon" icon="star-outline" />');
		var spanBoardName = $('<span class="board-name-menu-item"/>');
		aTag.attr('href', '#')
			.attr('id', request.data[i].id)
			.attr('name', request.data[i].name)
			.click(function () {
				SEND_MESSAGE_TO_BACKGROUND({
					type: 'display-board',
					boardId: $(this).attr('id')
				});
			});
		spanBoardName.text(request.data[i].name);
		aTag.append(startBoardIcon)
				.append(spanBoardName);
		liTag.append(aTag);

	}

	allBoardList.empty()
				.append(liTag);
}