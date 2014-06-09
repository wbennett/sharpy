var geddy = require('geddy');
geddy.start({
    environment: process.env.GEDDY_ENVIRONMENT || 'development'
})
