$("td.number").map(function (key, val) {
    var val = $(this).html();
    val = val.replace(",", ".");
    val = val.split(",").join("");
    val = parseFloat(val);
    $(this).html(val.toLocaleString('en-us'));
})

