
function initView() {
    loadChatList()
}

function loadChatList() {
    CallAjax(
        _ChatListUrl,
        'GET',
        null,
        function (res) {
            try {
                console.log(res)

                $('#chat-list').html(res)
            }
            catch (ex) {
                console.log(ex)
            }
        },
        function (err) {
            try {
                console.log(err)
            }
            catch (ex) {
                console.log(ex)
            }            
        }
    )
}

function loadChatCard() {
    CallAjax(
        _ChatListUrl,
        'GET',
        null,
        function (res) {
            try {
                console.log(res)

                $('#chat-list').html(res)
            }
            catch (ex) {
                console.log(ex)
            }
        },
        function (err) {
            try {
                console.log(err)
            }
            catch (ex) {
                console.log(ex)
            }
        }
    )
}