
FROM_BACKGROUND['get-all-column'] = function (request, sendResponse) {
	
	LOG && log('Column.index get-all-columns 4');
	LOG && log(currentBoard);
	if (currentBoard.id == request.boardId) {
		currentBoard.content.empty();
		for(var i=0,l=request.columns.length;i<l;i++){
			for (var j = 0; j < l; j++) {
				if (request.columns[j].priority == i) {
					var column = new Column(request.columns[j]);
					column.display();
				}
			}
		}
	}

}

FROM_BACKGROUND['create-column'] = function (request, sendResponse) {

	if (currentBoard.id == request.boardId) {
		var column = new Column(request.column);
		currentBoard.columnCount = currentBoard.columnCount + 1;
		currentBoard.columns.push(column);
		column.display();
	}

}

FROM_BACKGROUND['delete-column'] = function (request, sendResponse) {
	$('div[id=' + request.columnId + ']').remove();
}