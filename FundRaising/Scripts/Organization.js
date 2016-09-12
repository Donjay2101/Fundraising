$(document).on('change', '#Distributor', function () {
    var id = $(this).val();
    //alert('asdi'+id);
    $.ajax({
        url: '/Organization/GetDistributor/' + id,
        Type: 'GET',
        success: function (data) {
            debugger;
            alert('Data will be changed in some options.');
            if(data!=undefined)
            {
                $('#PricingLevel').val(data.PricingLevel);
                $('#GoalType').val(data.GoalType);
                $('#ParticipantOption').val(data.ParticipantOption);
                $('#Catalog').val(data.DefaultProductCatalog);
                $('#ShipToSchoolCatalog').val(data.DefaultSToSCatalog);
                $('#DefaultGoal').val(data.OrganzationGoal);
                var amt = data.FreeShipment;
                $('#FreeShippingAmount').val(amt);
                if(data.CollectTeacher==true)
                {
                    $('input[name=CollectTeacherGrade]').eq(0).attr('checked', 'checked');
                }
                else
                {
                    $('input[name=CollectTeacherGrade]').eq(1).attr('checked', 'checked');
                }

                if (data.CellPhoneRequired == true)
                {
                    $('input[name=CellPhoneRequired]').eq(0).attr('checked', 'checked');
                }
                else {
                    $('input[name=CellPhoneRequired]').eq(1).attr('checked', 'checked');
                }


                if (data.CellPhone == true) {
                    $('input[name=CollectCellPhone]').eq(0).attr('checked', 'checked');
                }
                else {
                    $('input[name=CollectCellPhone]').eq(1).attr('checked', 'checked');
                }


                //$('#ShipToSchoolCatalog').val(data.DefaultSToSCatalog);
            }
        },
        error: function (error) {
            console.log('error occured!'+error);
        }
    });


});



$(document).on('click', '#chkSchoolID', function () {
    var data = checkID();
    if (!data)
    {
        return;
    }
    $("#loader").css('display', 'block');
    var Id =$('#SchoolID').val();
    if (Id != null && Id != "")
    {
        $.ajax({
            url: '/Organization/CheckShoolID?ID=' + Id,
            success: function (data) {
                if(data == "false")
                {                    
                    $('#result').html("School Id available.");
                    $('#result').css("color",'green');
                }
                else
                {                   
                    $('#result').html("School Id already exists.");
                    $('#result').css("color", 'red');
                }
                $("#loader").css('display', 'none');
            },
            error: function (xhr) {
                alert(xhr.statusText);
            }
        });
        
    }
    else
    {
        alert('Student Id is not given.');
    }
    

});

function checkID()
{
    debugger;
    var Id = $('#SchoolID').val();

    if(isNaN(Id))
    {
        alert('School Id should be integer.');
    }
    var num = parseInt(Id);
    count=numDigits(num);
    //var count=0;
    //while (num > 0)
    //{
    //    num = num /10;
    //    count++;
    //}

    if(count!=4)
    {
        alert('School Id can contain only 4 digits.');
        return false;
    }
    else
    {
        return true;
    }


}

function numDigits(x) {
    return Math.max(Math.floor(Math.log10(Math.abs(x))), 0) + 1;
}

function openCampaignPage()
{
    debugger;
    var htmlString = "<div class='row' style='position:relative'><h1>Add Campaign</h1><span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div><div class='row' style='margin:0 auto;width:50%'>";
    htmlString+="<div class='row'>";
    htmlString += "<div class='col-md-4'> Campaign Name  </div> <div> <input type='text' id='txtCampainName' class='text-box single-line'/> </div></div>";
    htmlString+="<div class='row'><div class='col-md-4'> Campaign Start Date </div> <div> <input type='text' id='txtCampainStartDate' class='calendar input-validation-error is-datepick'/> </div></div>";
    htmlString += "<div class='row'><div class='col-md-4'> Campaign End Date </div> <div> <input type='text' id='txtCampainendDate' class='calendar input-validation-error is-datepick'/> </div></div>";
    htmlString += "<div class='row'><div> <input type='button' id='btnAddCampaing' class='calendar input-validation-error is-datepick'/> </div></div>";
    htmlString += "</div></div>";
   
    $("#Containerw").html("");
    $("#Containerw").attr('class', 'container1');
    $("#closed").attr('class', 'closed1');
    $("#Containerw").attr('style', 'width:40%;height:220px;');
    $("#Containerw").html(htmlString);
    $("#overlayw").css('display', 'block');
   
    

}


$(document).on('focus', '.is-datepick', function () {
     debugger;

    $(this).datepick({ dateFormat: 'yyyy-mm-dd' });
    //$('#txtCampainendDate').datepick({ dateFormat: 'yyyy-mm-dd' });
});

//var CampaignObject=function(Name,StartDate,EndDate,Campaign)
//{
//    this.Name = Name;
//    this.StartDate = StartDate;
//    this.EndDate = EndDate;
//    this.Campaign = Campaign;
//}
$(document).ready(function () {
    $('#txtCampainStartDate').datepick({ dateFormat: 'yyyy-mm-dd' });
    $('#txtCampainendDate').datepick({ dateFormat: 'yyyy-mm-dd' });
});

$(document).on('click', '#btnAddCampaing', function () {

    var Name = $('#txtCampaignName').val();
    var StartDate = $('#txtCampaignStartDate').val();
    var EndDate = $('#txtCampaignendDate').val();
    var OrganizatonID = $('#SchoolID').val();

    var campaignobj = new Object();
    campaignobj.CampaignName = Name;
    campaignobj.CampaignStartDate = StartDate;
    campaignobj.CampaignEndDate = EndDate;
    campaignobj.OrganizatonID = OrganizatonID;
    campaignobj.ID = 0;

    $.ajax({
        url: '/Campaign/Create',
        type: 'POST',
        ContentType:"Application/json",
        data:JSON.stringify({model:campaignobj}),        
        success: function () {

        },
        error: function () {

        }


    });

});