alert(2);
var tr = null;
try {
    $("#tr").html();
} catch (e)
{
    alert(1);
}
var type = "";
tr = tr.replace("checked", "");
tr = tr.replace("true", "false");
tr = tr.replace("True", "false");
$('.select2').select2({
    placeholder: "Select an option",
    allowClear: true
});
removeSelect();
function removeSelect() {
    setTimeout(function () {
        var a = $('select.select2');
        a.map(function (key, val) {
            $(val).removeClass("select2");
        });
    }, 100);
}
$('.date').datepicker({
    orientation: "left",
    autoclose: true
});
type = "";
$("#form-data").submit(function () {
    if (type == "ThuNo") {
        let number = parseInt($("#ConLai").val().split(",").join(""));
        if (number < 0) {
            toastError("Số tiền thanh toán không được lớn hơn số tiền còn nợ");
            return false;
        }
    }
    
    var lst = $(".number");
    lst.map(function (key, val) {
        $(val).val($(val).val().split(",").join(""));
    });
})
function Remove(a) {
    var tr = $(a.closest("tr"));
    tr.find(".IsDeleted").val("true");
    tr.html(tr.html().split("required").join(""));
    tr.hide();
}

$(".number").map(function (key, val) {
    var val = $(this).val();
    if (val != "") {
        val = val.split(",").join("");
        val = parseInt(val);
        $(this).val(val.toLocaleString('en-us'));
    }
})
$(document).on("click", ".chkTinhTrang", function () {
    $(this).closest("td").children(".TinhTrang").val($(this).is(":checked"));
})
$(document).on("keypress", ".date", function () {
    return false;
});
$(".Total").html(parseInt($(".Total").html()).toLocaleString('en-us'));
$(document).on("change", ".number", function () {
    var val = $(this).val();
    if (val != "") {
        val = val.split(",").join("");
        val = parseInt(val);
        //val = val * 1000;
        $(this).val(val.toLocaleString('en-us'));
    }
    else {
        $(this).val(0);
    }
    let total = 0;
    $(".number").map(function (key, val) {
        let number = $(this).val();
        if (number != "") {
            number = number.split(",").join("");
            number = parseInt(number);
            total += number;
            $(".Total").html(total.toLocaleString('en-us'));
        }
    });
});
$(document).on("change", ".number2", function () {
    var val = $(this).val();
    if (val != "") {
        val = val.split(",").join("");
        val = parseInt(val);
       // val = val * 1000;
        $(this).val(val.toLocaleString('en-us'));
    }
    else {
        $(this).val(0);
    }
});
//var substringMatcher = function () {
//    return function findMatches(q, cb) {
//        var matches;
//        // an array that will be populated with substring matches
//        matches = [];
//        var q = q.split(",").join("");
//        matches.push((q * 10).toLocaleString());
//        matches.push((q * 100).toLocaleString());
//        matches.push((q * 1000).toLocaleString());
//        cb(matches);
//    };
//};
//$('.number').typeahead({
//    hint: true,
//    highlight: true,
//    minLength: 1
//},
//{
//    name: 'states',
//    source: substringMatcher()
//});
$(document).on("keypress", ".number", function ($event) {
    return ($event.charCode >= 48 && $event.charCode <= 57) || $event.charCode == 0;
});
$(document).on("focus", ".number", function () {
    var tr = $(this).closest("tr");

    var temp = tr.find(".Temp").val();
});
$(document).on("keypress", ".Temp", function ($event) {
    if ($event.keyCode == 9) {
        $(".GhiChu").focus();
    }
});
$(document).on("keypress", ".SoThamChieu", function ($event) {
    if ($event.keyCode == 9)
        $(".addHTTT").click();
});