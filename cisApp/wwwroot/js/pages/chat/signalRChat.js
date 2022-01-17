var _connection

function connectSignalR() {
    _connection = new signalR.HubConnectionBuilder()
        .withUrl(_SignalRUrl + "?userId=" + _UserID).build();

    // add on sendMessage Listener
    _connection.on("SendMessage", function (messageObj) {
        console.log('SendMessage', messageObj)

        loadChatList()

        try {
            var recieverId = $('#RecieverId').val();
            if (recieverId == messageObj.senderId) {
                replaceChatMessage();
            }
        }
        catch (ex) {
            console.log('on replaceChatMessage SignalR', ex)
        }

        
        
    });


    _connection.onclose(function () {
        _connection.start()
    })

    //_connection.serverTimeoutInMilliseconds = 1000 * 60 * 10; // for  10 minute

    _connection.start()
}

function invokeSendMessage(recieverId, message, imgs) {
    _connection.invoke("SendMessage", recieverId, message, imgs).catch(function (err) {
        return console.error(err.toString());
    });
}