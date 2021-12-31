﻿
var _Page = 1

function initView() {
    loadChatList()
    loadChatCard()
}

function loadChatList() {
    CallAjax(
        _ChatListUrl,
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
        ps.element.scrollTop = 626

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
    var recieverId = $('#RecieverId').val();

    var url = _ChatMessageUrl.replace('__id__', recieverId).replace('__page__', page);
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


$('body').on('keydown', '.card-footer textarea', function (e) {
    if (e.keyCode == 13) {
        console.log('custom chat handle')

        sendMessage();
        //_handeMessaging(element);
        e.preventDefault();

        return false;
    }
});

$('body').on('click', '.card-footer .chat-send', function (e) {
    console.log('custom chat handle')

    sendMessage();
    //_handeMessaging(element);
});

function sendMessage() {
    var element = KTUtil.getById('kt_chat_content')
    var messagesEl = KTUtil.find(element, '.messages');
    var scrollEl = KTUtil.find(element, '.scroll');
    var textarea = KTUtil.find(element, 'textarea');

    if (textarea.value.length === 0) {
        return;
    }

    var node = document.createElement("DIV");
    KTUtil.addClass(node, 'd-flex flex-column mb-5 align-items-end');

    var html = '';
    html += '<div class="d-flex align-items-center">';
    html += '	<div>';
    html += '		<span class="text-muted font-size-sm">2 Hours</span>';
    html += '		<a href="#" class="text-dark-75 text-hover-primary font-weight-bold font-size-h6">You</a>';
    html += '	</div>';
    html += '	<div class="symbol symbol-circle symbol-40 ml-3">';
    html += '		<img alt="Pic" src="assets/media/users/300_12.jpg"/>';
    html += '	</div>';
    html += '</div>';
    html += '<div class="mt-2 rounded p-5 bg-light-primary text-dark-50 font-weight-bold font-size-lg text-right max-w-400px">' + textarea.value + '</div>';

    KTUtil.setHTML(node, html);
    messagesEl.appendChild(node);
    
    scrollEl.scrollTop = parseInt(KTUtil.css(messagesEl, 'height'));

    var ps;
    if (ps = KTUtil.data(scrollEl).get('ps')) {
        ps.update();
        
    }

    console.log('ps', ps)

    postMessage(textarea.value)
    textarea.value = '';
}

function postMessage(text) {

    var recieverId = $('#RecieverId').val();

    var data = {};
    data.RecieverId = recieverId
    data.Message = text

    CallAjax(
        _SendMessageUrl,
        'POST',
        //JSON.stringify(data),
        data,
        function (res) {
            try {
                //console.log(res)
                replaceChatMessage();
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


