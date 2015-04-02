
BACKGROUND['create-column'] = function (request, sender, sendResponse) {

	LOG && log('Column.background create column');

	var column = new Column();

	Board.getById(request.boardId, function (data) {
		var board = new Board(data);

		column.boardId = request.boardId;
		column.name = request.columnName;
		column.priority = board.columnCount;
		column.save();

		board.columnCount = board.columnCount + 1;
		board.save();

		chrome.tabs.query({}, function (tabs) {
			SEND_MESSAGE_TO_INDEX(tabs, {
				type: 'create-column',
				boardId: request.boardId,
				column: column
			});
		});
	});
}

BACKGROUND['get-all-column'] = function (request, sender, sendResponse) {

	LOG && log('Column.background get all column');

	Column.getAll(request.boardId, function (colList) {
		LOG && log(sender.tab);
		SEND_MESSAGE_TO_TAB(sender.tab, {
			type: 'get-all-column',
			boardId: request.boardId,
			columns: colList
		});
		sendResponse(colList);
	});
}

BACKGROUND['column-move-done'] = function (request, sender, sendResponse) {
	if (request.newIndex > request.oldIndex) {
		Column.getAll(request.boardId, function (data) {
			for (var i = 0, l = data.length; i < l; i++) {
				if (request.oldIndex < data[i].priority && data[i].priority <= request.newIndex) {
					var column = new Column(data[i]);
					column.priority = column.priority - 1;
					column.save();
				}
			}
			Column.getById(request.columnId, function (columnData) {
				var column = new Column(columnData);
				column.priority = request.newIndex;
				column.save();
			});
		});
	} else {
		Column.getAll(request.boardId, function (data) {
			for (var i = 0, l = data.length; i < l; i++) {
				if (request.newIndex <= data[i].priority && data[i].priority < request.oldIndex) {
					var column = new Column(data[i]);
					column.priority = column.priority + 1;
					column.save();
				}
			}
			Column.getById(request.columnId, function (columnData) {
				var column = new Column(columnData);
				column.priority = request.newIndex;
				column.save();
			});
		});
	}
}

BACKGROUND['update-column-name'] = function (request, sender, sendResponse) {

	Column.getById(request.columnId, function (data) {
		var column = new Column(data);
		column.name = request.columnName;
		column.save();
	});
}
/*
BACKGROUND['delete-column'] = function (request, sender, sendResponse) {

	Column.delete(request.columnId);
	Board.getColumnCount(request.boardId, function (data) {
		request.type = 'display-board';
		Column.UpdatePriorityLeft(request.boardId, data, request.columnIndex);
		BACKGROUND['display-board'](request, sender, sendResponse);
	});
}*/