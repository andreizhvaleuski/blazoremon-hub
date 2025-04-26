module.exports = ({ env }) => {
    const isProduction = env === 'production'

    return ({
        plugins: [
            require('autoprefixer'),
            isProduction && require('cssnano')
        ],
        map: isProduction ? false : 'inline'
    });
};
