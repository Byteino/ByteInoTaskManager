if (
    document.getElementById("loginForm") ||
    document.getElementById("registerForm")
) {
    function switchForm(formId) {
        document.getElementById("loginForm").classList.add("hide");
        document.getElementById("registerForm").classList.add("hide");
        document.getElementById(formId).classList.remove("hide");
    }
}

