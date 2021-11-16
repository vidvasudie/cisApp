
$(".bt-reset-pass").click(function (e) {

    Swal.fire({
        title: "reset password",
        text: "ยืนยันแก้ไขรหัสผ่านผู้ใช้งาน",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "ใช่, ยืนยัน",
        cancelButtonText: "ไม่, ยกเลิก",
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            Swal.fire(
                "สำเร็จ!",
                "รหัสผ่านถูกแก้ไข ระบบทำการส่งรหัสผ่านใหม่ให้ผู้ใช้งาน ทาง Email เรียบร้อยแล้ว!!!",
                "success"
            )
            // result.dismiss can be "cancel", "overlay",
            // "close", and "timer"
        } else if (result.dismiss === "cancel") {
            //Swal.fire(
            //    "Cancelled",
            //    "Your imaginary file is safe :)",
            //    "error"
            //)
        }
    });



});

$(".bt-delete").click(function (e) {

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
            Swal.fire(
                "สำเร็จ!",
                "ข้อมูลถูกลบออกจากระบบเรียบร้อยแล้ว",
                "success"
            )
            // result.dismiss can be "cancel", "overlay",
            // "close", and "timer"
        } else if (result.dismiss === "cancel") {
            //Swal.fire(
            //    "Cancelled",
            //    "Your imaginary file is safe :)",
            //    "error"
            //)
        }
    });



});


/*downgrade*/
$(".bt-downgrade").click(function (e) {

    Swal.fire({
        title: "ลดระดับผู้ใช้งาน",
        text: "ยืนยันการลดระดับผู้ใช้งานจาก Designer เป็น ผู้ใช้งานทั่วไป ใช่หรือไม่",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "ใช่, ยืนยัน",
        cancelButtonText: "ไม่, ยกเลิก",
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            Swal.fire(
                "สำเร็จ!",
                "ปรับแก้ไขระดับการใช้งานสำเร็จ",
                "success"
            )
            // result.dismiss can be "cancel", "overlay",
            // "close", and "timer"
        } else if (result.dismiss === "cancel") {
            //Swal.fire(
            //    "Cancelled",
            //    "Your imaginary file is safe :)",
            //    "error"
            //)
        }
    });



});
