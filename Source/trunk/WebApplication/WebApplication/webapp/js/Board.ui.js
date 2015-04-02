
Board.prototype.display = function () {

	LOG && log('Board.ui display 4 - id: '+this.id);

	this.wrapper = $('<div class="board-wrapper" />');
	this.content = $('<div class="board-content" />');
	this.content.attr('id', this.id);

	Layout.content.empty();
	Layout.title.val(this.name);
	var tmp = this;
	this.content.sortable({
		axis: "x",
		containment: "parent",
		revert: "invalid",
		start: function (e, ui) {
			$(this).attr('data-previndex', ui.item.index());
		},
		sort: function (event, ui) {
			var that = $(this),
				w = ui.helper.outerWidth();
			that.children().each(function () {
				if ($(this).hasClass('ui-sortable-helper') || $(this).hasClass('ui-sortable-placeholder'))
					return true;
				var dist = Math.abs(ui.position.left - $(this).position().left),
					before = ui.position.left > $(this).position().left;
				if ((w - dist) > (w / 2) && (dist < w)) {
					if (before)
						$('.ui-sortable-placeholder', that).insertBefore($(this));
					else
						$('.ui-sortable-placeholder', that).insertAfter($(this));
					return false;
				}
			});
		},
		update: function (event, ui) {
			var columnid = $(ui.item).attr('id');
			var newindex = ui.item.index();
			var oldindex = $(this).attr('data-previndex');
			SEND_MESSAGE_TO_BACKGROUND({
				type: 'column-move-done',
				columnId: columnid,
				newIndex: newindex,
				oldIndex: oldindex,
				boardId: tmp.id
			});
		}
	});
	
	

	if (this.id != null) {
		localStorage['currentDisplayBoard'] = this.id;
	}

	var self = this;

	SEND_MESSAGE_TO_BACKGROUND({
		type: 'get-all-column',
		boardId: this.id
	}, function (data) {
		self.columns = [];
		for (var i = 0, l = data.length; i < l; i++) {
			var column = new Column(data[i]);
			self.columns.push(column);
		}
		self.columns.sort(columnCompare);
	});
	currentBoard = self;

	this.wrapper.empty();
	if (this.columnCount > 0) {
		this.wrapper.append(this.content)
			.appendTo(Layout.content);
		Layout.addColumn.removeAttr('disabled');
	} else {
		var installColumnPanel = $('#install-column-panel').clone(true);
		this.wrapper.append(installColumnPanel)
			.append(this.content)
			.appendTo(Layout.content);
		Layout.addColumn.attr('disabled', 'disabled');
	}

	SEND_MESSAGE_TO_BACKGROUND({
		type: 'get-columnsetting',
		boardId: this.id
	});
}