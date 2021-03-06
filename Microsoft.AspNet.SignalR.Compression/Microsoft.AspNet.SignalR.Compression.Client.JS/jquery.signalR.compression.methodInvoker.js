﻿/*global window:false */
/// <reference path="jquery.signalr.compression.utilities.js" />
/// <reference path="jquery.signalr.compression.compressor.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        compression = signalR.compression,
        compressor = compression.compressor,
        utilities = compression._.utilities,
        events = compression.events,
        hubConnection = $.hubConnection,
        savedCreateHubProxy = hubConnection.prototype.createHubProxy;

    hubConnection.prototype.createHubProxy = function (hubName) {
        var proxy = savedCreateHubProxy.apply(this, arguments),
            savedInvoke = proxy.invoke,
            connection = this,
            compressionData = connection.compression._;

        proxy.invoke = function (methodName) {
            var contracts = compressionData.contracts,
                // Copy the argument so we don't modify existing value
                methodArgs = $.extend(true, [], arguments);

            if (contracts) {
                var returnContracts = contracts[0][hubName],
                    invokeContracts = contracts[1][hubName],
                    invokeContractData,
                    contractId,
                    contract,
                    enumerable,
                    enumerated,
                    arg;

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
                        arg = methodArgs[i];
                        contractId = invokeContractData[i - 1][0];
                        enumerable = invokeContractData[i - 1][1];

                        if (utilities.isPayload(contractId, contracts)) {
                            contract = utilities.getContract(contractId, contracts);

                            if (enumerable) {
                                enumerated = [];

                                for (var j = 0; j < arg.length; j++) {
                                    enumerated.push(compressor.compress(arg[j], contract, contracts));
                                }

                                methodArgs[i] = enumerated;
                            }
                            else {
                                methodArgs[i] = compressor.compress(arg, contract, contracts);
                            }
                        }
                    }
                }
            }

            connection.log("SignalR Compression: Invoking method '" + methodName + "' with args: " + methodArgs[1] + ".");
            $(connection.compression).triggerHandler(events.onInvokingServerMethod, [methodName, methodArgs[1]]);

            return savedInvoke.apply(this, methodArgs);
        };

        return proxy;
    };

}(window.jQuery, window));