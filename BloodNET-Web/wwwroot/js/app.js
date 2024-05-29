function themeToggler() {
    let table = document.getElementById('realtime-table');
    table.classList.toggle('table-dark');
    let toggleImg = document.getElementById('themeToggle');

    if (table.classList.contains('table-dark')) {
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
        document.getElementById('realtime-table').classList.add('table-dark');
        document.getElementById('themeToggle').src = "/img/svg/toggle-on.svg";
    } else if (theme === 'light') {
        document.getElementById('realtime-table').classList.remove('table-dark');
        document.getElementById('themeToggle').src = "/img/svg/toggle-off.svg";
    }
};

document.getElementById("themeToggle").addEventListener("click", themeToggler);