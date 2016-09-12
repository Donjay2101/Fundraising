
         //Get all Brochures
function getBrochures(val, ID) {
  
             $.ajax("/Brochure/getBrochures").done(function (data) {
                 $("#Containerw").html("");

                 if (val == 1)
                 {
                     var htmlString = "<div> <div class='row' style='position:relative'><h1 >Copy Brochure</h1>  <span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div><div class='row' style='margin-left:0px;margin-right:0px;padding-left: 20px;padding-top: 10px;'><div class='col-md-6'> <input type='radio' checked value='" + ID + "' id='r1' class='rad1' name='r1' > <span style='color:white;font-weight:bolder;'>Copy to existing brochure </span></div>  <div class='col-md-6'><input type='radio' value='" + ID + "' class='rad1' id='r2' name='r1' ><span style='color:white;font-weight:bolder;'>Copy to new brochure</span> </div> </div><div class='row' style='padding-top: 20px;padding-left: 35px;'><div id='containertable' class='col-md-4' ><span class='popuptext'>Copy Brochure from:</span><select  name='copyFrom' id='copyFrom'><option value='-1'>select--</option>"
                     
                     $.each(data.BrochureList, function (index, val) {
                         htmlString += "<option value=" + val.ID + ">" + val.BrochureID + ' - ' + val.BrochureName + "</option>"
                     })

                     

                     //var htmlString = "<h1>Copy Brochure</h1><table id='containertable' class='header_copyB' style='margin: 3% auto;'><tr> <td> Copy Brochure from:</td> <td>Copy Brochure to: </td></tr> <tr> <td><select  name='copyFrom' id='copyFrom'><option value='-1'>select--</option>"

                     
                     htmlString += "</select></div><div class='col-md-4'><span class='popuptext'>Copy Brochure to:</span><select name='copyTo' id='copyTo'><option value='-1'>select--</option>"
                     $.each(data.BrochureList, function (index, val) {
                         htmlString += "<option value=" + val.ID + ">" + val.BrochureID + ' - ' + val.BrochureName + "</option>";
                     })

                     htmlString += "</select> </div> <div style='margin-top: 20px;' class='col-md-4'><input type='submit' value='submit' name='submit' onclick='copyBrochure(1)' id='copyBrochure'/></div></div></div>";
                     // alert(htmlString);
                     console.log(htmlString);
                     $("#Containerw").append(htmlString);
                     $("#Containerw").removeAttr('class');
                     $("#Containerw").attr('class', 'container1');
                     $("#closed").attr('class', 'closed1');
                     $("#overlayw").css("display", "block");

                     if(ID!="" && ID!=undefined)
                     {
                         $("#copyFrom").val(ID);
                     }
                     $("#returnUrl").val(window.location.href);
                 }
                 else if(val==2)
                 {
                     var htmlString = "<div><div class='row' style='position:relative'><h1 >Copy to new Brochure</h1> <span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div><div class='col-md-2'></div><div class='col-md-10'><div class='row'><div class='row' style='margin-left:0px;margin-right:0px;padding-left: 20px;padding-top: 10px;'><div class='col-md-6' style='width:40%' >  <input type='radio' value='" + ID + "' id='r1' class='rad1' name='r1' > <span style='color:white;font-weight:bolder;'>Copy to existing brochure</span></div>  <div class='col-md-6' style='width:40%'><input type='radio' checked value='" + ID + "' class='rad1' id='r2' name='r1' ><span style='color:white;font-weight:bolder;'>Copy to new brochure</span> </div> </div><div class='row' style='padding-top: 30px; padding-left: 35px'>"

                     //var htmlString = "<h1>Copy Brochure</h1><table id='containertable' class='header_copyB' style='margin: 3% auto;'><tr><td>Borchure ID:</td>";
                     htmlString += "<div class='col-md-6' style='width:40%'><span class='popuptext'>Brochure ID</span><input style='color:black;border-radius: 5px;border:none;' type='text' id='txtBrochureID'/></div><div class='col-md-6' style='width:40%'><span class='popuptext'>Brochure Name</span><input style='color:black;border-radius: 5px;border:none;' type='text' id='txtBrochureName'/></div></div>";
                     htmlString += "<div class='row' style='padding: 20px;padding-left: 35px;'><div class='col-md-10'><span class='popuptext'> Copy Brochure from</span><select  name='copyFrom' id='copyFrom'><option value='-1'>select--</option>";
                     $.each(data.BrochureList, function (index, val) {
                         htmlString += "<option value=" + val.ID + ">" + val.BrochureID + ' - ' + val.BrochureName + "</option>";
                     })

                     htmlString += "</select></div></div><div style='text-align: center;'><input type='submit' value='submit' name='submit' onclick='copyBrochure(2)' id='copyBrochure' style='width:10%;margin-left: -132px;'/></div></div></div></div></div></div>";
                     // alert(htmlString);
                     //console.log(htmlString);
                     $("#Containerw").append(htmlString);
                     $("#Containerw").removeAttr('class');
                     $("#Containerw").attr('class', 'container1');
                     $("#closed").attr('class', 'closed1');
                     $("#overlayw").css("display", "block");

                     if (ID != "" && ID != undefined) {
                         $("#copyFrom").val(ID);
                     }
                     $("#returnUrl").val(window.location.href);
                 }
                 else
                 {
                     //alert('hello');
                     $("#Containerw").html("");
                     var htmlString = "<div><div class='row'><div class='col-md-12'><h1></h1><span style='cursor:pointer;position: absolute;right: 10px;top:-54px;color: white;font-size: 15px;' id='c'>X</span></div></div><div class='row'><div class='col-md-6'><span class='popuptext'>From Brochure</span> <select  name='brochure' id='CopyFromBrochure'><option value='-1'>select--</option>"
                     $.each(data.BrochureList, function (index, val) {
                         htmlString += "<option value=" + val.ID + ">" + val.BrochureID + ' - ' + val.BrochureName + "</option>";
                     });

                     htmlString += "</select></div><div class='col-md-6'><span class='popuptext'>To Brochure</span><select name='category' id='CopyToBrochure'><option value='-1'>select--</option>"
                     $.each(data.BrochureList, function (index, val) {
                         htmlString += "<option value=" + val.ID + ">" + val.BrochureID + ' - ' + val.BrochureName + "</option>";
                     });
                     htmlString += "</select></div></div><div class='row'><div class='col-md-4'></div><div class='col-md-4' style='text-align: center;'><input type='submit'  class='btnclass' value='move products' onclick='CopyCategory()' style='width:168px;margin-top: 20px;' name='submit' id='btnsubmit' /></br></div><div class='col-md-4'></div></div>";
                     htmlString += "<FieldSet style='margin:0 auto;text-align:left;width:50%'><table><tr><td><span><input type='radio' name='gradio' value='1'/>Overwrite existing items currently in the brochure/category. <font color='red'>Careful!</font></span></td></tr>";
                     htmlString += "<tr><td><span><input type='radio' name='gradio' checked value='2'/>Append items to existing brochure/category.</span></td></tr></table></fieldSet></div>";
                     htmlString += "<div id='CategoryContainer' class='row' style=';margin: 0 auto;height:400px;'>";
                     htmlString += "<div class='col-md-5' id='FromBrochureCatTable' style='min-height:300px;;overflow-y:scroll;margin:10px auto;border:1px solid #2E7572;float:left;width: 40%;margin-left: 10px;'></div>";
                     htmlString += "<div class='col-md-2' style='float: left;margin: 10px auto;width: 133px;height:125px;margin-left: 15px;text-align:center;padding-top:10px;' id='control'>";
                     htmlString += "<input type='button' value='&gt;&gt;' class='btnnav' id='wholeCopy'/></br><input class='btnnav' type='button' value='&gt;' id='singleCopy'/></br><input class='btnnav' type='button' value='&lt;' id='singleCopyR'/></br><input class='btnnav' type='button' value='&lt;&lt;' id='wholeCopyR'/></br></div>";

                     

                     htmlString += "<div class='col-md-5' style='overflow-y:scroll;margin: 10px auto;border:1px solid #2E7572;width: 40%;float: right;margin-right: 10px;min-height:300px;overflow-x: hidden;' id='ToBrochureCatTable'></div></div>";
                     console.log(htmlString);
                     $("#Containerw").html(htmlString);

                     $("#Containerw").attr('class', 'container2');
                     $("#closed").attr('class', 'closed2 ');
                     $("#overlayw").css("display", "block");
                 }
               


                 //console.log($("#returnUrl"));
                 //alert(window.location.href);
                 //$("#overlay").click(function (e) {
                 //    e.stopPropagation();
                 //    e.preventDefault();
                 //})
             }).
                 fail(function () {

                 })     
         }


        //copy Category
         function CopyCategory()
         {



             //getting Categories to copy in Brochure
             var categories="";
             $("#B2Table tbody tr ").each(function () {
                 elem = $(this).find("td").eq(0);

                 if (!$(elem).find('input').is(":disabled")) {                     
                     categories += $(elem).find('input').val()+',';
                    
                     // $("#B2Table tbody ").append('<tr class="cpyrow">' + $(this).html() + '</tr>');
                 }
             });

             categories = categories.substring(0, categories.lastIndexOf(','));
             // alert(categories);


             //getting copy to brochure and copyfrom brochure

             //copy to category
             var cpytoCategory='';
             cpytoCategory=$("#tocategory").val();
             if (cpytoCategory != '' && cpytoCategory != undefined) {
                 if (categories != '') {
                     //Copy- From
                     var from = $("#CopyFromBrochure").val();
                     //Copy- To
                     var to = $("#CopyToBrochure").val();


                     //option to select
                     var option = '';
                     var rad = document.getElementsByName('gradio');
                     // alert(option);
                     for (i = 0; i < 2; i++) {
                         // alert(option);
                         if (rad[i].checked) {
                             // alert(option); 
                             option = rad[i].value;
                         }

                     }

                     // alert(option);

                     //CopyCategory

                     $.ajax({
                         url: '/Brochure/CopyCategory',
                         type: 'GET',
                         data: { CopyFromBrochureID: from, copytoBrochureID: to, Categories: categories, Option: option, CopytoCategory: cpytoCategory },
                         contentType: 'application/json; charset=utf-8',
                         success: function (response) {
                             alert("category added to brochure successfully");
                             //  location.href = response;
                         },
                         error: function () {
                             //your error code
                         }
                     });
                 }
             }
             else
             {
                 alert('select category in which you want to copy');
             }
            

         }

     



//Copy Brochure
function copyBrochure(val)
{

  
    copyFrom = $("#copyFrom").val();
    copyTo = $("#copyTo").val();
  //  alert(copyFrom);
   // alert(copyTo);
    if (copyFrom != -1 && copyTo != -1)
    {
        //alert(msg);
        if (val == 1) {

            var msg = "your are about to copy Brochure : " + $("#copyFrom option:selected").text() + " to Brochure: " + $("#copyFrom option:selected").text() + "\n click ok to copy ";

            if (confirm(msg)) {
               // alert('hello');
                $.ajax({
                    url: '/Brochure/copyBrochure',
                    type: 'GET',
                    data: { copyFrom: copyFrom, CopyTo: copyTo, returnUrl: window.location.href },
                    contentType: 'application/json; charset=utf-8',
                    success: function (response) {
                        alert("Brochure  added successfully");
                        window.location.reload();                        
                    },
                    error: function () {
                        //your error code
                    }
                });
            }
            else {
                alert("Process aborted");
            }
        }
        else {
            var BrochureID = $("#txtBrochureID").val();
            var BrochureName = $("#txtBrochureName").val();
            var copyFrom = $("#copyFrom").val();
            if (BrochureID == "" || BrochureName == "")
            {
                alert('enter ID and name for new brochure ');
                return;
            }
            $.ajax({
                url: '/Brochure/CreateBrochureFromBrochure',
                type: 'GET',
                data: { BrochureID: BrochureID, BrochureName: BrochureName, copyFrom: copyFrom },
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    if (response == "0") {
                        alert("BrochureID already exists please try another ID");
                    }
                    else {
                        alert("Brochure  copied successfully");
                        window.location.reload();
                    }
                   
                    //  location.href = response;
                },
                error: function (data) {
                    alert(data.statusText);
                    //your error code
                }
            });
        }
    }
    else
    {
        alert('no brochure or category selected');
    }
    
   
    
}





//Normal Code to close the popup
$(document).on('click','#c',function(){
    $("#overlayw").css("display", "none");
})

//$(document).ready(function () {
//    $('').on('click',,function () {
//        alert('hello');

//    });                              
//});
        






// create container for Map Brochure
function createMapBrochure(brochure,category)
{
    $.ajax("/Brochure/getBrochures").done(function (data) {


        //alert('hello');
        var htmlString = " <div><div class='row'><div class='col-md-12'><h1></h1><span style='cursor:pointer;position: absolute;right: 10px;top:-54px;color: white;font-size: 15px;' id='c'>X</span></div></div><div class='row' style='padding: 40px;padding-left:86px;'> <div class='col-md-5' ><span class='popuptext'>Brochures</span><select  name='brochure' id='brochure'><option value='-1'>select--</option>"
        $.each(data.BrochureList, function (index, val) {
            if (brochure != null && brochure != undefined && brochure == val.BrochureName)
            {
                htmlString += "<option value=" + val.ID + " selected>" + val.BrochureID + ' - ' + val.BrochureName + "</option>";
            }
            else
            {
            htmlString += "<option value=" + val.ID + ">" + val.BrochureID + ' - ' + val.BrochureName + "</option>";
            }
            
        });

        htmlString += "</select></div><div class='col-md-5'><span class='popuptext'>Categories</span></td><td><select name='category' id='category'><option value='-1'>select--</option>"
        $.each(data.CategoryList, function (index, val) {
            if (category != null && category != undefined && category == val.CategoryName) {
               // alert(val.CategoryName);
                htmlString += "<option value=" + val.ID + " selected>" + val.CategoryID + ' - ' + val.CategoryName + "</option>";
            }
            else {
            htmlString += "<option value=" + val.ID + ">" + val.CategoryID + ' - ' + val.CategoryName + "</option>";
            }
            
        });
        htmlString += "</select></div></div><div class='row'><div class='col-md-10' id='prdocutcontainer' style=';margin: 0 auto;width: 75%;min-height:250px;background-color:#EFFAf8;margin-left: 75px;'></div></div>";

        // console.log(htmlString);
        //alert(htmlString);

        $("#Containerw").html(htmlString);

        $("#Containerw").attr('class', 'container2');
        $("#closed").attr('class', 'closed2 ');
        $("#overlayw").css("display", "block");
        if ($("#category").val() != -1)
        {
           // alert('hh');
            selectProducts();
        }

        // $("#returnUrl").val(window.location.href);
        //console.log($("#returnUrl"));
        //alert(window.location.href);
        //$("#overlay").click(function (e) {
        //    e.stopPropagation();
        //    e.preventDefault();
        //})

       
    }).
fail(function () {

});

}


//Code to  map Brochure with Category and ID

function MapBrochure()
{
    categoryID = $("#category").val();
    brochureID = $("#brochure").val();
    if (categoryID != -1 && brochureID != -1)
    {
        var productselem = document.getElementsByName('productchkbox');
        var products = "";
        for (i = 0; i < productselem.length; i++) {
            if (productselem[i].checked) {
                products += productselem[i].value + ',';
            }
        }
        products = products.substring(0, products.lastIndexOf(',', products.length));

        //alert(products);

        $.ajax({
            url: "/Brochure/MapBrochure",
            type: "GET",
            dateType: 'JSON',
            data: { CategoryID: categoryID, BrochureID: brochureID, products: products },
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                alert('Brochure with Category updated successfully');
              //  window.location.reload();
            },
            error: function (response) {
            }
        });
    }
    else
    {
        alert('no brochure or category selected');
    }
   
}













//Events section

//getting categories from serevr for copy From Brochure
$(document).on('change', '#CopyFromBrochure', function () {

    // var CategoryID = $(this).val();
    var BrochureID = $(this).val();
    if (BrochureID != -1) {
        $.ajax({
            url: "/Brochure/GetCategoriesByBrochureID",
            type: 'GET',
            dataType: 'JSON',
            data: { BrochureID: BrochureID },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#FromBrochureCatTable").html("");
                if (data.CategoryList.length > 0)
                {
                    var htmlString = "<table id='B1Table' style='    width:80%;' class='ContainerT'><caption>Categories</caption>"
                    $.each(data.CategoryList, function (index, val) {
                        htmlString += "<tr><td style='width:0%!important;text-align:center'><input type='checkbox' name='CategoryCheckbox'  value='" + val.ID + "'/></td>" +
                            "<td>" + val.CategoryID + "</td>" +
                            "<td>" + val.CategoryName + "</td></tr>";
                    });
                    htmlString += "</table>";
                    $("#FromBrochureCatTable").append(htmlString);
                }
                else
                {
                    alert('no categories available to copy select other brochure');
                }
              

                //$("#prdocutcontainer").html(htmlString);
            },
            error: function () {

            }
        });
    }





});


//getting categories from serevr for copy TO Brochure
$(document).on('change', '#CopyToBrochure', function () {

    // var CategoryID = $(this).val();
    var BrochureID = $(this).val();
    if (BrochureID != -1) {
        $.ajax({
            url: "/Brochure/GetCategoriesByBrochureID",
            type: 'GET',
            dataType: 'JSON',
            data: { BrochureID: BrochureID },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#ToBrochureCatTable").html("");
                if (data.CategoryList.length > 0)
                {
                    var htmlString = "<span>Copy to Category:</span><select id='tocategory' style='width: 94%;margin-top: 2px;margin-left:2px;'>"
                    $.each(data.CategoryList, function (index, val) {
                        htmlString += "<option value='" + val.ID + "'>" + val.CategoryName + "</option>";
                    });
                    htmlString += "</select><table style='width:94%' id='B2Table' class='ContainerT'><caption>Categories</caption></table>";
                    $("#ToBrochureCatTable").append(htmlString);
                }
                else
                {
                    alert('no category available to this brochure select other brochure');
                }
                

                //$("#prdocutcontainer").html(htmlString);
            },
            error: function () {

            }
        });
    }





});


// getting Products of Categories when category drop down changes;

$(document).on('change', '#category', function () {

    var CategoryID = $(this).val();
    var BrochureID = $("#brochure").val();
    if (CategoryID != -1 && BrochureID != -1) {
       // $("#prdocutcontainer").load("/Brochure/getProductsByBrochureID?BrochureID="+BrochureID+"&CAtegoryID="+CategoryID );
        $.ajax({
            url: "/Brochure/getProductsByBrochureID",
            type: 'GET',
            dataType: 'JSON',
            data: { BrochureID: BrochureID, CategoryID: CategoryID },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#prdocutcontainer").html("");

                var htmlString = "<input type='button' name='mapbutton' id='mapbtn'style='border:2px solid #54A792;width: 162px;' onclick='MapBrochure()' class='btnclass' value='add products to category'/> </div><div id='dif'  style='overflow-y: scroll;height: 378px;'>";

                htmlString += "<table id='producttable'><caption>Products</caption>"
                htmlString += "<tr style='font-weight:bold'><td></td><td>Item Number</td><td>Description</td><td>Product type</td></tr>"
                $.each(data.ProductList, function (index, val) {
                    htmlString += "<tr><td><input type='checkbox' " + val.check + " name='productchkbox' value='" + val.ID + "'/></td>" +
                        "<td>" + val.ItemNumber + "</td>" +
                        "<td>" + val.Description + "</td><td>" + val.productType+ "</td></tr>";
                });
                htmlString += "</table>";

                $("#prdocutcontainer").html(htmlString);
            },
            error: function () {

            }
        });
    }



});


//getting rpoducts when Brochure drop down changes index
$(document).on('change', '#brochure', function () {

    var CategoryID = $("#category").val();
    var BrochureID = $(this).val();
    if (CategoryID != -1 && BrochureID != -1) {
        $.ajax({
            url: "/Brochure/getProductsByBrochureID",
            type: 'GET',
            dateType: 'JSON',
            data: { BrochureID: BrochureID, CategoryID: CategoryID },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#prdocutcontainer").html("");

                var htmlString = "<input type='button' name='mapbutton' id='mapbtn' onclick='MapBrochure()' value='add category'/> </div><div id='dif' style='overflow-y: scroll;height: 378px;'>";
                htmlString += "<table id='producttable'><caption>Products</caption>"
                $.each(data.ProductList, function (index, val) {
                    htmlString += "<tr><td><input type='checkbox' " + val.check + " name='productchkbox' value='" + val.ID + "'/></td>" +
                        "<td>" + val.ItemNumber + "</td>" +
                        "<td>" + val.Description + "</td></tr>";
                });
                htmlString += "</table>";

                $("#prdocutcontainer").html(htmlString);
            },
            error: function () {

            }
        });
    }




});




$(document).on('click', '#wholeCopy', function () {
    //alert('hello');
    $("#B2Table tbody  .cpyrow").remove();
    $('#B1Table tbody tr').each(function () {
       // alert('hello');
       // var elem = $(this).find("td").eq(0);
        console.log($(this).html());
        $("#B2Table  ").append('<tr class="cpyrow">' + $(this).html() + '</tr>');

    });

});

$(document).on('click', '#singleCopy', function () {
    //alert('hello');
    $("#B2Table tbody  .cpyrow").remove();
    $('#B1Table tbody tr').each(function () {
        // alert('hello');
        var elem = $(this).find("td").eq(0);                
        if ($(elem).find('input').is(":checked"))
        {
                       
            $("#B2Table  ").append('<tr class="cpyrow">' + $(this).html() + '</tr>');
        }        
    });

});




$(document).on('click', '#wholeCopyR', function () {
  //  alert('hello');
    $("#B2Table tbody  .cpyrow").remove();
    //$('#B1Table tbody tr').each(function () {
    //    // alert('hello');
    //    // var elem = $(this).find("td").eq(0);
    //    console.log($(this).html());
    //    $("#B2Table tbody ").append('<tr>' + $(this).html() + '</tr>');

    //});
});




$(document).on('click', '#singleCopyR', function () {
    //alert('hello');
    
    $('#B2Table tbody tr').each(function () {
        // alert('hello');
        var elem = $(this).find("td").eq(0);
        if ($(elem).find('input').is(":checked") && !$(elem).find('input').is(":disabled")) {
            $(this).remove();
           // $("#B2Table tbody ").append('<tr class="cpyrow">' + $(this).html() + '</tr>');
        }
    });

});

//End Event Section




function selectProducts()
{
    var CategoryID = $("#category").val();
    var BrochureID = $("#brochure").val();
    if (CategoryID != -1 && BrochureID != -1) {
     //   $("#prdocutcontainer").load('/Brochure/getProductsByBrochureID?BrochureID=' + BrochureID + '&CategoryID=' + CategoryID);
        $.ajax({
            url: "/Brochure/getProductsByBrochureID",
            type: 'GET',
            dataType: 'JSON',
            data: { BrochureID: BrochureID, CategoryID: CategoryID },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#prdocutcontainer").html("");

                var htmlString = "<input type='button' name='mapbutton' id='mapbtn' onclick='MapBrochure()' style='width:162px' value='add products to category'/> </div><div id='dif'  style='overflow-y: scroll;height: 378px;'>";

                htmlString += "<table id='producttable'><caption>Products</caption>"
                htmlString+="<tr><td></td><td>Item Number</td><td>Description</td><td>Product type</td></td>"
                $.each(data.ProductList, function (index, val) {
                    
                    htmlString += "<tr><td><input type='checkbox' " + val.check + " name='productchkbox' value='" + val.ID + "'/></td>" +
                        "<td>" + val.ItemNumber + "</td>" +
                        "<td>" + val.Description + "</td>" + "<td>" + val.productType + "</td></tr>";
                });
                htmlString += "</table>";

                $("#prdocutcontainer").load(htmlString);
            },
            error: function () {
           
            }
        });
    }
}




//date 12/09/2015

//function to Add Categories to Brochure


function AddCategory() {
    var id = $("#ID").val();
    $.ajax({
        url: '/Brochure/GetCategoriesByBrochureID',
        type: 'GET',
        data: { BrochureID: id, option: 1 },
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            var htmlString = "<div style='position:relative;background:#006666;width:500px;height: 45px; padding: 5px;'>";
            htmlString += "<input type='button' name='mapbutton' id='mapbtn'style='border:2px solid #54A792;width: 162px;margin: 5px;background: #9AD0C2!important;  color: #2E7572; font-weight: bolder;' onclick='AddCategoryToBrochure()' class='btnclass' value='Update Categories'/><span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div><div id='dif'  style='overflow-y: scroll;height:400px;'>";

            htmlString += "<div style='background: aliceblue;text-align:center;width: 80%;margin: 0 auto;margin-top: 15px; margin-bottom: 15px;'><div class='row' id='producttable' style='margin-right:0px;margin-left:0px;'><h3>Categories</h3></div>"
            htmlString += "<div class='row' style='font-weight:bold;background:#9AD0C2;margin-right:0px;margin-left:0px;'><div class='col-md-3'></div><div class='col-md-7'>Category Name</div></div>"
            $.each(response.CategoryList, function (index, val) {
                htmlString += "<div class='row' ><div class='col-md-3'><input type='checkbox' " + val.check + " name='productchkbox' class='pChkbox' value='" + val.ID + "'/></div>" +
                    "<div class='col-md-7'>" + val.CategoryName + "</div></div>";
            });
            htmlString += "</div></div></div>";
            $('#Containerw').attr('style', 'position: fixed;top: 20%;left: 33%;background: #54A792;');
            $("#Containerw").html(htmlString);

            $("#closed").attr('class', 'closed1');
            $("#overlayw").css("display", "block");
        },

    });
}


function AddCategoryToBrochure()
{

    var BrochureID = $("#ID").val();    
    var elem = $('.pChkbox:checked');
    var Categories="";
    for (i = 0; i < elem.length; i++)
    {
        Categories += elem[i].value + ',';        
    }

    Categories = Categories.substr(0, Categories.lastIndexOf(','));
    
    var url = "/Brochure/MapCategory?BrochureID=" + BrochureID + "&Categories=" + Categories;
    $('#categoryDiv').load(url, function (response, status, xhr) {
        if(status=='success')
        {
            $("#overlayw").css("display", "none");
            alert('Categories updated successfully');
        }       
    });

}






















