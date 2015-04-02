ColumnSetting.prototype.save = function () {
			
	var today = Date.now();
	this.created = today;
	this.updated = today;

	var data = {
		"id": this.id,
		"columnId": this.columnId,
		"next": this.next,
		"boardId": this.boardId,
		"created": this.created,
		"updated": this.updated,
		"status": this.status
	};

	Database.saveColumnSetting(data);
};


ColumnSetting.getAll = function (boardId, success) {
	Database.getALlColumnSetting(boardId, function (data) {
		success(data);
	});
};



ColumnSetting.createOrUpdate = function (columnId, next, boardId, status) {
	var data = {
		"columnId": columnId,
		"next": next,
		"boardId": boardId,
		"status": status
	};
	Database.createOrUpdateColumnSetting(data);
};