var States = "'AL','AK','AZ','AR','CA','CO','CT','DE','FL','GA','HI','ID','IL','IN','IA','KS','KY','LA','ME','MD','MA','MI','MN','MS','MO','MT','NE','NV','NH','NJ','NM','NY','NC','ND','OH','OK','OR','PA','RI','SC','SD','TN','TX','UT','VT','VA','WA','WV','WI','WY'";
var statesWithFullName="'Alabama(AL)','Alaska(AK)','Arizona(AZ)','Arkansas(AR)','California(CA)','Colorado(CO)','Connecticut (CT) ','Delaware (DE) ','Florida (FL) ',	'Georgia (GA) ','Hawaii (HI) ',	'Idaho (ID) ','Illinois (IL) ',	'Indiana (IN) ','Iowa (IA) ',	'Kansas (KS) ',	'Kentucky (KY) ',	'Louisiana (LA) ',	'Maine (ME) ',	'Maryland (MD) ',	'Massachusetts (MA) ',	'Michigan (MI) ',	'Minnesota (MN) ',	'Mississippi (MS) ',	'Missouri (MO) ',	'Montana (MT) ',	'Nebraska (NE) ',	'Nevada (NV) ',	'New Hampshire (NH) ',	'New Jersey (NJ) ',	'New Mexico (NM) ',	'New York (NY) ',	'North Carolina (NC) ',	'North Dakota (ND) ',	'Ohio (OH) ',	'Oklahoma (OK) ',	'Oregon (OR) ',	'Pennsylvania (PA) ',	'Rhode Island (RI) ',	'South Carolina (SC) ',	'South Dakota (SD) ',	'Tennessee (TN) ',	'Texas (TX) ',	'Utah (UT) ',	'Vermont (VT) ',	'Virginia (VA) ',	'Washington (WA) ',	'West Virginia (WV) ',	'Wisconsin (WI) ',	'Wyoming (WY)'";

var TotalAmount = 0;
    var openShippingChargeForm = function () {
        debugger;
        var htmlString = "<div class='row' style='position:relative'><h1>Shipping Charge Distribution</h1><span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div>" +
        "<div class='row text-center' style='margin:0'><div class='col-md-2'></div><div class='col-md-8'><label style='color:white'>Free shipping amount</label><span></span><input type='number' min='0' id='txtmaxamount' name='txtmaxamount'/><input type='button' class='btnsubmit btnSalestax' id='btnSaveFreeShippingAmount' value='set amount' style='width: 93px;margin-left: 8px;'/></div></div>" +
        "<div class='row' style='margin-top:20px' >" +
        "<div class='col-md-3 text-center'>" +
        "<div class='row'><label style='color:white'>Lower Limit</label></div><div class='row'><input type='number' min='0' id='txtLowerLimit' /></div></div>" +
        "<div class='col-md-3 text-center'>" +
        "<div class='row'><label style='color:white'>Upper Limit</label></div><div class='col-md-3'><input type='number' min='0' id='txtUpperLimit' /></div></div>" +
        "<div class='col-md-3 text-center'>" +
        "<div> <label style='color:white'>Shipping Charge</label></div><div class='rows'><input type='number' min='0' id='txtShippingCharge' /></div></div>" +
         "<div class='col-md-3'>" +
    "<div class='row'><input type='button' class='btnsubmit btnSalestax' value='save' id='btnshippingsubmit' style='width: 60%;margin-top: 25px;'/></div>" +
        "</div></div>" +

        "<div class='row text-center' style='padding-left: 40px;    padding-left: 40px;height: 106px;margin-top:20px;'><div class='col-md-2'></div><div class='col-md-7' style='  overflow-y: scroll; overflow-x: hidden;'><table border='1' id='ShippingChargeTable'> " +
        "<tr><th>Limit</th><th>Shipping Charge</th><th></th></tr>";
        var amount = 0;
       

        $.ajax({
            url: "/Common/GetShippingcharge",
            type: "GET",
            success: function (data) {
                debugger;
                if(data!="null")
                {
                    var htmlstr = "";
                    for (i = 0; i < data.length;i++)
                    {
                        //debugger;
                        //htmlstr = "<tr><td>" + accounting.formatMoney(LowerLimit, "$") + "-" + accounting.formatMoney(upperlimit, "$") + "</td><td>" + accounting.formatMoney(ShippingCharge, "$") + "</td><td><a href='#' class='deleteRow'><span value='" + data[i].ID * + "'>Delete</span></a></td></tr>";
                        htmlString += "<tr><td>" + accounting.formatMoney(data[i].LowerLimit, "$") + "-" + accounting.formatMoney(data[i].UpperLimit, "$") + "</td><td>" + accounting.formatMoney(data[i].Charge, "$") + "</td><td><a href='#' class='deleteRow'><span value='" + data[i].ID + "'>Delete</span></a></td></tr>";

                        ///$('#ShippingChargeTable').append(htmlstr);
                    }
                    
                    amount=data[0].FreeAmount;
                    
                    
                }
                htmlString += "</table></div></div>";
                
            },
            error: function (err) {
               alert(err.statusText);
            }
        });
        $("#overlayw").show("slow", function () {
            $('#Containerw').attr('class', 'container1');
            $('#Containerw').html(htmlString);
            $('#Containerw').css('height', '400px');
            $('#txtmaxamount').val(amount);
        });
        
    }



    $(document).on('click', '#btnSaveFreeShippingAmount', function () {
        var amount = $('#txtmaxamount').val();
        
        $.ajax({
            url: '/Common/SaveFreeShippingAmount',
            data: { Amount: amount },
            type: 'POST',
            success: function (data) {
                if(data=='1')
                {
                    alert('Free amount updated successfully');
                }
                else
                {
                    alert(data);
                }
            },
            error: function (err) {
                alert(err.StatusText);
            }
        });
        
    });

    var ShippingCharge = function (lowerLimit, upperLimit, charge, freeAmount) {
        this.LowerLimit = lowerLimit;
        this.UpperLimit = upperLimit;
        this.Charge = charge;
        this.FreeAmount = freeAmount;
        //  return this;
    }

    $(document).on('click', '#btnshippingsubmit', function () {
        var LowerLimit = $('#txtLowerLimit').val();
        var upperlimit = $('#txtUpperLimit').val();
        var mshippammount = $('#txtmaxamount').val();        
        var Shippingcharge = $('#txtShippingCharge').val();
        debugger;
        var table = $('#ShippingChargeTable');
        var length = $('#ShippingChargeTable tr').length;
        var limit;
        var shippingCharge;
        //var ShippingChargeArr = [];
        //var freemt = $('#txtmaxamount').val();
        for (i = 1; i < length; i++) {
            limit = $('#ShippingChargeTable tr').eq(i).find('td').eq(0).html();
            var limitdata = limit.split('-');
            if (LowerLimit >= parseFloat(accounting.unformat(limitdata[0])) && LowerLimit <= parseFloat(accounting.unformat(limitdata[1])) || upperlimit>= parseFloat(accounting.unformat(limitdata[0])) && upperlimit <= parseFloat(accounting.unformat(limitdata[1])))
            {
                alert('limit has already been saved remove the previous to enter this limit.');
                return;
            }

             
            
        }
        //TotalAmount += parseFloat(upperlimit);
       
        //if (upperlimit > mshippammount)
        //{
        //    alert('upper limit exceeds minimum free shipping amount');
        //    return;
        //}

        //if (check(mshippammount))
        //{
        //    alert('enter minimum free shipping amount');
        //    return;
        //}

        //if (check(upperlimit)) {
        //    alert('upper limit missing');
        //    return;
        //}

        //if (check(LowerLimit)) {
        //    alert('lower limit missing');
        //    return;
        //}

        //if (check(ShippingCharge)) {
        //    alert('shipping charge missing');
        //    return;
        //}
        var val = new ShippingCharge(LowerLimit, upperlimit, Shippingcharge, mshippammount);
        var jsonstring = JSON.stringify(val);
        $.ajax({
            url: "/Common/ShippingChargeSave",
            type: "POST",
            data: { model: jsonstring },
            dataType: "json",
            success: function (data) {
                if (data != "0") {
                    //$('#loader').css('display', 'none');
                    //$("#overlayw").hide("slow", function () {
                    //    //$('#Containerw').attr('class', 'container1');
                    //    $('#Containerw').html("");
                    //});
                    htmlstring = "<tr><td>" + accounting.formatMoney(LowerLimit, "$") + "-" + accounting.formatMoney(upperlimit, "$") + "</td><td>" + accounting.formatMoney(Shippingcharge, "$") + "</td><td><a href='#' class='deleteRow'><span value='" + data+ "'>Delete</span></a></td></tr>";
                    $('#ShippingChargeTable').append(htmlstring);
                }
                else {
                    $('#loader').css('display', 'none');
                    alert('not valid request');
                }
            },
            error: function (err) {
                $('#loader').css('display', 'none');
                alert(err.statusText);
            }
        });
        
       
        

    });

  

    $(document).on('click', '.deleteRow', function () {
            
        debugger;
       var idx=$(this).closest('tr').index();

       id = $('#ShippingChargeTable').find('tr').eq(idx).find('td').eq(2).find('span').attr('value');
        $.ajax({
            url: "/Common/DeleteShippingCharge",
            data:{ID:id},
            type: 'POST',
            success: function (data) {
                debugger;
                if (data == "1")
                {
                    $('#ShippingChargeTable').find('tr').eq(idx).remove();
                    //$(this).closest('tr').remove();
                }
                
            },
            error: function () {

            }

        });
    });



    $(document).on('click','#btnSaveShippingCharge',function(){
        debugger;
        $('#loader').css('display', 'block');
        var table = $('#ShippingChargeTable');
        var length = $('#ShippingChargeTable tr').length;
        var limit;
        var shippingCharge;
        var ShippingChargeArr = [];
        var freemt=$('#txtmaxamount').val();
        for (i = 1; i < length;i++)
        {
            limit = $('#ShippingChargeTable tr').eq(i).find('td').eq(0).html();
            var limitdata = limit.split('-');

            shippingCharge = $('#ShippingChargeTable tr').eq(i).find('td').eq(1).html();
            var val=new ShippingCharge(accounting.unformat(limitdata[0]), accounting.unformat(limitdata[1]), accounting.unformat(shippingCharge), parseFloat(freemt));
            ShippingChargeArr.push(val);
        }

        var jsonstring = JSON.stringify(ShippingChargeArr);
        $.ajax({
            url: "/Common/ShippingChargeSave",            
            type: "POST",            
            data: { model: jsonstring },            
            dataType: "json",
            success: function (data) {
                if(data=="1")
                {
                    $('#loader').css('display', 'none');
                    $("#overlayw").hide("slow", function () {
                        //$('#Containerw').attr('class', 'container1');
                        $('#Containerw').html("");
                    });
                }
                else
                {
                    $('#loader').css('display', 'none');
                    alert('not valid request');
                }
            },
            error: function (err) {
                $('#loader').css('display', 'none');
                alert(err.statusText);
            }
        });
        
    })


    function DeleteconfirmSales(id, name) {
        var d = confirm("Are you sure you want delete the Brochure with Brochure ID: " + name);
        if (d) {
            $.ajax({
                url: "/Common/DeleteSalesTax",
                type: "POST",
                data:{ID:id},
                success: function (data) {
                    alert('Deleted successfully.');
                    openSalesTaxAddForm();
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
           // DeleteSalesTax(id, 'Common');
        }
    }

    function OpenSalesTaxForm()
    {
       // debugger;
        var htmlString = "<div class='row' style='position:relative'><h1>Sales Tax Form</h1><span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div>"+
            "<div style='text-align: center;height: 192px;width: 98%;margin: 0 auto;'><table class='clsSalesTaxTable' id='SalesTaxTable' align='center'><thead><tr><td>State</td><td>Tax Amount</td><td>Action</td><td>Delete</td></tr></thead>";

        $.ajax({
            url: "/Common/SalesTaxes",
            success: function (data) {
                debugger;
                htmlString += "<tbody>";
                for (i = 0; i < data.length; i++) {
                    htmlString += "<tr><td>" + data[i].State + "</td><td>" + data[i].TaxAmount + "</td><td><input type='checkbox' id='SalesTaxchkbx' "+ data[i].Checked+ " value='" + data[i].ID + "' /></td><td><a href='#' onclick='DeleteconfirmSales(\"" + data[i].ID + "\",\"" + data[i].State + "\")'>Delete</a></td></tr>";
                }
                htmlString += "</tbody></table></div><hr style='width:100%;border-color:#2E7572;margin:0'/><div style='text-align: right;margin-right: 8px;height: 17%;/* border: 1px solid; */line-height: 2;padding-top: 8px;'>  <input type='button' class='btnSalestax'  id='btnsetSalexTax' value='Set SalesTax'/><input type='button'  id='btnAddNewSalesTax' class='btnSalestax' value='Add New'/></div>";
                $("#overlayw").show("slow", function () {
                    $('#Containerw').attr('class', 'container1');
                    $('#Containerw').html(htmlString);
                });
                //htmlString=htmlString + htmlstr;
            },
            error: function () {

            }

        });
       // htmlString+=tableString;
        
       // var htmlStr;
      
        //console.log(htmlString);
        //console.log(htmlStr);
        //htmlString = htmlString + htmlStr;
       
    }

    function GetSalesData()
    {
        debugger;
        //htmlString = "<table class='clsSalesTaxTable' id='SalesTaxTable' align='center'><thead><tr><td>State</td><td>Tax Amount</td><td>Action</td><td>Delete</td></tr></thead>";
        $.ajax({
            url: "/Common/SalesTaxes",
            success: function (data) {
                debugger;
                htmlString += "<tbody>";
                for (i = 0; i < data.length; i++) {
                    htmlString += "<tr><td>" + data[i].State + "</td><td>" + data[i].TaxAmount + "</td><td><input type='checkbox' id='SalesTaxchkbx' " + data[i].Checked + " value='" + data[i].ID + "' /></td><td><a href='#' onclick='DeleteconfirmSales('" + data[i].ID + "','" + data[i].State + "')'>Delete</a></td></tr>";
                }
                htmlString += "</tbody></table></div><hr style='width:100%;border-color:#2E7572;margin:0'/><div style='text-align: right;margin-right: 8px;height: 17%;/* border: 1px solid; */line-height: 2;padding-top: 8px;'>  <input type='button' class='btnSalestax'  id='btnsetSalexTax' value='Set SalesTax'/><input type='button'  id='btnAddNewSalesTax' class='btnSalestax' value='Add New'/></div>";
                return htmlString;
                //htmlString=htmlString + htmlstr;
            },
            error: function () {

            }

        });
    }


    $(document).on('click', '#btnAddNewSalesTax', function () {

        openSalesTaxAddForm();
    });



    function openSalesTaxAddForm()
    {
        var data = $('#Containerw').html();

        sessionStorage.setItem('salestax', data);

        StateDoprDown = "<select id='stateDropdown'><option value='-1'>select--</option><option value='AL'>Alabama (AL) </option>	<option value='AK'>Alaska (AK) </option>	<option value='AZ'>Arizona (AZ) </option>	<option value='AR'>Arkansas (AR) </option>	<option value='CA'>California (CA) </option>	<option value='CO'>Colorado (CO) </option>	<option value='CT'>Connecticut (CT) </option>	<option value='DE'>Delaware (DE) </option>	<option value='FL'>Florida (FL) </option>	<option value='GA'>Georgia (GA) </option>	<option value='HI'>Hawaii (HI) </option>	<option value='ID'>Idaho (ID) </option>	<option value='IL'>Illinois (IL) </option>	<option value='IN'>Indiana (IN) </option>	<option value='IA'>Iowa (IA) </option>	<option value='KS'>Kansas (KS) </option>	<option value='KY'>Kentucky (KY) </option>	<option value='LA'>Louisiana (LA) </option>	<option value='ME'>Maine (ME) </option>	<option value='MD'>Maryland (MD) </option>	<option value='MA'>Massachusetts (MA) </option>	<option value='MI'>Michigan (MI) </option>	<option value='MN'>Minnesota (MN) </option>	<option value='MS'>Mississippi (MS) </option>	<option value='MO'>Missouri (MO) </option>	<option value='MT'>Montana (MT) </option>	<option value='NE'>Nebraska (NE) </option>	<option value='NV'>Nevada (NV) </option>	<option value='NH'>New Hampshire (NH) </option>	<option value='NJ'>New Jersey (NJ) </option>	<option value='NM'>New Mexico (NM) </option>	<option value='NY'>New York (NY) </option>	<option value='NC'>North Carolina (NC) </option>	<option value='ND'>North Dakota (ND) </option>	<option value='OH'>Ohio (OH) </option>	<option value='OK'>Oklahoma (OK) </option>	<option value='OR'>Oregon (OR) </option>	<option value='PA'>Pennsylvania (PA) </option>	<option value='RI'>Rhode Island (RI) </option>	<option value='SC'>South Carolina (SC) </option>	<option value='SD'>South Dakota (SD) </option>	<option value='TN'>Tennessee (TN) </option>	<option value='TX'>Texas (TX) </option>	<option value='UT'>Utah (UT) </option>	<option value='VT'>Vermont (VT) </option>	<option value='VA'>Virginia (VA) </option>	<option value='WA'>Washington (WA) </option>	<option value='WV'>West Virginia (WV) </option>	<option value='WI'>Wisconsin (WI) </option>	<option value='WY'>Wyoming (WY)</option></select>";

        var htmlString = "<div class='row' style='position:relative'><h1>Sales Tax Add Form</h1><span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div>" +
            "<div class='row' style='    margin: 0 auto;width: 74%;'><div class='col-md-4 text-center'><div class='row' style='color:white'>State</div><div class='row'>" + StateDoprDown + "</div></div>" +
            "<div class='col-md-4 text-center'><div class='row ' style='color:white'>Amount</div><div class='row'><input type='number' id='txtTaxamount' /></div></div>" +
            "<div class='col-md-4 text-center'><div class='row' style='color:white'>Action</div><div class='row'><input type='button' class='btnSalestax' value='Add Tax' id='btnsaveTaxadd' style='width:80px'/></div></div></div>" +
            "<hr width='100%' style='margin:8px;border-color:#2E7572;border-color: #A5ADAC;margin-left: 0px;'/><div style='text-align: center;height: 192px;width: 98%;overflow-y: scroll;margin: 0 auto;'><table class='clsSalesTaxTable' id='SalesTaxTable' align='center'><thead><tr><td>State</td><td>Tax Amount</td><td>Action</td><td>Delete</td></tr></thead>";
        $.ajax({
            url: "/Common/SalesTaxes",
            success: function (data) {
                debugger;
                htmlString += "<tbody>";
                for (i = 0; i < data.length; i++) {
                    htmlString += "<tr><td>" + data[i].State + "</td><td>" + data[i].TaxAmount + "</td><td><input type='checkbox' class='chkbx' id='SalesTaxchkbx' "+data[i].Checked+" value='" + data[i].ID + "' /></td><td><a href='#' onclick='DeleteconfirmSales(\"" + data[i].ID + "\",\"" + data[i].State + "\")'>Delete</a></td></tr>";
                }
                htmlString += "</tbody></table></div><hr style='width:100%;border-color:#2E7572;margin:0;border-color: #A5ADAC'/><div style='text-align: right;margin-right: 8px;height: 17%;/* border: 1px solid; */line-height: 2;padding-top: 8px;'>  <input type='button' class='btnSalestax'  id='btnsetSalexTax' value='Set SalesTax'/></div>";
                htmlString += "</div>";
                $("#overlayw").show("slow", function () {
                    $('#Containerw').attr('class', 'container1');
                    $('#Containerw').css('height', '356px');
                    $('#Containerw').html(htmlString);
                });
                //$('#Containerw').html(htmlString);
                
                //htmlString=htmlString + htmlstr;
            },
            error: function () {

            }

        });
       
    }

    var SalesTaxObject=function(State,amount)
    {
        this.State = State;
        this.TaxAmount = amount;
        this.Active = false;
    }

    $(document).on('click', '#btnsaveTaxadd', function () {
        var stateDropdown = $('#stateDropdown').val();

        if (stateDropdown == -1) {
            return;
        }
        var Amount = $('#txtTaxamount').val();
        if (Amount == null || Amount == "")
        {
            return;
        }

        var obj = new SalesTaxObject(stateDropdown, Amount);

       addTax(obj)

    });


    function addTax(data)
    {
        data = JSON.stringify(data);
        $.ajax({
            url: '/Common/AddTax',
            type: "POST",
            data: { model: data },
            success: function (d) {
                if(d=="1")
                {
                    alert("Tax added successfully.");
                }
                openSalesTaxAddForm();
            },
            error: function () {

            }

            });
    }


    $(document).on('click', '#btnsetSalexTax', function () {
        debugger;
        var id;
        var length = $('#SalesTaxTable tr').length;
        for (i = 1; i < length; i++) {
            if ($('#SalesTaxTable tr').eq(i).find('td').eq(2).find('input[type=checkbox]').is(':checked')) {
                id = $('#SalesTaxTable tr').eq(i).find('td').eq(2).find('input[type=checkbox]').attr('value');
                break;
            }
        }

        $.ajax({
            url: "/Common/SetSalesTax",
            data: { ID: id },
            type:"POST",
            success: function (data) {
                alert('Sales Tax region changed successfully');
            },
            error: function (err) {
                alert(err.statusText);
            }
        });


    });

    $(document).on('click', '.chkbx', function () {
        debugger;
       index=$(this).closest('tr').index();
       getCheckedITem(index+1);
    });

    function getCheckedITem(index)
    {
        
        var length = $('#SalesTaxTable tr').length;
        for (i = 1; i < length; i++) {
            //var res = $('#SalesTaxTable tr').eq(i).find('td').eq(2).find('input[type=checkbox]').attr('checked');
            if ($('#SalesTaxTable tr').eq(i).find('td').eq(2).find('input[type=checkbox]').is(':checked')) {
                $('#SalesTaxTable tr').eq(i).find('td').eq(2).find('input[type=checkbox]').removeAttr('checked');
            }
        }

        $('#SalesTaxTable tr').eq(index).find('td').eq(2).find('input[type=checkbox]').attr('checked', 'checked');
        $('#SalesTaxTable tr').eq(index).find('td').eq(2).find('input[type=checkbox]').prop('checked', 'true');
        
        
        
    }

  




