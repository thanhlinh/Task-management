
FROM_BACKGROUND['get-all-card'] = function (request, sender, sendResponse) {
	if (currentBoard.id == request.boardId) {
		$(document.getElementById(request.columnId).getElementsByClassName('column-content')).empty();
		for (var i = 0, l = request.cards.length; i < l; i++) {
			for (var j = 0; j < l; j++) {
				if (request.cards[j].priority == i) {
					var card = new Card(request.cards[j]);
					card.display();
				}
			}
		}
	}
}

FROM_BACKGROUND['create-card'] = function (request, sender, sendResponse) {

	if (currentBoard.id == request.boardId) {
		var card = new Card(request.card);
		card.display();
	}

}