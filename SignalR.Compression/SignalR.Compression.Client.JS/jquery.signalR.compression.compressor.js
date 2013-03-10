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