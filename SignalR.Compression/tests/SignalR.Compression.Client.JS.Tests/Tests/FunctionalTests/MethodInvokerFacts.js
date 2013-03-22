QUnit.module("Method Invoker Facts")

testUtilities.runWithAllTransports(function (transport) {
    QUnit.asyncTimeoutTest(transport + " transport is able to send a person to the server and have them echoed back.", testUtilities.defaultTestTimeout, function (end, assert, testName) {
        var connection = testUtilities.createHubConnection(end, assert, testName),
            methodInvocationHub = connection.createHubProxies().methodInvocationHub,
            person = {
                Age: 38,
                FirstName: "John",
                Guardian: null,
                LastName: "Doe"
            },
            compressedPerson = [38, "John", 0, "Doe"];

        QUnit.expect(3);

        methodInvocationHub.client.echo = function (result) {
            assert.equal(window.JSON.stringify(result), window.JSON.stringify(person), "Client decompresses result correctly.");
            end();
        };    

        connection.compression.serverInvokingClient(function (methodName, args) {
            assert.equal(window.JSON.stringify(args), window.JSON.stringify(compressedPerson), "Server compresses data properly prior to client decompressing.");
        });

        connection.compression.invokingServerMethod(function (methodName, args) {
            assert.equal(window.JSON.stringify(args), window.JSON.stringify(compressedPerson), "Client compresses data properly prior to sending data to server.");
        });

        connection.start({ transport: transport }).done(function () {
            methodInvocationHub.server.echoPerson(person);
        });

        return function () {
            connection.stop();
        };
    });
    
    QUnit.asyncTimeoutTest(transport + " transport is able to send a student to the server and have them echoed back.", testUtilities.defaultTestTimeout, function (end, assert, testName) {
        var connection = testUtilities.createHubConnection(end, assert, testName),
            methodInvocationHub = connection.createHubProxies().methodInvocationHub,
            student = {
                Age: 38,
                Debt: 36543.45,
                FirstName: "John",
                GPA: 1.337,
                Guardian: null,
                LastName: "Doe"
            },
            compressedStudent = [38,36543.45,"John",1.337,0,"Doe"];

        QUnit.expect(3);

        methodInvocationHub.client.echo = function (result) {
            // Gets rounded to 2 decimal places on server
            student.GPA = 1.34;

            assert.equal(window.JSON.stringify(result), window.JSON.stringify(student), "Client decompresses result correctly.");
            end();
        };

        connection.compression.serverInvokingClient(function (methodName, args) {
            // Gets rounded to 2 decimal places on server
            compressedStudent[3] = 1.34;
            assert.equal(window.JSON.stringify(args), window.JSON.stringify(compressedStudent), "Server compresses data properly prior to client decompressing.");
        });

        connection.compression.invokingServerMethod(function (methodName, args) {
            assert.equal(window.JSON.stringify(args), window.JSON.stringify(compressedStudent), "Client compresses data properly prior to sending data to server.");
        });

        connection.start({ transport: transport }).done(function () {            
            methodInvocationHub.server.echoStudent(student);
        });
    });
});