QUnit.module("Contract Retriever Facts")

testUtilities.runWithAllTransports(function (transport) {
    QUnit.asyncTimeoutTest(transport + " transport is able to retrieve contracts from endpoint with hub connection.", testUtilities.defaultTestTimeout, function (end, assert, testName) {
        var connection = testUtilities.createHubConnection(end, assert, testName);

        connection.start({ transport: transport }).done(function () {
            assert.isSet(connection.compression._.contracts, "Contracts object exists in private compression data.");

            end();
        });

        return function () {
            connection.stop();
        };
    });

    QUnit.asyncTimeoutTest(transport + " transport is not able to retrieve contracts from endpoint with persistent connection.", testUtilities.defaultTestTimeout, function (end, assert, testName) {
        var connection = testUtilities.createConnection("signalr", end, assert, testName),
            savedAjax = $.ajax;

        $.ajax = function (url, settings) {
            if (!settings) {
                settings = url;
                url = settings.url;
            }

            if (url.indexOf("/compression/contracts") >= 0) {
                assert.ok(false, "Attempted to retrieve contracts with a persistent connection.");
            }

            savedAjax.call(this, url, settings);
        }

        connection.start({ transport: transport }).done(function () {
            assert.isNotSet(connection.compression, "Contracts object exists in private compression data.");

            end();
        });

        return function () {
            $.ajax = savedAjax;
            connection.stop();
        };
    });

    QUnit.asyncTimeoutTest(transport + " transport is able to retrieve contracts in correct format", testUtilities.defaultTestTimeout, function (end, assert, testName) {
        var connection = testUtilities.createHubConnection(end, assert, testName);

        connection.start({ transport: transport }).done(function () {
            var contracts = connection.compression._.contracts,
                methodReturnContracts = contracts[0],
                methodInvokerContracts = contracts[1],
                payloadContracts = contracts[2];

            assert.equal(contracts.length, 3, "There are 3 items in the contracts list.");
            assert.equal(typeof methodReturnContracts, "object", "Method return contracts are an object.");
            assert.equal(typeof methodInvokerContracts, "object", "Method invoker contracts are an object.");
            assert.equal(typeof payloadContracts, "object", "Payload contracts are an object.");

            for (var hubName in methodReturnContracts) {
                assert.isSet($.connection[hubName], "Hub " + hubName + " exists on $.connection for method return contracts.");
            }

            for (var hubName in methodInvokerContracts) {
                assert.isSet($.connection[hubName], "Hub " + hubName + " exists on $.connection for method invoker contracts.");
            }

            end();
        });

        return function () {
            connection.stop();
        };
    });
});