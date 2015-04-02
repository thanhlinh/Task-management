Column.prototype.save = function () {
	var today = Date.now();
	this.updated = today;

	var data = {
		"id": this.id,
		"name": this.name,
		"owner": this.owner,
		"boardId": this.boardId,
		"cardCount": this.cardCount,
		"priority": this.priority,
		"created": this.created,
		"updated": this.updated,
		"status": 1
	};

	Database.saveColumn(data);
}

Column.getById = function (id, success) {
	Database.getColumnById(id, function (data) {
		success(data);
	});
};

Column.getAll = function (boardId, success) {
	Database.getAllColumn(boardId, function (data) {
		success(data);
	});
}