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
    if (!(toggleGrid == true)) {

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
    getNotifications();
});
var selectedCountries = "";
var selectedCities = "";
var selectedThemes = "";
var selectedSkills = "";
var searchText = "";
var sortMissionFilterVal = "";
var exploreMissionFilterVal = "";
function loadCard(paging) {
    var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
    if (!paging)
        paging = 1;
    $.ajax({

        type: "POST",
        url: "/Mission/getMissionsFromSP",
        dataType: "html",
        cache: false,
        data: { countryNames: selectedCountries, cityNames: selectedCities, themeNames: selectedThemes, skillNames: selectedSkills, searchText: searchText, sortValue: sortMissionFilterVal, exploreValue: exploreMissionFilterVal, pageNumber: paging, userId: userId },
        success: function (data) {
            $("#mission-card-views").html("");
            $('#mission-card-views').html(data);
            toggleListGrid();
            favouriteMissions();
            getFavouriteMissions();
            loadPagination();
            getAppliedMissions();
            sortMissionFilter();
            exploreMissionFilter();
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
        data: {},
        success: function (data) {
            var str = "";
            var countryDropDown = $(".countryDropDownList");
            for (var j = 0; j < data["data"].length; j++) {
                str += '<li class="p-1"><a class= "dropdown-item" id="' + data["data"][j].name + '" href = "#" > <input type="checkbox" name="country" id="c' + data["data"][j].name + '" value="' + data["data"][j].name + '"/> ' + data["data"][j].name + '</a></li>';
            }
            countryDropDown.append(str);

        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng country");
        }

    });

var ajaxRequest2 =
    $.ajax({
        type: "GET",
        url: "/Mission/GetCites",
        data: { country: selectedCountries },
        success: function (data) {
            var str = "";
            var cityDropDown = $(".cityDropDownList");
            for (var j = 0; j < data["data"].length; j++) {
                str += '<li class="p-1"><a class= "dropdown-item" id="' + data["data"][j].name + '" href = "#" > <input type="checkbox" name="country" id="c' + data["data"][j].name + '" value="' + data["data"][j].name + '"/> ' + data["data"][j].name + '</a></li>';
            }
            $(".cityDropDownList").empty();

            cityDropDown.append(str);

        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng city");
        }

    });

var ajaxRequest3 =
    $.ajax({
        type: "GET",
        url: "/Mission/GetThemes",
        data: {},
        success: function (data) {
            var str = "";
            var themeDropDown = $(".themeDropDownList");
            for (var j = 0; j < data["data"].length; j++) {
                var tempName = data["data"][j];
                tempName = tempName.replaceAll(" ", "");
                str += '<li class="p-1"><a class= "dropdown-item" id="' + tempName.trim() + '"  href = "#" > <input type="checkbox" name="country" id="c' + tempName.trim() + '"  value="' + data["data"][j] + '"/> ' + data["data"][j] + '</a></li>';
            }
            themeDropDown.append(str);

        },
        failure: function (response) {
            alert("failure");
        },
        error: function (xhr, status, error) {
            alert("Something went Worng themes");
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
                var tempName = data["data"][j].skillName;
                tempName = tempName.replaceAll(" ", "");
                str += '<li class="p-1"><a class= "dropdown-item" id="' + tempName.trim() + '" href = "#" > <input type="checkbox" name="country"id="c' + tempName.trim() + '"  value="' + data["data"][j].skillName + '"/> ' + data["data"][j].skillName + '</a></li>';
            }
            skillDropDown.append(str);

        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng skills");
        }

    });

$.when(ajaxRequest1, ajaxRequest2, ajaxRequest3, ajaxRequest4).done(function () {
    intializeChips();
    selectedCityCountryOfUser();
});
loadCityWithCountry();
// ====================================================================
// Default selected country and city of user
// ====================================================================
function selectedCityCountryOfUser() {
    var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
    $.ajax({
        type: "Get",
        url: '/Mission/GetCityCountryOfUser',
        data: { userId: userId },
        success: function (data) {
            if (data["data"] != "NO") {
                var city = data["data"].split(",")[0];
                var country = data["data"].split(",")[1];
                $("#" + country).trigger('click');
                $("#" + country + " #c" + country).prop('checked', true);
                $("#c" + country).prop('checked', true);
                $("#" + city).trigger('click');
                $("#" + city + " #c" + city).prop('checked', true);
                $("#c" + city).prop('checked', true);
            }
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
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
    selectedCountries = "";
    selectedCities = "";
    selectedSkills = "";
    selectedThemes = "";

    selectedCountries = "";
    $.each($(".countryDropDownList li a input:checkbox"), function () {
        this.checked = false;
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
        if ($("#c" + this.id).prop('checked') == false) {
            var tempName = $(this).text().replaceAll(" ", "");
            $(".home-chips .chips").append(
                '<div class="chip" id="chip-' + tempName.trim() + '">' +
                $(this).text() +
                '<span class="closebtn" id="close-' + tempName.trim() + '" onclick="this.parentElement.style.display=\'none\'">&times;</span>'
            );
            //$("#close-" + $(this).text().trim())[0].parentElement.style.display = 'block';
            $(".close-chips").show();
            $(".no-filter-text").hide();
            $(".close-chips").show();
            $("#c" + this.id).prop('checked', true);
        }
        else {
            var tempName = $(this).text().replaceAll(" ", "");
            $("#close-" + tempName.trim()).parents('.chip').remove();
            $("#c" + this.id).prop('checked', false);
        }

        //========================
        //filters
        //========================

        //country filters
        selectedCountries = "";
        $.each($(".countryDropDownList li a input:checkbox:checked"), function () {
            selectedCountries = selectedCountries.replace($(this).val() + ",", "");
            selectedCountries += $(this).val() + ",";
        });

        //theme filters
        selectedThemes = "";
        $.each($(".themeDropDownList li a input:checkbox:checked"), function () {
            selectedThemes = selectedThemes.replace($(this).val() + ",", "");
            selectedThemes += $(this).val() + ",";
        });

        //theme filters
        selectedSkills = "";
        $.each($(".skillDropDownList li a input:checkbox:checked"), function () {
            selectedSkills = selectedSkills.replace($(this).val() + ",", "");
            selectedSkills += $(this).val() + ",";
        });
        closeChipBtn();
        loadCityWithCountry();
        loadCard();
    });
}
function intializeChips2() {
    $(".cityDropDownList li a").on("click", function (e) {

        if ($("#c" + this.id).prop('checked') == false) {
            var tempName = $(this).text().replaceAll(" ", "");
            $(".home-chips .chips").append(
                '<div class="chip" id="chip-' + tempName.trim() + '">' +
                $(this).text() +
                '<span class="closebtn" id="close-' + tempName.trim() + '" onclick="this.parentElement.style.display=\'none\'">&times;</span>'
            );
            //$("#close-" + $(this).text().trim())[0].parentElement.style.display = 'block';
            $(".close-chips").show();
            $(".no-filter-text").hide();
            $(".close-chips").show();
            $("#c" + this.id).prop('checked', true);
        }
        else {
            var tempName = $(this).text().replaceAll(" ", "");
            $("#close-" + tempName.trim()).parents('.chip').remove();
            $("#c" + this.id).prop('checked', false);
        }

        //city filters
        selectedCities = "";
        $.each($(".cityDropDownList li a input:checkbox:checked"), function () {
            selectedCities = selectedCities.replace($(this).val() + ",", "");
            selectedCities += $(this).val() + ",";
        });
        closeChipBtn();
        loadCard();
    });
}
function closeChipBtn() {
    $(".closebtn").on("click", function () {
        var closeId = "c" + this.id.slice(6);
        $(".dropdown-item #" + closeId).prop("checked", false);
        selectedCountries = selectedCountries.replace(this.id.slice(6) + ",", "");
        selectedCities = selectedCities.replace(this.id.slice(6) + ",", "");
        selectedThemes = selectedThemes.replace(this.id.slice(6) + ",", "");
        selectedSkills = selectedSkills.replace(this.id.slice(6) + ",", "");
        loadCard();
    });
}
function loadCityWithCountry() {
    var ajaxCountry = $.ajax({
        type: "GET",
        url: "/Mission/GetCites",
        data: { country: selectedCountries },
        success: function (data) {
            var str = "";
            var cityDropDown = $(".cityDropDownList");
            for (var j = 0; j < data["data"].length; j++) {
                if (selectedCities.indexOf(data["data"][j].name) < 0) {
                    str += '<li class="p-1"><a class= "dropdown-item" id="' + data["data"][j].name + '" href = "#" > <input type="checkbox" name="country" id="c' + data["data"][j].name + '" value="' + data["data"][j].name + '"/> ' + data["data"][j].name + '</a></li>';
                }
                else {
                    str += '<li class="p-1"><a class= "dropdown-item" id="' + data["data"][j].name + '" href = "#" > <input type="checkbox" name="country" checked id="c' + data["data"][j].name + '" value="' + data["data"][j].name + '"/> ' + data["data"][j].name + '</a></li>';
                }
            }
            $(".cityDropDownList").empty();
            cityDropDown.append(str);

        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Something went Worng city");
        }
    });

    $.when(ajaxCountry).done(function () {
        intializeChips2();
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
// explore filter
// ====================================================================

function exploreMissionFilter() {
    $("#explore-dropdown").change(function () {
        exploreMissionFilterVal = $("#explore-dropdown").find(":selected").val();
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
var pagingNumber = 1;
function loadPagination() {
    var totalPages = parseInt($(".Pagination-total")[0].id.slice(6));
    var paging = "";
    $("#pagination li a").on("click", function (e) {
        e.preventDefault();
        paging = $(this).text();

        if (!isNaN(paging)) {
            pagingNumber = parseInt(paging);
            loadCard(paging);
        }
        else {
            if (paging == "<") {
                if (pagingNumber != 1)
                    loadCard(--pagingNumber);
            }
            else if (paging == ">") {
                if (pagingNumber != totalPages)
                    loadCard(++pagingNumber);
            }
        }
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
            var missionId = this.id.slice(18,);
            var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
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
                for (var i = 0; i < dataArr.length; i++) {
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
//=======================================================================================
//Applied mission
//=======================================================================================

function getAppliedMissions() {
    var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
    $(".mission-applied").each(function (index) {
        var divId = this.id;
        var missionId = this.id.slice(16);
        $.ajax({
            type: "post",
            url: '/Mission/getAppliedMissionOfUser',
            data: { userid: userId, missionId: missionId },
            success: function (response) {
                var status = response["status"];
                if (status == "applied") {
                    $("#" + divId).css("display", "block");
                    $("#l" + divId).css("display", "block");
                    $(".apply-btn-" + missionId).text("View Details");
                    $(".apply-btn-" + missionId).append('<img src="/images/right-arrow.png" class="mx-1 ms-3" alt="">')
                }
                else {
                    $("#" + divId).css("display", "none");
                    $("#l" + divId).css("display", "none");
                }

            },
            error: function (xhr, status, error) {
                // Handle error
                console.log(error);
            }
        });
    });
}

//=======================================================================================
//Notification
//=======================================================================================
function getNotifications() {

    var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
    $.ajax({
        type: "get",
        url: '/Mission/GetNotifications',
        data: { userid: userId },
        success: function (response) {
            var yesterdayDate = new Date();
            yesterdayDate.setDate(yesterdayDate.getDate() - 1);
            var notificationCount = 0;

            if (response.length == 0) {
                var olderSpan = '<li style="background-color:lightgray" class="p-2 border border-1"><span>No notifications yet</span></li>';

                $("#notification-count-span").css("display", "none");

                $("#notification-dropdown").append(olderSpan);
                return;
            }
            else {
                var notificationStr = "";
                var oldNotificationStr = "";
                var flags = false;
                for (var i = 0; i < response.length; i++) {
                    var notificationDate = new Date(response[i].createdAt.slice(0, 10));
                    if (yesterdayDate < notificationDate) {
                        notificationStr += '<li class="p-2 border border-1 d-flex justify-content-between align-items-center">' +
                            '<div class="d-flex">';
                        notificationStr += '<img style="border-radius:50%;height:45px;width:34px" src="' + response[i].notificationImage + '" />';
                        if (response[i].notificationType == "RecommendedMission") {
                            notificationStr += '<a href="/Mission/Mission_Volunteer?missionId=' + response[i].messageId + '" class="link-style-none px-1">' + response[i].notificationMessage + '</a>' +
                                '</div>';
                        }
                        else if (response[i].notificationType == "RecommendedStory") {
                            notificationStr += '<a href="/Story/StoryDetails?storyId=' + response[i].messageId + '" class="link-style-none px-1">' + response[i].notificationMessage + '</a>' +
                                '</div>';
                        }
                        else if (response[i].notificationType == "StoryApproved") {
                            notificationStr += '<a href="/Story/StoryDetails?storyId=' + response[i].messageId + '" class="link-style-none px-1">' + response[i].notificationMessage + '</a>' +
                                '</div>';
                        }
                        else if (response[i].notificationType == "StoryDeclined") {
                            notificationStr += '<span class="link-style-none px-1">' + response[i].notificationMessage + '</span>' +
                                '</div>';
                        }
                        else if (response[i].notificationType == "MissionApplicationApproved") {
                            notificationStr += '<span class="link-style-none px-1">' + response[i].notificationMessage + '</span>' +
                                '</div>';
                        }
                        else if (response[i].notificationType == "MissionApplicationDeclined") {
                            notificationStr += '<span class="link-style-none px-1">' + response[i].notificationMessage + '</span>' +
                                '</div>';
                        }
                        if (response[i].status) {
                            notificationCount++;
                            notificationStr += '<input class="notification-checkbox notification-status" type="checkbox" id="' + response[i].notificationId + '"  checked />';
                        }
                        else {
                            notificationStr += '<input class="notification-checkbox notification-status-inactive" type="checkbox" id="' + response[i].notificationId + '" checked />';
                        }
                        notificationStr += '</li>';
                    }
                    else {
                        flags = true;
                        oldNotificationStr += '<li class="p-2 border border-1 d-flex justify-content-between align-items-center">' +
                            '<div class="d-flex">';
                        console.log(response[i].notificationType);
                            oldNotificationStr += '<img style="border-radius:50%;height:45px;width:34px" src="' + response[i].notificationImage + '" />';
                        if (response[i].notificationType == "RecommendedMission") {
                            oldNotificationStr += '<a href="/Mission/Mission_Volunteer?missionId=' + response[i].messageId + '" class="link-style-none px-1">' + response[i].notificationMessage + '</a>' + '</div>';
                        }
                        else if (response[i].notificationType == "RecommendedStory") {
                            oldNotificationStr += '<a href="/Story/StoryDetails?storyId=' + response[i].messageId + '" class="link-style-none px-1">' + response[i].notificationMessage + '</a>' + '</div>';
                        }
                        else if (response[i].notificationType == "StoryApproved") {
                            oldNotificationStr += '<a href="/Story/StoryDetails?storyId=' + response[i].messageId + '" class="link-style-none px-1">' + response[i].notificationMessage + '</a>' + '</div>';
                        }
                        else if (response[i].notificationType == "StoryDeclined") {
                            oldNotificationStr += '<span class="link-style-none px-1">' + response[i].notificationMessage + '</span>' +
                                '</div>';
                        }
                        else if (response[i].notificationType == "MissionApplicationApproved") {
                            oldNotificationStr += '<span class="link-style-none px-1">' + response[i].notificationMessage + '</span>' +
                                '</div>';
                        }
                        else if (response[i].notificationType == "MissionApplicationDeclined") {
                            oldNotificationStr += '<span class="link-style-none px-1">' + response[i].notificationMessage + '</span>' +
                                '</div>';
                        }
                        if (response[i].status) {
                            notificationCount++;
                            oldNotificationStr += '<input class="notification-checkbox notification-status" type="checkbox" id="' + response[i].notificationId + '"  checked />';
                        }
                        else {
                            oldNotificationStr += '<input class="notification-checkbox notification-status-inactive" type="checkbox" id="' + response[i].notificationId + '"  checked />';
                        }
                        oldNotificationStr += '</li>';
                    }
                }
                var olderSpan = '<li style="background-color:lightgray" class="p-2 border border-1"><span>Older</span></li>';
                $("#notification-count-span").css("display", "block");
                $("#notification-count-span").text(notificationCount);
                $("#notification-dropdown").append(notificationStr);
                if (flags) {
                    $("#notification-dropdown").append(olderSpan);
                    $("#notification-dropdown").append(oldNotificationStr);
                }
                changeStatusNotification();
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}
clearNotification();
//Notification Clear
function clearNotification() {
    var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);

    $("#clear-notification-btn").on("click", function (e) {
        e.preventDefault();
        $.ajax({
            type: "get",
            url: '/Mission/ClearNotifications',
            data: { userid: userId },
            success: function (response) {
                const list = document.getElementById("notification-dropdown");
                var childCount = list.childElementCount;
                for (var i = 1; i < childCount; i++) {
                    console.log(list);
                    if (list.hasChildNodes()) {
                        list.removeChild(list.children[1]);
                    }
                }
                getNotifications();
                $("#notification-button").trigger("click");
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        });
    });
}
function changeStatusNotification() {

    $(".notification-checkbox").on("change", function (e) {
        e.preventDefault();
        $("#notification-button").trigger("click");

        var notificationId = this.id;
        $.ajax({
            type: "get",
            url: '/Mission/changeStatusNotification',
            data: { notificationId: notificationId },
            success: function (response) {
                const list = document.getElementById("notification-dropdown");
                var childCount = list.childElementCount;
                for (var i = 1; i < childCount; i++) {
                    console.log(list);
                    if (list.hasChildNodes()) {
                        list.removeChild(list.children[1]);
                    }
                }
                getNotifications();
                $("#notification-button").trigger("click");
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        });
    });

}

//=======================================================================================
//Notification Settings
//=======================================================================================