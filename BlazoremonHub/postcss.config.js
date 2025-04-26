﻿module.exports = ({ env }) => ({
    plugins: [
        require('autoprefixer'),
        env === 'production' ? require('cssnano')({ preset: 'default' }) : false
    ].filter(Boolean),
    map: env === 'production' ? false : 'inline'
});
