QUnit.module("Compressor Facts");

(function ($, window) {
    var contracts = [{ "methodInvocationHub": { "EchoPerson": [1, 0] }, "methodReturnerHub": { "GetParent": [2, 0], "GetTeacher": [4, 0] } }, { "methodInvocationHub": { "EchoPerson": [[1, 0]], "EchoStudent": [[3, 0]] } }, { "1": [["Age", [0, 2]], ["FirstName", [0, 0]], ["Guardian", [2, 0]], ["LastName", [0, 0]]], "2": [["Age", [0, 2]], ["Children", [1, 1]], ["FirstName", [0, 0]], ["Guardian", [2, 0]], ["LastName", [0, 0]], ["Mother", [2, 0]]], "3": [["Age", [0, 2]], ["Debt", [0, 2]], ["FirstName", [0, 0]], ["GPA", [0, 2]], ["Guardian", [2, 0]], ["LastName", [0, 0]]], "4": [["Age", [0, 2]], ["FirstName", [0, 0]], ["Guardian", [2, 0]], ["LastName", [0, 0]], ["Students", [3, 1]]] }];
    var compressor = $.signalR.compression.compressor;
    var utilities = $.signalR.compression._.utilities;

    QUnit.test("Compresses complex payloads correctly", function () {
        var uncompressed = {
            FirstName: "John",
            LastName: "Doe",
            Age: 38
        },
        expectedCompressed = [38, "John", 0, "Doe"],
        contract = utilities.getContract(1,contracts);

        QUnit.equal(window.JSON.stringify(compressor.compress(uncompressed, contract, contracts)), window.JSON.stringify(expectedCompressed), "Can accurately compress a Person payload.");

        uncompressed = {
            FirstName: "John",
            LastName: "Doe",
            Age: 38,
            Guardian: {
                LastName: "Boop",
                Age: 72,
                FirstName: "Betty",
                Children: [
                    {
                        Age: 8,
                        FirstName: "Bippi",
                        LastName: "Boop"
                    },
                    {
                        Age: 7,
                        LastName: "Boop",
                        FirstName: "Bobby"
                    }
                ]
            }
        };
        expectedCompressed = [38, "John", 
            [
                72, 
                [
                    [8, "Bippi", 0, "Boop"],
                    [7, "Bobby", 0, "Boop"]
                ],
                "Betty", 0, "Boop", 0
            ], "Doe"];
        contract = utilities.getContract(1, contracts);

        QUnit.equal(window.JSON.stringify(compressor.compress(uncompressed, contract, contracts)), window.JSON.stringify(expectedCompressed), "Can accurately compress a complex Person payload.");
    });
})($, window);