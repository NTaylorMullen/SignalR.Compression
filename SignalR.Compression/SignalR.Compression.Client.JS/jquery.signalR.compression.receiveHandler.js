/*global window:false */
/// <reference path="jquery.signalr.compression.utilities.js" />
/// <reference path="jquery.signalr.compression.decompressor.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        compression = signalR.compression,
        decompressor = compression.decompressor,
        utilities = compression._.utilities,
        savedReceived = signalR.prototype.received;

    signalR.prototype.received = function (fn) {
        var layer = function (minData) {
            var callbackId,
            connection = this,
            compressionData = connection._.compressionData,
            contracts = compressionData.contracts,
            data,
            contract,
            enumerable,
            result;

            if (contracts) {
                // Verify this is a return value
                if (typeof (minData.I) !== "undefined" && minData.R) {
                    data = compressionData.decompressResult.shift();
                    callbackId = minData.I;

                    // Check if we should decompress this payload
                    if (data.decompress) {
                        // Pull the contract for the given method
                        contract = utilities.getContractFromResponse(data.hubName, data.methodName, contracts);
                        enumerable = contract[1];
                        contract = contract[0];

                        if (enumerable === false) {
                            result = decompressor.decompress(minData.R, contract, contracts);
                        }
                        else {
                            result = [];
                            $.each(minData.R, function (i, val) {
                                result.push(decompressor.decompress(val, contract, contracts));
                            });
                        }

                        minData.R = result;
                    }
                }
                else if (typeof (minData.A) !== "undefined" && typeof (minData.C) !== "undefined") {
                    $.each(minData.A, function (i, arg) {
                        var contractId = minData.C[i],
                            contract,
                            enumerable = false;

                        // Checking if the contract that's sent down is an array of contractables
                        if (contractId.length > 2 && contractId.substring(contractId.length - 2) === "[]") {
                            contractId = contractId.substring(0, contractId.length - 2);
                            enumerable = true;
                        }

                        contractId = parseInt(contractId, 10);

                        // Verify there's a valid contract
                        if (utilities.isPayload(contractId, contracts)) {
                            contract = utilities.getContract(contractId, contracts);

                            if (enumerable === false) {
                                result = decompressor.decompress(arg, contract, contracts);
                            }
                            else {
                                result = [];
                                $.each(arg, function (i, val) {
                                    result.push(decompressor.decompress(val, contract, contracts));
                                });
                            }

                            minData.A[i] = result;
                        }
                    });
                }
            }

            fn.call(this, minData);
        };

        savedReceived.call(this, layer);
    };

}(window.jQuery, window));