var passport = require('../helpers/passport')
    , env = require('../../config/environment')
    , Client = require('node-rest-client').Client
    , requireAuth = passport.requireAuth;

var client = new Client();

var Develops = function () {
    this.before(requireAuth);
    this.respondsWith = ['html', 'json', 'xml', 'js', 'txt'];

    this.index = function (req, resp, params) {
        var s = this.session.get('sessionId');
        var data = {
            sessionId: s
        };
        this.respond({params: params, data: data});
    };

    this.eval = function (req, resp, params) {
        var self = this,
            sessionId = this.session.get('sessionId');

        console.log(sessionId);

        var reqbody = JSON.parse(req.body);
        reqbody.SessionId = sessionId;

        var args = {
            data: reqbody,
            headers:{"Content-Type": "application/json"}
        };

        client.post(env.sharpNestBaseUri + "eval?format=json",
            args,
            function(data,response){
                console.log(data);
                var d = JSON.parse(data);
                d.SessionId = null;
                self.respondWith(d);
            }
        )
    };

};

exports.Develops = Develops;

