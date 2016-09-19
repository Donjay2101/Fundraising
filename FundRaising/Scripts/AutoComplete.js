/// <reference path="Common.js" />
$(document).ready(function () {

    $('#SchoolName').autocomplete({
        minLength: 3,
        source: function (request, response) {

            $.ajax({
                url: "/Reports/SearchSchool",
                type: "GET",
                dataType: "JSON",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.Name, id: item.SchoolID };
                    }))
                }
            });
        },
        messages: {
            noResults: "", results: ""
        },
        select: function (event, ui) {
            var v = ui.item.id;
            $('#SchoolID').val(v);
            $.ajax({
                url: "/Reports/Getcampaigns?ID=" + v,
                type: "GET",
                success: function (data) {
                    if (data != null) {
                        debugger;
                        var htmlString = "<option  value='-1'>please select--</option>";
                        for (i = 0; i < data.length; i++) {
                            htmlString += "<option value='" + data[i].ID + "'>" + data[i].CampaignName + "</option>";
                        }
                        $('#cmbCampaign').html(htmlString);
                    }
                },
                error: function () {

                }
            });

            //$('#SchoolName').val(ui.item.Name);

        }
    });


    $('#SchoolID').autocomplete({
        minLength: 3,
        source: function (request, response) {

            $.ajax({
                url: "/Reports/SearchSchool",
                type: "GET",
                dataType: "JSON",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.SchoolID, id: item.Name };
                    }))
                }
            });
        },
        messages: {
            noResults: "", results: ""
        },
        select: function (event, ui) {
            var v = ui.item.id;
            $('#SchoolName').val(v);
            // $('#SchoolID')
            $.ajax({
                url: "/Reports/Getcampaigns?ID=" + ui.item.value,
                type: "GET",
                success: function (data) {
                    if (data != null) {
                        debugger;
                        var htmlString = "<option  value='-1'>please select--</option>";
                        for (i = 0; i < data.length; i++) {
                            htmlString += "<option value='" + data[i].ID + "'>" + data[i].CampaignName + "</option>";
                        }
                        $('#cmbCampaign').html(htmlString);
                    }
                },
                error: function () {

                }
            });

            //$('#SchoolName').val(ui.item.Name);

        }
    });
});