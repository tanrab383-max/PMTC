function toast(type, content, title) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-top-right",
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    var $toast = toastr[type](content, title);
}
function toastSuccess(content) {
    toast("success", content, "Thông báo");
}
function toastError(content) {
    toast("error", content, "Thông báo");
}
function toastWarning(content) {
    toast("warning", content, "Thông báo");
}
