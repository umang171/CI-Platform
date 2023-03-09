// Search filter
const filteMissionNavbar = document.getElementById("filter-mission-navbar");
const searchButton = document.getElementById("search-button");
const missionContent = document.getElementById("mission-content");
let missionContentHeight = missionContent.style.height;


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
    //loadCountry();
    //loadCity();
    //loadTheme();
    //loadSkill();
    loadMissions();
});

function loadMissions() {
    $.ajax({
        type: "GET",
        url: "/Mission/getmissions",
        data: "{}",
        success: function (data) {
            var str = "";
            var str2 = "";
            var gridList = $("#mission-grid .row");
            var listCards = $("#mission-list .row");
            gridList.empty();
            listCards.empty();
            data=JSON.parse(data["data"]);
            for (var j = 0; j < data.length; j++) {
                str += ' <div id="card-parent" class="list-card-parent col-lg-4 col-md-6 col-sm-6 col-12 grid-item h-auto mb-4"><div class="card"><img class="card-img-top w-100 card-img" src="' + "/" + data[j].MissionMedia[0].MediaPath + "/" + data[j].MissionMedia[0].MediaName + "" + data[j].MissionMedia[0].MediaType + '" alt="Card image cap" /><div id="card-image-content" class="d-flex flex-column justify-content-between  p-2"><div id="location-image" class="img-content p-2 rounded-5"><img src="/images/pin.png" alt="">' + data[j].City.Name + '</div><div class="d-flex flex-column align-items-end"><div class="img-heart img-content p-2 rounded-5 m-1"><img src="/images/heart.png" alt=""></div><div id="user-img" class="img-content p-2 rounded-5 m-1"><img src="/images/user.png" alt=""></div></div></div><h5 class="card-theme-title bg-white rounded-5 p-2">' + data[j].Theme.Title + '</h5><div class="card-body"><h5 class="card-title fs-3">' + data[j].Title + '</h5><p class="card-text">' + data[j].ShortDescription + '...</p><div id="star-content" class="d-flex align-items-center justify-content-between"><p class="my-auto">' + data[j].OrganizationName + '</p><div class="d-flex align-items-center"><img src="/images/selected-star.png" alt=""><img src="/images/selected-star.png" alt=""><img src="/images/selected-star.png" alt=""><img src="/images/star.png" alt=""><img src="/images/star.png" alt=""></div></div></div><div id="mission-duration-div"><h5 id = "card-time-duration-title" class="border bg-white rounded-5 m-auto fs-6 p-2" > From ' + data[j].StartDate.substring(0, 10) + ' until ' + data[j].EndDate.substring(0, 10) + '</h5 ></div><div id="seats-info-card" class="d-flex border border-1 position-relative pt-3 align-items-center justify-content-around"><div id="remain-seats-cards" class=" w-50 d-flex align-items-center p-1 px-lg-4 px-md-1"><img id="seats-left-image" src="/images/Seats-left.png" alt=""><div class="d-flex flex-column p-2 "><p id="count" class="m-0 p-0 fs-5">10</p><p id="seats-left-text" class="m-0 p-0">Seats left</p></div></div><div id="deadline-info-card" class=" w-40 d-flex align-items-center p-1 px-lg-4 px-md-1"><img id="deadline-image" src="/images/deadline.png" alt=""><div class="d-flex flex-column p-2 "><p id="deadline-date" class="m-0 p-0 fs-5">19/11/2022</p><p id="deadline-text" class="m-0 p-0">Deadline</p></div></div></div><div class="card-button-apply m-1 p-1 w-100 text-center"><button id="apply-btn" class="btn btn-outline-warning rounded-5 py-2 px-4 fs-5 ">Apply<img src="/images/right-arrow.png" class="mx-1 ms-3" alt=""></button></div></div ></div > ';

                str2 += '<div id="list-card-parent" class="col-12 d-flex p-0 mb-4 border shadow"><div class="list-image-part position-relative"> <img src="'+"/"+data[j].MissionMedia[0].MediaPath+"/"+data[j].MissionMedia[0].MediaName+""+data[j].MissionMedia[0].MediaType+'" class="list-img" alt = "" /><h5 class="list-card-theme-title bg-white rounded-5 p-2">' + data[j].Theme.Title + '</h5><div id="list-card-image-content" class="h-100 d-flex flex-column position-absolute  p-2"><div id="list-location-image" class="list-img-content p-2 rounded-5"><img src="/images/pin.png" alt="">' + data[j].City.Name + '</div><div class="d-flex flex-column align-items-end"><div class="img-heart list-img-content p-2 rounded-5 m-1"><img src="/images/heart.png" alt=""></div><div id="list-user-img" class="list-img-content p-2 rounded-5 m-1"><img src="/images/user.png" alt=""></div></div></div></div><div class="list-content-part d-flex flex-column justify-content-around p-2"><h5 class="card-title fs-3">' + data[j].Title + '</h5><p class="card-text">' + data[j].ShortDescription + '...</p><div id="list-info-content" class="d-flex justify-content-between"><div><div id="list-star-content" class="d-flex align-items-center justify-content-between"><p class="my-auto">' + data[j].OrganizationName + '</p><div class="d-flex align-items-center"><img src="/images/selected-star.png" alt=""><img src="/images/selected-star.png" alt=""><img src="/images/selected-star.png" alt=""><img src="/images/star.png" alt=""><img src="/images/star.png" alt=""></div></div><div class="list-card-button-apply m-1 p-1 w-100 "><button id="list-apply-btn" class="btn btn-outline-warning rounded-5 py-2 px-4 fs-5 ">Apply<img src="/images/right-arrow.png" class="mx-1 ms-3" alt=""></button></div></div><div><div id="list-mission-duration-div"><h5 id="list-card-time-duration-title" class="border bg-white rounded-5 m-auto fs-6 p-2">From ' + data[j].StartDate.substring(0, 10) + ' until ' + data[j].EndDate.substring(0, 10) + '</h5></div><div id="list-seats-info-card" class="d-flex border-top border-1 position-relative pt-3 align-items-center justify-content-around"><div id="remain-seats-cards" class=" w-50 d-flex align-items-center p-1 px-lg-4 px-md-1"><img id="list-seats-left-image" src="/images/Seats-left.png" alt=""><div class="d-flex flex-column p-2 "><p id="count" class="m-0 p-0 fs-5">10</p><p id="list-seats-left-text" class="m-0 p-0">Seats left</p></div></div><div id="list-deadline-info-card" class=" w-40 d-flex align-items-center p-1 px-lg-4 px-md-1"><img id="list-deadline-image" src="/images/deadline.png" alt=""><div class="d-flex flex-column p-2 "><p id="list-deadline-date" class="m-0 p-0 fs-5">19/11/2022</p><p id="list-deadline-text" class="m-0 p-0">Deadline</p></div></div></div></div></div></div></div>';
                //console.log(data[j]);
                
            }
            gridList.append(str);
            listCards.append(str2);
        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng");
        }

    });
}
//var ajaxRequest1, ajaxRequest2, ajaxRequest3, ajaxRequest4;
var ajaxRequest1 =
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

        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng");
        }

    });

var ajaxRequest2
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

        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng");
        }

    });

var ajaxRequest3
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

        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng");
        }

    });


var ajaxRequest4 =
    $.ajax({
        type: "GET",
        url: "/Mission/GetSkills",
        data: "{}",
        success: function (data) {
            var str = "";
            var skillDropDown = $(".skillDropDownList");
            for (var j = 0; j < data["data"].length; j++) {
                str += '<li class="p-1"><a class= "dropdown-item" href = "#" > <input type="checkbox" name="country"/> ' + data["data"][j].skillName + '</a></li>';
            }
            skillDropDown.append(str);

        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng");
        }

    });

$.when(ajaxRequest1, ajaxRequest2, ajaxRequest3, ajaxRequest4).done(function () {
    intializeChips();
});
// ====================================================================
// chips items
// ====================================================================
$(".close-chips").on("click", function (e) {
    $(".home-chips .chip").remove();
    $(this).hide();
    $(".no-filter-text").show();
});

function intializeChips() {
    $(".filters .dropdown-menu li a").on("click", function (e) {
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



// ====================================================================
// search 
// ====================================================================
$("#search-input").on("keyup", function (e) {
    var searchString = $("#search-input").val();
    if (searchString.length > 2) {
        $.ajax({
            type: "POST",
            url: "/Mission/searchMissions",
            data: { "searchText": $("#search-input").val() },
            success: function (data) {
                var str = "";
                var str2 = "";
                var gridList = $("#mission-grid .row");
                var listCards = $("#mission-list .row");
                gridList.empty();
                listCards.empty();
                data = JSON.parse(data["data"]);
                for (var j = 0; j < data.length; j++) {
                    str += ' <div id="card-parent" class="list-card-parent col-lg-4 col-md-6 col-sm-6 col-12 grid-item h-auto mb-4"><div class="card"><img class="card-img-top w-100 card-img" src="' + "/" + data[j].MissionMedia[0].MediaPath + "/" + data[j].MissionMedia[0].MediaName + "" + data[j].MissionMedia[0].MediaType + '" alt="Card image cap" /><div id="card-image-content" class="d-flex flex-column justify-content-between  p-2"><div id="location-image" class="img-content p-2 rounded-5"><img src="/images/pin.png" alt="">' + data[j].City.Name + '</div><div class="d-flex flex-column align-items-end"><div class="img-heart img-content p-2 rounded-5 m-1"><img src="/images/heart.png" alt=""></div><div id="user-img" class="img-content p-2 rounded-5 m-1"><img src="/images/user.png" alt=""></div></div></div><h5 class="card-theme-title bg-white rounded-5 p-2">' + data[j].Theme.Title + '</h5><div class="card-body"><h5 class="card-title fs-3">' + data[j].Title + '</h5><p class="card-text">' + data[j].ShortDescription + '...</p><div id="star-content" class="d-flex align-items-center justify-content-between"><p class="my-auto">' + data[j].OrganizationName + '</p><div class="d-flex align-items-center"><img src="/images/selected-star.png" alt=""><img src="/images/selected-star.png" alt=""><img src="/images/selected-star.png" alt=""><img src="/images/star.png" alt=""><img src="/images/star.png" alt=""></div></div></div><div id="mission-duration-div"><h5 id = "card-time-duration-title" class="border bg-white rounded-5 m-auto fs-6 p-2" > From ' + data[j].StartDate.substring(0, 10) + ' until ' + data[j].EndDate.substring(0, 10) + '</h5 ></div><div id="seats-info-card" class="d-flex border border-1 position-relative pt-3 align-items-center justify-content-around"><div id="remain-seats-cards" class=" w-50 d-flex align-items-center p-1 px-lg-4 px-md-1"><img id="seats-left-image" src="/images/Seats-left.png" alt=""><div class="d-flex flex-column p-2 "><p id="count" class="m-0 p-0 fs-5">10</p><p id="seats-left-text" class="m-0 p-0">Seats left</p></div></div><div id="deadline-info-card" class=" w-40 d-flex align-items-center p-1 px-lg-4 px-md-1"><img id="deadline-image" src="/images/deadline.png" alt=""><div class="d-flex flex-column p-2 "><p id="deadline-date" class="m-0 p-0 fs-5">19/11/2022</p><p id="deadline-text" class="m-0 p-0">Deadline</p></div></div></div><div class="card-button-apply m-1 p-1 w-100 text-center"><button id="apply-btn" class="btn btn-outline-warning rounded-5 py-2 px-4 fs-5 ">Apply<img src="/images/right-arrow.png" class="mx-1 ms-3" alt=""></button></div></div ></div > ';

                    str2 += '<div id="list-card-parent" class="col-12 d-flex p-0 mb-4 border shadow"><div class="list-image-part position-relative"> <img src="' + "/" + data[j].MissionMedia[0].MediaPath + "/" + data[j].MissionMedia[0].MediaName + "" + data[j].MissionMedia[0].MediaType + '" class="list-img" alt = "" /><h5 class="list-card-theme-title bg-white rounded-5 p-2">' + data[j].Theme.Title + '</h5><div id="list-card-image-content" class="h-100 d-flex flex-column position-absolute  p-2"><div id="list-location-image" class="list-img-content p-2 rounded-5"><img src="/images/pin.png" alt="">' + data[j].City.Name + '</div><div class="d-flex flex-column align-items-end"><div class="img-heart list-img-content p-2 rounded-5 m-1"><img src="/images/heart.png" alt=""></div><div id="list-user-img" class="list-img-content p-2 rounded-5 m-1"><img src="/images/user.png" alt=""></div></div></div></div><div class="list-content-part d-flex flex-column justify-content-around p-2"><h5 class="card-title fs-3">' + data[j].Title + '</h5><p class="card-text">' + data[j].ShortDescription + '...</p><div id="list-info-content" class="d-flex justify-content-between"><div><div id="list-star-content" class="d-flex align-items-center justify-content-between"><p class="my-auto">' + data[j].OrganizationName + '</p><div class="d-flex align-items-center"><img src="/images/selected-star.png" alt=""><img src="/images/selected-star.png" alt=""><img src="/images/selected-star.png" alt=""><img src="/images/star.png" alt=""><img src="/images/star.png" alt=""></div></div><div class="list-card-button-apply m-1 p-1 w-100 "><button id="list-apply-btn" class="btn btn-outline-warning rounded-5 py-2 px-4 fs-5 ">Apply<img src="/images/right-arrow.png" class="mx-1 ms-3" alt=""></button></div></div><div><div id="list-mission-duration-div"><h5 id="list-card-time-duration-title" class="border bg-white rounded-5 m-auto fs-6 p-2">From ' + data[j].StartDate.substring(0, 10) + ' until ' + data[j].EndDate.substring(0, 10) + '</h5></div><div id="list-seats-info-card" class="d-flex border-top border-1 position-relative pt-3 align-items-center justify-content-around"><div id="remain-seats-cards" class=" w-50 d-flex align-items-center p-1 px-lg-4 px-md-1"><img id="list-seats-left-image" src="/images/Seats-left.png" alt=""><div class="d-flex flex-column p-2 "><p id="count" class="m-0 p-0 fs-5">10</p><p id="list-seats-left-text" class="m-0 p-0">Seats left</p></div></div><div id="list-deadline-info-card" class=" w-40 d-flex align-items-center p-1 px-lg-4 px-md-1"><img id="list-deadline-image" src="/images/deadline.png" alt=""><div class="d-flex flex-column p-2 "><p id="list-deadline-date" class="m-0 p-0 fs-5">19/11/2022</p><p id="list-deadline-text" class="m-0 p-0">Deadline</p></div></div></div></div></div></div></div>';
                    //console.log(data[j]);                
                }
                gridList.append(str);
                listCards.append(str2);
            },
            failure: function (response) {
                alert("failure");
            },
            error: function (response) {
                alert("Something went Worng");
            }

        });
    }
    else {
        loadMissions();
    }
});




// ====================================================================
// Pagination
// ====================================================================
function pagination() {

    const paginationNumbers = document.getElementById("pagination-numbers");

    var paginatedList = document.querySelector("#mission-grid .row");
    var listCards = document.querySelector("#mission-list .row");
    const listItems = paginatedList.querySelectorAll(".list-card-parent")
    console.log(listItems);
}