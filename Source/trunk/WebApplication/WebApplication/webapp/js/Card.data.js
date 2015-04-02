
Card.prototype.save = function () {
	var today = Date.now();
	this.updated = today;

	var data = {
		"id": this.id,
		"name": this.name,
		"description": this.description,
		"owner": this.owner,
		"columnId": this.columnId,
		"boardId": this.boardId,
		"priority": this.priority,
		"checklistCount": this.checklistCount,
		"commentCount": this.commentCount,
		"runningTimeLog": this.runningTimeLog,
		"currentEstimated": this.currentEstimated,
		"attachedCount": this.attachedCount,
		"style": this.style,
		"created": this.created,
		"updated": this.updated,
		"status": 1
	};

	Database.saveCard(data);
};

Card.getById = function (id, success) {
	Database.getCardById(id, function (data) {
		success(data);
	});
};

Card.getAll = function (columnId, success) {
	Database.getAllCard(columnId, function (data) {
		success(data);
	});
};

Card.getAllOfBoard = function (boardId, success) {
	Database.getAllCardOfBoard(boardId, function (data) {
		success(data);
	});
};
