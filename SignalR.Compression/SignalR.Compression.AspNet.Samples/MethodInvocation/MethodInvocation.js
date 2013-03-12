(function ($) {
    var methodInvocationHub = $.connection.methodInvocationHub,
        toBox = function (obj) {
            return "<pre>" + JSON.stringify(obj) + "</pre>";
        }
        getPersonObject = function () {
            return {
                FirstName: $("#firstName").val(),
                LastName: $("#lastName").val(),
                Age: $("#age").val()
            };
        },
        getStudentObject = function () {
            return $.extend({
                GPA: $("#gpa").val(),
                Debt: $("#debt").val()
            }, getPersonObject());
        };

    methodInvocationHub.client.echo = function (result) {
        $("#echoDecompressed").html(toBox(result));
    };    

    $.connection.hub.compression.serverInvokingClient(function (methodName, args) {
        $("#echoCompressed").html("Invoking <strong>" + methodName + "</strong> on client with:<br />" + toBox(args));
    });

    $.connection.hub.compression.invokingServerMethod(function (methodName, args) {
        $("#invokingCompressed").html("Invoking <strong>" + methodName + "</strong> on server with:<br />" + toBox(args));
    });

    $.connection.hub.logging = true;

    $.connection.hub.start().done(function () {
        $("#sendPerson").click(function () {
            methodInvocationHub.server.echoPerson(getPersonObject());
        });

        $("#sendStudent").click(function () {
            methodInvocationHub.server.echoStudent(getStudentObject());
        });
    });
})($);
