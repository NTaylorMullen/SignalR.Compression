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