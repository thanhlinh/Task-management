
var Card = function (params) {
	this.id = null;
	this.name = null;
	this.description = null;
	this.owner = 0;
	this.columnId = 0;
	this.boardId = 0;
	this.priority = 0;
	this.checklistCount = 0;
	this.commentCount = 0;
	this.runningTimeLog = 0;
	this.currentEstimated = 0;
	this.attachedCount = 0;
	this.style = 'default';
	this.created = new Date();
	this.updated = null;
	this.status = 1;

	if (IS_INDEX) {
		this.wrapper = null;
		this.header = null;
		this.content = null;
		this.footer = null;
		this.avatar = null;
		this.addAvatarBtn = null;
		this.timerCount = null;
		this.timerPlayIcon = null;
		this.timerStopIcon = null;
		this.timerText = null;
		this.zoomOut = null;
		this.zoomOutIcon = null;
		this.textContent = null;
		this.status = null;
		this.checklistIcon = null;
		this.checklist = null;
		this.commentIcon = null;
		this.comment = null;
		this.attachedIcon = null;
		this.attached = null;
		this.estimated = null;
		this.zoomIn = null;
		this.columnContain = null;
	}

	if (typeof params === 'string') {
		Card.getById(params, function (card) {
			this.id = card.id;
			this.name = card.name;
			this.description = card.description;
			this.owner = card.owner;
			this.columnId = card.columnId;
			this.boardId = card.boardId;
			this.priority = card.priority;
			this.checklistCount = card.checklistCount;
			this.commentCount = card.commentCount;
			this.runningTimeLog = card.runningTimeLog;
			this.currentEstimated = card.currentEstimated;
			this.attachedCount = card.attachedCount;
			this.style = card.style;
			this.created = card.created;
			this.updated = card.updated;
			this.status = card.status;
		});
	} else if (typeof params === 'object') {
		this.id = params.id;
		this.name = params.name;
		this.description = params.description;
		this.owner = params.owner;
		this.columnId = params.columnId;
		this.boardId = params.boardId;
		this.priority = params.priority;
		this.checklistCount = params.checklistCount;
		this.commentCount = params.commentCount;
		this.runningTimeLog = params.runningTimeLog;
		this.currentEstimated = params.currentEstimated;
		this.attachedCount = params.attachedCount;
		this.style = params.style;
		this.created = params.created;
		this.updated = params.updated;
		this.status = params.status;
	} else {
		this.id = generateId('task');
	}

}