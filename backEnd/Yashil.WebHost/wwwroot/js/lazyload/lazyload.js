function lazyLoad() {
    const lazyImages = document.querySelectorAll('.lazy-img');
    const callback = function (entries, observer) {
        entries.forEach(entry => {
            if (!entry.isIntersecting) return;
            entry.target.src = entry.target.dataset.src;
            observer.unobserve(entry.target);
        })
    }

    const observer = new IntersectionObserver(callback, {
        root: null,
        threshold: 0.2
    })

    lazyImages.forEach(lazyImage => observer.observe(lazyImage));
}

function homePageSliderLazyLoad() {
    const lazyImages = document.querySelectorAll('.lazy-source');
    const callback = function (entries, observer) {
        entries.forEach(entry => {
            if (!entry.isIntersecting) return;
            const spinnerElement = entry.target.previousElementSibling;
            entry.target.srcset = entry.target.dataset.srcset;
            observer.unobserve(entry.target);
            spinnerElement.classList.add('d-none');
        })
    }

    const observer = new IntersectionObserver(callback, {
        root: null,
        threshold: 0.2
    })

    lazyImages.forEach(lazyImage => observer.observe(lazyImage));
}