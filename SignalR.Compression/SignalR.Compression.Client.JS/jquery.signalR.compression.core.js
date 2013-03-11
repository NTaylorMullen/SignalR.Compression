/*global window:false */
/// <reference path="Scripts/jquery-1.9.1.js" />
/// <reference path="Scripts/jquery.signalr-1.0.1.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        hubConnection = $.hubConnection,
        events = {
            onMethodResponse: "OnMethodResponse", // Triggers when a server returns a value from the server (passes compressed data)
            onInvokingServerMethod: "OnInvokingServerMethod", // Triggers when the client invokes a method on the server (passes compressed arguments)
            onServerInvokingClient: "OnServerInvokingClient" // Triggers when the server invokes a method on the client (passes compressed arguments)
        },
        compression = {
            _: {
                utilities: null // Created by jquery.signalR.compression.utilities
            },
            settings: null,
            compressor: null, // Created by jquery.signalR.compression.compressor
            decompressor: null, // Created by jquery.signalR.compression.decompressor
            events: events
        };

    $.extend(hubConnection.fn, {
        compression: {
            _: {
                decompressResult: [], // Array of Booleans representing if we should decompress an invocation result,
                contracts: {} // Contracts to abide by when sending/receiving data
            },
            methodResponse: function (callback) {
                /// <summary>Adds a callback that will be invoked when the server returns a value from the server (passes compressed data).</summary>
                /// <param name="callback" type="Function">A callback to execute when a server method returns a value</param>
                /// <returns type="signalR" />
                var connection = this;
                $(connection).bind(events.onMethodResponse, function (e, result) {
                    callback.call(connection, result);
                });
                return connection;
            },
            invokingServerMethod: function (callback) {
                /// <summary>Adds a callback that will be invoked when the client invokes a method on the server (passes compressed arguments).</summary>
                /// <param name="callback" type="Function">A callback to execute when the client invokes a server method</param>
                /// <returns type="signalR" />
                var connection = this;
                $(connection).bind(events.onInvokingServerMethod, function (e, methodName, args) {
                    callback.call(connection, methodName, args);
                });
                return connection;
            },
            serverInvokingClient: function (callback) {
                /// <summary>Adds a callback that will be invoked when the server invokes a method on the client (passes compressed arguments).</summary>
                /// <param name="callback" type="Function">A callback to execute when the server invokes a client method</param>
                /// <returns type="signalR" />
                var connection = this;
                $(connection).bind(events.onServerInvokingClient, function (e, methodName, args) {
                    callback.call(connection, methodName, args);
                });
                return connection;
            }
        }
    });

    signalR.compression = compression;

}(window.jQuery, window));