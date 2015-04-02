Card.prototype.display = function () {
	this.wrapper = $('<div class="card-wrapper"/>');
	this.header = $('<div class="card-header"/>');
	this.content = $('<div class="card-content text-center"/>');
	this.footer = $('<div class="card-footer"/>');
	this.avatar = $('<div class="card-avatar"/>');
	this.addAvatarBtn = $('<core-icon-button icon="add"/>');
	this.timerCount = $('<div class="timer-count text-center"/>');
	this.timerPlayIcon = $('<core-icon class="timer-play" icon="av:play-arrow"/>');
	this.timerStopIcon = $('<core-icon class="timer-stop" icon="av:stop"/>');;
	this.timerText = $('<p class="timer-text"/>');
	this.zoomOut = $('<div class="zoom-out"/>');
	this.zoomOutIcon = $('<core-icon class="zoom-out-icon" icon="communication:call-received"/>');
	this.textContent = $('<span class="text-content"/>');
	this.status = $('<div class="status"/>');
	this.checklistIcon = $('<core-icon icon="editor:format-list-bulleted"/>');
	this.checklist = $('<p class="check-list"/>');
	this.commentIcon = $('<core-icon icon="communication:chat"/>');
	this.comment = $('<p class="comment"/>');
	this.attachedIcon = $('<core-icon icon="editor:attach-file"/>');
	this.attached = $('<p class="attached"/>');
	this.estimated = $('<p class="estimate"/>');
	this.zoomIn = $('<core-icon icon="communication:call-made" class="zoom-in-icon"/>');
	this.columnContain = document.getElementById(this.columnId).getElementsByClassName('column-content');

	var self = this;

	if (this.style != "default") {
		this.wrapper.addClass('style2');
	}

	this.wrapper.attr('id', this.id)
		.append(this.zoomIn)
		.append(this.header)
		.append(this.content)
		.append(this.footer);

	this.zoomIn.click(function () {
		$(this).parent().removeClass('style2');
		SEND_MESSAGE_TO_BACKGROUND({
			type: 'change-card-style',
			cardId: self.id,
			style: 'default'
		});
	});

	this.zoomOut.click(function () {
		$(this).parent().parent().addClass('style2');
		SEND_MESSAGE_TO_BACKGROUND({
			type: 'change-card-style',
			cardId: self.id,
			style: 'style2'
		});
	});

	this.header.append(this.avatar)
		.append(this.timerCount)
		.append(this.zoomOut);

	this.avatar.append(this.addAvatarBtn);

	this.timerCount.append(this.timerStopIcon)
		.append(this.timerPlayIcon)
		.append(this.timerText);

	this.timerText.text('00:00:00');

	this.zoomOut.append(this.zoomOutIcon);

	this.content.append(this.textContent);

	this.textContent.text(this.name);

	this.footer.append(this.status)
		.append(this.estimated);

	this.status.append(this.checklistIcon)
		.append(this.checklist)
		.append(this.commentIcon)
		.append(this.comment)
		.append(this.attachedIcon)
		.append(this.attached);

	if (this.checkListCount == 0)
		this.checklistIcon.hide();
	if (this.commentCount == 0)
		this.commentIcon.hide();
	if (this.currentEstimated == 0)
		this.estimated.hide();

	this.timerStopIcon.hide();
	this.timerText.hide();

	$(this.columnContain).append(this.wrapper);

}