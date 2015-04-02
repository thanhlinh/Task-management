ColumnSettingDialog = function () {
	this.columnSettingTable = null;
	this.columnSettingTitle = null;
	this.columnSettingBody = null;
	this.columnQuantity = null;
	this.boardId = null;
	this.arraySetting = null;
};


ColumnSettingDialog.prototype = {
	display: function (boardId, columns, list) {
		this.columnSettingTable = $('<table />');
		this.columnSettingTitle = $('<thead />');
		this.columnSettingBody = $('<tbody />');
		this.arraySetting = [];
		this.columnQuantity = columns.length;

		for (var i = 0; i <= this.columnQuantity; i++) {
			var tbrow = $('<tr />');
			for (var j = 0; j <= this.columnQuantity; j++) {
				var tbcolumn = $('<td style="width: 150px" />');
				var tbCheckbox = $('<paper-checkbox />');
				if (i == 0) {
					if (j == 0) {

					} else {
						tbcolumn.text(columns[j - 1].name);
					}
				} else {
					if (j == 0) {
						tbcolumn.text(columns[i - 1].name);
					}
					else {
						tbCheckbox.attr('id', columns[i - 1].id);
						tbCheckbox.attr('oid', columns[j - 1].id);
						for (var index = 0; index < list.length; index++) {
							if (list[index].columnId == columns[i - 1].id && list[index].next == columns[j - 1].id) {
								tbCheckbox.attr('checked', true);
							}
						}
						if (i == j)
							tbCheckbox.attr('disabled', 'true');
						tbcolumn.append(tbCheckbox);
					}
				}

				var self = this.arraySetting;
				tbCheckbox.on('click', function () {
					if ($(this).attr('aria-checked') == 'false') {
						var count = 0;
						for (var i = 0; i < self.length; i++) {
							if (self[i].columnId == $(this).attr('id') && self[i].next == $(this).attr('oid')) {
								self[i].status = 1;
								count++;
								break;
							}
						}
						if (count == 0) {
							self.push({
								'columnId': $(this).attr('id'),
								'next': $(this).attr('oid'),
								'status': 1
							});
						}
					}
					else {
						var count = 0;
						for (var i = 0; i < self.length; i++) {
							if (self[i].columnId == $(this).attr('id') && self[i].next == $(this).attr('oid')) {
								self[i].status = 0;
								count++;
								break;
							}
						}
						if (count == 0) {
							self.push({
								'columnId': $(this).attr('id'),
								'next': $(this).attr('oid'),
								'status': 0
							});
						}
					}
				});
				tbrow.append(tbcolumn);
			}
			this.columnSettingBody.append(tbrow);
		}

		$('.dialog').empty();
		this.columnSettingTable.append(this.columnSettingBody)
													 .appendTo('.dialog');
	}

};