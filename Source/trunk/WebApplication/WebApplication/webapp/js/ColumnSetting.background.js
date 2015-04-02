
BACKGROUND['get-columnsetting'] = function (request, sendResponse) {
	
	ColumnSetting.getAll(request.boardId, function (columnSetting) {
		request.data = columnSetting;
		LOG && log('get column setting background');
		LOG && log(columnSetting);
		Column.getAll(request.boardId, function (colList) {
			request.columns = colList.sort(columnCompare);
			SEND_MESSAGE_TO_INDEX(request);
		});
	});

}

BACKGROUND['update-columnsetting'] = function (request, sendResponse) {

	var len = request.data.length;

	for (var i = 0; i < len; i++) {
		ColumnSetting.createOrUpdate(request.data[i].columnId, request.data[i].next, request.boardId, request.data[i].status);
	}
	request.type = 'get-columnsetting';
	setTimeout(function () {
		ColumnSetting.getAll(request.boardId, function (columnSetting) {
			request.data = columnSetting;
			SEND_MESSAGE_TO_INDEX(request);
		});
	}, 100);

}