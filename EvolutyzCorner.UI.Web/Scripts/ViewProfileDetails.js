function LoadTitle(id) {
  
    $.ajax({
        type: "GET",
        url: "/Profile/GetUserTitle",
        datatype: "Json",
        async: false,
        data: { id: id },
        success: function (data) {

            $(data).each(function () {
                $("#titleid").append($("<option></option>").val(this.Usr_Titleid).html(this.TitlePrefix));
            });
            $('#titleid option[value=' + id + ']').attr('selected', true);


        }
    });
}

function LoadGender(id) {

    $.ajax({
        type: "GET",
        url: "/Profile/GetUserGender",
        datatype: "Json",
        async: false,
        data: { id: id },
        success: function (data) {

            $(data).each(function () {
                $("#genderid").append($("<option></option>").val(this.Usr_GenderId).html(this.Gender));
            });
            $('#genderid option[value=' + id + ']').attr('selected', true);


        }
    });
}



var Profiledata = ''; var UserTitle = '', UserGender='';
$(document).ready(function () {
    GetViewProfileDetails();
    $("#DObId").datepicker({
        dateFormat: 'yy-mm-dd',
        maxDate: '0'
    });

    //$("#DOJId").datepicker({
    //    dateFormat: 'yy-mm-dd',
    //    maxDate: '0',
       
    //});

   // document.getElementById("DOJId").readOnly = true;


});

var loadFile = function (event) {
    var output = document.getElementById('imgLogo');
    output.src = URL.createObjectURL(event.target.files[0]);
    alert($('#imgLogo').attr('src')); 
    $('#imgLogo').show();
};




function GetViewProfileDetails() {
    $.ajax({
        url: "/Profile/GetViewProfileDetails",
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        success: function (resultData) {

            $("#frstNameid").val(resultData.UsrPFirstName);
            $("#Lstnameid").val(resultData.UsrPLastName);
            $("#Empid").val(resultData.UsrP_EmployeeID);
            $("#Emailid").val(resultData.UsrP_EmailID);
            $("#MobNumid").val(resultData.UsrP_MobNum);
            $("#PhnoNumid").val(resultData.UsrP_PhoneNumber);
            $("#DObId").val(resultData.UsrP_DOB);
            $("#DOJId").val(resultData.UsrP_DOJ);
            UserTitle = resultData.Usr_TitleId;
            LoadGender(resultData.Usr_GenderId);
            LoadTitle(UserTitle);
                        
            var filepath = "http://" + resultData.UsrP_ProfilePicture;
       
           
            $("#imgLogo").attr("src", filepath);
            console.log($("#imgLogo").attr("src", filepath));
              // URL.createObjectURL(filepath);                
            $('#imgLogo').show();
          

        },
    });
}
///////////////////////////////////////////////validations////////////////////


var Dobirth = new Date($('#DObId').val());
var Dojoing= new Date($('#DOJId').val());

if (Dojoing > Dobirth) {
    alert("DOJ should Less than DOB");
  
    $('#DOJId').val('');
}