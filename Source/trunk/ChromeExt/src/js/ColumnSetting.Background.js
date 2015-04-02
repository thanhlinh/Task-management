
BACKGROUND['get-columnsetting'] = function (request, sender, sendResponse) {
	
	ColumnSetting.getAll(request.boardId, function (columnSetting) {
		request.data = columnSetting;
		LOG && log('get column setting background');
		LOG && log(columnSetting);
		Column.getAll(request.boardId, function (colList) {
			request.columns = colList.sort(columnCompare);
			chrome.tabs.query({}, function (tabs) {
				SEND_MESSAGE_TO_INDEX(tabs, request);
			});
		});
	});

}

BACKGROUND['update-columnsetting'] = function (request, sender, sendResponse) {

	var len = request.data.length;

	for (var i = 0; i < len; i++) {
		ColumnSetting.createOrUpdate(request.data[i].columnId, request.data[i].next, request.boardId, request.data[i].status);
	}
	request.type = 'get-columnsetting';
	setTimeout(function () {
		ColumnSetting.getAll(request.boardId, function (columnSetting) {
			request.data = columnSetting;
			chrome.tabs.query({}, function (tabs) {
				SEND_MESSAGE_TO_INDEX(tabs, request);
			});
		});
	}, 100);

}