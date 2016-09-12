   function GetProducts(id)
   {
      // alert(id);
    $.ajax({
        url: '/Category/GetProductsByCategoryID',
        type: 'GET',
        data:{ID:id},
        contentType: 'application/json; charset=utf-8',
        success: function (response) {

            var htmlString = "<div style='text-align: center;width:100%;margin:0px auto;'><div style='position:relative;width:800px;background:#2E7572;padding: 5px;margin:0px auto;margin-top: 3%;'>";
            htmlString += "<input type='button' name='mapbutton' id='mapbtn'style='border:2px solid #54A792;width:162px;background: #9AD0C2!important;  color: #2E7572; font-weight: bolder;'onclick='mapProduct()' class='btnclass' value='Add Products to Category'/><span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div><div id='dif'  style='overflow-y: scroll;height:500px;width:800px;background:#54a792;margin:0px auto;'>";

            htmlString += "<div class='row' id='producttable' style='width:90%;background:aliceblue;margin: 0px auto;margin-top: 15px;margin-bottom: 15px;'><h3>Products</h3>"
            htmlString += "<div class='row' style='font-weight:bold;background: #9AD0C2;margin-right:0px;margin-left:0px;'><div class='col-md-2'></div><div class='col-md-2'>Item Number</div><div class='col-md-4'> Description</div><div class='col-md-4'> Product type</div> </div>"
            $.each(response.ProductList, function (index, val) {
                htmlString += "<div class='row'><div class='col-md-2'><input type='checkbox' " + val.check + " name='productchkbox' value='" + val.ID + "'/></div>" +
                    "<div class='col-md-2'>" + val.ItemNumber + "</div>" +
                    "<div class='col-md-4'>" + val.Description + "</div><div class='col-md-4'>" + val.productType + "</div></div>";
            });
            htmlString += "</table></div></div>";
            $('#Containerw').attr('class', 'ContainerwCategory');
            $("#Containerw").html(htmlString);
            
            $("#closed").attr('class', 'closed1');
            $("#overlayw").css("display", "block");
                    },
        error: function (data) {
            alert(data);
            //your error code
        }
    });
}


function mapProduct()
{
    categoryID = $("#ID").val();    
    if (categoryID != -1 && categoryID != undefined) {
        var productselem = document.getElementsByName('productchkbox');
        var products = "";
        for (i = 0; i < productselem.length; i++) {
            if (productselem[i].checked) {
                products += productselem[i].value + ',';
            }
        }
        products = products.substring(0, products.lastIndexOf(',', products.length));
        //alert(products);
        //alert(categoryID);
        $.ajax({
            url: '/Category/MapProduct',
            type: 'GET',
            data: { CategoryID: categoryID, products: products },
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                var data = response.split('-');
                //alert(data);
                console.log(data);
                if (data[0]== 'done')
                {
                  //  console.log(data);
                    $("#overlayw").css("display", "none");
                    //console.log($('#top_grid'));
                    var url = '/Category/loadProducts?CategoryID=' + data[1];
                  //  alert(url);
                    $("#gridproducts").load(url, function (response, status, xhr) {
                    //    alert('in');
                        if (status == "error") {
                      //      alert('in in');
                            var msg = "Sorry but there was an error: ";
                            Console.log(msg + xhr.status + " " + xhr.statusText);
                            //$("#error").html(msg + xhr.status + " " + xhr.statusText);
                        }
                        else if (status == 'success')
                        {
                            alert('Cateogry updated successfully');
                        }                        
                    });
                }
                
                
            }
        });
    }

}



function CopytoNew(Category, opt) {
   // alert('hello 0');
    if (opt == 1)
    {
       // alert('hello 6');
        var htmlString = "<div style='position:relative;'><h1 style='margin-right:0px;margin-left:0px;'>Copy Category</h1><span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div>";
        htmlString += "<div class='row' style='margin-left=0px;margin-right:0px;margin-top:40px;'><div class='col-md-1'></div><div class='col-md-5'><input type='radio' checked class='rad' name='cr1'value='" + Category + "' id='cr1'/><span style='color:white;font-weight:bolder;'>Copy to new Category</span></div><div class='col-md-5'><input type='radio' class='rad'  name='cr1'value='" + Category + "' id='cr2'/><span style='color:white;font-weight:bolder;'>Copy to existing Category</span></div></div><div class='row' style='margin-top:15px;'><div class='col-md-1'></div><div class='col-md-5'><span style='color:white;font-weight:bolder;'> Category ID</span><input type='text' id='NewID'/></div>"
        htmlString += "<div class='col-md-5'><span style='color:white;font-weight:bolder;'> Category Name</span><input type='text' id='newName'/></div></div>";
        htmlString += "<div class='row' style='margin-top:15px;'><div class='col-md-2'></div><div class='col-md-8 text-center'> <input id='copyCategory' type='button' onclick='CopyCategoryToNew(" + Category + ")' style='width: 87px;'  value='Copy'/></div></div></div>";
        $("#Containerw").html(htmlString);
        $("#Containerw").removeAttr('class');
        $("#Containerw").attr('class', 'container1');
        $("#Containerw").attr('style', 'width:40%');
        $("#closed").attr('class', 'closed1');
        $("#overlayw").css("display", "block");
    }
    else
    {
       // alert('hello');
        $.ajax("/Brochure/getBrochures").done(function (data) {
            var htmlString = "<div style='position:relative;'><h1 style='margin-right:0px;margin-left:0px;'>Copy Category</h1><span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div>";
            htmlString += "<div class='row' style='margin-left=0px;margin-right:0px;margin-top:40px;'><div class='col-md-1'></div><div class='col-md-5'><input type='radio' class='rad' name='cr1'value='" + Category + "' id='cr1'/><span style='color:white;font-weight:bolder;'>Copy to new Category</span></div><div class='col-md-5'><input type='radio' class='rad' checked name='cr1'value='" + Category + "' id='cr2'/><span style='color:white;font-weight:bolder;'>Copy to existing Category</span></div></div><div class='row' style='margin-top:15px;'><div class='col-md-1'></div><div class='col-md-5'><span style='color:white;font-weight:bolder;'> From Category</span><select id='From'>";
           $.each(data.CategoryList, function (index, val) {
                htmlString += "<option value=" + val.ID + ">" + val.CategoryID + ' - ' + val.CategoryName + "</option>"
           })
           htmlString+="</select></div>";
           htmlString += "<div class='col-md-5'><span style='color:white;font-weight:bolder;'>To Category</span><select id='To'><option value='-1;>select--</option>";
             $.each(data.CategoryList, function (index, val) {
                htmlString += "<option value=" + val.ID + ">" + val.CategoryID + ' - ' + val.CategoryName + "</option>"
             })
             htmlString += "</select></div></div>";
             htmlString += "<div class='row' style='margin-top:15px;'><div class='col-md-2'></div><div class='col-md-8 text-center'> <input id='copyExisting' type='button' style='width: 87px;' onclick='CopyCategoryToBrochure(" + Category + ")' value='Copy'/></div></div></div>";
            $("#Containerw").html(htmlString);
            $("#Containerw").removeAttr('class');
            $("#Containerw").attr('class', 'container1');
            $("#Containerw").attr('style', 'width:40%');
            $("#closed").attr('class', 'closed1');
            $("#overlayw").css("display", "block");
            $('#From').val(Category);
        });
    }
   


}

$(document).on('click', '#copyExisting', function () {
        
    var From =$('#From').val();

    var To = $('#To').val();

    alert('From:' + From);
    alert('To:' + To);

    $.ajax({
        url: '/Category/CopyCategory',
        data: { CopyFrom: From, CopyTo: To },
        success: function () {
            alert('Category Copied successfully');
        },
        error: function () {
            alert('not added');
        }
    });

});





$(document).on('click', '.rad', function (){
    
    var catid = $(this).val();
    var ID = $(this).attr('ID');
    //alert(ID);
    if(ID=='cr1')
    {
     //   alert('hello 1');
        CopytoNew(catid,1);
    }
    else
    {
     //   alert('hello 2');
        CopytoNew(catid,2)
    }

})

function CopyCategoryToNew(Category) {
    var ID= $("#NewID").val();
    var Name = $("#newName").val();
    
    $.ajax({
        url: '/Category/CopyCategoryToNew',
        type: 'GET',
        data: { CopyFrom:Category,CategoryID:ID,Name:Name},
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            alert("category added successfully");
            //  location.href = response;
        },
        error: function () {
            //your error code
        }
    });

    window.location.reload();
}


