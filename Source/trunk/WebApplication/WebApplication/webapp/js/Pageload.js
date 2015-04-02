var cssStyle = (typeof localStorage['TaskManagementStyle'] != 'undefined') ? localStorage['TaskManagementStyle'] : 'default';
if (DEBUG)
	$('#main-style-link').attr('href', 'css/' + cssStyle + '.less');
else
	$('#main-style-link').attr('href', 'css/' + cssStyle + '.css');


var Layout = {
	header: null,
	title: null,
	menuToggle: null,
	helperWrapperToggle: null,

	content: null,
	sidebar: null,
	helperWrapper: null,
	helper: null,
	addColumn: null,
	adv: null,
	theme: null,
	language: null,
	gbLoged:null,
	showGbLoged: null,
	teamListContent: null,
	showTeam: null,
	boardListContent: null,
	showBoard: null,
	modal: null,

	resize: function(){
		var wind = $(window);
		var width = wind.width();
		var height = wind.height();

		var modalPopup = $('.current-modal-popup');
		if (modalPopup.length > 0) {
			modalPopup.css({
				top: ((height - modalPopup.height()) >> 1) + 'px',
				left: ((width - modalPopup.width()) >> 1) + 'px'
			});
		}
		var advHeight = Layout.helperWrapper.height() - Layout.helper.height() - Layout.theme.height() - Layout.language.height();
		Layout.adv.css('height', advHeight);
	}
};

var errorMessage = {
	email: "The email address is not recognized. Please fix it and try again.",
	emailavailable: "The email address is available, please enter other email."
};

var currentBoard = null;

var columnCompare = function(a, b) {
	if (a.priority < b.priority)
		return -1;
	if (a.priority > b.priority)
		return 1;
	return 0;
}



var displayLogin = function (url, title, w, h) {

	var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
	var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
	width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
	height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

	var left = ((width / 2) - (w / 2)) + dualScreenLeft;
	var top = ((height / 2) - (h / 2)) + dualScreenTop;
	var newWindow = window.open(url, title, 'scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

	if (window.focus) {
		newWindow.focus();
	}
}

var displayModalPopup = function(obj) {
	var ele = $(obj);
	Layout.modal.fadeIn();
	ele.show();
	ele.addClass('current-modal-popup');
	Layout.resize();
};

var hideModalPopup = function() {
	Layout.modal.fadeOut();
	$('.current-modal-popup').hide().removeClass('current-modal-popup');
};

var isValidEmailAddress = function (emailAddress) {
	var pattern = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i);
	return pattern.test(emailAddress);
}

var slideBarActive = function () {
	if (!Layout.sidebar.hasClass('active')) {
		Layout.sidebar.addClass('active');
	}
	Layout.menuToggle.attr('icon', 'arrow-back');
	if (Layout.showGbLoged.hasClass('show-gb'))
		Layout.gbLoged.css('display', 'block');
	if (Layout.showBoard.hasClass('show-board'))
		Layout.boardListContent.css('display', 'block');
}

var slideBarHide = function () {
	if (Layout.sidebar.hasClass('active')) {
		Layout.sidebar.removeClass('active');
	}
	Layout.menuToggle.attr('icon', 'menu');
	Layout.gbLoged.css('display', 'none');
	Layout.teamListContent.css('display', 'none');
	Layout.boardListContent.css('display', 'none');
}

var helperWrapperActive = function () {
	if (!Layout.helperWrapper.hasClass('active'))
		Layout.helperWrapper.addClass('active');
	if (!Layout.helperWrapperToggle.hasClass('active'))
		Layout.helperWrapperToggle.addClass('active');
	Layout.helperWrapperToggle.attr('icon', 'arrow-forward');
	$('.dropdown-menu').css('right','230px');
}

var helperWrapperHide = function () {
	if (Layout.helperWrapper.hasClass('active'))
		Layout.helperWrapper.removeClass('active');
	if (Layout.helperWrapperToggle.hasClass('active'))
		Layout.helperWrapperToggle.removeClass('active');
	Layout.helperWrapperToggle.attr('icon', 'arrow-back');
	$('.dropdown-menu').css('right', '78px');
}

$(document).ready(function () {
	document.title = 'Task Management';

	$(document).ajaxStart(function () {
		$(".wait-ajax").css("display", "block");
	});

	$(document).ajaxComplete(function () {
		$(".wait-ajax").css("display", "none");
	});

	setTimeout(function () {
		$('.theme-item[name="' + cssStyle + '"]').addClass('theme-selected');
		SEND_MESSAGE_TO_BACKGROUND({
			type: 'get-all-board'
		});

		if (typeof localStorage.currentDisplayBoard != 'undefined') {
			LOG && log('Pageload 27');
			SEND_MESSAGE_TO_BACKGROUND({
				type: 'display-board',
				boardId: localStorage.currentDisplayBoard
			});
		}
	},300);

	Layout.header = $("#header");
	Layout.content = $("#content");
	Layout.sidebar = $("#sidebar");
	Layout.title = $("#title");
	Layout.menuToggle = $("#menu-toggle");
	Layout.helperWrapperToggle = $("#slide-helper");
	Layout.helperWrapper = $("#helper-wrapper");
	Layout.helper = $("#helper");
	Layout.addColumn = $(".add-column-input");
	Layout.adv = $("#adv");
	Layout.theme = $("#theme");
	Layout.language = $("#language-container");
	Layout.showGbLoged = $('.show-gb-loged');
	Layout.gbLoged = $('#gb_loged');
	Layout.teamListContent = $('.my-team-content');
	Layout.showBoard = $('.show-all-board');
	Layout.boardListContent = $('.private-board-content');

	Layout.modal = $("#modal");

	/*----------Left menu slide------------*/
	Layout.menuToggle.click(function (e) {
		if (Layout.sidebar.hasClass('active')) {
			slideBarHide();
		} else {
			slideBarActive();
		}
	});


	Layout.showGbLoged.click(function () {
		if ($(this).hasClass('show-gb')) {
			Layout.gbLoged.slideUp();
			$(this).removeClass('show-gb');
			$(this).attr('icon', 'expand-more');
		} else {
			Layout.gbLoged.slideDown();
			Layout.teamListContent.slideUp();
			Layout.boardListContent.slideUp();
			if (Layout.showBoard.hasClass('show-board')) {
				Layout.showBoard.removeClass('show-board');
				Layout.showBoard.attr('icon', 'expand-more');
			}
			$(this).addClass('show-gb');
			$(this).attr('icon', 'expand-less');
		}
	});

	Layout.showBoard.click(function () {
		if ($(this).hasClass('show-board')) {
			Layout.boardListContent.slideUp();
			$(this).removeClass('show-board');
			$(this).attr('icon', 'expand-more');
		} else {
			Layout.boardListContent.slideDown();
			Layout.teamListContent.slideUp();
			Layout.gbLoged.slideUp();
			if (Layout.showGbLoged.hasClass('show-gb')) {
				Layout.showGbLoged.removeClass('show-gb');
				Layout.showGbLoged.attr('icon', 'expand-more');
			}
			$(this).addClass('show-board');
			$(this).attr('icon', 'expand-less');
		}
	});

	var creating = false;
	$(".create-board-btn").click(function () {
		slideBarHide();
		creating = true;
		/*----------Create Board Name------------*/
		Layout.title.val('');
		Layout.title.attr('placeholder', 'Enter Board Name');
		Layout.content.empty();
		Layout.title.focus();
	});

	Layout.title.change(function () {
		$(this).blur(); //focusout
		if (creating) {

			SEND_MESSAGE_TO_BACKGROUND({
				type: 'create-board',
				boardName: $(this).val()
			});

			creating = false;
		} else {
			SEND_MESSAGE_TO_BACKGROUND({
				type: 'update-board-name',
				boardId: currentBoard.id,
				boardName: $(this).val()
			});
		}
	});

	Layout.addColumn.change(function () {
		LOG && log(currentBoard);
		
		if (currentBoard != null) { 
			LOG && log('Create Column');
			SEND_MESSAGE_TO_BACKGROUND({
				type: 'create-column',
				boardId: currentBoard.id,
				columnName: $(this).val()
			});
		}
		$(this).removeAttr('value');
		$(this).focus();
	});

	$('#show-dialog-columnSetting').click(function () {
		if (typeof localStorage.currentDisplayBoard != 'undefined') {
			SEND_MESSAGE_TO_BACKGROUND({
				type: 'get-columnsetting',
				boardId: currentBoard.id
			});
			setTimeout(function () {
				var columnSettingDialog = new ColumnSettingDialog();
				LOG && log(currentBoard);
				columnSettingDialog.display(currentBoard.id, currentBoard.columns, currentBoard.columnSetting);
				if (currentBoard.columnCount >= 2) {
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
									boardId: currentBoard.id,
									data: items
								});

								$(this).dialog("close");
							}
						}
					});
				} else {
					$('#notification .notification-content').text('You need to create more than 2 columns before setting column !');
					displayModalPopup('#notification');
				};
			}, 500);
		} else {
			$('#notification .notification-content').text('You need to select a board before setting column !');
			displayModalPopup('#notification');
		};
	});

	$('.logout-btn').click(function () {
		$('.wait-ajax').show();
		localStorage.removeItem('currentDisplayBoard');
		$.ajax({
			url: config('server') + "/ws/log-out",
			cache: false, async: false,
			success: function () {
				window.location.href = config('server');
			},
			error: function () {
				LOG && log('Logout error');
			},
			complete: function () {
				$('.wait-ajax').hide();
			}
		});
	});

	Layout.helperWrapperToggle.click(function () {
		if (Layout.helperWrapper.hasClass('active')) {
			helperWrapperHide();
		} else {
			helperWrapperActive();
		}
	});

	$('.add-column-icon').click(function () {
		helperWrapperActive();
		$('.add-column-input #decorator').$.focus();
	});

	$('.send-invite-btn').click(function () {
		$('.first-name-invite').val('');
		$('.last-name-invite').val('');
		$('.email-invite').val('');
		$('.description').val('');
		$('.email-error-message').text('');
		displayModalPopup('#sent-invite');
	});

	$('.close-send-invite').click(hideModalPopup);

	$('.upgrade-btn').click(function () {
		displayLogin(config('server') + '/accounts/upgrade', 'Upgrade account', '510', '320');
	});

	$('.notification-btn').click(hideModalPopup);
	$('.submit-invite').click(function () {
		var emailAddress = $('.email-invite').val();
		var firstName = $('.first-name-invite').val();
		var lastName = $('.last-name-invite').val();
		var description = $('.description textarea').val();
		if (!isValidEmailAddress(emailAddress)) {
			$('.email-error-message').text(errorMessage.email);
		} else {
			hideModalPopup();
			$.ajax({
				url: config('server') + "/ws/invite",
				data: {
					email: emailAddress,
					firstName: firstName,
					lastName: lastName,
					message: description
				},
				type: 'post',
				cache: false, async: true,
				success: function (datacb) {
					switch (datacb.message) {
						case 'emailinvalid': {
							$('.email-error-message').text(errorMessage.email);
							displayModalPopup('#sent-invite');
						}; break;
						case 'emailavailable': {
							$('.email-error-message').text(errorMessage.emailavailable);
							displayModalPopup('#sent-invite');
						}; break;
						case 'successful': {
							$('#notification .notification-content').text('Send invite is successful');
							displayModalPopup('#notification');
						}; break;
						default: {
							$('#notification .notification-content').text('Send invite is error');
							displayModalPopup('#notification');
						}
					}
				},
				error: function () {
					$('#notification .notification-content').text('Send invite is error');
					displayModalPopup('#notification');
				}
			});
		};
	});

	$('.theme-item').click(function () {
		if (!$(this).hasClass('theme-selected')) {
			$('.theme-item').removeClass('theme-selected');
			localStorage['TaskManagementStyle'] = $(this).addClass('theme-selected').attr('name');
			if (DEBUG) {
				location.reload();
			} else {
				$('#main-style-link').attr('href', 'css/' + localStorage['TaskManagementStyle'] + '.css');
			}
		}
	});

	$('#install-scrum-board').click(function () {
		SEND_MESSAGE_TO_BACKGROUND({
			type: 'create-scrum-board',
			boardId: currentBoard.id,
		});
		Layout.addColumn.removeAttr('disabled');
	});

	$('#install-custom-board').click(function () {
		$(this).parent().parent().remove();
		Layout.addColumn.removeAttr('disabled');
	});
	$('#search-icon').click(function () {
		$('.search-input').toggle();
	});


	Layout.modal.click(hideModalPopup);

	$(window).resize(Layout.resize);
	Layout.resize();
});
