QUnit.module("Decompressor Facts");

(function ($, window) {
    var contracts = [{ "methodInvocationHub": { "EchoPerson": [1, 0] }, "methodReturnerHub": { "GetParent": [2, 0], "GetTeacher": [4, 0] } }, { "methodInvocationHub": { "EchoPerson": [[1, 0]], "EchoStudent": [[3, 0]] } }, { "1": [["Age", [0, 2]], ["FirstName", [0, 0]], ["Guardian", [2, 0]], ["LastName", [0, 0]]], "2": [["Age", [0, 2]], ["Children", [1, 1]], ["FirstName", [0, 0]], ["Guardian", [2, 0]], ["LastName", [0, 0]], ["Mother", [2, 0]]], "3": [["Age", [0, 2]], ["Debt", [0, 2]], ["FirstName", [0, 0]], ["GPA", [0, 2]], ["Guardian", [2, 0]], ["LastName", [0, 0]]], "4": [["Age", [0, 2]], ["FirstName", [0, 0]], ["Guardian", [2, 0]], ["LastName", [0, 0]], ["Students", [3, 1]]] }];
    var decompressor = $.signalR.compression.decompressor;
    var utilities = $.signalR.compression._.utilities;

    QUnit.test("Decompresses complex payloads correctly", function () {
        var expectedDecompressed = {
            Age: 38,
            FirstName: "John",
            Guardian: null,
            LastName: "Doe"
        },
        compressed = [38, "John", 0, "Doe"],
        contract = utilities.getContract(1, contracts);

        QUnit.equal(window.JSON.stringify(decompressor.decompress(compressed, contract, contracts)), window.JSON.stringify(expectedDecompressed), "Can accurately decompress a Person payload.");

        expectedDecompressed = {
            Age: 38,
            FirstName: "John",
            Guardian: {
                Age: 72,
                Children: [
                    {
                        Age: 8,
                        FirstName: "Bippi",
                        Guardian: null,
                        LastName: "Boop"
                    },
                    {
                        Age: 7,
                        FirstName: "Bobby",
                        Guardian: null,
                        LastName: "Boop"
                    }
                ],
                FirstName: "Betty",
                Guardian: null,
                LastName: "Boop",
                Mother: null
            },
            LastName: "Doe"
        };
        compressed = [38, "John",
            [
                72,
                [
                    [8, "Bippi", 0, "Boop"],
                    [7, "Bobby", 0, "Boop"]
                ],
                "Betty", 0, "Boop", 0
            ], "Doe"];
        contract = utilities.getContract(1, contracts);

        QUnit.equal(window.JSON.stringify(decompressor.decompress(compressed, contract, contracts)), window.JSON.stringify(expectedDecompressed), "Can accurately compress a Person payload.");
    });
})($, window);