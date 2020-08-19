var path = require('path');
var webpack = require("webpack");

module.exports = {
    mode: 'development',
    watch: true,
    entry: {
        'launcher': './_ts/launcher.tsx'
    },
    devtool: 'source-map',
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, 'wwwroot', 'js'),
        publicPath: 'js/'
    },
    devServer: {
        contentBase: './wwwroot/js',
        hot: false
    },
    resolve: {
        extensions: ['*', '.js', '.jsx', '.ts', '.tsx']
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: [
                    {
                        loader: 'babel-loader',
                        options: {
                            "presets": ["@babel/preset-env", "@babel/preset-react", "@babel/preset-typescript"]
                        }
                    }
                ]
            }
        ]
    },
    plugins: [
        new webpack.NoEmitOnErrorsPlugin()
    ]
};