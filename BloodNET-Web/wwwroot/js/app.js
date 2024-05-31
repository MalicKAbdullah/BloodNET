function themeToggler() {
    let body = document.querySelector('body');
    body.classList.toggle('bg-dark');
    body.classList.toggle('text-white');

    let toggleImg = document.getElementById('themeToggle');

    if (body.classList.contains('bg-dark')) {
        toggleImg.src = "/img/svg/toggle-on.svg";
        setCookie('theme', 'dark');
    } else {
        toggleImg.src = "/img/svg/toggle-off.svg";
        setCookie('theme', 'light');
    }
}

function setCookie(name, value) {
    document.cookie = name + "=" + (value || "") + "; path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

window.onload = function () {
    var theme = getCookie('theme');
    if (theme === 'dark') {
        document.querySelector('body').classList.add('bg-dark');
        document.querySelector('body').classList.add('text-white');

        document.getElementById('themeToggle').src = "/img/svg/toggle-on.svg";
    } else if (theme === 'light') {
        document.getElementById('themeToggle').src = "/img/svg/toggle-off.svg";
    }
};

document.getElementById("themeToggle").addEventListener("click", themeToggler);