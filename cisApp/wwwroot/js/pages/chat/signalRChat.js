var _connection

function connectSignalR() {
    _connection = new signalR.HubConnectionBuilder()
        .withUrl(_SignalRUrl + "?userId=" + _UserID).build();

    // add on sendMessage Listener
    // ถ้ามีข้อความส่งมาจะ trigger
    _connection.on("SendMessage", function (messageObj) {
        console.log('SendMessage', messageObj)

        loadChatList()

        try {
            var recieverId = $('#RecieverId').val();
            if (recieverId == messageObj.senderId) {
                replaceChatMessage();
                //invokeReadMessage(recieverId)
            }

            
        }
        catch (ex) {
            console.log('on replaceChatMessage SignalR', ex)
        }

        
        
    });

    _connection.on("ReadMessage", function (messageObj) {
        console.log('ReadMessage', messageObj)

    });


    _connection.onclose(function () {
        // ในกรณีมีปัญหา connect เปิดได้ 30 วิแล้ว ปิดไปเอง
        // สั่งเปิดทุกครั้งที่โดนตัด
        _connection.start()
    })

    //_connection.serverTimeoutInMilliseconds = 1000 * 60 * 10; // for  10 minute
    // ทำการเปิด connection
    _connection.start()
}

// สำหรับตอนส่งข้อความหาคนอื่น ต้อง trigger SendMessage กลับไปด้วย
function invokeSendMessage(recieverId, message, imgs) {
    _connection.invoke("SendMessage", recieverId, message, imgs).catch(function (err) {
        return console.error(err.toString());
    });
}

function invokeReadMessage(recieverId) {
    console.log('InvokeReadMessage', recieverId)
    _connection.invoke("ReadMessage", recieverId).catch(function (err) {
        return console.error(err.toString());
    });
}