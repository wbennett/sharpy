var User = function () {
  this.defineProperties({
    username: {type: 'string', required: true, on: ['create', 'update']}
  , password: {type: 'string', required: true, on: ['create', 'update']}
  , email: {type: 'string', required: true, on: ['create', 'update']}
  , activationToken: {type: 'string'}
  , activatedAt: {type: 'datetime'}
  , sessionId: {type: 'string'}
  });

  this.validatesLength('username', {min: 3});
  this.validatesLength('password', {min: 8});
  this.validatesConfirmed('password', 'confirmPassword');

  this.hasMany('Passports');
};

User.prototype.isActive = function () {
  return !!this.activatedAt;
};

User = geddy.model.register('User', User);
