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

//==============================================================================
//Ajax calls
//==============================================================================
$(document).ready(function () {
    loadCountry();
    loadCity();
    loadTheme();
    loadSkill();
});
function loadCountry() {
    $.ajax({
        type: "GET",
        url: "/Mission/GetCountries",
        data: "{}",
        success: function (data) {
            var str = "";
            var countryDropDown = $(".countryDropDownList");
            for (var j = 0; j < data["data"].length; j++) {
                str += '<li class="p-1"><a class= "dropdown-item" href = "#" > <input type="checkbox" name="country"/> ' + data["data"][j].name + '</a></li>';
            }
            countryDropDown.append(str);
            intializeChips();
            
        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng");
        }

    });
}
function loadCity() {
    $.ajax({
        type: "GET",
        url: "/Mission/GetCites",
        data: "{}",
        success: function (data) {
            var str = "";
            var cityDropDown = $(".cityDropDownList");
            for (var j = 0; j < data["data"].length; j++) {
                str += '<li class="p-1"><a class= "dropdown-item" href = "#" > <input type="checkbox" name="country"/> ' + data["data"][j].name + '</a></li>';
            }
            cityDropDown.append(str);
            intializeChips();

        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng");
        }

    });
}
function loadTheme() {
    $.ajax({
        type: "GET",
        url: "/Mission/GetThemes",
        data: "{}",
        success: function (data) {
            var str = "";
            var themeDropDown = $(".themeDropDownList");
            for (var j = 0; j < data["data"].length; j++) {
                str += '<li class="p-1"><a class= "dropdown-item" href = "#" > <input type="checkbox" name="country"/> ' + data["data"][j].title + '</a></li>';
            }
            themeDropDown.append(str);
            intializeChips();

        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng");
        }

    });
}

function loadSkill() {
    $.ajax({
        type: "GET",
        url: "/Mission/GetSkills",
        data: "{}",
        success: function (data) {
            var str = "";
            console.log(data);
            var skillDropDown = $(".skillDropDownList");
            for (var j = 0; j < data["data"].length; j++) {
                str += '<li class="p-1"><a class= "dropdown-item" href = "#" > <input type="checkbox" name="country"/> ' + data["data"][j].skillName + '</a></li>';
            }
            skillDropDown.append(str);
            intializeChips();

        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng");
        }

    });
}








// ====================================================================
// chips items
// ====================================================================
$(".close-chips").on("click", function (e) {
    $(".home-chips .chip").remove();
    $(this).hide();
    $(".no-filter-text").show();
});
$(".filters .dropdown-menu li a").on("click", function (e) {
    console.log("hi");
    $(".home-chips .chips").append(
        '<div class="chip">' +
        $(this).text() +
        '<span class="closebtn" onclick="this.parentElement.style.display=\'none\'">&times;</span>'
    );
    $(".close-chips").show();
    $(".no-filter-text").hide();
    $(".close-chips").show();
});

function intializeChips() {
    $(".filters .dropdown-menu li a").on("click", function (e) {
        console.log("hi");
        $(".home-chips .chips").append(
            '<div class="chip">' +
            $(this).text() +
            '<span class="closebtn" onclick="this.parentElement.style.display=\'none\'">&times;</span>'
        );
        $(".close-chips").show();
        $(".no-filter-text").hide();
        $(".close-chips").show();
    });
}