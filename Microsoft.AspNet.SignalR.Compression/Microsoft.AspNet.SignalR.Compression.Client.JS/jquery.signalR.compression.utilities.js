/*global window:false */
/// <reference path="jquery.signalr.compression.core.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        compression = signalR.compression,
        utilities,
        compressionTypeIds = {
            defaultTypeId: 0,
            enumerableTypeId: 1,
            numericTypeId: 2
        };

    utilities = {
        getContractFromResponse: function (hubName, methodName, contracts) {
            var contractInfo = contracts[0][hubName][methodName];

            return [utilities.getContract(contractInfo[0], contracts), contractInfo[1]];
        },
        buildResult: function (hubName, methodName, decompress) {
            return {
                decompress: decompress,
                hubName: hubName,
                methodName: methodName
            };
        },
        getContract: function (contractId, contracts) {
            return contracts[2][contractId];
        },
        isPayload: function (contractId, contracts) {
            return !!utilities.getContract(contractId, contracts);
        },
        isDefault: function (compressionTypeId) {
            return compressionTypeIds.defaultTypeId === compressionTypeId;
        },
        isEnumerable: function (compressionTypeId) {
            return compressionTypeIds.enumerableTypeId === compressionTypeId;
        },
        isNumeric: function (compressionTypeId) {
            return compressionTypeIds.numericTypeId === compressionTypeId;
        }
    };
    
    compression._.utilities = utilities;

}(window.jQuery, window));