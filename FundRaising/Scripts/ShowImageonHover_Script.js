// Declare General Variables 
var sTooTipId = 'divTooTip';
var sToolTipPlacement = "Right"; // Right, Left

// For Hiding tool tip
function HideTooTipImage() {
    // Get the Div element which was created dynamically on ShowToolTipImage function
    var divTip = document.getElementById(sTooTipId);
    if (divTip) {
        while (divTip.childNodes.length > 0)
            // Remove all child content which are added inside on the Div content
            divTip.removeChild(divTip.childNodes[0]);
    }
    // Invisible th Div (which removed all child content)
    divTip.style.visibility = "hidden";

}

function MoveToolTipImage(event) {

    // Verify if the Div content already present?
    if (document.getElementById(sTooTipId)) {

        // Get the Div content 
        var newDiv = document.getElementById(sTooTipId);

        if ('pageX' in event) { // all browsers except IE before version 9
            var pageX = event.pageX;
            var pageY = event.pageY;
        }
        else { // IE before version 9
            var pageX = event.clientX + document.documentElement.scrollLeft;
            var pageY = event.clientY + document.documentElement.scrollTop;
        }

        if (sToolTipPlacement == "Right")
        {
            
           // newDiv.style.bottom = "5px";
            newDiv.style.left = (pageX + 17) + "px";
            
            
        }
            
        else // Left
        {
         //   console.log("pageX " + pageX);
            newDiv.style.left = (pageX - (parseInt(newDiv.style.width) + 17)) + "px";
            //newDiv.style.bottom = "5px";
        }
            

        // Portion of div when hide by browser top
        if ((pageY - (parseInt(newDiv.style.height) + 3)) < 0)
            // Showing below the cursor
            newDiv.style.top = pageY + "px";
        else
            // Showing above the cursor
            newDiv.style.top = (pageY - (parseInt(newDiv.style.height) + 3)) + "px";

        // Finally visibling the div which has the image
        newDiv.style.visibility = "visible";
    }
}
// For showing tool tip
function ShowToolTipImage(event,url,opt) {
    //var url = '';
    var newDiv = null;
    console.log('in show tool tip');
    //url = url;
    if (opt == 0)
    {
        
        console.log('in show tool tip with option 0');

        console.log("0=" + url);

        // Verify if the Div content already present?
        if (!document.getElementById(sTooTipId)) {

            // If not create a new Div element
            newDiv = document.createElement("div");
            // Set the id for the Div element to refer further
            newDiv.setAttribute("id", sTooTipId);

            // Add it to the page
            document.body.appendChild(newDiv);

        }
        else {
            // Get the Div content which was invisible on HideTooTipImage function
            newDiv = document.getElementById(sTooTipId);
        }


        // Here the pId is the id of the image + a flag
        // (0 - when no image stored on the database or
        // 1 - when image stored on the in database)
        // which indicate whether the image required to show or not
        //if (pId.split(",")[1] == 'False ')
        //    url = "Images/ImageNotAvailable.jpg";
        //else

       
        var strImageContent =
        '<div class="border_preview"> ' +
        '   <div id="loader_container"> ' +
        '       <div id="loader"> ' +
        '           <div align="center">Loading image preview...</div> ' +
        '           <div id="loader_bg"> ' +
        '               <div id="progress"> </div>' +
        '           </div> ' +
        '       </div> ' +
        '   </div> ' +
        '   <div class="preview_temp_load"> ' +
        '       <img id="previewImage" onload="javascript:remove_loading();" src="' + url + '" border="0" style="width:100%!important;height:100%!important"> ' +
        '   </div> ' +
        '</div>';
        newDiv.innerHTML = strImageContent;

        newDiv.style.zIndex = 2;
        newDiv.style.width = "340px";
        newDiv.style.height = "80px";

        newDiv.style.position = "absolute";

        if ('pageX' in event) { // all browsers except IE before version 9
            var pageX = event.pageX;
            var pageY = event.pageY;
        }
        else { // IE before version 9
            var pageX = event.clientX + document.documentElement.scrollLeft;
            var pageY = event.clientY + document.documentElement.scrollTop;
        }


        if (sToolTipPlacement == "Right")
            newDiv.style.left = (pageX + 17) + "px";
        else // Left
            newDiv.style.left = (pageX - (parseInt(newDiv.style.width) + 17)) + "px";

        // Portion of div when hide by browser top
        if ((pageY - (parseInt(newDiv.style.height) + 3)) < 0)
            // Showing below the cursor
            newDiv.style.top = pageY + "px";
        else
            // Showing above the cursor
            newDiv.style.top = (pageY - (parseInt(newDiv.style.height) + 3)) + "px";

        // Finally visibling the div which has the image
        newDiv.style.visibility = "visible";
    }
    else
    {
        console.log('in show tool tip with option 1'+opt);
        var newDiv = null;
        var oldDiv = document.getElementById("overlay");

        var imgContent = '<span id="close"><span style=" cursor: pointer;">X</span>' +
             '</span>';

        imgContent += '<div id="imgdiv">' +
        '<img id="mainimg" style="width:100%;height:100%;z-index:100;" src=\'' + url + '\' alt="No Image Available"/>' +
        '</div>';
        oldDiv.innerHTML = imgContent;

        oldDiv.style.display = 'block';
    }
    
}

var t_id = setInterval(animate, 20);
var pos = 0;
var dir = 2;
var len = 0;

function animate() {
    var elem = document.getElementById('progress');
    if (elem != null) {
        if (pos == 0) len += dir;
        if (len > 32 || pos > 79) pos += dir;
        if (pos > 79) len -= dir;
        if (pos > 79 && len == 0) pos = 0;
        elem.style.left = pos + "px";
        elem.style.width = len + "px";
    }
}
function remove_loading() {
    var targelem = document.getElementById('loader_container');
    targelem.style.display = 'none';
    targelem.style.visibility = 'hidden';
}



$(document).ready(function () {
    $("#close").click(function (e) {
        
        $("#overlay").empty();
        $("#overlay").hide();
        //$("#overlay").css('display', 'none');
    });


    $('html').on('click', '#overlay> #imgdiv', function (e) {
        e.stopPropagation();
    });    
    $('html').on('click', '#overlay', function () {
        $("#overlay").empty();
        $("#overlay").hide();
    });  
});