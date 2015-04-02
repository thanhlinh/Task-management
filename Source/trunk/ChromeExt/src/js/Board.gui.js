/*----------Board Draw----*/
var BoardUi = function () {
	this.mainContent = null;
	this.boardContent = null;
	this.divAddColumn = null;
	this.boardTitle = null;
};


BoardUi.prototype = {
	display: function (boardId, boardName) {
		this.mainContent = $('#main-content');
		this.boardContent = $('<div class="BoadrContent"/>');
		this.divAddColumn = $('<div class="add-column"/>');
		this.boardTitle = $('input.boardname');
		/*----------Change Board Name------------*/
		this.boardTitle.val(boardName).attr('id', boardId)
			.change(function () {
				var boardid = $(this).attr('id');
				var newname = $(this).val();
				SEND_MESSAGE_TO_BACKGROUND({
					type: 'update-name',
					boardId: parseInt(boardid),
					newName: newname
				});
				$(this).blur();
			});
		/*----------------Column move in board---------------------*/
		this.boardContent.sortable({
			axis: "x",
			containment: "parent",
			revert: "invalid",
			start: function (e, ui) {
				$(this).attr('data-previndex', ui.item.index());
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
					boardId: displaying['boardid'],
					boardName: displaying['boardname']
				});
			}
		});
		/*----------------------------------------*/
		this.mainContent.empty();
		this.boardContent.attr('id', boardId);
		this.mainContent.append(this.boardContent);

	},
};

