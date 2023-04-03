// Search filter
const filteMissionNavbar=document.getElementById("filter-mission-navbar");
const searchButton = document.getElementById("search-button");
const missionContent=document.getElementById("mission-content");
let missionContentHeight=missionContent.style.height;


function getStyle(element) {
  if (typeof getComputedStyle !== "undefined") {
      return getComputedStyle(element);
  }
  return element.currentStyle; // Old IE
}

var heightStyle = getStyle(missionContent).height;
heightStyle=+heightStyle.slice(0,-2)+78+78;


let flag=0;
let flagSideBar=0;



function myFunction(x) {
  if (x.matches) { // If media query matches
    missionContent.setAttribute('style','height:'+(heightStyle-79)+'px;');

    
  } else {
    missionContent.setAttribute('style','height:'+heightStyle+'px;');

  }
}

var x = window.matchMedia("(min-width: 576px)")
myFunction(x) // Call listener function at run time
x.addListener(myFunction) // Attach listener function on state changes

// =================================================================================
// menu-sidebar
// =================================================================================
const menuImg=document.getElementById("menuimg");
const sideBar=document.getElementById("menu-side-bar");
const closeImg=document.getElementById("close-img");

menuImg.addEventListener("click",(e)=>{
  e.preventDefault();
  if(flagSideBar%2==0){
    sideBar.setAttribute('style','left:0px !important');
  }
flagSideBar++;
});

closeImg.addEventListener("click",(e)=>{
  e.preventDefault();
  if(flagSideBar%2!=0){
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


// =================================================================================
// Add skills
// =================================================================================
$( document ).ready(function() {
});
(function () {
    $("#btnRight").click(function (e) {
      var selectedOpts = $("#lstBox1 option:selected");
      if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
        }

      $("#lstBox2").append($(selectedOpts).clone());
       $(selectedOpts).remove();
      e.preventDefault();
    });

    
    $("#btnLeft").click(function (e) {
      var selectedOpts = $("#lstBox2 option:selected");
      if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
      }
      $("#lstBox1").append($(selectedOpts).clone());
      $(selectedOpts).remove();
      e.preventDefault();
    });
   
})(jQuery);



var addedSkills = "";
$("#skill-save-btn").click(function (e) {
    addedSkills = "";
    var selectedOpts = $("#lstBox2 option");

    var options = $('#lstBox2 option');

    var values = $.map(options, function (option) {
        addedSkills += option.innerHTML + "\n";
    });
    $("#skill-textarea").val(addedSkills);
});
addedSkills = "";

var options = $('#lstBox2 option');

var values = $.map(options, function (option) {
    addedSkills += option.innerHTML + "\n";
});
$("#skill-textarea").val(addedSkills);
// =================================================================================
//User profile upload
// =================================================================================
$("#user-profile-main-img").on("click",function(e){
  $('#imgupload').trigger('click');
});
function readURL(input) {
  if (input.files && input.files[0]) {
    var reader = new FileReader();

    reader.onload = function (e) {
      $('#user-profile-main-img').attr('src', e.target.result);
    };

    reader.readAsDataURL(input.files[0]);
  }
}


$("#addSkillBtn").on("click", function (e) {
    e.preventDefault();
});
// =================================================================================
//Get cities
// =================================================================================
$('#profileCountryDropdown').change(function () {
    var optionSelected = $(this).find("option:selected");
    var countryId = optionSelected.val();
    var cityDropDown = $("#profileCityDropdown");
    $.ajax({
        type: "POST",
        url: '/Account/getCityNames',
        data: { countryId: countryId },
        success: function (response) {
            var str = "";
            cityDropDown.empty();
            for (var j = 0; j < response["data"].length; j++) {
                str += "<option value=" + response["data"][j].cityId + ">" + response["data"][j].name +"</option>";
            }
            cityDropDown.append(str);
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
});
