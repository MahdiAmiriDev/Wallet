
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


