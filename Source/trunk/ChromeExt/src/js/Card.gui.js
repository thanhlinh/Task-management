CardUi = function () {
	this.card = null;
	this.cardTop = null;
	this.cardContent = null;
	this.cardBottom = null;
	this.topAvatar = null;
	this.addAvatarBtn = null;
	this.timerCount = null;
	this.timerPlayIcon = null;
	this.timeStopIcon = null;
	this.timerText = null;
	this.cardView = null;
	this.viewDetail = null;
	this.cardTextContent = null;
	this.cardStatus = null;
	this.checklistIcon = null;
	this.checklistText = null;
	this.commentIcon = null;
	this.commentText = null;
	this.attachedIcon = null;
	this.attachedText = null;
	this.estimated = null;

	this.column = null;
};


CardUi.prototype = {
	display: function (card) {
		this.card = $('<div />');
		this.cardTop = $('<div />');
		this.cardContent = $('<div />');
		this.cardBottom = $('<div />');
		this.topAvatar = $('<div />');
		this.addAvatarBtn = $('<core-icon-button />');
		this.timerCount = $('<div />');
		this.timerPlayIcon = $('<core-icon/>');
		this.timerStopIcon = $('<core-icon/>');;
		this.timerText = $('<p />');
		this.cardView = $('<div />');
		this.viewDetail = $('<core-icon />');
		this.cardTextContent = $('<span />');
		this.cardStatus = $('<div />');
		this.checklistIcon = $('<core-icon />');
		this.checklistText = $('<p />');
		this.commentIcon = $('<core-icon />');
		this.commentText = $('<p />');
		this.attachedIcon = $('<core-icon />');
		this.attachedText = $('<p />');
		this.estimated = $('<p />');
		this.columnContain = document.getElementById(card.columnId).getElementsByClassName('column-contain');

		//---------draw card on column------------------

		this.card.attr('id', card.id);
		this.card.addClass('card');
		this.cardTop.addClass('card-top');
		this.topAvatar.addClass('top-avatar');
		this.addAvatarBtn.attr('icon', 'add');
		this.timerCount.addClass('timer-count')
								.addClass('text-center');
		this.timerPlayIcon.addClass('timerplay')
										.attr('icon', 'av:play-arrow');
		this.timerStopIcon.addClass('timerstop')
										.attr('icon', 'av:stop');
		this.timerText.addClass('timer-text');
		this.cardView.addClass('card-view');
		this.viewDetail.addClass('view-detail')
							.attr('icon', 'communication:call-made');
		this.cardContent.addClass('card-content')
								.addClass('text-center');
		this.cardTextContent.addClass('card-text-content');;
		this.cardBottom.addClass('card-bot');
		this.cardStatus.addClass('status');
		this.checklistIcon.attr('icon', 'editor:format-list-bulleted');
		this.checklistText.addClass('checklist-text');
		this.commentIcon.attr('icon', 'communication:chat');
		this.commentText.addClass('comment-text');
		this.attachedIcon.attr('icon', 'editor:attach-file');
		this.attachedText.addClass('attach-text');
		this.estimated.addClass('estimate');

		this.card.append(this.cardTop)
				.append(this.cardContent)
				.append(this.cardBottom);

		this.cardTop.append(this.topAvatar)
						.append(this.timerCount)
						.append(this.cardView);

		this.topAvatar.append(this.addAvatarBtn);

		this.timerCount.append(this.timerStopIcon)
								.append(this.timerPlayIcon)
								.append(this.timerText);
		this.timerText.text('00:00:00');
		this.cardView.append(this.viewDetail);
		this.cardContent.append(this.cardTextContent);
		this.cardBottom.append(this.cardStatus)
								.append(this.estimated);
		this.cardStatus.append(this.checklistIcon)
								.append(this.checklistText)
								.append(this.commentIcon)
								.append(this.commentText)
								.append(this.attachedIcon)
								.append(this.attachedText);

		if (card.checkListCount == 0)
			this.checklistIcon.hide();
		if (card.commentCount == 0)
			this.commentIcon.hide();
		if (card.attachedCount == 0)
			this.attachedIcon.hide();
		if (card.currentEstimated == 0)
			this.estimated.hide();
		this.cardTextContent.text(card.name);

		this.timerStopIcon.hide();
		this.timerText.hide();

		var timerPlayIcon = this.timerPlayIcon;
		var timerStopIcon = this.timerStopIcon;
		var timerText = this.timerText;
		var timeLog;

		$(this.columnContain).append(this.card);

		//-----------action listenning--------------

		this.timerPlayIcon.click(function () {
			var iconString = $(this).attr('icon');

			/* setTimerText();
			 if (iconString == 'av:play-arrow') {
					 $(this).attr('icon', 'av:pause');
					 timerStopIcon.show();
					 timerText.show();
					 timeLog = new TimeLog();
					 timeLog.taskId = card.id;
					 timeLog.startSave();

			 } else {
					 $(this).attr('icon', 'av:play-arrow');
					 timeLog.pauseSave();
			 }
	 });

	 this.timerStopIcon.click(function () {
			 timerStopIcon.hide();
			 timerPlayIcon.hide();
			 timeLog.stopSave();
			 TimeLog.getAllOfTask(card.id, function (items) {
					 var lenght = items.length;
			 }); */
		});

		//-----------set Timervalue--------------
		var setTimerText = function () {
			/*
			TimeLog.getAllOfTask(card.id, function (items) {
					var durationTotal = 0;
					for (var i = 0; i < items.length; i++) {
							durationTotal += items[i].duration;
					}
					if (items.length == 0) {
							timerStopIcon.hide();
							timerText.hide();
					}
					else {
							var state = items[items.length-1].state;
							//stop
							if (state == -1) {
									timerPlayIcon.hide();
									timerStopIcon.hide();
									timerText.text(timerfomat(durationTotal));
									timerText.show();
							}
							else {
									//pause
									timerStopIcon.show();
									if (state == 0) {
											timerText.text(timerfomat(durationTotal));
											timerText.show();
											timerPlayIcon.attr('icon', 'av:play-arrow')
																	 .show();
									}
									//play
									else {
											

											timerPlayIcon.attr('icon', 'av:pause')
																	 .show();
									}
							}
					}
			});*/

		}
		// setTimerText();
	}
};
function timerfomat(x) {
	x = parseInt(x / 1000).toString();
	var pattern = /(-?\d+)(\d{2})/;
	while (pattern.test(x))
		x = x.replace(pattern, "$1:$2");
	return x;
}