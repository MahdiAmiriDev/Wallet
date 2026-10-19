const shrinkText = function (text, limit) {
    const allTitles = document.querySelectorAll(text);
    allTitles.forEach(title => {
        title.innerText.length > limit ? title.innerText = title.innerText.substring(0, limit) + '...' : ''
    })
}