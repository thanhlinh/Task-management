$(document).ready(function(){
	$("#menu-toggle").click(function(e) {
        e.preventDefault();
        $("#wrapper, #sidebar-wrapper  div").toggleClass("active");
	});
	$('#col1, #col2').sortable({
		connectWith: '.column-contain'
	});
	var playbt = $('.time-count core-icon');
	var toggle = false;
	playbt.click(function(){
		var divparent = $(this).parent();
		if(divparent.hasClass('counting')){
			var timecount = $('.time-count.counting p');
			timecount.hide();
			divparent.removeClass('counting');
		}
		else {
			divparent.addClass('counting');
			var timecount = $('.time-count.counting p');
			timecount.show();
		}
	});
	// alert($('#board-wrapper .card').length);
	$('#board-wrapper .card #column-action').click(function(){
		$('#board-wrapper').slideUp(200);
		$('#column-detail-wrapper').slideDown(200);
		$('#page-content-wrapper').css("background-color","#FAFAFA");
	});
	$('.card-short').click(function(){
        var sender = $(this);
        if(sender.hasClass('selected'))
			sender.removeClass('selected');
        else
			sender.addClass('selected');
	});
	$('.details-card-description-text').click(function(){
		$('.details-card-description .details-card-description-text').hide();
		$('.details-card-description-edit-hide-on').show();
	});
	$('.details-card-description-edit-hide-on .cancel-description').click(function(){
	$('.details-card-description .details-card-description-text').show();
		$('.details-card-description-edit-hide-on').hide();
	});
	$('.details-card-icon-name .details-card-name').click(function(){
		$('.details-card-edit-name-hide-on').show();
	});
	$('.details-card-edit-name-hide-on .button-cancel-name').click(function(){
		$('.details-card-edit-name-hide-on').hide();
	});
	$('.details-card-checklist .details-card-activity .details-card-activity-icon-name .details-card-activity-name').click(function(){
		$('.details-card-checklist-edit-hide-on').show();
	});
	$('.details-card-checklist-edit-hide-on .cancel-checklist').click(function(){
		$('.details-card-checklist-edit-hide-on').hide();
	});
});