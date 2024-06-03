const tables = document.querySelectorAll('table');
const body = document.querySelector('body');
const imgSvg = document.querySelectorAll('header img, header svg :not(.login-form i svg)');    // Get the label elements inside the donor-container class
const labels = document.querySelectorAll('.donor-container label');
const links = document.querySelectorAll('a:not(.btn-design-outline)');
const accordion = document.querySelectorAll('.accordion-button');
const dropdownMenu = document.querySelectorAll('.dropdown-menu');
const cards = document.querySelectorAll('.card');

let toggleImg = document.getElementById('themeToggle');


function themeToggler() {


    body.classList.toggle('bg-dark');
    body.classList.toggle('text-white');

    accordion.forEach((ad) => {

        ad.classList.toggle('bg-dark');
        ad.classList.toggle('text-white');
    });

    dropdownMenu.forEach((ad) => {

        ad.classList.toggle('bg-dark');
        ad.classList.toggle('text-white');
    });

   tables.forEach((ele) => {
        ele.classList.toggle('table-dark');
    });

    cards.forEach((ele) => {
        ele.classList.toggle('text-white');
    });


    if (body.classList.contains('bg-dark')) {
        toggleImg.src = "/img/svg/light-mode-toggle-icon.png";

        imgSvg.forEach((a) => {
            a.style.filter = 'invert(1)'
        });

        // Loop through each label and set its color to white
        labels.forEach((label) => {
            label.style.color = 'white';
        });

        links.forEach((h3) => {
            h3.style.color = 'white';
        });

        setCookie('theme', 'dark');
    } else {
        toggleImg.src = "/img/svg/dark-mode-toggle-icon.png";

        imgSvg.forEach((a) => {
            a.style.filter = 'invert(0)'
        });

        // Loop through each label and set its color to white
        labels.forEach((label) => {
            label.style.color = 'black';
        });

        links.forEach((h3) => {
            h3.style.color = 'black';
        });

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
        body.classList.add('bg-dark');
        body.classList.add('text-white');

        toggleImg.src = "/img/svg/light-mode-toggle-icon.png";

        imgSvg.forEach((a) => {
            a.style.filter = 'invert(1)'
        });

        // Loop through each label and set its color to white
        labels.forEach((label) => {
            label.style.color = 'white';
        });

        links.forEach((h3) => {
           h3.style.color = 'white';
        });

        accordion.forEach((ad) => {
            ad.classList.toggle('bg-dark');
            ad.classList.toggle('text-white');
        });

        dropdownMenu.forEach((ad) => {
            ad.classList.toggle('bg-dark');
            ad.classList.toggle('text-white');
        });

        tables.forEach((ele) => {
            ele.classList.toggle('table-dark');
        });

        cards.forEach((ele) => {
            ele.classList.toggle('text-white');
        });

    } else if (theme === 'light') {
        toggleImg.src = "/img/svg/dark-mode-toggle-icon.png";

        imgSvg.forEach((a) => {
            a.style.filter = 'invert(0)'
        });

        // Loop through each label and set its color to white
        labels.forEach((label) => {
            label.style.color = 'black';
        });

        links.forEach((h3) => {
            h3.style.color = 'black';
        });
    }
};

document.getElementById("themeToggle").addEventListener("click", themeToggler);