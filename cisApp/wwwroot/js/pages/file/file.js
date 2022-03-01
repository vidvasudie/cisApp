const _KeywordDataAttr = 'keyword-data'

$(document).ready(function () {
    console.log('document ready')


})

$('body').on('click', '.custom-category', function () {
    var elem = $(this)
    console.log('keyword=', elem.attr(_KeywordDataAttr));

    if ($(this).hasClass('active')) {
        // remove filter
        $('#Category').val('')

        $('.custom-category.active').removeClass('active')
    }
    else {
        $('#Category').val(elem.attr(_KeywordDataAttr))

        $('.custom-category.active').removeClass('active')

        $(elem).addClass('active')
    }

    PagerClick(1)
})

$('body').on('click', '.custom-tag', function () {
    var elem = $(this)
    console.log('keyword=', elem.attr(_KeywordDataAttr));

    if ($(this).hasClass('active')) {
        // remove filter
        $('#Tag').val('')

        $('.custom-tag.active').removeClass('active')
    }
    else {
        $('#Tag').val(elem.attr(_KeywordDataAttr))

        $('.custom-tag.active').removeClass('active')

        $(elem).addClass('active')
    }
    

    PagerClick(1)
})

$('body').on('click', '.custom-search-by', function () {
    var elem = $(this)
    console.log('keyword=', elem.attr(_KeywordDataAttr));
    $('#SearchBy').val(elem.attr(_KeywordDataAttr))

    $('.custom-search-by.active').removeClass('active')

    $(elem).addClass('active')

    PagerClick(1)
})

$('body').on('change', '#Designer', function () {
    var elem = $(this)
    console.log('keyword=', elem.val());
    PagerClick(1)
})