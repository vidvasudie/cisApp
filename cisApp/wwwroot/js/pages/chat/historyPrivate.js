
var _Page = 1

function initView() {    
    loadChatList()
    loadChatCard()

}

$('body').on('change', '#chatSearchText', function () {
    loadChatList()
})

function loadChatList() {
    var url = _ChatListUrl
    var text = $('#chatSearchText').val()
    if (text.length > 0) {
        url = url + '&text=' + text
    }
    CallAjax(
        url,
        'GET',
        null,
        function (res) {
            try {
                //console.log(res)

                $('#chat-list').html(res)

                // Init aside and user list
                KTAppChat.initAside();
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
        _ChatCardUrl,
        'GET',
        null,
        function (res) {
            try {
                chatCardInit(res)
                
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



$('body').on('click', '.chat-list-item', function () {
    //console.log('chat-list-item on click')
    var elem = $(this)
    var url = $(elem).attr(_dataUrlAttr)
    //console.log('chat-list-data-url', url)
    CallAjax(
        url,
        'GET',
        null,
        function (res) {
            try {
                chatCardInit(res)
                _Page = 1
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
})

function chatCardInit(res) {
    //console.log(res)


    $('#kt_chat_content').html(res)

    KTAppChat.initChat();

    var element = KTUtil.getById('kt_chat_content')
    var scrollEl = KTUtil.find(element, '.scroll');

    var ps;
    if (ps = KTUtil.data(scrollEl).get('ps')) {
        console.log('ps scroolTop', ps)
        console.log('lastScrollTop', ps.contentHeight)
        ps.element.scrollTop = ps.contentHeight

        setTimeout(function () {
            ps.element.scrollTop = ps.contentHeight
        }, 500);

        $(ps.element).on('ps-y-reach-start', function () {
            console.log('ps-y-reach-start')

            prependChatMessage()
        })
        //ps.update(ps);
    }
}

function replaceChatMessage() {
    try {
        getChatMessage(function (res) {
            $('#charCardMessage').html(res);

            KTAppChat.initChat();

            var element = KTUtil.getById('kt_chat_content')
            var scrollEl = KTUtil.find(element, '.scroll');

            var ps;
            if (ps = KTUtil.data(scrollEl).get('ps')) {
                console.log('ps scroolTop', ps)
                console.log('lastScrollTop', ps.contentHeight)
                ps.element.scrollTop = ps.contentHeight

                setTimeout(function () {
                    ps.element.scrollTop = ps.contentHeight
                }, 500);

            }
        }, 1)
    }
    catch (ex) {

    }
}

function prependChatMessage() {
    try {
        _Page = _Page + 1
        console.log('prependChatMessage')
        getChatMessage(function (res) {
            
            

            var element = KTUtil.getById('kt_chat_content')
            var scrollEl = KTUtil.find(element, '.scroll');

            var ps;
            if (ps = KTUtil.data(scrollEl).get('ps')) {
                console.log('ps scroolTop', ps)
                var scrollTopBefore = ps.element.scrollHeight;

                console.log('scrollTopBefore', scrollTopBefore)

                $('#charCardMessage').prepend(res);

                var scrollTopAfter = ps.element.scrollHeight;

                console.log('scrollTopAfter', scrollTopAfter)

                ps.element.scrollTop = scrollTopAfter - scrollTopBefore - 200

                //ps.update(ps);
            }
        }, _Page)
    }
    catch (ex) {

    }
}

function getChatMessage(callbackFunc, page = 1) {
    
    var senderId = $('#SenderId').val();
    var recieverId = $('#RecieverId').val();

    console.log('onGetChatMessage recieverId=', recieverId, ' ,senderId=', senderId)

    var url = _ChatMessageUrl.replace('__id1__', senderId).replace('__id2__', recieverId).replace('__page__', page);
    CallAjax(
        url,
        'GET',
        null,
        function (res) {
            try {
                callbackFunc(res)
            }
            catch (ex) {
                console.log(ex)
            }
        },
        function (err) {
            console.log(err)
        }
    )
}

