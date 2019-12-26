// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable tr").filter(function () {
            console.log($(this).parent());
            $(this).parents().eq(3).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});
