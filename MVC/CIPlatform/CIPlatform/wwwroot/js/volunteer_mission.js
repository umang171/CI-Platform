
const missionContent = document.getElementById("mission-content");
let missionContentHeight = missionContent.style.height;

// console.log(missionContent);
// console.log(missionContentHeight);

function getStyle(element) {
    if (typeof getComputedStyle !== "undefined") {
        return getComputedStyle(element);
    }
    return element.currentStyle; // Old IE
}

var heightStyle = getStyle(missionContent).height;
heightStyle = +heightStyle.slice(0, -2) + 78;
// console.log(heightStyle);


let flag = 0;
let flagSideBar = 0;


function myFunction(x) {
    if (x.matches) { // If media query matches
        missionContent.setAttribute('style', 'height:' + (heightStyle) + 'px;');


    } else {
        missionContent.setAttribute('style', 'height:' + heightStyle + 'px;');
    }
}

var x = window.matchMedia("(min-width: 280px)")
myFunction(x) // Call listener function at run time
x.addListener(myFunction) // Attach listener function on state changes

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

// tabs in volunteer
function openCity(evt, cityName) {
    // console.log(evt);
    // Declare all variables
    var i, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}


let tabcontent = document.getElementsByClassName("tabcontent");
let tablinks = document.getElementsByClassName("tablinks");
document.getElementById("Mission").style.display = "block";
tablinks[0].classList.add("active")

// ========================================================================================
// show image
// ========================================================================================
const carouselImages = document.querySelectorAll(".carousel-images");
const previewImage = document.getElementById("preview-image");
for (let i = 0; i < carouselImages.length; i++) {
    carouselImages[i].addEventListener("click", (e) => {
        e.preventDefault();
        previewImage.src = carouselImages[i].src;
    });
}

// console.log(carouselImages);
// console.log(previewImage.src);
$(document).ready(function () {
    loadRelatedMissions();
    favouriteMissions();
    getFavouriteMissions();
    relatedfavouriteMissions();
    relatedgetFavouriteMissions();
    getRatingOfUser();
    starRatings();
    getComments();
    getRecentVolunteers();
    getAppliedMissions();
});

// ======================================================================================================
// Favourite Mission
// ======================================================================================================

function favouriteMissions() {
    $("#favourite-btn").on("click", function (event) {
        event.preventDefault();
        if (this.style.backgroundColor == "white") {
            var missionId = $(".volunteer-button-apply")[0].id.slice(18);
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
            var missionId = $(".volunteer-button-apply")[0].id.slice(18);
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
            var id = $(".volunteer-button-apply")[0].id.slice(18);
            for (var i = 0; i < dataArr.length; i++) {
                if (dataArr[i] == id) {
                    document.getElementById("favourite-btn").style.backgroundColor = "red";
                    document.querySelector("#favourite-btn span").style.color = 'white';
                    $("#favourite-img-heart").attr("src", "/images/heart.png");
                    break;
                }
                document.getElementById("favourite-btn").style.backgroundColor = "white";
                document.querySelector("#favourite-btn span").style.color = '#757575';
                $("#favourite-img-heart").attr("src", "/images/heart1.png");
            }
            //});
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
}
//=====================================================================================================
//related favourite Missions
//=====================================================================================================

function relatedfavouriteMissions() {
    $("#related-mission-grid .favourite-mission-div").on("click", function (event) {
        event.preventDefault();
        if (this.style.backgroundColor == "black") {
            var missionId = this.id.slice(18,);
            var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
            $.ajax({
                type: "POST",
                url: '/Mission/addFavouriteMissions',
                data: { userId: userId, missionId: missionId },
                success: function (data) {
                    relatedgetFavouriteMissions();
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
                    relatedgetFavouriteMissions();
                },
                error: function (xhr, status, error) {
                    // Handle error
                    console.log(error);
                }
            });

        }
    });
}
function relatedgetFavouriteMissions() {
    var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
    $.ajax({
        type: "GET",
        url: '/Mission/getFavouriteMissionsOfUser',
        data: { userid: userId },
        success: function (data) {
            var dataArr = data["data"].split(",");

            $("#related-mission-grid .favourite-mission-div").each(function (index) {
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
//====================================================================================================
//Star rating
//====================================================================================================
function starRatings() {
    $("#stars-rating .rating-stars").on("click", function (event) {
        var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
        var missionId = $(".volunteer-button-apply")[0].id.slice(18);
        var starRating = Math.ceil(parseFloat($("#star-input-id").rating().val()));
        $.ajax({
            type: "POST",
            url: '/Mission/addRatingStars',
            data: { userId: userId, missionId: missionId, ratingStars:starRating },
            success: function (data) {
            }
        });
    });
}

function getRatingOfUser() {
    var missionId = $(".volunteer-button-apply")[0].id.slice(18);
    var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
    $.ajax({
        type: "POST",
        url: '/Mission/getRatingOfUser',
        data: { userId: userId, missionId: missionId},
        success: function (response) {
            var rating=response["rating"]*20;
            $(".filled-stars").width(rating+"%");
        }
    });
}
//====================================================================================================
//related missions
//====================================================================================================
function loadRelatedMissions() {
    var themeName = $("#mission-theme").text();
    var cityName = $("#mission-city-name").text();
    var missionId = $(".volunteer-button-apply")[0].id.slice(18);
   
    $.ajax({

        type: "POST",
        url: "/Mission/getRelatedMissions",
        dataType: "html",
        cache: false,
        data: { themeName: themeName, cityName: cityName, missionId:missionId },
        success: function (data) {
            $("#related-mission-grid").html("");
            $('#related-mission-grid').html(data);

            relatedfavouriteMissions();
            relatedgetFavouriteMissions();
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
}

//====================================================================================================
//Add comment
//====================================================================================================
$("#mission-comment-btn").on("click", function (e) {
    e.preventDefault();
    var comment=$("#comment").val();
    $("#comment").val("");

    var missionId = $(".volunteer-button-apply")[0].id.slice(18);
    var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);

    $.ajax({
        type: "POST",
        url: '/Mission/addComment',
        data: { userId: userId, missionId: missionId, comment: comment },
        success: function (data) {
            getComments();
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
});
function getComments() {
    var missionId = $(".volunteer-button-apply")[0].id.slice(18);
    $.ajax({
        type: "GET",
        url: '/Mission/getComments',
        dataType: "html",
        data: { missionId: missionId },
        success: function (data) {
            $("#volunteer-comments-div").html("");
            $("#volunteer-comments-div").html(data);
            
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
}
//========================================================================================
//Recommend to coworker
//========================================================================================
$("#send-email-btn").on("click", function (e) {
    e.preventDefault();
    var missionId = $(".volunteer-button-apply")[0].id.slice(18);
    var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
    var userEmail = $("#recommend-input").val();
    var userSelect = $("#recommend-select").val();
    var isValid = true;
    if (userSelect != "no") {
        userEmail = userSelect;
    } else {
        console.log("select not", userEmail);
        if (userEmail == null || userEmail == "") {
            alert("Please enter email");
            isValid = false;
        }
    }
    if (isValid) {
        $.ajax({
            type: "POST",
            url: '/Mission/recommendToCoworker',
            data: { fromUserId: userId, missinoId: missionId, toUserEmail: userEmail },
            success: function (data) {

            },
            error: function (xhr, status, error) {
                // Handle error
                console.log(error);
            }
        });
    }
});

//========================================================================================
//Apply to mission
//========================================================================================
$("#volunteer-apply-btn").on("click", function (e) {
    e.preventDefault();
    var missionId = $(".volunteer-button-apply")[0].id.slice(18);
    var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
    $.ajax({
        type: "POST",
        url: '/Mission/addToApplication',
        data: { missionId: missionId, userId: userId },
        success: function (data) {
            alert("You have applied for the mission");
            getAppliedMissions();
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
});

//========================================================================================
//get recent volunteers
//========================================================================================
function getRecentVolunteers() {
    var missionId = $(".volunteer-button-apply")[0].id.slice(18);
    
    $.ajax({
        type: "POST",
        url: '/Mission/getRecentVolunteers',
        dataType: "html",
        cache: false,
        data: { missionId: missionId },
        success: function (data) {
            $("#recent-volunteers-div").html("");
            $("#recent-volunteers-div").html(data);
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
}

//========================================================================================
//applied mission
//========================================================================================
function getAppliedMissions() {
    var missionId = $(".volunteer-button-apply")[0].id.slice(18);
    var userId = $("#rightNavbar .user-btn")[0].id.slice(9,);
    $.ajax({
        type: "post",
        url: '/Mission/getAppliedMissionOfUser',
        data: { userid: userId, missionId: missionId },
        success: function (response) {
            var status = response["status"];
            if (status == "applied") {
                $("#volunteer-apply-btn").text("Already Applied");
                $("#volunteer-apply-btn").prop('disabled', true);
            }
            else if (status == "PENDING"){
                $("#volunteer-apply-btn").text("Pending for approval");
                $("#volunteer-apply-btn").prop('disabled', true);
            }

        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
}