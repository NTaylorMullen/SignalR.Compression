QUnit.module("Event Facts");

QUnit.test("methodResponse event does not bind to multiple connections.", function () {
    var connection1 = testUtilities.createHubConnection(),
        connection2 = testUtilities.createHubConnection(),
        asserted1 = false,
        asserted2 = false;

    QUnit.expect(2);

    connection1.compression.methodResponse(function () {
        asserted1 = !asserted1;
        QUnit.ok(asserted1, "Method response triggered only once for connection 1");
    });

    connection2.compression.methodResponse(function () {
        asserted2 = !asserted2;
        QUnit.ok(asserted2, "Method response triggered only once for connection 2");
    });
    
    $(connection1.compression).triggerHandler($.signalR.compression.events.onMethodResponse);
    $(connection2.compression).triggerHandler($.signalR.compression.events.onMethodResponse);
});