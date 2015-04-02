
FROM_BACKGROUND['get-columnsetting'] = function (request, sender, sendResponse) {
	currentBoard.columnSetting = request.data;
	currentBoard.columns = request.columns;
	LOG && log('get column setting');
	LOG && log(currentBoard);
}