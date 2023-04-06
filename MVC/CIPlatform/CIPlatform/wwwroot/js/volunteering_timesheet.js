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

//===============================================================================================================
//Add Hour timesheet
//===============================================================================================================
$("#hour-timesheet-submit-btn").on("click", function (e) {
    var userId = $(".user-btn")[0].id.slice(9)
    var missionId = $("#volunteer-timesheet-hour-mission").val();
    var volunteerDate = $("#volunteer-hour-timesheet-date").val();
    var volunteerHour = $("#volunteer-hour-timesheet-hour").val();
    var volunteerMinutes = $("#volunteer-hour-timesheet-minutes").val();
    var volunteerMessage = $("#volunteer-hour-timesheet-message").val();
    var time = volunteerHour + ":" + volunteerMinutes;

    if ((volunteerHour >= 0 && volunteerHour <= 23) && (volunteerMinutes >= 0 && volunteerMinutes <= 59)) {
        $.ajax({
            type: "POST",
            url: '/Account/addVolunteerTimesheet',
            data: { UserId: userId, MissionId: missionId, DateVolunteered: volunteerDate, Time: time, Notes: volunteerMessage },
            success: function (data) {
                getVolunteerTimesheetHourBased();
            },
            error: function (xhr, status, error) {
                // Handle error
                console.log(error);
            }
        });
    }
    else {
        alert("Please enter hour between 0 to 23 and minutes between 0 to 59");
    }
});
//===============================================================================================================
//Add goal timesheet
//===============================================================================================================

$("#goal-timesheet-submit-btn").on("click", function (e) {
    var userId = $(".user-btn")[0].id.slice(9)
    var missionId = $("#volunteer-timesheet-goal-mission").val();
    var volunteerDate = $("#volunteer-goal-timesheet-date").val();
    var volunteerAction = $("#volunteer-goal-timesheet-action").val();
    var volunteerMessage = $("#volunteer-goal-timesheet-message").val();
    $.ajax({
        type: "POST",
        url: '/Account/addVolunteerTimesheet',
        data: { UserId: userId, MissionId: missionId, DateVolunteered: volunteerDate, Action: volunteerAction, Notes: volunteerMessage },
        success: function (data) {
            getVolunteerTimesheetGoalBased();
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
});
//===============================================================================================================
//get hour timesheet data
//===============================================================================================================
$(document).ready(function () {
    getVolunteerTimesheetHourBased();
    getVolunteerTimesheetGoalBased();
});
function getVolunteerTimesheetHourBased() {
    var userId = $(".user-btn")[0].id.slice(9);
    $.ajax({
        type: "POST",
        url: '/Account/getVolunteerTimesheetRecordHourBased',
        dataType: "html",
        cache: false,
        data: { userId: userId },
        success: function (data) {
            $("#hour-based-records").html("");
            $("#hour-based-records").html(data);
            loaddeleteVolunteerBtn();
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
}

function getVolunteerTimesheetGoalBased() {
    var userId = $(".user-btn")[0].id.slice(9);
    $.ajax({
        type: "POST",
        url: '/Account/getVolunteerTimesheetRecordGoalBased',
        dataType: "html",
        cache: false,
        data: { userId: userId },
        success: function (data) {
            $("#goal-based-records").html("");
            $("#goal-based-records").html(data);
            loaddeleteVolunteerBtn();
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
}

//===============================================================================================================
//delete timesheet data
//===============================================================================================================
function loaddeleteVolunteerBtn() {
    console.log($(".delete-hour-data"),"call");
$(".delete-hour-data").on("click", function (e) {
    deleteVolunteerTimesheet(this.id.slice(7));
});
$(".delete-goal-data").on("click", function (e) {
    deleteVolunteerTimesheet(this.id.slice(7));
});

}

function deleteVolunteerTimesheet(timesheetId) {
    $.ajax({
        type: "POST",
        url: '/Account/deleteVolunteerTimesheet',
       
        data: { timesheetId: timesheetId },
        success: function (data) {
            getVolunteerTimesheetGoalBased();
            alert("Record successfully deleted");
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
}