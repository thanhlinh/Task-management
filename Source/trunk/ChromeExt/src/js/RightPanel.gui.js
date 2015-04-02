/*----------Add Column Input------------*/
var RightPanel = function () {
	this.main = null;
	this.addColumndiv = null;
	this.description = null;
	this.advertisement = null;
	this.themesPanel = null;
	this.theme = {};
	this.languagePanel = null;
	this.language = {};
	this.inputColumn = null;
};

RightPanel.prototype = {
	display: function () {
		this.main = $('<div id="right-panel"/>');

		this.addColumndiv = $('<div class="add-column" />');
		this.description = $('<div class="description" />');
		this.advertisement = $('<div class="advertisement" />');
		this.themesPanel = $('<div class="themesPanel" />');
		this.theme['default'] = $('<div class="default-theme theme-selected theme-item"/>');
		this.theme['sioux'] = $('<div class="sioux-theme theme-item"/>');
		this.languagePanel = $('<select class="languagePanel selectpicker" />');
		this.language['en'] = $('<option class="en-language" />');
		this.language['vi'] = $('<option class="vi-language" />');
		this.inputColumn = $('<paper-input />');
		this.inputColumn.attr('class', 'add-column-input');
		this.inputColumn.attr('label', 'Add a column');
		this.addColumndiv
			.append(this.inputColumn);

		this.theme['default']
			.hover(
			function () {
				$(this).css('opacity','1');
				},
			function () {
				if (!$(this).hasClass('theme-selected')) {
					$(this).css('opacity', '0.5');
				}
			})
			.click(function () {
				SEND_MESSAGE_TO_BACKGROUND({
					type: 'change-theme',
					name: '.default-theme',
					href: 'css/default.less'
				});
			});

		this.theme['sioux']
			.hover(
			function () {
				$(this).css('opacity', '1');
			},
			function () {
				if (!$(this).hasClass('theme-selected')) {
					$(this).css('opacity', '0.5');
				}
			})
			.click(function () {
				SEND_MESSAGE_TO_BACKGROUND({
					type: 'change-theme',
					name: '.sioux-theme',
					href: 'css/sioux.less'
				});
			});

		this.themesPanel
			.append(this.theme['default'])
			.append(this.theme['sioux']);

		this.language['en']
			.text('English')
			.appendTo(this.languagePanel);
		this.language['vi']
			.text('Tiếng Việt')
			.appendTo(this.languagePanel);

		this.main
			.append(this.addColumndiv)
			.append(this.description)
			.append(this.advertisement)
			.append(this.themesPanel)
			.append(this.languagePanel);


		$('#main-content').append(this.main);	

		this.inputColumn.change(function () {

			var boardID = displaying['boardid'];

			SEND_MESSAGE_TO_BACKGROUND({
				type: 'create-column',
				boardId: boardID,
				columnName: $(this).val()
			});

			$(this).removeAttr('value');
		});
	}
};