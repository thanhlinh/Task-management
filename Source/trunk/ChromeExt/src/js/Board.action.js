$(document).ready(function () {
	var installColumnPanel = $('#install-column-panel');
	/*----------Create New Board------------*/
	
	/*----------Create Scrum Board------------*/
	$('#install-scrum-board').on('click', function () {
		LOG && log('Create Scrum Board');
		installColumnPanel.appendTo('#hide-div');
		//var boardId = $('.BoadrContent').attr('id');

		displaying['boardid'] = $('.BoadrContent').attr('id');
		displaying['boardname'] = $('input.boardname').val();

		SEND_MESSAGE_TO_BACKGROUND({
			type: 'create-scrum-board',
			boardId: displaying['boardid'],
			boardName: displaying['boardname']
		})
	});
	/*----------Create custom Board------------*/
	$('#install-issue-board').on('click', function () {
		alert('The function is not complete. you just use scrum board or custom board');
	});
	/*---------------------------------------*/
	/*----------Create custom Board------------*/
	$('#install-custom-board').on('click', function () {
		LOG && log('Create Custom Board');
		installColumnPanel.appendTo('#hide-div');
		displaying['boardid'] = $('.BoadrContent').attr('id');
		displaying['boardname'] = $('input.boardname').val();

		SEND_MESSAGE_TO_BACKGROUND({
			type: 'display-board',
			boardId: displaying['boardid'],
			boardName: displaying['boardname'],
			createCustomBoard: true
		});
	});
	/*---------------------------------------*/
});