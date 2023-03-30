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



// ========================================================================================
// show image
// ========================================================================================
const carouselImages = document.querySelectorAll(".carousel-images");
const previewImage = document.getElementById("preview-image");
for (let i = 0; i < carouselImages.length; i++) {
    console.log(carouselImages[i].src);
    carouselImages[i].addEventListener("click", (e) => {
        e.preventDefault();
        previewImage.src = carouselImages[i].src;
    });
}

// console.log(carouselImages);
// console.log(previewImage.src);
$(document).ready(function () {
    getTotalViews();
});

// ========================================================================================
// total views
// ========================================================================================

function getTotalViews() {
    var storyid = $(".story-title")[0].id.slice(6);
    $.ajax({
        type: "POST",
        url: '/Story/getTotalStoryViews',
        data: { storyId: storyid },
        success: function (data) {
            var total = data["data"];
            $("#totalView").text(total)
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
    var storyid = $(".story-title")[0].id.slice(6);
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
            url: '/Story/recommendToCoworker',
            data: { storyId: storyid, toUserEmail: userEmail },
            success: function (data) {
                console.log("success");
            },
            error: function (xhr, status, error) {
                // Handle error
                console.log(error);
            }
        });
    }
});

//==================================================================================================
//Logout
//==================================================================================================
$("#logoutLink").on("click", function (e) {
    e.preventDefault();
    $.ajax({
        type: "GET",
        url: "/Story/logout",
        success: function () {
            location.reload();
        }
    });
});