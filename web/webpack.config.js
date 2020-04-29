const path = require("path");
const analyser = require("webpack-bundle-analyzer").BundleAnalyzerPlugin;

module.exports = {
    entry: "./src/index.tsx",
    mode: "development",
    devtool: "inline-source-map",
    plugins: [
        new analyser()
    ],
    output: {
        path: path.resolve(__dirname, "dist"),
        filename: "bundle.js",
        publicPath: "/output"
    },
    resolve: {
        extensions: [".ts", ".tsx", ".js"],
        alias: {
            "react": "preact/compat",
            "react-dom": "preact/compat"
        }
    },
    module: {
        rules: [
            { 
                test: /\.tsx?$/, 
                loader: 'ts-loader', 
                exclude: /node_modules/ 
            }
        ]
    },
    devServer: {
        contentBase: "./wwwroot",
        publicPath: "/output",
        hot: true,
        headers: {
            "Access-Control-Allow-Origin": "*"
        },
        historyApiFallback: true,
        index: "index.html"
    }
}