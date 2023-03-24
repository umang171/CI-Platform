

// =================================================================================
// menu-sidebar
// =================================================================================
const menuImg = document.getElementById("menuimg");
const sideBar = document.getElementById("menu-side-bar");
const closeImg = document.getElementById("close-img");

menuImg.addEventListener("click", (e) => {
    e.preventDefault();
    if (flagSideBar % 2 == 0) {
        sideBar.setAttribute('style', 'left:0px !important');
    }
    flagSideBar++;
});

closeImg.addEventListener("click", (e) => {
    e.preventDefault();
    if (flagSideBar % 2 != 0) {
        sideBar.setAttribute('style', 'left:-50vw !important');
    }
    flagSideBar++;
});

function myFunction2(y) {
    if (y.matches) { // If media query matches
        sideBar.setAttribute('style', 'display:none !important');
    }
}

let y = window.matchMedia("(min-width: 768px)")
myFunction2(y) // Call listener function at run time
y.addListener(myFunction2) // Attach listener function on state changes

//=================================================================================================
//ShareStory
//=================================================================================================
$(document).ready(function () {
});
//=================================================================================================
//Texteditor of tinymce
//=================================================================================================
var init = tinymce.init({
    selector: 'textarea#default',
    height: 250,
    menubar: false,
    branding: false,
    toolbar: 'bold italic strikethrough | subscript superscript | removeformat ',
    content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px }'
});

var storyDescription = "";
$.when(init).done(function () {
    storyDescription = tinyMCE.activeEditor.getContent();
    console.log(storyDescription);
});

//=================================================================================================
//Texteditor of tinymce
//=================================================================================================
