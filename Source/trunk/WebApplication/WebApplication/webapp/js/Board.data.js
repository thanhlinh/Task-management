Board.prototype.save = function () {
	var today = Date.now();
	this.updated = today;

	var data = {
		"id": this.id,
		"name": this.name,
		"owner": this.owner,
		"priority": this.priority,
		"columnCount": this.columnCount,
		"created": this.created,
		"updated": this.updated,
		"status": 1,
		"hashCode": md5(this.id.toString() + this.name.toString() + this.columnCount.toString() + this.created.toString() + this.status.toString())
	};

	Database.saveBoard(data);
};

Board.getById = function (id, success) {

	Database.getBoardById(id, function (data) {
		success(data);
	});
};

Board.getAll = function (success) {
	Database.getAllBoard(function (data) {
		success(data);
	});
};