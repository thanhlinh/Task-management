
var Column = function (params) {
	this.id = null;
	this.name = null;
	this.owner = 0;
	this.boardId = 0;
	this.cardCount = 0;
	this.priority = 0;
	this.created = new Date();
	this.updated = null;
	this.status = 1;

	if (IS_INDEX) {
		this.wrapper = null;
		this.header = null;
		this.content = null;
		this.footer = null;
		this.colName = null;
		this.colSettingDiv = null;
		this.colSetting = null;
		this.colSettingUl = null;
		this.renameItem = null;
		this.renameSetting = null;
		this.delItem = null;
		this.delSetting = null;
		this.addCardBtn = null;
	}

	if (typeof params === 'string') {
		Column.getById(params, function (column) {
			this.id = column.id;
			this.name = column.name;
			this.owner = column.owner;
			this.boardId = column.boardId;
			this.cardCount = column.cardCount;
			this.priority = column.priority;
			this.created = column.created;
			this.updated = column.updated;
			this.status = column.status;
		});
	} else if (typeof params === 'object') {
		this.id = params.id;
		this.name = params.name;
		this.owner = params.owner;
		this.boardId = params.boardId;
		this.cardCount = params.cardCount;
		this.priority = params.priority;
		this.created = params.created;
		this.updated = params.updated;
		this.status = params.status;
	} else {
		this.id = generateId('column');
	}
}