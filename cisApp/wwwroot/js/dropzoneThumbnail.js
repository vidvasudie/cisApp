$(document).ready(function () {

    // single file upload
    $('#kt_dropzone_1').dropzone({
        url: _DropzoneUrl, // Set the url for your upload script location
        paramName: "upload_file", // The name that will be used to transfer the file
        maxFiles: 1,
        maxFilesize: 5, // MB
        addRemoveLinks: true,
        thumbnailWidth: null,
        thumbnailHeight: null,
        init: function () {
            this.on("thumbnail", function (file, dataUrl) {
                $('.dz-image').last().find('img').attr({ width: '100%', height: '100%' });
            })
            this.on("success", function (file) {
                $('.dz-image').css({ "width": "100%", "height": "auto" });
            })
            this.on('maxfilesexceeded', function (file) {
                console.log('maxfilesexceeded')
                this.removeAllFiles();
                this.addFile(file);
            });

            if (_DropzoneFilePath != '') {
                var mockFile = {
                    name: _DropzoneFileName, size: 12345, accepted: true, url: _DropzoneFilePath
                };

                console.log(mockFile.url)

                var dropzoneThis = this;
                this.files.push(mockFile);
                this.emit('addedfile', mockFile);
                //this.createThumbnailFromUrl(mockFile, mockFile.url);
                this.createThumbnailFromUrl(mockFile,
                    dropzoneThis.options.thumbnailWidth,
                    dropzoneThis.options.thumbnailHeight,
                    dropzoneThis.options.thumbnailMethod, true, function (thumbnail) {
                        dropzoneThis.emit('thumbnail', mockFile, mockFile.url);

                        $('.dz-remove').on('click', function (e) {
                            console.log('fileRemove clicked')
                            $(_ModelNamePrefix + "fileRemove").val(true)
                        })
                    });
                this.emit('complete', mockFile);

                $('.dz-image').last().find('img').attr({ width: '100%', height: '100%' });
                $('.dz-image').css({ "width": "100%", "height": "auto" });

                console.log('init', this.files)
            }
        },
        accept: function (file, done) {
            done();
        },
        success: function (file) {
            console.log('success')
            console.log(file)
            $(_ModelNamePrefix + "FileBase64").val(file.dataURL)
            $(_ModelNamePrefix + "FileName").val(file.name)
            $(_ModelNamePrefix + "FileSize").val(file.size)

            $('.dz-remove').on('click', function (e) {
                console.log('fileRemove clicked')
                $(_ModelNamePrefix + "FileRemove").val(true)
            })
        },
    });

    // multiple file upload
    $('#kt_dropzone_multi').dropzone({
        url: _DropzoneUrl, // Set the url for your upload script location
        paramName: "upload_file", // The name that will be used to transfer the file
        maxFiles: 1,
        maxFilesize: 5, // MB
        addRemoveLinks: false,
        thumbnailWidth: null,
        thumbnailHeight: null,
        init: function () { 
            this.on("success", function (file) {
                console.log('success')
                console.log(file)
                console.log(file.name)
                console.log(file.size)
                //console.log(file.dataURL)
                var _this = this;
                var suc = function (html) {
                    $('.' + elementId).append(html);
                    _this.removeAllFiles();

                    try {
                        UpdateGallery()
                    }
                    catch (ex) {

                    }
                }
                var err = function (e) {
                    toastr.error('อัพโหลดรูปไม่สำเร็จกรุณาลองใหม่');
                    console.log(e)
                    _this.removeAllFiles();
                }
                CallAjax(_DropzonePreviewUrl, 'POST', { FileName: file.name, Size: file.size, FileBase64: file.dataURL, JobExTypeId: obj.type }, suc, err);
            })
        },
        accept: function (file, done) {
            done();
        },
        //success: _DropzoneSuccess,
    }); 

});

function initDropzone(elementId, obj) {
    $('#'+elementId).dropzone({
        url: _DropzoneUrl, // Set the url for your upload script location
        paramName: "upload_file", // The name that will be used to transfer the file
        maxFiles: 1,
        maxFilesize: 5, // MB
        addRemoveLinks: false,
        thumbnailWidth: null,
        thumbnailHeight: null,
        init: function () {
            this.on("success", function (file) {
                console.log('success')
                console.log(file)
                console.log(file.name)
                console.log(file.size)
                //console.log(file.dataURL)
                var _this = this;
                var suc = function (html) {
                    $('.' + elementId).append(html);
                    _this.removeAllFiles();

                    try {
                        UpdateGallery()
                    }
                    catch (ex) {

                    }
                }
                var err = function (e) {
                    toastr.error('อัพโหลดรูปไม่สำเร็จกรุณาลองใหม่');
                    console.log(e)
                    _this.removeAllFiles();
                }
                CallAjax(_DropzonePreviewUrl, 'POST', { FileName: file.name, Size: file.size, FileBase64: file.dataURL, JobExTypeId: obj.type }, suc, err);
            })
        },
        accept: function (file, done) {
            done();
        },
        //success: _DropzoneSuccess,
    });
}