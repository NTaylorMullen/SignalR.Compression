﻿window.document.testUrl = "auto";

var testUtilities;

(function ($, window) {
    var ios = !!navigator.userAgent.match(/iPod|iPhone|iPad/),
        rfcWebSockets = !!window.WebSocket,
        iosVersion;

    function wrapConnectionStart(connection, end, assert) {
        var savedConnectionStart = connection.start;

        connection.start = function () {
            return savedConnectionStart.apply(connection, arguments).fail(function (reason) {
                assert.ok(false, "Failed to initiate signalr connection: " + window.JSON.stringify(reason));
                end();
            });
        }
    }

    if (ios && rfcWebSockets) {
        iosVersion = navigator.userAgent.match(/OS (\d+)/);
        if (iosVersion && iosVersion.length === 2) {
            rfcWebSockets = parseInt(iosVersion[1], 10) >= 6;
        }
    }

    testUtilities = {
        transports: {
            longPolling: {
                enabled: true
            },
            foreverFrame: {
                enabled: !window.EventSource && navigator.appName === "Microsoft Internet Explorer"
            },
            serverSentEvents: {
                enabled: !!window.EventSource
            },
            webSockets: {
                enabled: !!rfcWebSockets
            }
        },
        transportNames: null, // This is set after the initial creation
        runWithAllTransports: function (fn) {
            this.runWithTransports(this.transportNames, fn);
        },
        runWithTransports: function (transports, fn) {
            $.each(transports, function (_, transport) {
                if (testUtilities.transports[transport].enabled) {
                    fn(transport);
                }
            });
        },
        defaultTestTimeout: (function () {
            var defaultTestTimeout = window.location.search.match(/defaultTestTimeout=(\d+)/);
            
            // There is no default test timeout parameter
            if (!defaultTestTimeout) {
                // Return the default
                return 10000;
            }
            
            // Match returns an array, so we need to take the second element (string).
            defaultTestTimeout = defaultTestTimeout[1];
            // Convert to integer and translate to milliseconds
            defaultTestTimeout = parseInt(defaultTestTimeout, 10) * 1000;

            return defaultTestTimeout;
        })(),
        createHubConnection: function (end, assert, testName) {
            var connection,
                qs = (testName ? "test=" + window.encodeURIComponent(testName) : "");

            if (window.document.testUrl !== 'auto') {
                connection = $.hubConnection(window.document.testUrl, { qs: qs });
            } else {
                connection = $.hubConnection('signalr', { useDefaultPath: false, qs: qs });
            }

            connection.logging = true;
            wrapConnectionStart(connection, end, assert);

            return connection;
        },
        createConnection: function (url, end, assert, testName) {
            var connection,
                qs = (testName ? "test=" + window.encodeURIComponent(testName) : "");

            if (window.document.testUrl !== 'auto') {
                connection = $.connection(window.document.testUrl + '/' + url, qs);
            } else {
                connection = $.connection(url, qs);
            }

            connection.logging = true;
            wrapConnectionStart(connection, end, assert);

            return connection;
        }
    };

    // Create the transport names based off of the transport list
    testUtilities.transportNames = $.map(testUtilities.transports, function (_, transportName) {
        return transportName;
    });
})($, window);