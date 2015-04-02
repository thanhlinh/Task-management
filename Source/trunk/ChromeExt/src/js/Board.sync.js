
//sync_to_server['board'] = function () {
//	log && log('sync board to server');
//	board.getall(function (boards) {
//		if (syncing.board)
//			return;
//		var syncitems = [];
//		for (var i = 0, l = boards.length; i < l; i++) {
//			if (typeof boards[i].sync == 'undefined' || boards[i].sync)
//				continue;
//			var item = boards[i];
//			item.updated = new date(item.updated);
//			item.created = new date(item.created);
//			syncitems.push(item);
//		}
//		console.log(syncitems);
//		sync(
//			'board',
//			{ boards: json.stringify(syncitems) },
//			function (syncedboards) {
//				log && log(syncedboards);
//				for (var j = 0, l = syncedboards.length; j < l; j++) {
//					log && log('board line 24: update board after sync');
//					log && log(syncedboards[j]);
//					board.sync(syncedboards[j]);
//				}
//			}
//		);
//	});
//};