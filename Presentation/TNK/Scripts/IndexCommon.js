
$('.date').datepicker({
    orientation: "left",
    autoclose: true
});
$('.date1').datepicker({
    orientation: "left",
    autoclose: true
});
$(".number2").map(function (key, val) {
    var val = $(this).html();
    if (val != "") {
        val = val.split(",").join("");
        val = parseInt(val);
        $(this).html(val.toLocaleString('en-us'));
    }
});
    
   