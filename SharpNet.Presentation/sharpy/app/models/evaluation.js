var Evaluation = function (){
    this.defineProperties({
        StandardOut: {type: 'string'},
        StandardError: {type: 'string'},
        ReturnValue: {type: 'string'}
    });
};

Evaluation = geddy.model.register('Evaluation',Evaluation);