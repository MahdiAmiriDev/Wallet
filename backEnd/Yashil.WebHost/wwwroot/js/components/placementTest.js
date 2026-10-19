
// code for Covers Slider placement comments
var swiper = new Swiper(".coversSliderplacement", {
    slidesPerView: 1,
    spaceBetween: 30,
    autoplay: {
        delay: 2500,
        disableOnInteraction: false,
    },
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        640: {
            slidesPerView: 1,
            spaceBetween: 20,
        },
        768: {
            slidesPerView: 1,
            spaceBetween: 40,
        },
        992: {
            slidesPerView: 2,
            spaceBetween: 40,
        },
        1024: {
            slidesPerView: 3,
            spaceBetween: 40,
        },
    },
});




// code for count number
$(function () {
    function count($this) {
        var current = parseInt($this.html(), 10);
        $this.html(++current);
        if (current !== $this.data('count')) {
            setTimeout(function () { count($this) }, 10);
        }

    }

    $(".statistics-numbers").each(function () {
        $(this).data('count', parseInt($(this).html(), 10));
        $(this).html('0');
        count($(this));
    });
    
  
});



//code for FAQ
var acc = document.getElementsByClassName("accordion");
var i;

for (i = 0; i < acc.length; i++) {
    acc[i].addEventListener("click", function () {





        var panel = this.nextElementSibling;
        this.classList.toggle("active");
        if (panel.style.maxHeight) {
            panel.style.maxHeight = null;

        } else {
            panel.style.maxHeight = panel.scrollHeight + "px";
        }
    });
}