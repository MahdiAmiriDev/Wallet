export class Pagination {
    /**
     * @param {HTMLElement} tableWrapper - the wrapper in which the content should be listed
     * @param {HTMLElement} paginationWrapper - the wrapper where the page numbering is inserted
     * @param {Number} firstSite - which page should be displayed first, default "1"
     * @param {Number} rowsPerPage - how many rows to display per page, default "10"
     */
    constructor(tableWrapper, paginationWrapper, firstSite = 1, rowsPerPage = 10) {
        this.table = tableWrapper;
        this.paginationWrapper = paginationWrapper;
        this.firstSiteIndex = Number(firstSite) - 1;
        this.rowsPerPage = rowsPerPage;
        this.pageArray = [];
        this.currentSiteNumberIndex;

        this.startPagination();
    }

    startPagination() {
        this.pageArray = this.getPageArray();
        if (this.pageArray.length <= this.rowsPerPage) return;

        this.paginationWrapper.classList.add('pagination-number-wrapper');

        this.buildPagination();
        this.setSite(this.firstSiteIndex);
    }

    getPageArray(containsClass = null) {
        const rows = this.table.children;
        let pageArray = [];
        let siteArray = [];
        let site = 0;

        for (let i = 0; i < rows.length; i++) {
            if (containsClass) {
                if (!rows[i].classList.contains(containsClass)) {
                    ({ siteArray, pageArray, site } = this.setPageArray(siteArray, pageArray, rows[i], site));
                }
            } else {
                ({ siteArray, pageArray, site } = this.setPageArray(siteArray, pageArray, rows[i], site));
            }
        }
        return pageArray;
    }

    setPageArray(siteArray, pageArray, row, site) {
        siteArray.push(row);

        if (siteArray.length < this.rowsPerPage) {
            pageArray[site] = siteArray;
        }

        if (siteArray.length === this.rowsPerPage) {
            pageArray[site] = siteArray;
            siteArray = [];
            site += 1;
        }

        return { siteArray, pageArray, site };
    }

    buildPagination() {
        const firstPageButton = document.createElement('button');
        firstPageButton.innerHTML = this.generateIcons('chevron-right-pipe');
        firstPageButton.classList.add('pagination-button', 'first-page--button');
        firstPageButton.title = 'صفحه ابتدایی';
        this.setOpenFirstPageEvent(firstPageButton);
        this.paginationWrapper.append(firstPageButton);

        const previousPageButton = document.createElement('button');
        previousPageButton.innerHTML = this.generateIcons('chevron-right');
        previousPageButton.classList.add('pagination-button', 'previous-page--button');
        previousPageButton.title = 'صفحه قبل';
        this.setOpenPreviousPageEvent(previousPageButton);
        this.paginationWrapper.append(previousPageButton);

        const numberWrapper = document.createElement('div');
        numberWrapper.classList.add('pagination-site-number-wrapper');
        for (let key of this.pageArray.keys()) {
            const siteNumber = key + 1;
            const siteButton = document.createElement('button');
            siteButton.innerHTML = siteNumber;
            siteButton.setAttribute('data-array-index', key);
            siteButton.classList.add(`site-button-${key}`);
            const firstSiteNumberIndex = this.firstSiteIndex;
            if (firstSiteNumberIndex === key) siteButton.classList.add('active');
            this.setOpenSiteEvent(siteButton);
            if (key === this.pageArray.length - 1 && this.pageArray.length >= 4)
                this.buildPaginationPlaceholder(numberWrapper, 'second-pagination-placeholder');
            numberWrapper.append(siteButton);
            if (key === 0 && this.pageArray.length >= 4)
                this.buildPaginationPlaceholder(numberWrapper, 'first-pagination-placeholder');
        }
        this.paginationWrapper.append(numberWrapper);

        const nextPageButton = document.createElement('button');
        nextPageButton.innerHTML = this.generateIcons('chevron-left');
        nextPageButton.classList.add('pagination-button', 'next-page--button');
        nextPageButton.title = 'صفحه بعد';
        this.setOpenNextPageEvent(nextPageButton);
        this.paginationWrapper.append(nextPageButton);

        const lastPageButton = document.createElement('button');
        lastPageButton.innerHTML = this.generateIcons('chevron-left-pipe');
        lastPageButton.classList.add('pagination-button', 'last-page--button');
        lastPageButton.title = 'صفحه آخر';
        this.setOpenLastPageButton(lastPageButton);
        this.paginationWrapper.append(lastPageButton);
        this.togglePaginationButtons('beginning', true);
    }

    generateIcons(icon) {
        switch (icon) {
            case 'chevron-right':
                return `
                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" stroke-width="1.6" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                  <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                  <path d="M9 6l6 6l-6 6" />
                </svg>`;
            case 'chevron-left':
                return `
                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" stroke-width="1.6" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                  <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                  <path d="M15 6l-6 6l6 6" />
                </svg>`;
            case 'chevron-right-pipe':
                return `
                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" stroke-width="1.6" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                  <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                  <path d="M6 6l6 6l-6 6" />
                  <path d="M17 5v13" />
                </svg>`;
            case 'chevron-left-pipe':
                return `
                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" stroke-width="1.6" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                  <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                  <path d="M7 6v12" />
                  <path d="M18 6l-6 6l6 6" />
                </svg>`;
        }
    }

    buildPaginationPlaceholder(numberWrapper, className) {
        const pointsPlaceholderElement = document.createElement('div');
        pointsPlaceholderElement.innerHTML = '...';
        pointsPlaceholderElement.classList.add('pagination-placeholder', className);
        numberWrapper.append(pointsPlaceholderElement);
    }

    setOpenSiteEvent(button) {
        button.addEventListener('pointerdown', () => {
            this.setSite(button.getAttribute('data-array-index'));
            this.setButtonStatus();
        });
    }

    setOpenFirstPageEvent(button) {
        button.addEventListener('pointerdown', () => {
            if (this.currentSiteNumberIndex === 0) return;

            this.setSite(0);
            this.setButtonStatus(0);
        });
    }

    setOpenPreviousPageEvent(button) {
        button.addEventListener('pointerdown', () => {
            if (this.currentSiteNumberIndex === 0) return;

            const siteNumberIndex = this.currentSiteNumberIndex - 1;

            this.setSite(siteNumberIndex);
            this.setButtonStatus();
        });
    }

    setOpenNextPageEvent(button) {
        button.addEventListener('pointerdown', () => {
            if (this.currentSiteNumberIndex === this.pageArray.length - 1) return;

            const siteNumberIndex = this.currentSiteNumberIndex + 1;
            this.setSite(siteNumberIndex);
            this.setButtonStatus();
        });
    }

    setOpenLastPageButton(button) {
        button.addEventListener('pointerdown', () => {
            const lastArrayKey = this.pageArray.length - 1;
            if (this.currentSiteNumberIndex === lastArrayKey) return;
            this.setSite(lastArrayKey);
            this.setButtonStatus();
        });
    }

    setSite(siteNumberIndex) {
        this.currentSiteNumberIndex = Number(siteNumberIndex);
        for (let key of this.pageArray.keys()) this.setSiteActivity(key);
        this.displayPaginations();
    }

    setSiteActivity(siteNumberIndex) {
        this.pageArray[siteNumberIndex].forEach((row) => {
            if (siteNumberIndex === this.currentSiteNumberIndex) {
                row.classList.remove('pagination-inactive');
                return;
            }
            row.classList.add('pagination-inactive');
        });
    }

    setButtonStatus() {
        const buttons = this.paginationWrapper.querySelector('.pagination-site-number-wrapper').getElementsByTagName('button');

        Array.from(buttons).forEach((button) => {
            if (Number(button.getAttribute('data-array-index')) === Number(this.currentSiteNumberIndex)) {
                button.classList.add('active');
                if (Number(this.currentSiteNumberIndex) === 0) {
                    this.togglePaginationButtons('beginning', true);
                    this.togglePaginationButtons('end', false);
                }
                if (Number(this.currentSiteNumberIndex) === this.pageArray.length - 1) {
                    this.togglePaginationButtons('end', true);
                    this.togglePaginationButtons('beginning', false);
                }
                if (Number(this.currentSiteNumberIndex) < this.pageArray.length - 1 && Number(this.currentSiteNumberIndex) > 0) {
                    this.togglePaginationButtons('beginning', false);
                    this.togglePaginationButtons('end', false);
                }
                return;
            }
            button.classList.remove('active');
        });
    }

    togglePaginationButtons(position, isDisabled) {
        const firstPageButtonElement = document.querySelector('.first-page--button');
        const previousPageButtonElement = document.querySelector('.previous-page--button');
        const lastPageButtonElement = document.querySelector('.last-page--button');
        const nextPageButtonElement = document.querySelector('.next-page--button');
        if (position === 'beginning' && isDisabled) {
            firstPageButtonElement.disabled = true;
            previousPageButtonElement.disabled = true;
        } else if (position === 'beginning') {
            firstPageButtonElement.disabled = false;
            previousPageButtonElement.disabled = false;
        }
        if (position === 'end' && isDisabled) {
            lastPageButtonElement.disabled = true;
            nextPageButtonElement.disabled = true;
        } else if (position === 'end') {
            lastPageButtonElement.disabled = false;
            nextPageButtonElement.disabled = false;
        }
    }

    displayPaginations() {
        const lastPageIndex = this.pageArray.length - 1;
        const buttons = this.paginationWrapper.querySelector('.pagination-site-number-wrapper').getElementsByTagName('button');

        for (var i = 0; i < buttons.length; i++) {
            buttons[i].classList.add('pagination-button-not-displayed');
            if (i === 0 || i === lastPageIndex) buttons[i].classList.remove('pagination-button-not-displayed');
        }

        if (this.paginationWrapper.querySelector(`.site-button-${this.currentSiteNumberIndex - 1}`))
            this.paginationWrapper
                .querySelector(`.site-button-${this.currentSiteNumberIndex - 1}`)
                .classList.remove('pagination-button-not-displayed');
        if (this.currentSiteNumberIndex === lastPageIndex) {
            if (this.paginationWrapper.querySelector(`.site-button-${this.currentSiteNumberIndex - 2}`))
                this.paginationWrapper
                    .querySelector(`.site-button-${this.currentSiteNumberIndex - 2}`)
                    .classList.remove('pagination-button-not-displayed');
            if (this.paginationWrapper.querySelector(`.site-button-${this.currentSiteNumberIndex - 3}`))
                this.paginationWrapper
                    .querySelector(`.site-button-${this.currentSiteNumberIndex - 3}`)
                    .classList.remove('pagination-button-not-displayed');
        }
        if (this.currentSiteNumberIndex === lastPageIndex - 1) {
            if (this.paginationWrapper.querySelector(`.site-button-${this.currentSiteNumberIndex - 2}`))
                this.paginationWrapper
                    .querySelector(`.site-button-${this.currentSiteNumberIndex - 2}`)
                    .classList.remove('pagination-button-not-displayed');
        }

        if (this.paginationWrapper.querySelector(`.site-button-${this.currentSiteNumberIndex}`))
            this.paginationWrapper
                .querySelector(`.site-button-${this.currentSiteNumberIndex}`)
                .classList.remove('pagination-button-not-displayed');

        if (this.paginationWrapper.querySelector(`.site-button-${this.currentSiteNumberIndex + 1}`))
            this.paginationWrapper
                .querySelector(`.site-button-${this.currentSiteNumberIndex + 1}`)
                .classList.remove('pagination-button-not-displayed');
        if (this.currentSiteNumberIndex === 0) {
            if (this.paginationWrapper.querySelector(`.site-button-${this.currentSiteNumberIndex + 2}`))
                this.paginationWrapper
                    .querySelector(`.site-button-${this.currentSiteNumberIndex + 2}`)
                    .classList.remove('pagination-button-not-displayed');
            if (this.paginationWrapper.querySelector(`.site-button-${this.currentSiteNumberIndex + 3}`))
                this.paginationWrapper
                    .querySelector(`.site-button-${this.currentSiteNumberIndex + 3}`)
                    .classList.remove('pagination-button-not-displayed');
        }
        if (this.currentSiteNumberIndex === 1) {
            if (this.paginationWrapper.querySelector(`.site-button-${this.currentSiteNumberIndex + 2}`))
                this.paginationWrapper
                    .querySelector(`.site-button-${this.currentSiteNumberIndex + 2}`)
                    .classList.remove('pagination-button-not-displayed');
        }

        this.setPlaceholder();
    }

    setPlaceholder() {
        if (this.pageArray.length >= 6 && this.currentSiteNumberIndex >= 3 && this.paginationWrapper.querySelector('.first-pagination-placeholder')) {
            this.paginationWrapper.querySelector('.first-pagination-placeholder').classList.add('active');
        } else if (this.paginationWrapper.querySelector('.first-pagination-placeholder')) {
            this.paginationWrapper.querySelector('.first-pagination-placeholder').classList.remove('active');
        }

        const siteNumber = this.currentSiteNumberIndex + 1;
        let currentToLastDifference;
        if (this.pageArray.length >= 6) currentToLastDifference = this.pageArray.length - siteNumber;
        if (
            currentToLastDifference &&
            currentToLastDifference >= 3 &&
            this.paginationWrapper.querySelector('.second-pagination-placeholder')
        ) {
            this.paginationWrapper.querySelector('.second-pagination-placeholder').classList.add('active');
        } else if (this.paginationWrapper.querySelector('.second-pagination-placeholder')) {
            this.paginationWrapper.querySelector('.second-pagination-placeholder').classList.remove('active');
        }
    }

    /**
     * @description reindex the pagiantion
     * @param {String} containsClass - optional class for aditional filter
     */
    reindexPagination(containsClass = null) {
        this.pageArray = this.getPageArray(containsClass);

        this.paginationWrapper.innerHTML = '';
        if (this.pageArray.length === 0) return;

        this.buildPagination();
        this.setSite(this.firstSiteIndex);
    }
}