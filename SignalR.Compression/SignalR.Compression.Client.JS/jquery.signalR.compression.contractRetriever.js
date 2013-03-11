/*global window:false */
/// <reference path="jquery.signalr.compression.core.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        savedStart = signalR.prototype.start,
        events = signalR.events;

    signalR.prototype.start = function (options, callback) {
        var dataType = "json",
            deferred = $.Deferred(),
            connection = this,
            jsonp = false,
            url = connection.url,
            startArgs = arguments;

        if (connection.compression) {
            if ($.type(options) === "object") {
                if (options.jsonp === true) {
                    jsonp = true;
                }
                else if (signalR.fn.isCrossDomain(connection.url)) {
                    jsonp = !$.support.cors;
                }
            }

            if (jsonp) {
                dataType = "jsonp";
            }

            //
            if (url.toLowerCase().indexOf("/signalr", url.length - 8) !== -1) {
                url = url.substr(0, url.length - 8);
            }

            url += "/compression/contracts";

            $.ajax({
                url: url,
                global: false,
                cache: false,
                type: "GET",
                data: {},
                dataType: dataType,
                error: function (error) {
                    $(connection).triggerHandler(events.onError, [error.responseText]);
                    deferred.reject("SignalR: Error during contract retrieval request: " + error.responseText);
                },
                success: function (result) {
                    connection.compression._.contracts = result.Contracts;

                    savedStart.apply(connection, startArgs).done(function () {
                        deferred.resolve(connection);
                    }).fail(function () {
                        deferred.reject.apply(this, arguments);
                    });
                }
            });

            return deferred.promise();
        }
        
        return savedStart.apply(connection, startArgs);
    };

}(window.jQuery, window));