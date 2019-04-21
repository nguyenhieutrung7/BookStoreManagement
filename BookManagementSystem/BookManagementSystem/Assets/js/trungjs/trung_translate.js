function googleTranslateElementInit() {
    var currentLang = $('#lang-google-translate').text();
    if (currentLang === 'vi') {
        console.log('language:vi');
    }
    else {
        new Promise(function (resolve, reject) {
            new google.translate.TranslateElement({
                pageLanguage: 'vi', includedLanguages: currentLang,
                layout: google.translate.TranslateElement.FloatPosition.TOP_LEFT
            }, 'google_translate_element')
            resolve();
        }).then(function () {
            var select = document.getElementsByClassName('goog-te-combo')[0];
            select.selectedIndex = 1;
            select.addEventListener('click', function () {
                select.dispatchEvent(new Event('change'));
            });
            select.click();
            setTimeout(select.click(), 250);
        });

    }
}