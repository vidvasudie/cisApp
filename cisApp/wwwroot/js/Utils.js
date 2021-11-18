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


$('body').on('click', '.btn-submit', function () {
    $('#kt_form').submit();
})