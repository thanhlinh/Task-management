$(document).ready(function () {
	/*----------Show dialog columnSetting------------*/
	$('#show-dialog-columnSetting').click(function () {
		SEND_MESSAGE_TO_BACKGROUND({
			type: 'get-columnsetting',
			boardId: displaying['boardid']
		});
		var columnSettingDialog = new ColumnSettingDialog();
		columnSettingDialog.display(displaying['boardid'], displaying['columns'], listNext);
		if (displaying['columns'].length >= 2) {
			$("#dialog-columnSetting").dialog({
				modal: true,
				draggable: false,
				resizable: false,
				position: ['center'],
				show: 'blind',
				hide: 'blind',
				width: "auto",
				dialogClass: 'ui-dialog-osx',
				buttons: {
					"Save": function () {
						var items = columnSettingDialog.arraySetting;

						SEND_MESSAGE_TO_BACKGROUND({
							type: 'update-columnsetting',
							boardId: displaying['boardid'],
							data: items
						});

						$(this).dialog("close");
					}
				}
			});
		} else {
			$('.dialog-content').empty()
						.append('You need to create more than 2 columns before setting column !');
			$('#notification-dialog').dialog({
				modal: true,
				draggable: false,
				resizable: false,
				position: ['center'],
				show: 'blind',
				hide: 'blind',
				width: "auto",
				dialogClass: 'ui-dialog-osx',
				buttons: {
					"Yes": function () {
						$(this).dialog("close");
					}
				}
			});
		}


	});

});

