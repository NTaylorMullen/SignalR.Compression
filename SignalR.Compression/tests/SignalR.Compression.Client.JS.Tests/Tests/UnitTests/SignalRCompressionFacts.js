QUnit.module("SignalR Compression Facts");

QUnit.test("SignalR Compression API info is available", function () {
    QUnit.isSet($.signalR.compression, "Verifies SignalR Compression is available.");
    QUnit.isSet($.signalR.compression.compressor, "Verifies SignalR Compression Compressor is available.");
    QUnit.isSet($.signalR.compression.decompressor, "Verifies SignalR Compression De-compressor is available.");
    QUnit.isSet($.signalR.compression.decompressor, "Verifies SignalR Compression Events are available.");
});