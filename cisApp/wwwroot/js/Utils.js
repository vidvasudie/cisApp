﻿const _dataUrlAttr = 'data-url'
const _dataGuidAttr = 'data-guid'

function redirect(data) {
    if (data.success) {
        // Display a success toast, with a title
        //toastr.success('บันทึกข้อมูลสำเร็จ', 'ดำเนินการบันทึกข้อมูลเรียบร้อยแล้ว', { fadeIn: 300 })
        Swal.fire(
            "บันทึกข้อมูลสำเร็จ",
            "ดำเนินการบันทึกข้อมูลเรียบร้อยแล้ว",
            "success"
        )
        window.setTimeout(function () {
            // do whatever you want to do
            if (data.isRedirect) {
                window.location = data.redirectUrl;
            }
        }, 600);
    }
    else {
        // Display an error toast, with a title
        //toastr.error('บันทึกข้อมูลไม่สำเร็จ', data.message)
        Swal.fire(
            "บันทึกข้อมูลไม่สำเร็จ",
            data.message,
            "error"
        )
    }
}
function GetValidatePersonalId(urlAction) {
    var pid = $('.valid-pid').val();
    var $pid = ".valid-pid";
    if (pid !== '' && pid !== null) {
        var url = urlAction + '?pid=' + pid;
        $.ajax({
            url: url,
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function (res) {
                if (res.status == false) {
                    $($pid).val('').focus();
                    alertError("เลขบัตรประชาชนไม่ถูกต้อง กรุณาลองใหม่!");
                }
            },
            error: function (err) {
                alertError("เกิดข้อผิดพลาด กรุณาลองใหม่!");
            }
        })
    }
}
function GetDataDistrict(urlAction) {
    var valueProvice = $('.province').val();
    var optionEmpty = '<option value="">' + 'เลือกข้อมูล' + '</option>';
    var attdistrict = ".district";
    $(attdistrict).empty().append(optionEmpty);
    $('.subdistrict').empty().append(optionEmpty);
    $('.postcode').val('');
    if (valueProvice !== '' && valueProvice !== null) {
        var url = urlAction + '?provinceId=' + valueProvice;
        $.ajax({
            url: url,
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function (res) {
                if (res.status == true) { 
                    if (res.data.length > 0) {
                        for (let i = 0; i < res.data.length; i++) {
                            $(attdistrict).append("<option value='" + res.data[i].id + "'>" + res.data[i].nameTh + "</option>");
                        }
                        //$(attdistrict).trigger("chosen:updated");
                    }
                }
                else {
                    alertError("เกิดข้อผิดพลาด กรุณาลองใหม่!");
                }
            },
            error: function (err) {
                alertError("เกิดข้อผิดพลาด กรุณาลองใหม่!");
            }
        })
    }
}
function GetDataSubDistrict(urlAction) {
    var valueDistrict = $('.district').val();
    var optionEmpty = '<option value="">' + 'เลือกข้อมูล' + '</option>';
    var attsubdistrict = ".subdistrict";
    $(attsubdistrict).empty().append(optionEmpty);
    $('.postcode').val('');
    if (valueDistrict !== '' && valueDistrict !== null) {
        var url = urlAction + '?districtId=' + valueDistrict;  
        $.ajax({
            url: url,
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function (res) {
                if (res.status == true) {
                    if (res.data.length > 0) {
                        for (let i = 0; i < res.data.length; i++) {
                            $(attsubdistrict).append("<option value='" + res.data[i].id + "'>" + res.data[i].nameTh + "</option>");
                        }
                        //$(attsubdistrict).trigger("chosen:updated");
                    }
                }
                else {
                    alertError("เกิดข้อผิดพลาด กรุณาลองใหม่!");
                }
            },
            error: function (err) {
                alertError("เกิดข้อผิดพลาด กรุณาลองใหม่!");
            }
        })
    }
}
function GetDataPostCode(urlAction) {
    var valueSubDistrict = $('.subdistrict').val();
    var attzipcode = ".postcode";
    $(attzipcode).val('');
    if (valueSubDistrict !== '' && valueSubDistrict !== null) {
        var url = urlAction + '?subdistrictId=' + valueSubDistrict;
        $.ajax({
            url: url,
            method: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function (res) {
                if (res.status == true) {
                    if (res.data != null) {
                        $(attzipcode).val(res.data.postCode);
                    }
                }
                else {
                    alertError("เกิดข้อผิดพลาด กรุณาลองใหม่!");
                }
            },
            error: function (err) {
                alertError("เกิดข้อผิดพลาด กรุณาลองใหม่!");
            }
        })
    }
}

$('body').on('click', '.btn-submit', function () {
    $('#kt_form').submit();
})

$('body').on('keypress', '.is-number-only', function (e) {
    keys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']
    return keys.indexOf(event.key) > -1
}) 
$('body').on('keyup', '.is-number-only', function (e) {
    keys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']
    return keys.indexOf(event.key) > -1
})

$('body').on('click', '.bt-delete', function (e) {

    var elem = $(this);

    Swal.fire({
        title: "ลบข้อมูล",
        text: "ยืนยันการลบรายการนี้ใช่หรือไม่",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "ใช่, ยืนยัน",
        cancelButtonText: "ไม่, ยกเลิก",
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            var url = $(elem).attr(_dataUrlAttr)
            $.ajax({
                url: url,
                method: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (res) {
                    redirect(res)                    
                },
                error: function (err) {
                    redirect(err)                    
                }
            })
            
        } else if (result.dismiss === "cancel") {
            
        }
    });
});

$('body').on('click', '.bt-update', function (e) {

    var elem = $(this);

    Swal.fire({
        title: "แก้ไขข้อมูล",
        text: "ยืนยันการแก้ไขข้อมูล",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "ใช่, ยืนยัน",
        cancelButtonText: "ไม่, ยกเลิก",
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            var url = $(elem).attr(_dataUrlAttr)
            window.location = url;

        } else if (result.dismiss === "cancel") {

        }
    });
});