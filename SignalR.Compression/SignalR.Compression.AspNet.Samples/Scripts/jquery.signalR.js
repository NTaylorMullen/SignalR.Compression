/* jquery.signalR.compression.core.js */
/*global window:false */
/// <reference path="Scripts/jquery-1.9.1.js" />
/// <reference path="Scripts/jquery.signalr-1.0.1.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        compression = {
            _: {
                utilities: null // Created by jquery.signalR.compression.utilities
            },
            settings: null,
            compressor: null, // Created by jquery.signalR.compression.compressor
            decompressor: null // Created by jquery.signalR.compression.decompressor
        };

    signalR.compression = compression;

}(window.jQuery, window));
/* jquery.signalR.compression.utilities.js */
/*global window:false */
/// <reference path="jquery.signalr.compression.core.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        compression = signalR.compression,
        utilities;

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
        }
    };
    
    compression._.utilities = utilities;

}(window.jQuery, window));
/* jquery.signalR.compression.compressor.js */
/*global window:false */
/// <reference path="jquery.signalr.compression.utilities.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        compression = signalR.compression,
        utilities = compression._.utilities,
        compressor = {
            compress: function (uncompressed, contract, contracts) {
                var result,
                    enumerated,
                    that = this;

                if (uncompressed) {
                    result = [];

                    $.each(contract, function (i, val) {
                        var propertyName = val[0],
                            compressedTypeId = val[1][0],
                            enumerable = val[1][1];

                        // Check the payload type of the parameter, if it's a payload we need to recursively compress it
                        if (utilities.isPayload(compressedTypeId, contracts)) {
                            if (enumerable) {
                                enumerated = [];

                                for (var j = 0; j < uncompressed[propertyName].length; j++) {
                                    enumerated[j] = that.compress(uncompressed[propertyName][j], utilities.getContract(compressedTypeId, contracts), contracts);
                                }

                                result[propertyName].push(enumerated);
                            }
                            else {
                                result.push(that.compress(uncompressed[propertyName], utilities.getContract(compressedTypeId, contracts), contracts));
                            }
                        }
                        else {
                            result.push(uncompressed[propertyName] || null);
                        }
                    });
                }

                return result || null;
            }
        };
    
    compression.compressor = compressor;

}(window.jQuery, window));
/* jquery.signalR.compression.decompressor.js */
/*global window:false */
/// <reference path="jquery.signalr.compression.utilities.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        compression = signalR.compression,
        utilities = compression._.utilities,
        decompressor = {
            decompress: function (compressed, contract, contracts) {
                var result,
                    that = this;

                if (compressed) {
                    result = {};

                    $.each(contract, function (i, val) {
                        var propertyName = val[0],
                            compressedTypeId = val[1][0],
                            enumerable = val[1][1],
                            enumerated;

                        // Check the payload type of the parameter, if it's a payload we need to recursively decompress it
                        if (utilities.isPayload(compressedTypeId, contracts)) {
                            if (enumerable) {
                                enumerated = [];

                                for (var j = 0; j < compressed[i].length; j++) {
                                    enumerated[j] = that.decompress(compressed[i][j], utilities.getContract(compressedTypeId, contracts), contracts);
                                }

                                result[propertyName] = enumerated;
                            }
                            else {
                                result[propertyName] = that.decompress(compressed[i], utilities.getContract(compressedTypeId, contracts), contracts);
                            }
                        }
                        else {
                            result[propertyName] = compressed[i];
                        }
                    });
                }

                return result;
            }
        };

    compression.decompressor = decompressor;

}(window.jQuery, window));
/* jquery.signalR.compression.contractRetriever.js */
/*global window:false */
/// <reference path="jquery.signalr.compression.core.js" />

(function ($, window) {
    "use strict";

    var signalR = $.signalR,
        savedConnection = signalR.fn.init;

    $.signalR.fn.init = function () {
        var connection = this,
            compressionData = {
                decompressResult: [], // Array of Booleans representing if we should decompress an invocation result,
                contracts: {} // Contracts to abide by when sending/receiving data
            };

        savedConnection.apply(connection, arguments),

        connection._.compressionData = compressionData;

        connection.starting(function (result) {
            compressionData.contracts = result.Contracts;
        });

        return connection;
    };

}(window.jQuery, window));
/* jquery.signalR.compression.methodInvoker.js */
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
/* jquery.signalR.compression.receiveHandler.js */
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
