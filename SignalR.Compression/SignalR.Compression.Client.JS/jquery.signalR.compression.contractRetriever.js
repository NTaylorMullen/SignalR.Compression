/*global window:false */
/// <reference path="jquery.signalr.compression.core.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        savedConnection = signalR.fn.init;

    $.signalR.fn.init = function () {
        var connection = this,
            compressionData = {
                decompressResult: [], // Array of Booleans representing if we should decompress an invocation result,
                contracts: {} // Contracts to abide by when sending/receiving data
            };

        savedConnection.apply(connection, arguments),

        connection._.compressionData = compressionData;

        connection.starting(function (result) {
            compressionData.contracts = result.Contracts;
        });

        return connection;
    };

}(window.jQuery, window));