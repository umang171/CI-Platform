// ===================================================================================================
// close navbar
// ===================================================================================================
const adminNavbar=document.getElementById("admin-menubar");
$("#admin-menu-close-btn").click(function(){
    adminNavbar.setAttribute('style','display:none !important');    
});
$("#admin-menu-btn").click(function(){
    adminNavbar.setAttribute('style','display:block !important');    
});


function myFunction2(y) {
    if (y.matches) { // If media query matches
      adminNavbar.setAttribute('style','display:block !important');    
    }
    else{
        adminNavbar.setAttribute('style','display:none !important');    
    }
  }
  
  let y = window.matchMedia("(min-width: 992px)")
  myFunction2(y) // Call listener function at run time
  y.addListener(myFunction2) // Attach listener function on state changes
  