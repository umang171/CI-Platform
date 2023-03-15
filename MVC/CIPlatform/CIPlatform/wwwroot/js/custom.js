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
var toggleGrid = true;
function toggleListGrid() {
    
    //console.log("toggle");
    const missionListView = document.getElementById("mission-list");
    //console.log(missionListView);
    const missionGridView = document.getElementById("mission-grid");
    const gridViewBtn = document.getElementById("grid-view-btn");
    const listViewBtn = document.getElementById("list-view-btn");

    listViewBtn.addEventListener("click", (e) => {
        e.preventDefault();
        toggleGrid = false;
        listViewBtn.setAttribute("style", "background-color:#dee2e6 !important;");
        gridViewBtn.setAttribute("style", "background-color:white !important;");
        toggleListGrid();
    });
    gridViewBtn.addEventListener("click", (e) => {
        e.preventDefault();
        toggleGrid = true;
        listViewBtn.setAttribute("style", "background-color:white !important;");
        gridViewBtn.setAttribute("style", "background-color:#dee2e6 !important;");
        toggleListGrid();
    });
    if (!(toggleGrid ==true)) {
        
        missionListView.setAttribute("style", "display:block !important;");
        missionGridView.setAttribute("style", "display:none !important;");
    }
    else {
        
        missionListView.setAttribute("style", "display:none !important;");
        missionGridView.setAttribute("style", "display:block !important;");
    }
}


//==============================================================================
//Ajax calls
//==============================================================================
$(document).ready(function () {    
    loadCard();
    
});
var selectedCountries = "";
var selectedCities = "";
var selectedThemes = "";
var selectedSkills = "";
var searchText = "";
var sortMissionFilterVal = "";
function loadCard(paging) {
    if (!paging)
        paging = 1;
    $.ajax({
        
        type: "POST",
        url: "/Mission/getMissionsFromSP",
        dataType: "html",
        cache:false,
        data: { countryNames: selectedCountries, cityNames: selectedCities, themeNames: selectedThemes, skillNames: selectedSkills, searchText: searchText, sortValue: sortMissionFilterVal,pageNumber: paging  },
        success: function (data) {
            $("#mission-card-views").html("");
            $('#mission-card-views').html(data);
            toggleListGrid();
            favouriteMissions();
            getFavouriteMissions();
            loadPagination();

            sortMissionFilter();
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
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
                str += '<li class="p-1"><a class= "dropdown-item" href = "#" > <input type="checkbox" name="country" value="' + data["data"][j].name+'"/> ' + data["data"][j].name + '</a></li>';
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
                str += '<li class="p-1"><a class= "dropdown-item" href = "#" > <input type="checkbox" name="country" value="' + data["data"][j].name +'"/> ' + data["data"][j].name + '</a></li>';
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
                str += '<li class="p-1"><a class= "dropdown-item" href = "#" > <input type="checkbox" name="country" value="' + data["data"][j].title +'"/> ' + data["data"][j].title + '</a></li>';
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
                str += '<li class="p-1"><a class= "dropdown-item" href = "#" > <input type="checkbox" name="country" value="' + data["data"][j].skillName+'"/> ' + data["data"][j].skillName + '</a></li>';
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
    selectedCountries = "";
    selectedCities = "";
    selectedSkills = "";
    selectedThemes = "";

    selectedCountries = "";
    $.each($(".countryDropDownList li a input:checkbox"), function () {
        this.checked=false;
    });
    //city filters
    selectedCities = "";
    $.each($(".cityDropDownList li a input:checkbox"), function () {
        this.checked = false;
    });

    //theme filters
    selectedThemes = "";
    $.each($(".themeDropDownList li a input:checkbox"), function () {
        this.checked = false;
    });

    //theme filters
    selectedSkills = "";
    $.each($(".skillDropDownList li a input:checkbox"), function () {
        this.checked = false;
    });
    loadCard();
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

        //========================
        //filters
        //========================
                
        //country filters
        selectedCountries = "";
        $.each($(".countryDropDownList li a input:checkbox:checked"), function () {
            selectedCountries += $(this).val() + ",";
        });

        //city filters
        selectedCities = "";
        $.each($(".cityDropDownList li a input:checkbox:checked"), function () {
            selectedCities += $(this).val() + ",";
        });

        //theme filters
        selectedThemes = "";
        $.each($(".themeDropDownList li a input:checkbox:checked"), function () {
            selectedThemes += $(this).val() + ",";
        });

        //theme filters
        selectedSkills = "";
        $.each($(".skillDropDownList li a input:checkbox:checked"), function () {
            selectedSkills += $(this).val() + ",";
        });
        loadCard();
    });
}



// ====================================================================
// sort filter
// ====================================================================

function sortMissionFilter() {
    $("#sort-dropdown").change(function () {
        sortMissionFilterVal = $("#sort-dropdown").find(":selected").val();
        loadCard();
    });
}

// ====================================================================
// search 
// ====================================================================
$("#search-input").on("keyup", function (e) {
    searchText = $("#search-input").val();
    if (searchText.length > 2) {
        loadCard();
    }
    else {
        searchText = "";
        loadCard();
    }
});
// ====================================================================

//pagination
console.log("in grid");
function loadPagination() {
    var paging = "";
    $("#pagination li a").on("click", function (e) {
        console.log("in page");
        e.preventDefault();
        paging = $(this).text();
        console.log(paging);
        loadCard(paging);
    })
}
//=================================================================================================
//Favourite mission
//=================================================================================================
function favouriteMissions() {
    $("#mission-card-views .favourite-mission-div").on("click", function (event) {
        event.preventDefault();
        if (this.style.backgroundColor == "black") {
            this.style.opacity = 1;
            var missionId =this.id.slice(18,);
            var userId =$("#rightNavbar .user-btn")[0].id.slice(9,);
            $.ajax({
                type: "POST",
                url: '/Mission/addFavouriteMissions',
                data: { userId: userId, missionId: missionId },
                success: function (data) {
                    getFavouriteMissions();

                },
                error: function (xhr, status, error) {
                    // Handle error
                    console.log(error);
                }
            });

        }
        else {
           
            var missionId = this.id.slice(18,);
            var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
            $.ajax({
                type: "POST",
                url: '/Mission/removeFavouriteMissions',
                data: { userId: userId, missionId: missionId },
                success: function (data) {
                    getFavouriteMissions();
                },
                error: function (xhr, status, error) {
                    // Handle error
                    console.log(error);
                }
            });

        }
    });
}
function getFavouriteMissions() {
    var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
    $.ajax({
        type: "GET",
        url: '/Mission/getFavouriteMissionsOfUser',
        data: { userid: userId },
        success: function (data) {
            var dataArr = data["data"].split(",");

            $("#mission-card-views .favourite-mission-div").each(function (index) {
                var id = this.id.slice(18);
                for (var i = 0; i < dataArr.length - 1; i++) {
                    if (dataArr[i] == id) {
                        this.style.backgroundColor = "red";
                        this.style.opacity = 1;
                        break;
                    }
                    this.style.backgroundColor = "black";
                    this.style.opacity = 0.4;
                }
            });
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
}