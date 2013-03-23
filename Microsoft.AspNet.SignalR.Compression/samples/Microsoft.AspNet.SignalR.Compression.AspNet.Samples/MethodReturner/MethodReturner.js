(function ($) {
    var methodReturnerHub = $.connection.methodReturnerHub,
        toBox = function (obj) {
            return "<pre>" + JSON.stringify(obj) + "</pre>";
        };

    // Need to have at least one client side method to be subscribed to the hub
    methodReturnerHub.client.foo = function () { };

    $.connection.hub.compression.methodResponse(function (result) {
        $("#resultCompressed").html(toBox(result));
    });

    $.connection.hub.logging = true;

    $.connection.hub.start().done(function () {
        $("#getParent").click(function () {
            methodReturnerHub.server.getParent().done(function(result) {
                $("#resultDecompressed").html(toBox(result));
            });
        });

        $("#getTeacher").click(function () {
            methodReturnerHub.server.getTeacher().done(function(result) {
                $("#resultDecompressed").html(toBox(result));
            });
        });
    });
})($);
