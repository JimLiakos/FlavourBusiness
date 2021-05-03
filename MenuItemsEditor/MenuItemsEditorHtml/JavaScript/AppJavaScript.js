function log(str) {
    $('#result').text($('#result').text() + " " + str);
}


function logA(str) {

    $('#result').text($('#result').text() + " " + str);

    return $('#result').text() + $('#name').val();
}

function invokeCSCode(data) {
    try {

        log("Sending Data:" + data);

        if (typeof invokeCSharpAction === "function")
            invokeCSharpAction(data);
        else
            cscallbackObj.notify(data);
    }
    catch (err) {
        log(err);
    }
}