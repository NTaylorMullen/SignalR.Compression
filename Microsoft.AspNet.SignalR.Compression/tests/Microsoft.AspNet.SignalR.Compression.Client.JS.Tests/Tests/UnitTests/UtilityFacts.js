QUnit.module("Utility Facts");

(function ($, window) {
    var contracts = [{ "methodInvocationHub": { "EchoPerson": [1, 0] }, "methodReturnerHub": { "GetParent": [2, 0], "GetTeacher": [4, 0] } }, { "methodInvocationHub": { "EchoPerson": [[1, 0]], "EchoStudent": [[3, 0]] } }, { "1": [["Age", [0, 2]], ["FirstName", [0, 0]], ["Guardian", [2, 0]], ["LastName", [0, 0]]], "2": [["Age", [0, 2]], ["Children", [1, 1]], ["FirstName", [0, 0]], ["Guardian", [2, 0]], ["LastName", [0, 0]], ["Mother", [2, 0]]], "3": [["Age", [0, 2]], ["Debt", [0, 2]], ["FirstName", [0, 0]], ["GPA", [0, 2]], ["Guardian", [2, 0]], ["LastName", [0, 0]]], "4": [["Age", [0, 2]], ["FirstName", [0, 0]], ["Guardian", [2, 0]], ["LastName", [0, 0]], ["Students", [3, 1]]] }];
    var utilities = $.signalR.compression._.utilities;

    QUnit.test("getContractFromResponse pulls a contract id from a resulting set of contracts", function () {
        var hubNames = ["methodInvocationHub", "methodReturnerHub", "methodReturnerHub"];
        var methodNames = ["EchoPerson", "GetParent", "GetTeacher"];
        var contract;

        for(var i = 0; i < hubNames.length; i++) {
            contract = utilities.getContractFromResponse(hubNames[i], methodNames[i], contracts);

            QUnit.isSet(contract, "Successfully parsed a non null contract from getContractFromResponse with hub name " + hubNames[i] + " and method name " + methodNames[i]);
            QUnit.ok(contract[0].length > 1, "Contract has multiple members");
            QUnit.equal(contract[1], 0, "Contract is a default compression type");
        }
    });

    QUnit.test("isPayload correctly identifies payloads.", function () {
        var payloadIds = [1, 2, 3, 4, -1, 100, 200, -3];
        var results = [true, true, true, true, false, false, false, false];

        for (var i = 0; i < payloadIds.length; i++) {
            QUnit.equal(utilities.isPayload(payloadIds[i], contracts), results[i]);
        }
    });

    QUnit.test("isDefault correctly identifies default compression types.", function () {
        // Last Name
        var defaultContractType = utilities.getContract(4, contracts)[3][1][1];
        // Students
        var enumerableContractType = utilities.getContract(4, contracts)[4][1][1];
        // Age
        var numericContractType = utilities.getContract(4, contracts)[0][1][1];

        QUnit.ok(utilities.isDefault(defaultContractType), "Last Name is a default contract type.");
        QUnit.ok(!utilities.isDefault(enumerableContractType), "Students is not a default contract type.");
        QUnit.ok(!utilities.isDefault(numericContractType), "Age is not a default contract type.");
    });

    QUnit.test("isEnumerable correctly identifies enumerable compression types.", function () {
        // Last Name
        var defaultContractType = utilities.getContract(4, contracts)[3][1][1];
        // Students
        var enumerableContractType = utilities.getContract(4, contracts)[4][1][1];
        // Age
        var numericContractType = utilities.getContract(4, contracts)[0][1][1];

        QUnit.ok(!utilities.isEnumerable(defaultContractType), "Last Name is not an enumerable contract type.");
        QUnit.ok(utilities.isEnumerable(enumerableContractType), "Students is an enumerable contract type.");
        QUnit.ok(!utilities.isEnumerable(numericContractType), "Age is not an enumerable contract type.");
    });

    QUnit.test("isNumeric correctly identifies numeric compression types.", function () {
        // Last Name
        var defaultContractType = utilities.getContract(4, contracts)[3][1][1];
        // Students
        var enumerableContractType = utilities.getContract(4, contracts)[4][1][1];
        // Age
        var numericContractType = utilities.getContract(4, contracts)[0][1][1];

        QUnit.ok(!utilities.isNumeric(defaultContractType), "Last Name is not a numeric contract type.");
        QUnit.ok(!utilities.isNumeric(enumerableContractType), "Students is a numeric contract type.");
        QUnit.ok(utilities.isNumeric(numericContractType), "Age is a numeric contract type.");
    });

})($, window);