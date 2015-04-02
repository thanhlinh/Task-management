var TimeLog = function () {
	this.id = generateId('time-log');
	this.taskId = 0;
	this.start = 0;
	this.state = 0;
	this.end = 0;
	this.duration = 0;
	this.sync = 1;

	return true;
};

TimeLog.State = {
	start: 1,
	pause: 0,
	stop: -1
};

TimeLog.prototype = {
	startSave: function () {

		if (Database.connected) {
			var db = Database.db;
			var trans = db.transaction("TimeLog", "readwrite");
			var store = trans.objectStore("TimeLog");
			var date = Date.now();

			this.start = date;

			var request = store.put({
				"id": this.id,
				"taskId": this.taskId,
				"start": this.start,
				"state": TimeLog.State.start,
				"end": 0,
				"duration": 0,
				"sync": 1
			});

			request.onsuccess = function (e) {
				console.log(e.value);
			}

			request.onerror = function (e) { }

		}
	},

	pauseSave: function () {

		if (Database.connected) {
			var db = Database.db;
			var trans = db.transaction("TimeLog", "readwrite");
			var store = trans.objectStore("TimeLog");
			var date = Date.now();

			this.end = date;

			var request = store.put({
				"id": this.id,				
				"taskId": this.taskId,
				"start": this.start,
				"state": TimeLog.State.pause,
				"end": this.end,
				"duration": this.end - this.start,
				"sync": 1
			});

			request.onsuccess = function (e) {
				console.log(e.value);
			}

			request.onerror = function (e) { }
		}
	},

	stopSave: function () {

		if (Database.connected) {
			var db = Database.db;
			var trans = db.transaction("TimeLog", "readwrite");
			var store = trans.objectStore("TimeLog");
			var date = Date.now();

			this.end = date;

			var request = store.put({
				"id": this.id,
				"taskId": this.taskId,
				"start": this.start,
				"state": TimeLog.State.stop,
				"end": this.end,
				"duration": this.end - this.start,
				"sync": 1
			});

			request.onsuccess = function (e) {
				console.log(e.value);
			}

			request.onerror = function (e) { }
		}
	}
};


TimeLog.getAllOfTask = function (taskId, success, error) {

	var items = [];
	var ended = 0;

	if (Database.connected) {
		var db = Database.db;
		var trans = db.transaction("TimeLog", "readonly");
		var store = trans.objectStore("TimeLog");

		trans.oncomplete = function (evt) {
			success(items);
		};

		var keyRange = IDBKeyRange.lowerBound(0);
		var cursorRequest = store.openCursor(keyRange);

		cursorRequest.onsuccess = function (e) {
			var result = e.target.result;
			if (!!result == false)
				return;

			if (result.value.taskId == taskId) {
				items.push(result.value);
			}
			result.continue();
		};
		cursorRequest.onerror = error;
	}
}