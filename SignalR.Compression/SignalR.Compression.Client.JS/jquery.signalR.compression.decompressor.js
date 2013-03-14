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
                            payloadId = val[1][0],
                            compressionTypeId = val[1][1],
                            enumerable = utilities.isEnumerable(compressionTypeId),
                            convertToNull = !utilities.isNumeric(compressionTypeId),
                            enumerated;

                        // Check the payload type of the parameter, if it's a payload we need to recursively decompress it
                        if (compressed[i] && utilities.isPayload(payloadId, contracts)) {
                            if (enumerable) {
                                enumerated = [];

                                for (var j = 0; j < compressed[i].length; j++) {
                                    enumerated[j] = that.decompress(compressed[i][j], utilities.getContract(payloadId, contracts), contracts);
                                }

                                result[propertyName] = enumerated;
                            }
                            else {
                                result[propertyName] = that.decompress(compressed[i], utilities.getContract(payloadId, contracts), contracts);
                            }
                        }
                        else {
                            if (compressed[i] === 0 && convertToNull) {
                                compressed[i] = null;
                            }

                            result[propertyName] = compressed[i];
                        }
                    });
                }

                return result;
            }
        };

    compression.decompressor = decompressor;

}(window.jQuery, window));