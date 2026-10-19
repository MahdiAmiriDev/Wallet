const TerserPlugin = require("terser-webpack-plugin");

module.exports = {
    entry: {
        certificate: './js/lit/certificate.js',
        company_location: './js/leaflet/company_location.js',
        stepped_resume: './js/resume/stepped_resume.js',
        show_resume: './js/resume/show_resume.js',
        job_offer_detail: './js/components/job_offer_detail.js',
        edit_recipient: './js/components/edit_recipient.js'
    },
    output: {
        filename: '[name].min.js',
        path: __dirname + '/js/components/dist',
    },
    optimization: {
        minimize: true,
        minimizer: [new TerserPlugin({
            terserOptions: {
                keep_fnames: true,
                compress: {
                    keep_infinity: true,
                },
                format: {
                    comments: false
                },
                mangle: false, // Do not mangle variable names
                safari10: true,
            },
        })],
    },
    watch: true,
    mode: 'production',
};

// npx webpack --config webpack.config.js
