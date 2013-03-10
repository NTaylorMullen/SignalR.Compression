/*global window:false */
/// <reference path="jquery.signalr.compression.utilities.js" />
/// <reference path="jquery.signalr.compression.compressor.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        compression = signalR.compression,
        compressor = compression.compressor,
        utilities = compression._.utilities,
        hubConnection = $.hubConnection,
        savedCreateHubProxy = hubConnection.prototype.createHubProxy;

    hubConnection.prototype.createHubProxy = function (hubName) {
        var proxy = savedCreateHubProxy.apply(this, arguments),
            savedInvoke = proxy.invoke,
            connection = this,
            compressionData = connection._.compressionData;

        proxy.invoke = function (methodName) {
            var contracts = compressionData.contracts,
                methodArgs = $.makeArray(arguments);

            if (contracts) {
                var returnContracts = contracts[0][hubName],
                    invokeContracts = contracts[1][hubName],
                    invokeContractData,
                    contractId,
                    contract,
                    enumerable,
                    enumerated;


                // Check if we need to return a result
                if (returnContracts && returnContracts[methodName]) {
                    compressionData.decompressResult.push(utilities.buildResult(hubName, methodName, true));
                }
                else {
                    compressionData.decompressResult.push(utilities.buildResult(hubName, methodName, false));
                }

                if (invokeContracts && invokeContracts[methodName]) {
                    invokeContractData = invokeContracts[methodName];

                    for (var i = 1; i < methodArgs.length; i++) {
                        contractId = invokeContractData[i - 1][0];
                        enumerable = invokeContractData[i - 1][1];

                        if (utilities.isPayload(contractId, contracts)) {
                            contract = utilities.getContract(contractId, contracts);

                            if (enumerable) {
                                enumerated = [];

                                for (var j = 0; j < methodArgs[i].length; j++) {
                                    enumerated.push(compressor.compress(methodArgs[i][j], contract, contracts));
                                }

                                methodArgs[i] = enumerated;
                            }
                            else {
                                methodArgs[i] = compressor.compress(methodArgs[i], contract, contracts);
                            }
                        }
                    }
                }
            }
            return savedInvoke.apply(this, methodArgs);
        };

        return proxy;
    };

}(window.jQuery, window));