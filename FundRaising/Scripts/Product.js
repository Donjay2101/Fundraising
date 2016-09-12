
function copyProduct(data)
{
    debugger;
    //var data= $('#editlink').attr('href');
    //var id = data.split('/');

    var id = data.getAttribute('value');



    $.ajax({
        url: "/Product/GetProductByID",
        type: "GET",
        data: {ID:id},
        dataType:"JSON",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if(data.ID!=-1)
            {
                $("#overlayw").css('display', 'block');
                $("#Containerw").html("");
                var htmlString = "<div class='row' style='position:relative'><h1>Copy Product</h1><span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div><div class='row' style='margin:0 auto;width:50%'><span style='color:white;font-weight:bolder;font-size:larger;'> Making copy of product:<b><span id='item_number'>" + data.ItemNumber + "</span> , " + data.Description + "</b></span></br></br><input type='hidden' id='cproductID' value='"+id+"'/></div>";
                htmlString += "<div><div class='row' ><div class='col-md-2'></div><div class='col-md-10'><span style='color:white;font-weight:bolder;'>Enter new <b>Item number</b> for the new Product</span><input type='text' id='txtitemNumber'/><input type='submit' id='btnsubmit' onclick='submitCopyProduct()' style='width: 10%'/></div></div></div>";
                $("#Containerw").attr('class', 'container1');
                $("#closed").attr('class', 'closed1 ');
                $("#Containerw").attr('style', 'width:40%;height:220px;');
                $("#Containerw").html(htmlString);
                
            }
        },
        error: function () {
        }
    });   
}


function submitCopyProduct()
{
    debugger;
    var result = false;
    var id = $("#cproductID").val();
    var itemNumber = $("#txtitemNumber").val();

    result = true;
    //alert(result);

   
        $.ajax({
            url: "/product/SaveProduct",
            type:'POST',
            data: { productID: parseInt(id), itemnumber: itemNumber },                   
            datatype:"JSON",          
            success: function (data) {
                if (data == '-1')
                {
                    alert('item Number exists enter other Item Number');
                }
                else
                {
                    alert("product saved successfully");
                    window.location.reload();
                }                               
            },
            error: function (err) {
                alert(err.statusText);
            }


        });
      
        return result;
    }



function uploadProducts()
{
    $("#overlayw").css('display', 'block');
    $("#Containerw").html("");
    htmlstring = "<div class='row' style='position:relative'><h1>Upload Products</h1><span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div> <div class='row'><br><form  id='uploadForm' ><div class='col-md-3'></div><div class='col-md-4'><input id='fileupload' type='file' name='files[]' multiple style='color: white;'></div> <div class='col-md-3'><input type='button' value='upload' id='btnUpload'/></div>" + "</form></div>";
    $("#Containerw").html(htmlstring);
    $("#Containerw").attr('class', 'container1');
    $("#closed").attr('class', 'closed1');
}


$(document).on('click', '#btnUpload', function () {
    debugger;
    // Checking whether FormData is available in browser  
    if (window.FormData !== undefined) {

        var fileUpload = $("#fileupload").get(0);
        var files = fileUpload.files;

        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            var name = files[i].name;
            var ext = name.substring(name.lastIndexOf('.')+1, name.length);
            if (ext.toUpperCase() == "ZIP")
            {
                fileData.append(files[i].name, files[i]);
            }            
            else
            {
                alert('Zipped Folder needed to upload.');
                return;
            }

            
        }

        $('#loader').css('display','block');


        // Adding one more key to FormData object  
        //    fileData.append('username','Manas');  

        $.ajax({
            url: '/product/UploadProducts',
            type:"POST",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: fileData,           
            success: function (result) {
                alert(result);
                $('#loader').css('display', 'none');
                $("#Containerw").html("");
                $("#overlayw").css('display', 'none');

                $('#data-container').load('/product/Products');
                
            },
            error: function (err) {
                debugger;
                alert(err.statusText);
            }
        });
    } else {
        alert("FormData is not supported.");
    }
   
});

//$(document).ready(function(){  
//    $('#btnUpload').click(function () {  
       
//    });  
//}); 

