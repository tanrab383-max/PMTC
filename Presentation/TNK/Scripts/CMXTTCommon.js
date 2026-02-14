$(document).ready(function () {
    $('.select2').select2({
        placeholder: "Select an option",
        allowClear: true
    });
    $('.date').datepicker({
        orientation: "left",
        autoclose: true
    });

})
$("input.number").map(function (key, val) {
    var val = $(this).val();
    val = val.split(",").join("");
    val = parseInt(val);
    $(this).val(val.toLocaleString('en-us'));
})
$(window).keydown(function (e) {
    //Xu ly control_s: submit form
    if (e.ctrlKey && e.keyCode === 83) {
        $("form").submit();
        return false;
    }

    //Xu ly control_B: submit form
    if (e.ctrlKey && e.keyCode === 66) {
        $(".back").click();
        return false;
    }

})

try {
    var tr = $("#tr").html();
    var type = "";
    tr = tr.replace("checked", "");
    tr = tr.replace("true", "false");
    tr = tr.replace("True", "false");
} catch (e)
{ }
type = "";
removeSelect();
function removeSelect() {
    setTimeout(function () {
        var a = $('select.select2');
        a.map(function (key, val) {
            $(val).removeClass("select2");
        });
    }, 100);
}

var total;
function calTotal() {
    debugger;
    total = 0;
    $("#tbody").find("tr").map(function (key, val) {
        if ($(val).is(":visible")) {
            var isDeleted = $(val).find(".IsDeleted").val();
            if (isDeleted != "true") {
                var sotien = parseInt($(val).find(".number").val().split(",").join(""));
                if (isNaN(sotien))
                    sotien = 0;
                total += sotien;
            }
        }
    });
    if (isNaN(total))
        total = 0;
    $("#TongCongThanhToan").html(total.toLocaleString("en-us"));
    Cal();
    TinhConLai();
}
function Remove(a) {
    var tr = $(a.closest("tr"));
    tr.find(".IsDeleted").val("true");
    tr.html(tr.html().split("required").join(""));
    tr.hide("fadeOut");
    calTotal();
    if (($("#noPhaiTra_SoTienNo").length > 0)) {
        CalThanhToan(a);
    }
}



$(".Total").html(parseInt($(".Total").html()).toLocaleString('en-us'));
$(document).on("click", ".chkTinhTrang", function () {
    $(this).closest("td").children(".TinhTrang").val($(this).is(":checked"));
})
//$(document).on("keypress", ".date", function () {
//    return false;
//});
function CalThanhToan(a) {
    debugger;
    setTimeout(function () {
        var total = 0;
        $("#tbody").find("tr").map(function (key, val) {
            if ($(val).is(":visible")) {
                var isDeleted = $(val).find(".IsDeleted").val();

                if (isDeleted != "true") {
                    let i = parseInt($(val).find(".number").val().split(",").join(""));
                    if (!isNaN(i))
                        total += i;
                }

            }
        });

        $("#ThanhToan").find(".Total").html(total.toLocaleString('en-us'));

        if ($("#item_GiaVon").val() != undefined && $("#item_ConLai").val() != undefined) {
            var buyprice = parseInt($("#item_GiaVon").val().split(",").join(""));
            var rest = buyprice - total;

            $("#item_ConLai").val(rest.toLocaleString('en-us'));
        }

        if ($("#Item_TongCong").val() != undefined && $("#Item_ConLai").val() != undefined) {
            var buyprice = parseInt($("#Item_TongCong").val().split(",").join(""));
            var rest = buyprice - total;

            $("#Item_ConLai").val(rest.toLocaleString('en-us'));
        }
        if ($("#item_GiaVon").val() != undefined && $("#noPhaiTra_SoTienNo").val() != undefined) {
            total = parseInt($(".Total").text().split(",").join(""));
            var buyprice = $("#item_GiaVon").val() == "" ? 0 : parseInt($("#item_GiaVon").val().split(",").join(""));
            var rest = buyprice - total;

            // alert(rest);
            $("#noPhaiTra_SoTienNo").val(rest.toLocaleString('en-us'));
            var conLai = parseInt($("#noPhaiTra_SoTienNo").val().split(",").join("")) - parseInt($("#noPhaiTra_SoTienDaTra").val().split(",").join(""));
            $("#noPhaiTra_SoTienConLai").val(conLai.toLocaleString('en-us'));
        }
        Cal();
        TinhConLai();
    }, 100);
}




$(document).on("keypress", ".number", function ($event) {
    return ($event.charCode >= 48 && $event.charCode <= 57) || $event.charCode == 0;
});
function CheckValidateThongTinNguoiThanhToan_PhieuThu() {
    debugger;
    //doi voi cac phieu thu (tru phieu bán xe và dịch vụ) thì bắt số tiền nhập phải >0
    //if ($("#TongCongThanhToan").length > 0) {
    //    if ($("#TongCongThanhToan").html() == "0") {
    //        //truong hop co nhap so tien tong cong can thu, neu can thu = 0 thi khong can canh bao so tien thanh toan
    //        if ($("#item_ConLai").val() != undefined && $("#item_ConLai").val() != "") {
    //            //neu các phieu thu cho con thi ko bat validate ở đây
    //            let aaaa = 1;
    //        }
    //        else if ($("#item_TongCong").val() != undefined && $("#item_TongCong").val() != "") {
    //            let tongCongCanTra = parseInt($("#item_TongCong").val().split(",").join(""));
    //            if (tongCongCanTra > 0) {
    //                toastWarning("Vui lòng nhập thanh toán.");
    //                return false;
    //            }
    //        }
    //        else {
    //            toastWarning("Vui lòng nhập thanh toán.");
    //            return false;
    //        }

    //    }
    //}
    if ($("#obj_KeToanTruong").length > 0) {
        //trúc khóa ngày 5/1
        //if ($("#obj_KeToanTruong").val() == "") {
        //    toastWarning("Vui lòng chọn Kế toán trưởng.");
        //    $("#obj_KeToanTruong").focus();
        //    return false;
        //}
        //if ($("#obj_NguoiLapPhieu").val() == "") {
        //    toastWarning("Vui lòng chọn Người lập phiếu.");
        //    $("#obj_NguoiLapPhieu").focus();
        //    return false;
        //}
        //if ($("#obj_NguoiNopTien").val() == "") {
        //    toastWarning("Vui lòng chọn Người nộp tiền.");
        //    $("#obj_NguoiNopTien").focus();
        //    return false;
        //}
        //if ($("#obj_NguoiThuTien").val() == "") {
        //    toastWarning("Vui lòng chọn Người thu tiền.");
        //    $("#obj_NguoiThuTien").focus();
        //    return false;
        //}
    }
    if ($("#Item_KeToanTruong").length > 0) {
        //trúc khóa ngày 5/1
        //if ($("#Item_KeToanTruong").val() == "") {
        //    toastWarning("Vui lòng chọn Kế toán trưởng.");
        //    $("#Item_KeToanTruong").focus();
        //    return false;
        //}
        //if ($("#Item_NguoiLapPhieu").val() == "") {
        //    toastWarning("Vui lòng chọn Người lập phiếu.");
        //    $("#Item_NguoiLapPhieu").focus();
        //    return false;
        //}
        //if ($("#Item_NguoiNopTien").val() == "") {
        //    toastWarning("Vui lòng chọn Người nộp tiền.");
        //    $("#Item_NguoiNopTien").focus();
        //    return false;
        //}
        //if ($("#Item_NguoiThuTien").val() == "") {
        //    toastWarning("Vui lòng chọn Người thu tiền.");
        //    $("#Item_NguoiThuTien").focus();
        //    return false;
        //}
    }

    if ($("#item_KeToanTruong").length > 0) {
        //trúc khóa ngày 5/1
        //if ($("#item_KeToanTruong").val() == "") {
        //    toastWarning("Vui lòng chọn Kế toán trưởng.");
        //    $("#item_KeToanTruong").focus();
        //    return false;
        //}
        //if ($("#item_NguoiLapPhieu").val() == "") {
        //    toastWarning("Vui lòng chọn Người lập phiếu.");
        //    $("#item_NguoiLapPhieu").focus();
        //    return false;
        //}
        //if ($("#item_NguoiNopTien").val() == "") {
        //    toastWarning("Vui lòng chọn Người nộp tiền.");
        //    $("#item_NguoiNopTien").focus();
        //    return false;
        //}
        //if ($("#item_NguoiThuTien").val() == "") {
        //    toastWarning("Vui lòng chọn Người thu tiền.");
        //    $("#item_NguoiThuTien").focus();
        //    return false;
        //}
    }
    return true;
}

function CheckValidatePhieuDichVu() {
    var result = true
    //if ($("#obj_SoHoaDon").val() == "") {
    //    toastWarning("Vui lòng nhập Số hóa đơn.");
    //    $("#obj_SoHoaDon").focus();
    //    return false;
    //}

    //if ($("#obj_SoChungTu").val() == "") {
    //    toastWarning("Vui lòng nhập Số chứng từ.");
    //    $("#obj_SoChungTu").focus();
    //    return false;
    //}
    $("#LuoiBaoHiem").find("tr").map(function (key, val) {
        var tuitienbh = $(val).find(".ctybh").select2('val');
        var sotienbh = $(val).find(".number").val().split(",").join("");

        tuitienbh = tuitienbh == "Chọn" ? "" : tuitienbh;
        //alert(tuitienbh);
        sotienbh = sotienbh == "" ? 0 : parseInt(sotienbh);
        //alert(sotienbh);
        if (tuitienbh != "" && sotienbh === 0) {
            toastWarning("Vui lòng nhập số tiền bảo hiểm");
            $(val).find(".number").focus();
            result = false;
            return false;
        }
        if (tuitienbh == "" && sotienbh != 0) {
            toastWarning("Vui lòng nhập công ty bảo hiểm");
            $(val).find(".ctybh").focus();
            result = false;
            return false;
        }
    })
    if (!result)
        return false;
    $("#DanhSachCoc").find("tr").map(function (key, val) {
        var soTienCoTheSuDung = parseInt($(val).find(".SoTienCoTheSuDung").val().split(",").join(""));
        var soTienThanhToan = parseInt($(val).find(".SoTienThanhToan").val().split(",").join(""));

        if (soTienCoTheSuDung < soTienThanhToan) {
            toastWarning("Số tiền dùng cọc không được vượt quá số tiền có thể thanh toán.Vui lòng kiểm tra lại.");
            $(val).find(".SoTienThanhToan").focus();
            result = false;
            return false;
        }
    })
    if (!result)
        return false;
    if (parseInt($("#NPT_SoTienNo").val().split(",").join("")) < 0) {
        toastWarning("Tổng thanh toán không thể lớn hơn số tiền phải thu.Vui lòng kiểm tra lại.");
        result = false;
        return false;
    }

    if (!result)
        return false;
    //if ($("#obj_HoaHongTaiXe").val() != "0"
    //                && $("#obj_NguoiDuyetHHTX").val() == ""
    //                ) {
    //    toastWarning("Vui lòng nhập thông tin Người duyệt hoa hồng tài xế.");
    //    $("#obj_NguoiDuyetHHTX").focus();
    //    return false;
    //}
    if (!result)
        return false;
    if ($("#obj_HoaHongTaiXe").val() == "0"
                    && $("#obj_NguoiDuyetHHTX").val() != ""
                    ) {
        toastWarning("Vui lòng nhập thông tin Số tiền hoa hồng tài xế.");
        $("#obj_HoaHongTaiXe").focus();
        return false;
    }
    if (!result)
        return false;
    return true;
}


function CheckValidatePhieuBanXe() {
    //if ($("#obj_SoHoaDon").val() == "") {
    //    toastWarning("Vui lòng nhập Số hóa đơn.");
    //    $("#obj_SoHoaDon").focus();
    //    return false;
    //}

    //if ($("#obj_SoChungTu").val() == "") {
    //    toastWarning("Vui lòng nhập Số chứng từ.");
    //    $("#obj_SoChungTu").focus();
    //    return false;
    //}

    if ($("#obj_HoTen").val() == "") {
        toastWarning("Vui lòng nhập Họ tên khách hàng.");
        $("#obj_HoTen").focus();
        return false;
    }

    //if ($("#Total").html() != "0" > 0 && $("#NguoiBaoLanh").val() == "") {
    //    toastWarning("Vui lòng nhập thông tin Người bảo lãnh.");
    //    $("#NguoiBaoLanh").focus();
    //    return false;
    //}


    return true;
}


function CheckValidatePhieuChi() {
    debugger;
    Cal();
    if ($("#item_SoChungTu").val() != undefined && $("#item_SoChungTu").val() == "") {
        toastWarning("Vui lòng nhập số chứng từ.");
        $("#item_SoChungTu").focus();
        return false;
    }
    if ($("#Item_SoChungTu").val() != undefined && $("#Item_SoChungTu").val() == "") {
        toastWarning("Vui lòng nhập số chứng từ.");
        $("#Item_SoChungTu").focus();
        return false;
    }
    if ($("#item_DoiTac").val() != undefined && $("#item_DoiTac").val() == ""
         && $("#item_DoiTac").offsetWidth > 0 && $("#item_DoiTac").offsetHeight > 0
        ) {
        toastWarning("Vui lòng nhập nhà cung cấp.");
        $("#item_DoiTac").focus();
        return false;
    }

    if ($("#Item_DoiTac").val() != undefined && $("#Item_DoiTac").val() == ""
        && $("#Item_DoiTac").offsetWidth > 0 && $("#Item_DoiTac").offsetHeight > 0
        ) {
        toastWarning("Vui lòng nhập nhà cung cấp.");
        $("#Item_DoiTac").focus();
        return false;
    }

    if ($("#Item_NhaCungCap").val() != undefined && $("#Item_NhaCungCap").val() == ""
          && $("#Item_NhaCungCap").offsetWidth > 0 && $("#Item_NhaCungCap").offsetHeight > 0
        ) {
        toastWarning("Vui lòng nhập nhà cung cấp.");
        $("#Item_NhaCungCap").focus();
        return false;
    }

    if ($("#item_MaXe").val() != undefined && $("#item_MaXe").val() == "") {
        toastWarning("Vui lòng nhập loại xe.");
        $("#item_MaXe").focus();
        return false;
    }


    if ($("#item_SoKhung").val() != undefined && $("#item_SoKhung").val() == "") {
        toastWarning("Vui lòng nhập số khung.");
        $("#item_SoKhung").focus();
        return false;
    }

    if ($("#item_HoTen").val() != undefined && $("#item_HoTen").val() == "") {
        toastWarning("Vui lòng nhập họ tên.");
        $("#item_HoTen").focus();
        return false;
    }

    if ($("#Item_LyDoChi").val() != undefined && $("#Item_LyDoChi").val() == "") {
        toastWarning("Vui lòng chọn lý do chi.");
        $("#Item_LyDoChi").focus();
        return false;
    }

    if ($("#Item_TaiSan").val() != undefined && $("#Item_TaiSan").val() == "") {
        toastWarning("Vui lòng chọn tài sản.");
        $("#Item_TaiSan").focus();
        return false;
    }

    if ($("#item_NoiDung").val() != undefined && $("#item_NoiDung").val() == "") {
        toastWarning("Vui lòng nhập nội dung.");
        $("#item_NoiDung").focus();
        return false;
    }

    //if ($("#noPhaiTra_SoTienConLai").val() != undefined && parseInt($("#noPhaiTra_SoTienConLai").val().split(",").join("")) < 0) {
    //    toastWarning("Vui lòng kiểm tra lại thông tin thanh toán.");
    //    return false;
    //}

    if ($(".lstCTNKPT").val() == "") {
        toastWarning("Vui lòng chọn kho.");
        return false;
    }

    //if ($("#ThanhToan").val() != undefined) {
    //    var isFalse = true;
    //    console.log($("#ThanhToan tbody").find("tr"));
    //    $("#ThanhToan tbody").find("#tr").each(function (index, val) {

    //        if ($(this).find('[element="htttBank"]').val() == ""
    //            && $(this).find('[element="htttBank"]').is(":visible")
    //            ) {
    //            toastWarning("Vui lòng chọn túi tiền thanh toán.");
    //            isFalse = false;
    //            return;
    //        }
    //        if ($(this).find('[element="htttSoTien"]').val() == ""
    //             && $(this).find('[element="htttSoTien"]').is(":visible")
    //            ) {
    //            toastWarning("Vui lòng nhập số tiền thanh toán.");
    //            isFalse = false;
    //            return;
    //        }
    //    });
    //    if (!isFalse)
    //        return isFalse;
    //}
    if ($("#maPhieuNo").val() != undefined && parseInt($("#maPhieuNo").val().indexOf("NMXTT")) >= 0) {
        calTotal();
        if (total > parseInt($("#item_GiaVon").val().split(",").join(""))) {
            toastWarning("Số tiền thanh toán không được lớn hơn giá mua");
            return false;
        }
    }

    if(total > parseInt($("#item_GiaVon").val().split(",").join("")) || $("#noPhaiTra_SoTienNo").val() < 0)
    {
        toastWarning("Số tiền thanh toán không được lớn hơn giá mua");
        return false;
    }
    if ($("#maLoaiPhieu").val() != undefined && $("#maLoaiPhieu").val() == "CMXTT")
    {
        debugger;
        var a = $("#noPhaiTra_SoTienNo").val();
        if (parseInt($("#noPhaiTra_SoTienNo").val().split(",").join("")) < 0)
        {
            toastWarning("Số tiền thanh toán không được lớn hơn giá mua");
            return false;
        }
    }
    if ($("#maPhieuNo").val() != undefined && parseInt($("#maPhieuNo").val().indexOf("NMXTT")) >= 0) {
        calTotal();
        if (parseInt($("#TongCongThanhToan").val().split(",").join("")) > parseInt($("#item_GiaVon").val().split(",").join("")) && $("#TongCongThanhToan").val() != undefined) {
            toastWarning("Số tiền thanh toán không được lớn hơn số tiền mua");
            return false;
        }
    }

    if ($("#ConLai").val() != undefined && parseInt($("#ConLai").val().split(",").join("")) < 0) {
        // alert(parseInt($("#ConLai").val().split(",").join("")));
        toastWarning("Số tiền thanh toán không được lớn hơn số tiền còn nợ");
        return false;
    }

    //if ($(".Total").text() != undefined && parseInt($(".Total").text().split(",").join("")) <= 0) {
    //    //alert($("#maPhieuNo").val());
    //    if ($("#maPhieuNo").val() != undefined && parseInt($("#maPhieuNo").val().indexOf("NMXTT")) >= 0) {
    //    }
    //    else {
    //        toastWarning("Vui lòng nhập số tiền thanh toán.");
    //        return false;
    //    }
    //}
    //if ($("#item_ConLai").val() != undefined && parseInt($("#item_ConLai").val().split(",").join("")) < 0) {
    //    toastWarning("Vui lòng kiểm tra lại thông tin thanh toán.");
    //    return false;
    //}
    //if ($("#Item_ConLai").val() != undefined && parseInt($("#Item_ConLai").val().split(",").join("")) < 0) {
    //    toastWarning("Vui lòng kiểm tra lại thông tin thanh toán.");
    //    return false;
    //}

    if ($("#type").val() != undefined && $("#type").val() == "NGCNG") {
        if (parseInt($(".Total").text()) != parseInt($(".SoTienNo").val())) {
            toastWarning("Chi gia công ngoài cần chi 100%.");
            return false;
        }
    }

    if (($("#Item_TongCong").val() != undefined
        && $(".TotalInput").text() != undefined
         && $(".TotalInput").text() != ""
        && parseInt($("#Item_TongCong").val().split(",").join("")) != parseInt($(".TotalInput").text().split(",").join("")))
    ) {
        toastWarning("Tổng tiền phải bằng tổng tiền nhập kho.");
        $("#Item_TongCong").focus();
        return false;
    }

    //trúc khóa ngày 5/1
    //if ($("#item_KeToanTruong").val() != undefined && $("#item_KeToanTruong").val() == "") {
    //    toastWarning("Vui lòng chọn Kế toán trưởng.");
    //    $("#item_KeToanTruong").focus();
    //    return false;
    //}
    //if ($("#item_NguoiLapPhieu").val() != undefined && $("#item_NguoiLapPhieu").val() == "") {
    //    toastWarning("Vui lòng chọn Người lập phiếu.");
    //    $("#item_NguoiLapPhieu").focus();
    //    return false;
    //}
    //if ($("#item_NguoiNopTien").val() != undefined && $("#item_NguoiNopTien").val() == "") {
    //    toastWarning("Vui lòng chọn Người nộp tiền.");
    //    $("#item_NguoiNopTien").focus();
    //    return false;
    //}
    //if ($("#item_NguoiThuTien").val() != undefined && $("#item_NguoiThuTien").val() == "") {
    //    toastWarning("Vui lòng chọn người thu tiền.");
    //    $("#item_NguoiThuTien").focus();
    //    return false;
    //}

    //trúc khóa ngày 5/1
    //if ($("#Item_KeToanTruong").val() != undefined && $("#Item_KeToanTruong").val() == "") {
    //    toastWarning("Vui lòng chọn Kế toán trưởng.");
    //    $("#Item_KeToanTruong").focus();
    //    return false;
    //}
    //if ($("#Item_NguoiLapPhieu").val() != undefined && $("#Item_NguoiLapPhieu").val() == "") {
    //    toastWarning("Vui lòng chọn Người lập phiếu.");
    //    $("#Item_NguoiLapPhieu").focus();
    //    return false;
    //}
    //if ($("#Item_NguoiNopTien").val() != undefined && $("#Item_NguoiNopTien").val() == "") {
    //    toastWarning("Vui lòng chọn Người nộp tiền.");
    //    $("#Item_NguoiNopTien").focus();
    //    return false;
    //}
    //if ($("#Item_NguoiThuTien").val() != undefined && $("#Item_NguoiThuTien").val() == "") {
    //    toastWarning("Vui lòng chọn người thu tiền.");
    //    $("#Item_NguoiThuTien").focus();
    //    return false;
    //}

    var lst = $(".number");
    lst.map(function (key, val) {
        $(val).val($(val).val().split(",").join(""));
    });
    return true;
}
$(".FormPhieuChi").submit(function () {
    Cal();
    debugger;
    //CalThanhToan(a);
    calTotal();
    return CheckValidatePhieuChi();
    //alert(b);

});
function convertJsonDateTimeToJs(input) {
    try {
        if (input.indexOf("Date") !== -1) {
            var resultDate = new Date(+input.replace(/\/Date\((-?\d+)\)\//gi, "$1"));
            return resultDate;
        } else {
            return input;
        }
    } catch (e) {
        return input;
    }
}
function TinhConLai() {
    debugger
    var total = parseInt($("#TongCongThanhToan").text().split(",").join(""));
    var Coc = parseInt($("#TongSoTienChi").text().split(",").join(""));
    var buyprice = $("#item_GiaVon").val() == "" ? 0 : parseInt($("#item_GiaVon").val().split(",").join(""));
    var rest = buyprice - (total + Coc);
    $("#item_ConLai").val(rest.toLocaleString('en-us'));
}
function formatDateToString(date, format) {

    if (date != null && date != undefined) {
        if ("undefined" === typeof date.getDate) {
            date = Date.parse(date);
            if ("undefined" === typeof date.getDate) {
                return '';
            }
        }

        var dd = date.getDate();
        if (dd < 10)
            dd = "0" + dd;
        var mm = date.getMonth() + 1;
        if (mm < 10)
            mm = "0" + mm;
        var y = date.getFullYear();
        var hours = date.getHours();
        var minute = date.getMinutes();
        var second = date.getSeconds();
        switch (format) {
            case "dd/mm/yyyy": return dd + "/" + mm + "/" + y; break;
            case "dd/mm/yyyy h:m:s": return dd + "/" + mm + "/" + y + ' ' + hours + ':' + minute + ':' + second; break;
            case "mm/dd/yyyy": return mm + "/" + dd + "/" + y; break;
            case "mm/dd/yyyy h:m:s": return mm + "/" + dd + "/" + y + ' ' + hours + ':' + minute + ':' + second; break;
            case "yyyy-mm-dd": return y + "-" + mm + "-" + dd; break;
            case "yyyy/mm/dd h:m:s": return y + "-" + mm + "-" + dd + ' ' + hours + ':' + minute + ':' + second; break;
            default: return mm + "/" + dd + "/" + y; break;
        }
    }
    else
        return '';
}