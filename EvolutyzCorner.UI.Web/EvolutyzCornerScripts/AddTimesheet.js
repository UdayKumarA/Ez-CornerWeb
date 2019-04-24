
var dt = new Date();
var presentyear = dt.getFullYear();
var currMon = dt.getMonth();

$(document).ready(function () {
    $("#timesheet_management").addClass("active");
    $("#timesheet_management > ul.treeview-menu > li:last-child").addClass("click");
    $("#timesheet_management").siblings().removeClass("active");


   

});

function LoadClientDetails() {
    $.ajax({
        type: "GET",
        url: "/Timesheet/Usertimesheet",
        datatype: "Json",
        success: function (data) {

        }
    });
}



function addInput() {

    var selectHTML = "";
    selectHTML = "<select style='width:100%' name='uc1$ddlTask' id='uc1_ddlTask'>";
    for (i = 0; i < choices.length; i = i + 1) {
        selectHTML += "< option value= '" + choices[i] + "' > " + choices[i] + "</option > ";
    }
    selectHTML += "</select > ";
    newDiv.innerHTML = selectHTML;
    document.getElementById(divName).appendChild(newDiv);
}




$(document).ready(function (e) {

   
    var years = [presentyear - 5, presentyear - 4, presentyear - 3, presentyear - 2, presentyear - 1, presentyear];
    var option = '';

    for (i = 0; i < years.length; i++) {

       var option = document.createElement("OPTION");
        option.text = years[i];
        option.value = years[i];

       // // if year is 2019 selected
        if (years[i] == presentyear) {
            option.selected = true;
        }
        $('#year').append(option);
    }
    
    LoadMonthAccToYear();

    $('#year').on('change', function () {
        LoadMonthAccToYear();

    });

});

   


function LoadMonthAccToYear() {
    
    var option = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var currMon = (new Date).getMonth();
    var currYear = (new Date).getFullYear();
    var id = $('#year').val();
    $('#month').html('');

    if (currYear == id && currMon) {

        for (i = 0; i <= currMon; i++) {
            $('#month').append($("<option />").val(option[i]).text(option[i]));
            //option.text =i;
            //option.value = i;

            if (i == currMon) {
                $('#month').val(option[i]);
            }
           
        }

      
    }
    else {
        for (i = 0; i <= 11; i++) {
            $('#month').append($("<option />").val(option[i]).text(option[i]));
            if (i == currMon) {
                $('#month').val(option[i]);
            }
        }
       
    }
    
}






   //var optn;
    //var d = new Date();
    //var curmon = d.getMonth();


    //var curYear = d.getFullYear();

    //for (y = 2000; y <= 2020; y++) {
    //    var optn = document.createElement("OPTION");
    //    optn.text = y;
    //    optn.value = y;

    //    // if year is 2015 selected
    //    if (y == curYear) {
    //        optn.selected = true;
    //    }

    //    document.getElementById('year').options.add(optn);

    //}

    //var monthArray = new Array();
    //monthArray[0] = "Jan";
    //monthArray[1] = "Feb";
    //monthArray[2] = "Mar";
    //monthArray[3] = "Apr";
    //monthArray[4] = "May";
    //monthArray[5] = "Jun";
    //monthArray[6] = "Jul";
    //monthArray[7] = "Aug";
    //monthArray[8] = "Sept";
    //monthArray[9] = "Oct";
    //monthArray[10] = "Nov";
    //monthArray[11] = "Dec";
    //for (m = 0; m <= 11; m++) {
    //    optn = document.createElement("OPTION");
    //    optn.text = monthArray[m];

    //    optn.value = (m + 1);

    //    if (curmon == m) {
    //        optn.selected = true;
    //    }

    //    document.getElementById('month').options.add(optn);

    //}


    //$('#month').prop('Select Month', curmon);