
var Board = function (params) {

	this.id = null;
	this.name = null;
	this.owner = 0;
	this.priority = 0;
	this.columnCount = 0;
	this.created = new Date();
	this.updated = null;
	this.status = 1;
	this.hashCode = null;
	
	if (IS_INDEX) {
		this.columnSetting = null;
		this.columns = [];
		
		this.wrapper = null;
		this.content = null;
	}

	if (typeof params === 'string') {
		LOG && log('params === string');
		Board.getById(params, function (board) {
			this.id = board.id;
			this.name = board.name;
		});

	} else if (typeof params === 'object') {
		LOG && log('params === object');
		this.id = params.id;
		this.name = params.name;
		this.owner = params.owner;
		this.columnCount = params.columnCount;
		this.created = params.created;
		this.updated = params.updated;
		this.status = params.status;
	} else {
		this.id = generateId('board');
	}

}