

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

//=================================================================================================
//ShareStory
//=================================================================================================
$(document).ready(function () {
});
//=================================================================================================
//Texteditor of tinymce
//=================================================================================================
var init = tinymce.init({
    selector: 'textarea#default',
    height: 250,
    menubar: false,
    branding: false,
    toolbar: 'bold italic strikethrough | subscript superscript | removeformat ',
    content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px }'
});

var storyDescription = "";
$.when(init).done(function () {
});

//=================================================================================================
//Image upload
//=================================================================================================
var storyFileNames = "";
$(function () {
    $("#dropSection").filedrop({
        fallback_id: 'btnUpload',
        fallback_dropzoneClick: true,

        url: '/Story/Upload',

        //allowedfiletypes: ['image/jpeg', 'image/png', 'image/gif', 'application/pdf', 'application/doc'],
        allowedfileextensions: ['.doc', '.docx', '.pdf', '.jpg', '.jpeg', '.png', '.gif'],
        paramname: 'postedFiles',
        maxfiles: 5, //Maximum Number of Files allowed at a time.
        maxfilesize: 4, //Maximum File Size in MB.
        dragOver: function () {
            $('#dropSection').addClass('img-dropdown-active');
        },
        dragLeave: function () {
            $('#dropSection').removeClass('img-dropdown-active');
        },
        drop: function () {
            $('#dropSection').removeClass('img-dropdown-active');
        },
        uploadFinished: function (i, file, response, time) {
            let currentDate = new Date().toJSON().slice(0, 10).replaceAll("-", "");
            var userId = $(".user-btn")[0].id.slice(9);

            var arr = file.name.split(".");
            fileName = arr[0] + currentDate + userId+ "." + arr[1];

            storyFileNames = storyFileNames.concat("images/uploads/" + fileName + ",");
            
            $('#uploadedFiles').append('<div><span id="img-' + fileName + '" class = "close-img" style = "position: absolute;cursor:pointer;transform: translate(76px,0px);background-color: black;color:white;padding: 0px 3px;border: 1px solid;" > X</span ><img src="/images/uploads/' + fileName + '" class="px-2" style="height:100px;width:100px;" /></div >');
        },
        afterAll: function (e) {
            //To do some task after all uploads done.
            //=================================================================================================
            //close img
            //================================================================================================
            $(".close-img").on("click", function (e) {
                console.log("clicked");
                removeFile = "images/uploads/" + this.id.slice(4) + ",";
                storyFileNames=storyFileNames.replace(removeFile, "");
                this.parentElement.style.display = 'none';
            });
        }
    })

})

//=================================================================================================
//Save story
//=================================================================================================

$("#story-save-btn").on("click", function (e) {
    $("#story-preview-btn").addClass("orange-btn");
    $("#story-preview-btn").prop('disabled',false);
    var userId = $(".user-btn")[0].id.slice(9);
    var missionId = $("#sort-dropdown").val();
    var storyTitle = $("#story-title").val();
    var storyPublishedDate = $("#story-publish-date").val();
    var storyVideoUrl = $("#video-url-textarea").val();
    storyDescription = tinyMCE.activeEditor.getContent();

    //console.log("Mission title:", missionTitle);
    //console.log("Story title:", storyTitle);
    //console.log("Story published date:", storyPublishedDate);
    //console.log("Story description:", storyDescription);
    //console.log("Story video url:", storyVideoUrl);
    //console.log("Story images:", storyFileNames);
    //console.log("User id:", userId);

    $.ajax({
        type: "POST",
        url: '/Story/saveStory',
        data: { userId: userId, missionId: missionId, storyTitle: storyTitle, storyPublishedDate: storyPublishedDate, storyDescription: storyDescription, storyVideoUrl: storyVideoUrl, storyFileNames: storyFileNames },
        success: function (data) {
            var storyId = data["storyId"];
            console.log(storyId);
            $("#preview-link").attr('href', '/Story/StoryDetails?storyId=' + storyId);
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
});

//=================================================================================================
//Submit story
//=================================================================================================

$("#story-submit-btn").on("click", function (e) {
    var userId = $(".user-btn")[0].id.slice(9);
    var missionId = $("#sort-dropdown").val();
    var storyTitle = $("#story-title").val();
    var storyPublishedDate = $("#story-publish-date").val();
    var storyVideoUrl = $("#video-url-textarea").val();
    storyDescription = tinyMCE.activeEditor.getContent();

    //console.log("Mission title:", missionTitle);
    //console.log("Story title:", storyTitle);
    //console.log("Story published date:", storyPublishedDate);
    //console.log("Story description:", storyDescription);
    //console.log("Story video url:", storyVideoUrl);
    //console.log("Story images:", storyFileNames);
    //console.log("User id:", userId);

    $.ajax({
        type: "POST",
        url: '/Story/submitStory',
        data: { userId: userId, missionId: missionId, storyTitle: storyTitle, storyPublishedDate: storyPublishedDate, storyDescription: storyDescription, storyVideoUrl: storyVideoUrl, storyFileNames: storyFileNames },
        success: function (data) {
            window.location="/Story/Index";
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log(error);
        }
    });
});
