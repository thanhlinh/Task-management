
var Database = {
	connected: false,
	db: null,
	successCb: [],

	dbname: 'SiouxTaskManagement',
	dbversion: 1,
	schema: {
		Card: { keyPath: 'id' },
		Column: { keyPath: 'id' },
		Board: { keyPath: 'id' },
		ColumnSetting: { keyPath: 'id' },
		TimeLog: { keyPath: 'id' }
	},

	onsuccess: function (cb) {
		console.log('push call back');
		Database.successCb.push(cb);
	},

	error: function (e) {

	},

	upgradeneeded: function (e) {
		var db = e.target.result;
		e.target.transaction.onerror = Database.error;

		for (var name in Database.schema) {
			if (db.objectStoreNames.contains(name)) {
				db.deleteObjectStore(name);
			}
			var store = db.createObjectStore(name, Database.schema[name]);
		}
	},

	success: function (e) {
		Database.connected = true;
		Database.db = e.target.result;
		for (var i = 0, l = Database.successCb.length; i < l; i++) {
			Database.successCb[i]();
		}
	},

	init: function () {
		LOG && log('indexddb 47: Database init');
		$.ajax({
			url: config('server') + "/ws/account-info",
			cache: false, async: false,
			success: function(account) {
				if (account.success) {
					LOG && log('-- USE DATABASE ' + Database.dbname + '_' + account.data.db);
					var request = indexedDB.open(Database.dbname + '_' + account.data.db, Database.dbversion);
					request.onupgradeneeded = Database.upgradeneeded;
					request.onsuccess = Database.success;
					request.onerror = Database.error;
					LOGGED_USER = account.data;
					Enable.backgroundSync = true;
					LOG && log('get account info success');
				} else {
					window.location.href = config('server') + "/Login/Login";
					LOG && log('-- USE DATABASE ' + Database.dbname);
					var request = indexedDB.open(Database.dbname, Database.dbversion);
					request.onupgradeneeded = Database.upgradeneeded;
					request.onsuccess = Database.success;
					request.onerror = Database.error;
					LOGGED_USER = false;
					LOG && log('get account info not success');
				}
			},
			error: function () {
				LOG && log('-- USE DATABASE ' + Database.dbname);
				var request = indexedDB.open(Database.dbname, Database.dbversion);
				request.onupgradeneeded = Database.upgradeneeded;
				request.onsuccess = Database.success;
				request.onerror = Database.error;
				LOGGED_USER = false;
				LOG && log('get account info error');
			}
		});
	},

	saveBoard: function (data, callback) {
		
		data.updated = new Date(data.updated);
		data.created = new Date(data.created);
		$.ajax({
			url: config('server') + '/ws/save-board',
			data: {
				data: JSON.stringify(data)
			}, cache: false,
			async: false,
			type: 'post',
			success: function (e) {
				if (Database.connected) {
					var db = Database.db;
					var trans = db.transaction("Board", "readwrite");
					var store = trans.objectStore("Board");
					var request = store.put(e.data);
					request.onsuccess = callback;
					request.onerror = function (e) { };
				}
			},
			error: function (e) {
				LOG && log("ADD BOARD ERROR");
				LOG && log(e);
			}
		});
	},

	getBoardById: function(data, callback){
		$.ajax({
			url: config('server') + '/ws/get-board',
			data: {
				boardId: data
			}, cache: false,
			type: 'get',
			success: function (e) {
				LOG && log("GET BOARD success");
				LOG && log(e);
				callback(e.data);
			},
			error: function (e) {
				LOG && log("GET BOARD ERROR");
				LOG && log(e);
			}
		});
	},

	getAllBoard: function(callback){
		$.ajax({
			url: config('server') + '/ws/get-all-board',
			cache: false,
			type: 'get',
			success: function (e) {
				LOG && log("Get ALL BOARD success");
				LOG && log(e);
				callback(e.data);
			},
			error: function (e) {
				LOG && log("GET ALL BOARD ERROR");
				LOG && log(e);
			}
		});
	},

	saveColumn: function (data, callback){
		data.updated = new Date(data.updated);
		data.created = new Date(data.created);
		$.ajax({
			url: config('server') + '/ws/save-column',
			data: {
				data: JSON.stringify(data)
			}, cache: false,
			async: false,
			type: 'post',
			success: function (e) {
				LOG && log("ADD COLUMN success");
				LOG && log(e);
				if (Database.connected) {
					var db = Database.db;
					var trans = db.transaction("Column", "readwrite");
					var store = trans.objectStore("Column");
					var request = store.put(e.data);
					request.onsuccess = callback;
					request.onerror = function (e) { };
				}
			},
			error: function (e) {
				LOG && log("ADD COLUMN ERROR");
				LOG && log(e);
			}
		});
	},

	getColumnById: function (data, callback) {
		$.ajax({
			url: config('server') + '/ws/get-column',
			data: {
				columnId: data
			}, cache: false,
			type: 'get',
			success: function (e) {
				LOG && log("GET COLUMN success");
				LOG && log(e);
				callback(e.data);
			},
			error: function (e) {
				LOG && log("GET COLUMN ERROR");
				LOG && log(e);
			}
		});
	},

	getAllColumn: function (data, callback) {
		$.ajax({
			url: config('server') + '/ws/get-all-column',
			data: {
				boardId: data
			}, cache: false,
			success: function (e) {
				LOG && log("Get ALL COLUMN success");
				LOG && log(e);
				callback(e.data);
			},
			error: function (e) {
				LOG && log("GET ALL COLUMN ERROR");
				LOG && log(e);
			}
		});
	},

	saveCard: function (data, callback) {
		data.updated = new Date(data.updated);
		data.created = new Date(data.created);
		LOG && log(data);
		$.ajax({
			url: config('server') + '/ws/save-card',
			data: {
				data: JSON.stringify(data)
			}, cache: false,
			async: false,
			type: 'post',
			success: function (e) {
				LOG && log("ADD CARD success");
				LOG && log(e);
				if (Database.connected) {
					var db = Database.db;
					var trans = db.transaction("Card", "readwrite");
					var store = trans.objectStore("Card");
					var request = store.put(e.data);
					request.onsuccess = callback;
					request.onerror = function (e) { };
				}
			},
			error: function (e) {
				LOG && log("ADD CARD ERROR");
				LOG && log(e);
			}
		});
	},

	getCardById: function (data, callback) {
		$.ajax({
			url: config('server') + '/ws/get-card',
			data: {
				cardId: data
			}, cache: false,
			type: 'get',
			success: function (e) {
				LOG && log("GET CARD success");
				LOG && log(e);
				callback(e.data);
			},
			error: function (e) {
				LOG && log("GET CARD ERROR");
				LOG && log(e);
			}
		});
	},

	getAllCard: function (data, callback) {
		$.ajax({
			url: config('server') + '/ws/get-all-card',
			data: {
				columnId: data
			}, cache: false,
			success: function (e) {
				LOG && log("Get ALL CARD success");
				LOG && log(e);
				callback(e.data);
			},
			error: function (e) {
				LOG && log("GET ALL CARD ERROR");
				LOG && log(e);
			}
		});
	},

	getAllCardOfBoard: function(data, callback){
		$.ajax({
			url: config('server') + '/ws/get-all-card-of-board',
			data: {
				boardId: data
			}, cache: false,
			success: function (e) {
				LOG && log("Get ALL CARD OF BOARD success");
				LOG && log(e);
				callback(e.data);
			},
			error: function (e) {
				LOG && log("GET ALL CARD OF BOARD ERROR");
				LOG && log(e);
			}
		});
	},

	getALlColumnSetting: function (data, callback) {
		$.ajax({
			url: config('server') + '/ws/get-all-columnsetting',
			data: {
				boardId: data
			}, cache: false,
			type:"get",
			success: function (e) {
				LOG && log("Get ALL COLUMNSETTING success");
				LOG && log(e);
				callback(e.data);
			},
			error: function (e) {
				LOG && log("GET ALL COLUMNSETTING ERROR");
				LOG && log(e);
			}
		});
	},

	createOrUpdateColumnSetting: function(data, callback){
		$.ajax({
			url: config('server') + '/ws/save-columnsetting',
			data: {
				data: JSON.stringify(data)
			}, cache: false,
			async: false,
			type: 'post',
			success: function (e) {
				LOG && log("CREATE OR UPDATE COLUMNSETTING success");
				LOG && log(e);
			},
			error: function (e) {
				LOG && log("CREATE OR UPDATE COLUMNSETTINGS ERROR");
				LOG && log(e);
			}
		});
	}
};

Database.init();
