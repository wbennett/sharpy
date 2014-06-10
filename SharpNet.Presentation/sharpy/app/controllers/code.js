var passport = require('../helpers/passport')
    , env = require('../../config/environment')
    , Client = require('node-rest-client').Client
    , requireAuth = passport.requireAuth;

var client = new Client();

var Code = function () {
    this.before(requireAuth);
    this.respondsWith = ['html', 'json', 'xml', 'js', 'txt'];

    this.index = function (req, resp, params) {
        var self = this;
        var s = this.session.get('sessionId');
        var data = {
            sessionId: s
        };
        if (s != null)
        {
            this.respond({params: params, data: data});
        }else{
            this.activate(function(session){
                    data.sessionId = session;
                    self.respond({params: params, data: data})
                }
            )
        }


    };

    this.activate = function(callback){
        var self = this;
        var activateSessionArgs = {
            data: {},
            headers:{"Content-Type": "application/json"}
        };
        client.post(env.sharpNetBaseUri + "activate?format=json",
            activateSessionArgs,
            function(data,response){
                console.log(data);
                //set the session id
                var d = JSON.parse(data);
                self.session.set('sessionId',
                    d.SessionId);

                callback(d.SessionId);
            }
        )
    }

    this._evaluate = function(args, callback){
        var self = this;
        client.post(env.sharpNetBaseUri + "eval?format=json",
            args,
            function(data,response){
                console.log(data);
                var d = JSON.parse(data);
                self.session.set('sessionId', d.SessionId);
                var m = new geddy.model.Evaluation.create();
                m.StandardOut = d.StandardOut;
                m.StandardError = d.StandardError;
                m.ReturnValue = d.ReturnValue;
                callback(m);
            }
        )
    }

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

        if(sessionId)
        {
            this._evaluate(args,function(m){
                self.respond(m,{format: "json"});
            });
        }else {
            this.activate(function(session){
                args.data.SessionId = session
                self._evaluate(args,function(m){
                    self.respond(m,{format: "json"});
                })
            });
        }
    };

};

exports.Code = Code;

