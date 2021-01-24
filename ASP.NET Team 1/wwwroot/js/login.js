// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.onload = function () {
    let errDiv = document.getElementById("err_msg");

    let form = document.getElementById("form");

    form.onsubmit = function () {
        let elemUname = document.getElementById("userId");
        let elemPwd = document.getElementById("password");

        let username = elemUname.value.trim();
        let password = elemPwd.value.trim();

        if (username.length === 0 || password.length === 0) {
            errDiv.innerHTML = "Please ensure both Username and Passwords are filled";
            return false;
        }
        return true;
    }

    let elems = document.getElementsByClassName("form-control");
    for (let i = 0; i < elems.length; i++) {
        elems[i].onfocus = function () { errDiv.innerHTML = "" };
    }

}




