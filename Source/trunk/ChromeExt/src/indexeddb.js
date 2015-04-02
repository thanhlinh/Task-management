
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
			type: 'post',
			async: false,
			success: function (e) {
				LOG && log('save board to sever is success');
				LOG && log(e);
				if (Database.connected) {
					if (LOGGED_USER != false) {
						var db = Database.db;
						var trans = db.transaction("Board", "readwrite");
						var store = trans.objectStore("Board");
						var request = store.put(e.data);
						request.onsuccess = callback;
						request.onerror = function (e) { };
					} else {
						var db = Database.db;
						var trans = db.transaction("Board", "readwrite");
						var store = trans.objectStore("Board");
						var request = store.put(data);
						request.onsuccess = callback;
						request.onerror = function (e) { };
					}
				}
			},
			error: function (e) {
				var db = Database.db;
				var trans = db.transaction("Board", "readwrite");
				var store = trans.objectStore("Board");
				var request = store.put(data);
				request.onsuccess = callback;
				request.onerror = function (e) { };
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
				if (LOGGED_USER != false) {
					callback(e.data);
				} else {
					if (Database.connected) {
						var db = Database.db;
						var trans = db.transaction("Board", "readonly");
						var store = trans.objectStore("Board");
						var request = store.get(data);
						request.onsuccess = function (e) {
							var board = e.target.result;
							callback(board);
						}
						request.onerror = function (e) { };
					}
				}
			},
			error: function (e) {
				if (Database.connected) {
					var db = Database.db;
					var trans = db.transaction("Board", "readonly");
					var store = trans.objectStore("Board");
					var request = store.get(data);
					request.onsuccess = function (e) {
						var board = e.target.result;
						callback(board);
					}
					request.onerror = function (e) { };
				}
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
				if (LOGGED_USER != false) {
					callback(e.data);
				} else {
					if (Database.connected) {
						var items = [];
						var db = Database.db;
						var trans = db.transaction("Board", "readonly");
						var store = trans.objectStore("Board");
						trans.oncomplete = function (evt) {
							callback(items);
						};
						var keyRange = IDBKeyRange.lowerBound(0);
						var cursorRequest = store.openCursor(keyRange);
						cursorRequest.onsuccess = function (e) {
							var result = e.target.result;
							if (!!result == false)
								return;
							if (result.value.status == 1)
								items.push(result.value);
							result.continue();
						};
						cursorRequest.onerror = function (e) { };
					}
				}
			},
			error: function (e) {
				if (Database.connected) {
					var items = [];
					var db = Database.db;
					var trans = db.transaction("Board", "readonly");
					var store = trans.objectStore("Board");
					trans.oncomplete = function (evt) {
						callback(items);
					};
					var keyRange = IDBKeyRange.lowerBound(0);
					var cursorRequest = store.openCursor(keyRange);
					cursorRequest.onsuccess = function (e) {
						var result = e.target.result;
						if (!!result == false)
							return;
						if (result.value.status == 1)
							items.push(result.value);
						result.continue();
					};
					cursorRequest.onerror = function (e) { };
				}
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
				if (Database.connected) {
					if (LOGGED_USER != false) {
						var db = Database.db;
						var trans = db.transaction("Column", "readwrite");
						var store = trans.objectStore("Column");
						var request = store.put(e.data);
						request.onsuccess = callback;
						request.onerror = function (e) { };
					} else {
						var db = Database.db;
						var trans = db.transaction("Column", "readwrite");
						var store = trans.objectStore("Column");
						var request = store.put(data);
						request.onsuccess = callback;
						request.onerror = function (e) { };
					}
				}
			},
			error: function (e) {
				var db = Database.db;
				var trans = db.transaction("Column", "readwrite");
				var store = trans.objectStore("Column");
				var request = store.put(data);
				request.onsuccess = callback;
				request.onerror = function (e) { };
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
				if (LOGGED_USER != false) {
					callback(e.data);
				} else {
					if (Database.connected) {
						var db = Database.db;
						var trans = db.transaction("Column", "readonly");
						var store = trans.objectStore("Column");
						var request = store.get(data);
						request.onsuccess = function (e) {
							var column = e.target.result;
							callback(column);
						}
						request.onerror = function (e) { };
					}
				}
			},
			error: function (e) {
				if (Database.connected) {
					var db = Database.db;
					var trans = db.transaction("Column", "readonly");
					var store = trans.objectStore("Column");
					var request = store.get(data);
					request.onsuccess = function (e) {
						var column = e.target.result;
						callback(column);
					}
					request.onerror = function (e) { };
				}
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
				if (LOGGED_USER != false) {
					callback(e.data);
				} else {
					if (Database.connected) {
						var items = [];
						var db = Database.db;
						var trans = db.transaction("Column", "readonly");
						var store = trans.objectStore("Column");
						var keyRange = IDBKeyRange.lowerBound(0);
						var cursorRequest = store.openCursor(keyRange);
						trans.oncomplete = function (evt) {
							callback(items);
						};
						cursorRequest.onsuccess = function (e) {
							var result = e.target.result;
							if (!!result == false)
								return;
							if (result.value.boardId == data && result.value.status == 1) {
								items.push(result.value);
							}
							result.continue();
						};
						cursorRequest.onerror = function (e) { };
					}
				}
			},
			error: function (e) {
				if (Database.connected) {
					var items = [];
					var db = Database.db;
					var trans = db.transaction("Column", "readonly");
					var store = trans.objectStore("Column");
					var keyRange = IDBKeyRange.lowerBound(0);
					var cursorRequest = store.openCursor(keyRange);
					trans.oncomplete = function (evt) {
						callback(items);
					};
					cursorRequest.onsuccess = function (e) {
						var result = e.target.result;
						if (!!result == false)
							return;
						if (result.value.boardId == data && result.value.status == 1) {
							items.push(result.value);
						}
						result.continue();
					};
					cursorRequest.onerror = function (e) { };
				}
			}
		});
	},

	saveCard: function (data, callback) {
		data.updated = new Date(data.updated);
		data.created = new Date(data.created);
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
					if (LOGGED_USER != false) {
						var db = Database.db;
						var trans = db.transaction("Card", "readwrite");
						var store = trans.objectStore("Card");
						var request = store.put(e.data);
						request.onsuccess = callback;
						request.onerror = function (e) { };
					} else {
						var db = Database.db;
						var trans = db.transaction("Card", "readwrite");
						var store = trans.objectStore("Card");
						var request = store.put(data);
						request.onsuccess = callback;
						request.onerror = function (e) { };
					}
				}
			},
			error: function (e) {
				var db = Database.db;
				var trans = db.transaction("Card", "readwrite");
				var store = trans.objectStore("Card");
				var request = store.put(data);
				request.onsuccess = callback;
				request.onerror = function (e) { };
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
				if (LOGGED_USER != false) {
					callback(e.data);
				} else {
					var db = Database.db;
					var trans = db.transaction("Card", "readonly");
					var store = trans.objectStore("Card");
					var request = store.get(data);
					request.onsuccess = function (e) {
						var card = e.target.result;
						callback(card);
					}
					request.onerror = function (e) { };
				}
			},
			error: function (e) {
				var db = Database.db;
				var trans = db.transaction("Card", "readonly");
				var store = trans.objectStore("Card");
				var request = store.get(data);
				request.onsuccess = function (e) {
					var card = e.target.result;
					callback(card);
				}
				request.onerror = function (e) { };
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
				if (LOGGED_USER != false) {
					callback(e.data);
				} else {
					var items = [];
					if (Database.connected) {
						var db = Database.db;
						var trans = db.transaction("Card", "readonly");
						var store = trans.objectStore("Card");
						var keyRange = IDBKeyRange.lowerBound(0);
						var cursorRequest = store.openCursor(keyRange);
						trans.oncomplete = function (evt) {
							callback(items);
						};
						cursorRequest.onsuccess = function (e) {
							var result = e.target.result;
							if (!!result == false)
								return;
							if (result.value.columnId == data && result.value.status == 1)
								items.push(result.value);
							result.continue();
						};
						cursorRequest.onerror = function (e) { };
					}
				}
			},
			error: function (e) {
				var items = [];
				if (Database.connected) {
					var db = Database.db;
					var trans = db.transaction("Card", "readonly");
					var store = trans.objectStore("Card");
					var keyRange = IDBKeyRange.lowerBound(0);
					var cursorRequest = store.openCursor(keyRange);
					trans.oncomplete = function (evt) {
						callback(items);
					};
					cursorRequest.onsuccess = function (e) {
						var result = e.target.result;
						if (!!result == false)
							return;
						if (result.value.columnId == data && result.value.status == 1)
							items.push(result.value);
						result.continue();
					};
					cursorRequest.onerror = function (e) { };
				}
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
				if (LOGGED_USER != false) {
					callback(e.data);
				} else {
					var items = [];
					if (Database.connected) {
						var db = Database.db;
						var trans = db.transaction("Card", "readonly");
						var store = trans.objectStore("Card");
						trans.oncomplete = function (evt) {
							callback(items);
						};
						var keyRange = IDBKeyRange.lowerBound(0);
						var cursorRequest = store.openCursor(keyRange);
						cursorRequest.onsuccess = function (e) {
							var result = e.target.result;
							if (!!result == false)
								return;
							if (result.value.boardId == data && result.value.status == 1)
								items.push(result.value);
							result.continue();
						};
						cursorRequest.onerror = function (e) { };
					}
				}
			},
			error: function (e) {
				var items = [];
				if (Database.connected) {
					var db = Database.db;
					var trans = db.transaction("Card", "readonly");
					var store = trans.objectStore("Card");
					trans.oncomplete = function (evt) {
						callback(items);
					};
					var keyRange = IDBKeyRange.lowerBound(0);
					var cursorRequest = store.openCursor(keyRange);
					cursorRequest.onsuccess = function (e) {
						var result = e.target.result;
						if (!!result == false)
							return;
						if (result.value.boardId == data && result.value.status == 1)
							items.push(result.value);
						result.continue();
					};
					cursorRequest.onerror = function (e) { };
				}
			}
		});
	},

	saveColumnSetting: function (data, callback) {
		if (Database.connected) {
			var db = Database.db;
			var trans = db.transaction("ColumnSetting", "readwrite");
			var store = trans.objectStore("ColumnSetting");
			var request = store.put(data);

			request.onsuccess = callback;

			request.onerror = function (e) { };
		}
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
				if (LOGGED_USER != false) {
					callback(e.data);
				} else {
					var items = [];
					if (Database.connected) {
						var db = Database.db;
						var trans = db.transaction("ColumnSetting", "readonly");
						var store = trans.objectStore("ColumnSetting");
						var keyRange = IDBKeyRange.lowerBound(0);
						var cursorRequest = store.openCursor(keyRange);
						trans.oncomplete = function (evt) {
							callback(items);
						};
						cursorRequest.onsuccess = function (e) {
							var result = e.target.result;
							if (!!result == false)
								return;
							if (result.value.boardId == data && result.value.status == 1) {
								items.push(result.value);
							}
							result.continue();
						};
						cursorRequest.onerror = function (e) { };
					}
				}
			},
			error: function (e) {
				var items = [];
				if (Database.connected) {
					var db = Database.db;
					var trans = db.transaction("ColumnSetting", "readonly");
					var store = trans.objectStore("ColumnSetting");
					var keyRange = IDBKeyRange.lowerBound(0);
					var cursorRequest = store.openCursor(keyRange);
					trans.oncomplete = function (evt) {
						callback(items);
					};
					cursorRequest.onsuccess = function (e) {
						var result = e.target.result;
						if (!!result == false)
							return;
						if (result.value.boardId == data && result.value.status == 1) {
							items.push(result.value);
						}
						result.continue();
					};
					cursorRequest.onerror = function (e) { };
				}
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
				if (LOGGED_USER != false) {
					callback(e.data);
				} else {
					if (Database.connected) {
						var db = Database.db;
						var trans = db.transaction("ColumnSetting", "readwrite");
						var store = trans.objectStore("ColumnSetting");
						var keyRange = IDBKeyRange.lowerBound(0);
						var cursorRequest = store.openCursor(keyRange);
						var count = 0;
						trans.oncomplete = function (evt) {
							if (count == 0) {
								var columnSetting = new ColumnSetting();
								columnSetting.columnId = data.columnId;
								columnSetting.next = data.next;
								columnSetting.boardId = data.boardId;
								columnSetting.save();
							}
						};
						cursorRequest.onsuccess = function (e) {
							var result = e.target.result;
							if (!!result == false)
								return;
							if (result.value.columnId == data.columnId && result.value.next == data.next) {
								var columnSetting = result.value;
								columnSetting.status = data.status;
								var today = Date.now();
								columnSetting.updated = today;
								var request = store.put({
									"id": columnSetting.id,
									"columnId": columnSetting.columnId,
									"next": columnSetting.next,
									"boardId": columnSetting.boardId,
									"created": columnSetting.created,
									"updated": columnSetting.updated,
									"status": columnSetting.status
								});
								count++;
								return;
							}
							result.continue();
						};
						cursorRequest.onerror = function (e) { };
					}
				}
			},
			error: function (e) {
				if (Database.connected) {
					var db = Database.db;
					var trans = db.transaction("ColumnSetting", "readwrite");
					var store = trans.objectStore("ColumnSetting");
					var keyRange = IDBKeyRange.lowerBound(0);
					var cursorRequest = store.openCursor(keyRange);
					var count = 0;
					trans.oncomplete = function (evt) {
						if (count == 0) {
							var columnSetting = new ColumnSetting();
							columnSetting.columnId = data.columnId;
							columnSetting.next = data.next;
							columnSetting.boardId = data.boardId;
							columnSetting.save();
						}
					};
					cursorRequest.onsuccess = function (e) {
						var result = e.target.result;
						if (!!result == false)
							return;
						if (result.value.columnId == data.columnId && result.value.next == data.next) {
							var columnSetting = result.value;
							columnSetting.status = data.status;
							var today = Date.now();
							columnSetting.updated = today;
							var request = store.put({
								"id": columnSetting.id,
								"columnId": columnSetting.columnId,
								"next": columnSetting.next,
								"boardId": columnSetting.boardId,
								"created": columnSetting.created,
								"updated": columnSetting.updated,
								"status": columnSetting.status
							});
							count++;
							return;
						}
						result.continue();
					};
					cursorRequest.onerror = function (e) { };
				}
			}
		});
	}
};

Database.init();