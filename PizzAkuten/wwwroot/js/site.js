// Write your JavaScript code.
$(".paymentMethod").click(function () {
    if ($('#master').is(':checked')) {
        $("#creditCardForm").removeClass("hidden");
    }
    if ($('#visa').is(':checked')) {
        $("#creditCardForm").removeClass("hidden");
    }
    if ($('#swish').is(':checked')) {
        $("#creditCardForm").addClass("hidden");
    }
});



