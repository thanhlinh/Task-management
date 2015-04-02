var ColumnSetting = function (params) {
	this.id = null;
	this.columnId = null;
	this.next = 0;
	this.boardId = null;
	this.created = new Date();
	this.updated = null;
	this.status = 1;

	if (typeof params === 'string') {
		LOG && log('params === string');
		ColumnSetting.getById(params, function (columnSetting) {
			this.id = columnSetting.id;
		});

	} else if (typeof params === 'object') {
		LOG && log('params === object');
		this.id = params.id;
		this.columnId = params.columnId;
		this.next = params.next;
		this.boardId = params.boardId;
		this.created = params.created;
		this.updated = params.updated;
		this.status = params.status;
	} else {
		this.id = generateId('column-setting');
	}
}