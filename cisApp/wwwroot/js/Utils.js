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