QUnit.module("Method Returner Facts")

testUtilities.runWithAllTransports(function (transport) {
    QUnit.asyncTimeoutTest(transport + " transport is able to get a parent from the server.", testUtilities.defaultTestTimeout, function (end, assert, testName) {
        var connection = testUtilities.createHubConnection(end, assert, testName),
            methodReturnerHub = connection.createHubProxies().methodReturnerHub,
            parent = {
                Age: 36,
                Children: [
                    {
                        Age: 9,
                        FirstName: "Daughter",
                        Guardian: null,
                        LastName: "Doe"
                    },
                    {
                        Age: 7,
                        FirstName: "Johny",
                        Guardian: null,
                        LastName: "Doe"
                    }
                ],
                FirstName: "John",
                Guardian: null,
                LastName: "Doe",
                Mother: {
                    Age: 62,
                    Children: null,
                    FirstName: "Mom",
                    Guardian: null,
                    LastName: null,
                    Mother: null
                }
            },
            compressedParent = [36, [[9, "Daughter", 0, "Doe"], [7, "Johny", 0, "Doe"]], "John", 0, "Doe", [62, 0, "Mom", 0, 0, 0]];

        QUnit.expect(2);

        $.connection.hub.compression.methodResponse(function (result) {
            assert.equal(window.JSON.stringify(result), window.JSON.stringify(compressedParent), "Parent is compressed upon receiving a response from a method invocation.");
        });

        connection.start({ transport: transport }).done(function () {
            methodReturnerHub.server.getParent().done(function (result) {
                assert.equal(window.JSON.stringify(result), window.JSON.stringify(parent), "Parent is decompressed after the final result is passed through to the deferred.");
                end();
            });
        });

        return function () {
            connection.stop();
        };
    });

    QUnit.asyncTimeoutTest(transport + " transport is able to get a teacher from the server.", testUtilities.defaultTestTimeout, function (end, assert, testName) {
        var connection = testUtilities.createHubConnection(end, assert, testName),
            methodReturnerHub = connection.createHubProxies().methodReturnerHub,
            teacher = {
                Age: 29,
                FirstName: "Teacher",
                Guardian: null,
                LastName: "Dude",
                Students: [
                    {
                        Age: 10,
                        Debt: 0,
                        FirstName: "Numero",
                        GPA: 0,
                        Guardian: null,
                        LastName: "Uno"
                    },
                    {
                        Age: 10,
                        Debt: 0,
                        FirstName: "Numero",
                        GPA: 0,
                        Guardian: null,
                        LastName: "Dos"
                    }
                ]
            },
            compressedTeacher = [29, "Teacher", 0, "Dude", [[10, 0, "Numero", 0, 0, "Uno"], [10, 0, "Numero", 0, 0, "Dos"]]];

        QUnit.expect(2);

        $.connection.hub.compression.methodResponse(function (result) {
            assert.equal(window.JSON.stringify(result), window.JSON.stringify(compressedTeacher), "Teacher is compressed upon receiving a response from a method invocation.");
        });

        connection.start({ transport: transport }).done(function () {
            methodReturnerHub.server.getTeacher().done(function (result) {
                assert.equal(window.JSON.stringify(result), window.JSON.stringify(teacher), "Teacher is decompressed after the final result is passed through to the deferred.");
                end();
            });
        });

        return function () {
            connection.stop();
        };
    });
});