var LOG = true;
var DEBUG = true;
var LOGGED_USER = false;
var IS_INDEX = false;
var IS_BACKGROUND = false;
var sync_to_server = [];

var log = function () {
	return console.log.apply(console, arguments);
};

var Enable = {
	backgroundListener: true,
	backgroundSync: false
};

var Syncing = {
	board: false
}

var Configuration = {
	server: 'http://localhost:34003/',
	hook: '/ws/hook-ext',
	syncToServerInterval: 5000,
	syncFromServerInterval: 5000,
	locale: 'en',
	theme: 'default',
	version: '1.0'
};

var config = function (name, value) {
	if (typeof value == 'undefined')
		return Configuration[name];

	Configuration[name] = value;
	localStorage['config'] = JSON.stringify(Configuration);
};

if (localStorage['config'] != null) {
	Configuration = JSON.parse(localStorage['config']);
}

var server = function (a, b, c) {
	if (typeof a == 'string') {
		if ($.isFunction(b)) {
			var opts = {
				url: config('server') + a,
				type: 'get',
				cache: false,
				success: b
			};
			$.ajax(opts);
		} else {
			var opts = {
				url: config('server') + a,
				data: b,
				type: 'post',
				cache: false,
				success: c
			};
			$.ajax(opts);
		}
	} else {
		a.url = config('server') + a.url;
		$.ajax(a);
	}
};

var sync = function (name, data, success) {
	Syncing[name] = true;
	var opts = {
		url: config('server') + '/sync/' + name,
		data: data,
		type: 'post',
		cache: false,
		success: function (json) {
			Syncing[name] = false;
			success(json)
		},
		error: function () {
			Syncing[name] = false;
		}
	}
	$.ajax(opts);
};

var columnCompare = function (a, b) {
	if (a.priority < b.priority)
		return -1;
	if (a.priority > b.priority)
		return 1;
	return 0;
}

var formatGuid = function (guid) {
	if (guid.length == 32)
		return guid.substr(0, 8) + '-' + guid.substr(8, 4) + '-' + guid.substr(12, 4) + '-' + guid.substr(16, 4) + '-' + guid.substr(20);
	return guid;
};

var generateId = function (model) {
	var accountId = (LOGGED_USER !== false) ? LOGGED_USER.id : 0;
	var random = Math.floor(Math.random() * 10000000000) % 4294967295;
	var aid = accountId.toString(16);
	while (aid.length < 8)
		aid = '0' + aid;
	var tid = (new Date).getTime().toString(16);
	while (tid.length < 13)
		tid = '0' + tid;
	var rid = random.toString(16);
	while (rid.length < 8)
		rid = '0' + rid;
	switch (model) {
		case 'board':
			return formatGuid(aid + '001' + tid + rid);
		case 'column':
			return formatGuid(aid + '002' + tid + rid);
		case 'column-setting':
			return formatGuid(aid + '003' + tid + rid);
		case 'task':
			return formatGuid(aid + '004' + tid + rid);
		case 'time-log':
			return formatGuid(aid + '005' + tid + rid);
		default:
			return formatGuid(aid + '000' + tid + rid);
	}
};

//server({
//    url: 'account-info',
//    cache: false,
//    success: function (json) {

//    }
//});

//server('account-info', function (json) {

//});

//server(
//    'account-info',
//    { name: '' },
//    function (json) {

//    }
//);

//server('account-info', function (result) {
//    if (result.success) {
//        $('').text(result.data.name);
//    } else {

//    }
//});