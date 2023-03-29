// Search filter
const filteMissionNavbar=document.getElementById("filter-mission-navbar");
const searchButton = document.getElementById("search-button");
const missionContent=document.getElementById("mission-content");
let missionContentHeight=missionContent.style.height;

console.log(missionContent);
console.log(missionContentHeight);

function getStyle(element) {
  if (typeof getComputedStyle !== "undefined") {
      return getComputedStyle(element);
  }
  return element.currentStyle; // Old IE
}

var heightStyle = getStyle(missionContent).height;
heightStyle=+heightStyle.slice(0,-2)+78;
console.log(heightStyle);


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
    console.log( "ready!" );
});
(function () {
    $("#btnRight").click(function (e) {
      var selectedOpts = $("#lstBox1 option:selected");
      if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
      }
      $("#lstBox2").append($(selectedOpts).clone());
    //   $(selectedOpts).remove();
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