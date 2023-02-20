// Search filter
const filteMissionNavbar=document.getElementById("filter-mission-navbar");
const searchButton = document.getElementById("search-button");

let flag=0;
let flagSideBar=0;

searchButton.addEventListener("click",(e)=>{
  e.preventDefault();
  if(flag%2==0){
        filteMissionNavbar.setAttribute('style','display:block !important')
    }
    else{
        filteMissionNavbar.setAttribute('style','display:none !important')

    }
    flag++;
});

function myFunction(x) {
  if (x.matches) { // If media query matches
    filteMissionNavbar.setAttribute('style','display:block !important')
    
  } else {
    filteMissionNavbar.setAttribute('style','display:none !important')
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
    filterSideBar.setAttribute('style','display:none !important');    
  }
}

let y = window.matchMedia("(min-width: 768px)")
myFunction2(y) // Call listener function at run time
y.addListener(myFunction2) // Attach listener function on state changes

// =====================================================================================================
// filter - button
// =====================================================================================================
const filterImg = document.getElementById("filter-image");
const filterSideBar = document.getElementById("filter-side-bar");
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
// chips items
// ====================================================================
var myModal = new bootstrap.Modal(document.getElementById('myModal'), options);
$('.example-class').ripple(options);