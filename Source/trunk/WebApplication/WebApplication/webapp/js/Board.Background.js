
BACKGROUND['create-board'] = function (request, sendResponse) {

	var board = new Board();
	board.name = request.boardName;
	board.save();
	request.board = board;
	request.type = 'display-board';
	setTimeout(function () {
		BACKGROUND['get-all-board'](request, sendResponse);
		SEND_MESSAGE_TO_INDEX(request);
	}, 100);
}

BACKGROUND['get-all-board'] = function (request, sendResponse) {
	Board.getAll(function (boards) {
		SEND_MESSAGE_TO_INDEX({
			type: 'get-all-board',
			data: boards
		});
	});
};

BACKGROUND['display-board'] = function (request, sendResponse) {
	LOG && log('Board.background : display-board');
	LOG && log(request);
	Board.getById(request.boardId, function (board) {
		request.board = board;
		SEND_MESSAGE_TO_INDEX(request);
	});
}

BACKGROUND['update-board-name'] = function (request, sendResponse) {

	Board.getById(request.boardId, function (data) {
		var board = new Board(data);
		board.name = request.boardName;
		board.save();
		setTimeout(function () {
			BACKGROUND['get-all-board'](request, sendResponse);
		}, 100);
	});
}

BACKGROUND['create-scrum-board'] = function (request, sendResponse) {
	var todayColumn = new Column();
	todayColumn.boardId = request.boardId;
	todayColumn.name = "Todo";
	todayColumn.priority = 0;
	todayColumn.save();
	var todoColumn = new Column();
	todoColumn.boardId = request.boardId;
	todoColumn.name = "Today";
	todoColumn.priority = 1;
	todoColumn.save();
	var reviewColumn = new Column();
	reviewColumn.boardId = request.boardId;
	reviewColumn.name = "Reviewing";
	reviewColumn.priority = 2;
	reviewColumn.save();
	var doneColumn = new Column();
	doneColumn.boardId = request.boardId;
	doneColumn.name = "Done";
	doneColumn.priority = 3;
	doneColumn.save();
	Board.getById(request.boardId, function (data) {
		var board = new Board(data);
		board.columnCount = 4;
		board.save();
		request.board = board;
		request.type = 'display-board';
		setTimeout(function () {
		SEND_MESSAGE_TO_INDEX(request);
		}, 500);
	});
}