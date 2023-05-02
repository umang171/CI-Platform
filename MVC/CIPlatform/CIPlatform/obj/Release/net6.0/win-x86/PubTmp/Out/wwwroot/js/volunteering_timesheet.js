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
$(".add-vol-data-btn").on("click", function () {
    $(".timesheet-submit-btn").text("Submit");
    $("#volunteer-hour-timesheet-date").val("");
    $("#volunteer-hour-timesheet-hour").val("");
    $("#volunteer-hour-timesheet-minutes").val("");
    $("#volunteer-hour-timesheet-message").val("");
    $("#volunteer-goal-timesheet-date").val("");
    $("#volunteer-goal-timesheet-action").val("");
    $("#volunteer-goal-timesheet-message").val("");

});
$("#volunteer-hour-timesheet-date").on("change", function (e) {

    var missionId = $("#volunteer-timesheet-hour-mission").val();
    
    $.ajax({
        type: "POST",
        url: '/Account/GetDatesOfMission',
        data: { MissionId: missionId},
        success: function (data) {
            const dates = data.split(",");
            $("#volunteer-hour-timesheet-date").prop("min", dates[0]);
            $("#volunteer-hour-timesheet-date").prop("max", dates[1]);
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
    
});
$("#hour-timesheet-submit-btn").on("click", function (e) {

    var userId = $(".user-btn")[0].id.slice(9);
    var missionId = $("#volunteer-timesheet-hour-mission").val();
    var volunteerDate = $("#volunteer-hour-timesheet-date").val();
    var volunteerHour = $("#volunteer-hour-timesheet-hour").val();
    var volunteerMinutes = $("#volunteer-hour-timesheet-minutes").val();
    var volunteerMessage = $("#volunteer-hour-timesheet-message").val();
    var time = volunteerHour + ":" + volunteerMinutes;
    if ($(".timesheet-submit-btn")[0].value == "Submit") {
        if ((volunteerHour >= 0 && volunteerHour <= 23) && (volunteerMinutes >= 0 && volunteerMinutes <= 59)) {
            $.ajax({
                type: "POST",
                url: '/Account/addVolunteerTimesheet',
                data: { UserId: userId, MissionId: missionId, DateVolunteered: volunteerDate, Hour: volunteerHour, Minutes: volunteerMinutes, Notes: volunteerMessage, Action: -1 },
                success: function (data) {
                    getVolunteerTimesheetHourBased();
                    if (data["status"] == 2) {
                        alert(data['errorMessage']);
                    }                
                    if (data["status"] == 1) {
                        $('#hourTimesheetModal').modal('toggle');
                    }
                    $("#volunteer-hour-timesheet-date").removeProp("min");
                    $("#volunteer-hour-timesheet-date").removeProp("max");
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
    }
    else {
        var timesheetId=$("#hour-timesheetId").val();
        if ((volunteerHour >= 0 && volunteerHour <= 23) && (volunteerMinutes >= 0 && volunteerMinutes <= 59)) {
            $.ajax({
                type: "POST",
                url: '/Account/editVolunteerTimesheet',
                data: { TimesheetId: timesheetId, UserId: userId, MissionId: missionId, DateVolunteered: volunteerDate, Hour: volunteerHour, Minutes: volunteerMinutes, Notes: volunteerMessage,Action: -1 },
                success: function (data) {
                    getVolunteerTimesheetHourBased();
                    if (data["status"] == 2) {
                        alert(data['errorMessage']);
                    } 
                    if (data["status"] == 1) {
                        $('#hourTimesheetModal').modal('toggle');
                    }
                    $("#volunteer-hour-timesheet-date").removeProp("min");
                    $("#volunteer-hour-timesheet-date").removeProp("max");
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
    }
});
//===============================================================================================================
//Add goal timesheet
//===============================================================================================================
document.getElementById('volunteer-goal-timesheet-date').max = new Date(new Date().getTime() - new Date().getTimezoneOffset() * 60000).toISOString().split("T")[0];

function getRemainingActions(missionId) {
    var remain = 0;
    $.ajax({
        type: "POST",
        url: '/Account/GetRemainingActions',
        data: { missionId: missionId },
        success: function (data) {
            document.getElementById('volunteer-goal-timesheet-action').max =
                data["count"];
            remain = data["count"];
            return remain;
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });

    return remain;
}
$("#goal-timesheet-submit-btn").on("click", function (e) {
    var userId = $(".user-btn")[0].id.slice(9)
    var missionId = $("#volunteer-timesheet-goal-mission").val();
    var volunteerDate = $("#volunteer-goal-timesheet-date").val();
    var volunteerAction = $("#volunteer-goal-timesheet-action").val();
    var volunteerMessage = $("#volunteer-goal-timesheet-message").val();
    getRemainingActions(missionId);
    if ($(".timesheet-submit-btn")[1].value == "Submit") {
        $.ajax({
            type: "POST",
            url: '/Account/addVolunteerTimesheet',
            data: { UserId: userId, MissionId: missionId, Hour: 0, Minutes: 0,  DateVolunteered: volunteerDate, Action: volunteerAction, Notes: volunteerMessage },
            success: function (data) {
                getVolunteerTimesheetGoalBased();
                if (data["status"] == 1) {
                    $('#goalTimesheetModal').modal('toggle');
                }
                if (data["status"] == 3) {
                    alert(data["errorMessage"]);
                }
            },
            error: function (xhr, status, error) {
                // Handle error
                console.log(error);
            }
        });
    }
    else {
        var timesheetId = $("#goal-timesheetId").val();
        $.ajax({
            type: "POST",
            url: '/Account/editVolunteerTimesheet',
            data: { TimesheetId: timesheetId, UserId: userId, Hour: 0, Minutes: 0, MissionId: missionId, DateVolunteered: volunteerDate, Action: volunteerAction, Notes: volunteerMessage },
            success: function (data) {
                getVolunteerTimesheetGoalBased();
                if (data["status"] == 1) {
                    $('#goalTimesheetModal').modal('toggle');
                }
                if (data["status"] == 3) {
                    alert(data["errorMessage"]);
                }
            },
            error: function (xhr, status, error) {
                // Handle error
                console.log(error);
            }
        });
    }
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
            loaddeleteVolunteerBtnHour();
            loadEditVolunteerBtnHour();
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
            loaddeleteVolunteerBtnGoal();
            loadEditVolunteerBtnGoal();
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
function loaddeleteVolunteerBtnHour() {
    $(".delete-hour-data").on("click", function (e) {

        deleteVolunteerTimesheet(this.id.slice(7));
    });

}
function loaddeleteVolunteerBtnGoal() {
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
            getVolunteerTimesheetHourBased();
            getVolunteerTimesheetGoalBased();
            alert("Record successfully deleted");
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
}


//===============================================================================================================
//edit timesheet data
//===============================================================================================================
function loadEditVolunteerBtnHour() {
    $(".edit-hour-data").on("click", function (e) {
        getEditVolunteerTimesheet(this.id.slice(5));
    });
}
function loadEditVolunteerBtnGoal() {
    $(".edit-goal-data").on("click", function (e) {
        getEditVolunteerTimesheet(this.id.slice(5));
    });

}
function getEditVolunteerTimesheet(timesheetId) {
    $.ajax({
        type: "POST",
        url: '/Account/getEditVolunteerTimesheet',

        data: { timesheetId: timesheetId },
        success: function (data) {
            if (data["data"].action == null) {
                $("#volunteer-hour-timesheet-date")[0].type = 'date';
                $("#volunteer-timesheet-hour-mission").val(data["data"].missionId);
                $("#volunteer-hour-timesheet-date").val(data["data"].dateVolunteered.slice(0, 10));
                $("#volunteer-hour-timesheet-hour").val(data["data"].hour);
                $("#volunteer-hour-timesheet-minutes").val(data["data"].minutes);
                $("#volunteer-hour-timesheet-message").val(data["data"].notes);
                $("#hour-timesheetId").val(data["data"].timesheetId)
            }
            else {
                $("#volunteer-timesheet-goal-mission").val(data["data"].missionId);
                $("#volunteer-goal-timesheet-date").val(data["data"].dateVolunteered.slice(0, 10));
                $("#volunteer-goal-timesheet-action").val(data["data"].action);
                $("#volunteer-goal-timesheet-message").val(data["data"].notes);
                $("#goal-timesheetId").val(data["data"].timesheetId)
            }

            $(".timesheet-submit-btn").val("Update");

        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
}