Column.prototype.display = function () {
	this.wrapper = $('<div class="column-wrapper col-xs-2" />');
	this.header = $('<div class="column-header" />');
	this.content = $('<div class="column-content" />');
	this.footer = $('<div class="column-footer" />');
	this.colName = $('<input class="lead column-name"/>');;
	this.colSettingDiv = $('<div class="dropdown"/>');
	this.colSetting = $('<paper-icon-button id="column-action" class="dropdown-toggle" data-toggle="dropdown"/>');
	this.colSettingUl = $('<ul class="dropdown-menu" />');
	this.renameItem = $('<li />');
	this.renameSetting = $('<a class="rename-column"/>');
	this.delItem = $('<li />');
	this.delSetting = $('<a class="delete-column"/>');
	this.addCardBtn = $('<paper-input class="add-card-input"/>');

	this.wrapper.attr('id',this.id);
	
	var self = this;

	this.colName
		.val(this.name)
		.prop('disabled', true)
		.focusout(function () {
			$(this).prop('disabled', true);
		})
		.change(function () {
			SEND_MESSAGE_TO_BACKGROUND({
				type: 'update-column-name',
				columnId: self.id,
				columnName: $(this).val()
			});
			$(this).prop('disabled', true);
			$(this).blur();
		});

	this.colSetting.attr('icon', 'expand-more');
	
	this.addCardBtn.attr('label', 'Add a card');

	this.renameSetting.text("Rename")
		.click(function () {
			self.colName.prop('disabled', false)
				.focus();
		});

	this.delSetting.text("Delete")
		.click(function () {
			SEND_MESSAGE_TO_BACKGROUND({
				type: 'delete-column',
				columnId: self.id,
				boardId: currentBoard.id,
			});
		});

	this.renameItem.append(this.renameSetting);
	this.delItem.append(this.delSetting);
	this.colSettingUl.append(this.renameItem)
		.append(this.delItem);
	this.colSettingDiv.append(this.colSetting)
		.append(this.colSettingUl);

	this.header.append(this.colName)
		.append(this.colSettingDiv);

	var self = this;

	this.addCardBtn.change(function () {
		
		SEND_MESSAGE_TO_BACKGROUND({
			type: 'create-card',
			name: $(this).val(),
			boardId: currentBoard.id,
			columnId: self.id
		});
		self.addCardBtn.val('');
		self.addCardBtn.focus();
	});

	this.footer.append(this.addCardBtn);

	this.content.sortable({
		helper: 'clone',
		forcePlaceholderSize: true,
		placeholder: "card-placeholder-style",
		dropOnEmpty: true,
		connectWith: '.column-content',
		revert: true,
		cursor: 'move',
		start: function (e, ui) {
			$(this).attr('data-previndex', ui.item.index());
			$(this).attr('old-column', $(ui.helper).parent().parent().attr('id'));
		},
		beforeStop: function (event, ui) {
			var columnId = self.id;
			var columnholderid = $(ui.placeholder).parent().parent().attr('id');
			for (var index = 0,l =currentBoard.columnSetting.length; index < l; index++) {
				if ((currentBoard.columnSetting[index].columnId == columnId) &&
						(currentBoard.columnSetting[index].next == columnholderid))
					return true;
			}
			return false;
		},
		stop: function (event, ui) {
			var cardid = $(ui.item).attr('id');
			var columnid = $(ui.item).parent().parent().attr('id');
			var oldColumnid = $(this).attr('old-column');
			var newIndex = ui.item.index();
			var oldindex = $(this).attr('data-previndex');
			$(this).removeAttr('data-previndex');
			LOG && log(oldindex + " : " + newIndex);
			LOG && log("old columnid : " + oldColumnid);
			//update to all tabs
			SEND_MESSAGE_TO_BACKGROUND({
				type: 'card-move-done',
				cardId: cardid,
				oldIndex: oldindex,
				oldColumnId: oldColumnid,
				columnId: columnid,
				cardPriority: newIndex,
				boardId: currentBoard.id,
				boardName: currentBoard.name
			});

		}

	});

	this.wrapper.append(this.header)
		.append(this.content)
		.append(this.footer);

	$('.board-wrapper .board-content').append(this.wrapper);

	SEND_MESSAGE_TO_BACKGROUND({
		type: 'get-all-card',
		boardId: this.boardId,
		columnId: this.id
	});
	
}