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
                            payloadId = val[1][0],
                            compressionTypeId = val[1][1],
                            enumerable = utilities.isEnumerable(compressionTypeId),
                            convertToZero = !utilities.isNumeric(compressionTypeId);

                        // Check the payload type of the parameter, if it's a payload we need to recursively compress it
                        if (uncompressed[i] && utilities.isPayload(payloadId, contracts)) {
                            if (enumerable) {
                                enumerated = [];

                                for (var j = 0; j < uncompressed[propertyName].length; j++) {
                                    enumerated[j] = that.compress(uncompressed[propertyName][j], utilities.getContract(payloadId, contracts), contracts);
                                }

                                result[propertyName].push(enumerated);
                            }
                            else {
                                result.push(that.compress(uncompressed[propertyName], utilities.getContract(payloadId, contracts), contracts));
                            }
                        }
                        else {
                            if ((uncompressed[propertyName] === null ||  typeof uncompressed[propertyName] === 'undefined') && convertToZero) {
                                uncompressed[propertyName] = 0;
                            }
                            else if (uncompressed[propertyName] === 'undefined') { // If this is true then the value is a numeric set to undefined, need to reset it to null
                                uncompressed[propertyName] = null;
                            }

                            result.push(uncompressed[propertyName]);
                        }
                    });
                }

                return result || null;
            }
        };
    
    compression.compressor = compressor;

}(window.jQuery, window));