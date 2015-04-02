
BACKGROUND['create-card'] = function (request, sendResponse) {

	LOG && log('Card.background create card');

	var card = new Card();

	Column.getById(request.columnId, function (data) {
		var column = new Column(data);

		card.name = request.name;
		card.columnId = request.columnId;
		card.boardId = request.boardId;
		card.priority = column.cardCount;
		card.save();

		column.cardCount = column.cardCount + 1;
		column.save();
		request.card = card;
		request.column = column;
		SEND_MESSAGE_TO_INDEX(request);
	});
}

BACKGROUND['get-all-card'] = function (request, sendResponse) {

	LOG && log('Card.background get all card');
	Card.getAll(request.columnId, function (cardList) {
		SEND_MESSAGE_TO_INDEX({
			type: 'get-all-card',
			boardId: request.boardId,
			columnId: request.columnId,
			cards: cardList
		});
	});
}

BACKGROUND['card-move-done'] = function (request, sendResponse) {
	
	Card.getById(request.cardId, function (data) {
		var card = new Card(data);
		
		card.columnId = request.columnId;
		Card.getAll(request.columnId, function (allCardNewColumn) {
			for (var i = 0, l = allCardNewColumn.length; i < l; i++) {
				var _card = new Card(allCardNewColumn[i]);
				if (_card.priority >= request.cardPriority) {
					_card.priority = _card.priority + 1;
					_card.save();
				}
			}
			Card.getAll(request.oldColumnId, function (allCardOldColumn) {
				for (var i = 0, l = allCardOldColumn.length; i < l; i++) {
					var _card = new Card(allCardOldColumn[i]);
					if (_card.priority > request.oldIndex) {
						_card.priority = _card.priority - 1;
						_card.save();
					}
				}
				card.priority = request.cardPriority;
				card.save();
			})
		});
	});
	Column.getById(request.oldColumnId, function (data) {
		var column = new Column(data);
		column.cardCount = column.cardCount - 1;
		column.save();
	});
	Column.getById(request.columnId, function (data) {
		var column = new Column(data);
		column.cardCount = column.cardCount + 1;
		column.save();
	});
}

BACKGROUND['change-card-style'] = function (request, sendResponse) {

	LOG && log('Card change style');
	Card.getById(request.cardId, function (data) {
		var card = new Card(data);
		card.style = request.style;
		card.save();
	});
}