// Search filter
const filteMissionNavbar = document.getElementById("filter-mission-navbar");
const searchButton = document.getElementById("search-button");
const missionContent = document.getElementById("mission-content");
let missionContentHeight = missionContent.style.height;

console.log("working");

function getStyle(element) {
    if (typeof getComputedStyle !== "undefined") {
        return getComputedStyle(element);
    }
    return element.currentStyle; // Old IE
}

var heightStyle = getStyle(missionContent).height;
heightStyle = +heightStyle.slice(0, -2) + 78;


let flag = 0;
let flagSideBar = 0;

searchButton.addEventListener("click", (e) => {
    e.preventDefault();
    if (flag % 2 == 0) {
        filteMissionNavbar.setAttribute('style', 'display:block !important');
        missionContent.setAttribute('style', 'height:' + (heightStyle - 79) + 'px;');
    }
    else {
        filteMissionNavbar.setAttribute('style', 'display:none !important');
        missionContent.setAttribute('style', 'height:' + heightStyle + 'px;');
    }
    flag++;
});

function myFunction(x) {
    if (x.matches) { // If media query matches
        filteMissionNavbar.setAttribute('style', 'display:block !important');
        missionContent.setAttribute('style', 'height:' + (heightStyle - 79) + 'px;');


    } else {
        filteMissionNavbar.setAttribute('style', 'display:none !important');
        missionContent.setAttribute('style', 'height:' + heightStyle + 'px;');

    }
}

var x = window.matchMedia("(min-width: 576px)")
myFunction(x) // Call listener function at run time
x.addListener(myFunction) // Attach listener function on state changes

// =================================================================================
// menu-sidebar
// =================================================================================
const menuImg = document.getElementById("menuimg");
const sideBar = document.getElementById("menu-side-bar");
const closeImg = document.getElementById("close-img");
const filterSideBar = document.getElementById("filter-side-bar");

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
        filterSideBar.setAttribute('style', 'display:none !important');
    }
}

let y = window.matchMedia("(min-width: 768px)")
myFunction2(y) // Call listener function at run time
y.addListener(myFunction2) // Attach listener function on state changes

// =====================================================================================================
// filter - button
// =====================================================================================================
const filterImg = document.getElementById("filter-image");
const filtercloseImg = document.getElementById("filter-img-close");

let flagFilter = 0;

filterImg.addEventListener("click", (e) => {
    e.preventDefault();
    if (flagFilter % 2 == 0) {
        filterSideBar.setAttribute('style', 'left:0px !important');
    }
    flagFilter++;
});

filtercloseImg.addEventListener("click", (e) => {
    e.preventDefault();
    if (flagFilter % 2 != 0) {
        filterSideBar.setAttribute('style', 'left:-50vw !important');
    }
    flagFilter++;
});
// ====================================================================
// chips items
// ====================================================================
let filterCheckItems = document.querySelectorAll(".filter-check-item");

for (let i = 0; i < filterCheckItems.length; i++) {
    filterCheckItems[i].addEventListener("click", (e) => {
        e.preventDefault();
    });

}

function demo() {
    console.log("hello");
}

// ====================================================================
// Toggle List and Grid view
// ====================================================================
const missionListView = document.getElementById("mission-list");
const missionGridView = document.getElementById("mission-grid");
const gridViewBtn = document.getElementById("grid-view-btn");
const listViewBtn = document.getElementById("list-view-btn");

listViewBtn.addEventListener("click", (e) => {
    e.preventDefault();
    listViewBtn.setAttribute("style", "background-color:#dee2e6 !important;");
    gridViewBtn.setAttribute("style", "background-color:white !important;");
    missionListView.setAttribute("style", "display:block !important;");
    missionGridView.setAttribute("style", "display:none !important;");
});
gridViewBtn.addEventListener("click", (e) => {
    e.preventDefault();
    listViewBtn.setAttribute("style", "background-color:white !important;");
    gridViewBtn.setAttribute("style", "background-color:#dee2e6 !important;");
    missionListView.setAttribute("style", "display:none !important;");
    missionGridView.setAttribute("style", "display:block !important;");
});


// ====================================================================
// Change Heart color
// ====================================================================
const imgHearts = document.querySelectorAll(".img-heart");
for (let i = 0; i < imgHearts.length; i++) {
    imgHearts[i].setAttribute("style", "backgrond-color:black");
    imgHearts[i].addEventListener("click", (e) => {
        e.preventDefault();
    });
}