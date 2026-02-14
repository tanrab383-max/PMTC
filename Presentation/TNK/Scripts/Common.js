$(window).keydown(function (e) {
    //Xu ly control_s: submit form
    if (e.ctrlKey && e.keyCode === 83) {
        $("form").submit();
        return false;
    }

})