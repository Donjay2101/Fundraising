function showLoadingDiv() {
    $('#loader').css('display', 'none');
}
function hideLoadingDiv() {
    $('#loader').css('display', 'none');
}

function getGiftData()
{
    var Name = $('#gftName').val();
    var Email = $('#gftEmail').val();
    var Address = $('#gftAddress').val();
    var City = $('#gftCity').val();
    var State = $('#gftState').val();
    var Zip = $('#gftZip').val();
    var itemID = $('#itemID').val();
    if (Name == "" || Email == "" || Address == "" || City == "" || State == "" || Zip == "") {
        alert("some required fields does not have value, pleas fill all fields.");
        return;
    }

    var obj = {};

    obj.Name = Name;
    obj.Email = Email;
    obj.Address = Address;
    obj.City = City;
    obj.State = State;
    obj.Zip = Zip;
    obj.itemID = itemID;


    sessionStorage.setItem('Gift', JSON.stringify(obj));

    var i = {};
    i=sessionStorage.getItem('Gift');
    console.log(i.Name);
    $('#overlayw').hide();
    //$.ajax({
    //    url: "/Customer/SaveGift",
    //    data: { model: obj },
    //    type: "POST",
    //    success: function (data) {

    //    }

    //});
}
    function Delete(id,controller)
    {
     
            $.ajax({
                url:"/"+controller+"/Delete",
                data:{id:id},
                datatype:"JSON",
                type:"POST",
                success:function(data){
                    alert('data deleted successfully');
                    window.location.reload();
                },
                error: function(){
                    alert('something went wrong please try after sometime');
                }

            });
    }
    

    //$(document).ready(function () {
    //    //alert('hello');
    //    $(document).on("submit", function () {

    //        var url = window.location.href;
    //        splitedUrl = url.split('/');
    //        returnUrl="";
    //        for (i = 3; i < splitedUrl.length;i++)
    //        {
    //            returnUrl += "/" + splitedUrl[i];
    //        }

    //        createCookie('returnUrl',returnUrl);
    //    });
    //})




    function createCookie(name,value, days) {
        var expires = "";
        document.cookie = name + "=" + value + expires + "; path=/";
    }

    function readCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }


    function check(elem) {
        if (elem == undefined || elem == '') {
            return true;
        }
        else {
            return false;
        }
    }


    function GenerateOrder()
    {

        $.ajax({
            url: "/Common/DownloadOrders",
            type: "GET",            
            success: function (data) {
                debugger;
                if (data == 'notfound')
                {
                    alert('No order found for the day');
                }
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }


    function openOrderDownLoadForm()
    {
        debugger;
        htmlString = "<form id='form1' name='form1' method='POST' action='/'><div class='row' style='position:relative'><h1>Orders Download Form</h1><span style='cursor:pointer;position: absolute;right: 10px;top: 10px;color: white;font-size: 15px;' id='c'>X</span></div>";
        htmlString += "<div style='min-height: 613px;overflow-y: scroll;'><table id='orderTable' class='clsSalesTaxTable' style='width:98%;'><tr><th>Report Date</th><th>#Files</th><th>File Name</th><th>DownloadLink</th></tr>";
        $.ajax({
            url: '/Order/downloadData',
            success: function (data) {
                debugger;
                if(data!=null)
                {
                    for (i = 0; i < data.length;i++)
                    {
                        if (data[i].Files > 0)
                        {
                            htmlString += "<tr><td>" + data[i].CreatedDate + "</td><td>" + data[i].Files + "</td><td>" + data[i].FileName + "</td><td><a href='#' class='dlink' onclick='downloadFile(\""+data[i].FileName+"\")'>Download</a></td></tr>"
                        }
                        else
                        {
                            htmlString += "<tr style='background:#C7C7C7'><td>" + data[i].CreatedDate + "</td><td>" + data[i].Files + "</td><td>" + data[i].FileName + "</td><td><span>No Data</span></td></tr>"
                        }
                        
                    }                                    
                }
                else {
                    htmlString += "<td colspan='4'><b>No data available to download.</b></td>"
                }
                htmlString += "</table></div>";
                $("#overlayw").show("slow", function () {
                    $('#Containerw').attr('class', 'container1');
                    $('#Containerw').html(htmlString);
                    $('#Containerw').css('height', '682px');
                    $('#Containerw').css('margin-top', '5%');
                });
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
        
    }

    function downloadFile(name)
    {

        debugger;
        if (name != undefined && name != "")
        {

            document.form1.action = "/order/DownloadFile?name=" + name;
            $('#form1').submit();
        }
        

    }

    $(document).on('click', '.dlink', function () {
        debugger;
        var index = $(this).closest('tr').index();
        $('#orderTable tr').eq(index).css('background', '#868686');
        $('#orderTable tr').eq(index).find('td').eq(3).find('a').css('color', 'white');
        //if (!$(this).find('td').eq(1).html() == '0')
        //{
        //    $(this).css('background', '#868686');
        //    $(this).find('td').eq(3).find('a').css('color', 'white');           
        //}
        

    });



    function ConvertJsonDateToDate(jsonDate)
    {
        var shortDate = null;
        if (jsonDate) {
            var regex = /-?\d+/;
            var matches = regex.exec(jsonDate);
            var dt = new Date(parseInt(matches[0]));
            var month = dt.getMonth() + 1;
            var monthString = month > 9 ? month : '0' + month;
            var day = dt.getDate();
            var dayString = day > 9 ? day : '0' + day;
            var year = dt.getFullYear();
            shortDate = monthString + '/' + dayString + '/' + year;
        }
        return shortDate;
    }



