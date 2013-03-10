/*global window:false */
/// <reference path="Scripts/jquery-1.9.1.js" />
/// <reference path="Scripts/jquery.signalr-1.0.1.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        compression = {
            _: {
                utilities: null // Created by jquery.signalR.compression.utilities
            },
            settings: null,
            compressor: null, // Created by jquery.signalR.compression.compressor
            decompressor: null // Created by jquery.signalR.compression.decompressor
        };

    signalR.compression = compression;

}(window.jQuery, window));