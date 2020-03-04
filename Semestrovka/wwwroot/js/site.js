// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function validateComments(input) {
    if (input.value.length <4) {
        input.setCustomValidity("Дайте более развернутый ответ.");
    }
    else {
        // Длина комментария отвечает требованию, 
        // поэтому очищаем сообщение об ошибке
        input.setCustomValidity("");
    }
}