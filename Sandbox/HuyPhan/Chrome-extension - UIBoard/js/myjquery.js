$(document).ready(
	function(){
		$('.usdmoney').keypress(function(){
			var usd = parseInt($('.usdmoney').val());
			if(isNaN(usd)) usd = 0;
			var vnd = usd * 21276;
			$('.vndmoney').val(vnd); 
		});
		$('.vndmoney').keypress(function(){
			var vnd = parseInt($('.vndmoney').val());
			if(isNaN(vnd)) vnd = 0;
			var usd = vnd/21276;
			$('.usdmoney').val(usd);
		});
		$('.card').click(function(){
        var sender = $(this);
        if(sender.hasClass('selected'))
          sender.removeClass('selected');
        else
          sender.addClass('selected');
      });
	}
)