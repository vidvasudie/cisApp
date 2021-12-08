function UpdateGallery() {
    try {
        $('.thumbnail-gallery').html('')
        $('.single-img').each((index, elem) => {
            var imgElem = $(elem).find('img')
            console.log(index, $(imgElem).attr('src'));

            var img = document.createElement("img");
            img.src = $(imgElem).attr('src')
            img.className = 'img-thumbnail img-list'

            $('.thumbnail-gallery').append(img)
        })
    }
    catch (ex) {

    }
}