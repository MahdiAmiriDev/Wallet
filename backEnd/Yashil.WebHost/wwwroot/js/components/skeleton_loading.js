/* https://dgknca.github.io/css-skeletons/ - Documentation For Skeleton Loader Classes */

function skeletonLoading() {
    const skeletonLoadingContainerElements = document.querySelectorAll('.skeleton-loading--container');

    const callback = function (entries, observer) {
        entries.forEach(entry => {
            if (!entry.isIntersecting) return;
            const skeletonContainerElement = entry.target;
            const imageContainerElement = skeletonContainerElement.parentElement.querySelector(('.skeleton-content--container'));
            const imageElements = imageContainerElement.querySelectorAll('img');
            for (let i = 0; i < imageElements.length; i++) {
                imageElements[i].src = imageElements[i].dataset.src;
            }
            setTimeout(() => {
                skeletonContainerElement.classList.add('d-none');
                imageContainerElement.classList.remove('d-none');
            }, 300)
            observer.unobserve(entry.target);
        })
    }

    const observer = new IntersectionObserver(callback, {
        root: null,
        threshold: 0.05
    })

    skeletonLoadingContainerElements.forEach(skeletonLoadingContainerElement => observer.observe(skeletonLoadingContainerElement));
}