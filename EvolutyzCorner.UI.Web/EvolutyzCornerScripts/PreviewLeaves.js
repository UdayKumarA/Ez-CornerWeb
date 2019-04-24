///// <reference path="jquery-1.10.2.js" />
// <reference path="bootstrap.min.js" />

var TimesheetID = '', fulldate = '';
var resultDataArray; var objUserSessionId = '';

$(document).ready(function () {


    $("#tabCon").empty();
    $("#body_ClientDetails").attr("style", "display: none;");
    $("#divEditTimesheetData").attr("style", "display: none;");
    $('#loading-image').attr("style", "display: block;");
    $.ajax({
        url: "/LeaveApplicationManagement/GetLeavePreview",
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        // cache: false,
        success: function (resultData) {


            if (resultData.RoleName) {
                objUserSessionId = resultData.RoleName;

                if ((objUserSessionId === 'Super Admin') || (objUserSessionId === 'Admin') || (objUserSessionId === 'Manager')) {

                    $("#UserGridData").attr("style", "display: table;");
                    $("#ManagerGridData").attr("style", "display: table;");
                    if (resultData.myleaves) {
                        var objUsertimesheets = resultData.myleaves;
                        $('#UserGridData').DataTable({
                            // 'destroy': true,
                            'data': objUsertimesheets,
                            'paginate': true,
                            'sort': true,
                            'Processing': true,
                            'columns': [
                                { 'data': 'Usrl_UserId', 'visible': false },
                                { 'data': 'Usrl_LeaveId', 'visible': false },
                                { 'data': 'accntmail', 'visible': false},
                                { 'data': 'ProjectName' },
                                { 'data': 'userName' },

                                {
                                    'data': 'LeaveStartDate'
                                    //"type": "date ",
                                    //"render": function (value) {
                                    //    if (value === null) return "";
                                    //    return dateConversion(value);
                                    //}

                                },
                                {
                                    'data': 'LeaveEndDate',
                                    //"type": "date ",
                                    //"render": function (value) {
                                    //    if (value === null) return "";                                      
                                    //    return dateConversion(value);
                                    //}
                                },
                                { 'data': 'No_Of_Days' },
                                {
                                    'data': "LeaveApprovalStatus",
                                    "data": function (data) {
                                        return '<span class="badge badge-radius" data-toggle="tooltip" title="' + data.Leavestatus + '"></span>';
                                    }
                                },                               
                            ]
                        });




                    }

                    if (resultData.leavesforapproval) {
                        var objManagertimesheets = resultData.leavesforapproval;
                      

                        $('#ManagerGridData').DataTable({
                            //   'destroy': true,
                            'data': objManagertimesheets,
                            'paginate': true,
                            'sort': true,
                            'Processing': true,
                            'columns': [
                                { 'data': 'Usrl_UserId' },
                                { 'data': 'Usrl_LeaveId' },
                                { 'data': 'accntmail', 'visible': false },
                                { 'data': 'ProjectName' },
                                { 'data': 'userName' },
                                { 'data': 'LeaveStartDate' },
                                { 'data': 'LeaveEndDate' },
                                { 'data': 'No_Of_Days' },
                                { 'data': 'ManagerID1', 'visible': false },
                                { 'data': 'ManagerName1', 'visible': false},
                                { 'data': 'ManagerEmail1', 'visible': false },
                                { 'data': 'UserEmail', 'visible': false },
                                {
                                    'data': "LeaveApprovalStatus",
                                    "data": function (data) {
                                        return '<span class="badge badge-radius" data-toggle="tooltip" title="' + data.Leavestatus + '"></span>';
                                    }
                                },
                                {
                                    "render": function (Usrl_LeaveId, type, full, meta) {
                                        if (full.Leavestatus === 'Approved') {
                                            return null;

                                            //return '<i  class="fa fa-edit" title="Edit"></i>';
                                        }
                                        else {
                                            //return '<i id="ApproveID" class="fa fa-check" title="Approve"></i> <i id="RejectId" title="Reject" class="fa fa-times" ></i> '
                                            return '<a class="btn btn-icn"   id="AcceptLeave" data-accountmail="' + full.accntmail + '" data-username="' + full.userName + '" data-mngrid="' + full.ManagerID1 + '" data-mngrname= "' + full.ManagerName1 + '" data-mngrmail="' + full.ManagerEmail1 + '"  data-UserId="' + full.Usrl_UserId + '" data-status="4"  data-LeaveStartDate="' + full.LeaveStartDate + '" data-LeaveEndDate="' + full.LeaveEndDate + '"  data-LeaveId="' + full.Usrl_LeaveId + '" data-usermail="' + full.UserEmail + '"   onclick="userleaveconsumed(this)" ><i id="Rejected" class="fa fa-check" title="Approve"></i> </a><a class="btn btn-icn"   id="RejectLeave"   data-accountmail="' + full.accntmail + '" data-username="' + full.userName + '" data-mngrid="' + full.ManagerID1 + '" data-mngrname= "' + full.ManagerName1 + '" data-mngrmail="' + full.ManagerEmail1 + '"  data-UserId="' + full.Usrl_UserId + '" data-status="5"  data-LeaveStartDate="' + full.LeaveStartDate + '" data-LeaveEndDate="' + full.LeaveEndDate + '"  data-LeaveId="' + full.Usrl_LeaveId + '" data-usermail="' + full.UserEmail +'"   onclick="userleaveconsumed(this)" ><i id="RejectId" title="Reject" class="fa fa-times" ></i>';
                                        }
                                    },
                                },
                         
                            ]
                        });
                    }

                }
                else {
                    $("#ManagerGridData").attr("style", "display: none;");
                    $("#UserGridData").attr("style", "display: table;");
                    $("#ManagerGridPanel").attr("style", "display: none;");
                    if (resultData.myleaves) {
                        var objUsertimesheet = resultData.myleaves;
                        $('#UserGridData').DataTable({
                            // 'destroy': true,
                            'data': objUsertimesheet,
                            'paginate': true,
                            'sort': true,
                            'Processing': true,
                            'columns': [
                                { 'data': 'Usrl_UserId', 'visible': false },
                                { 'data': 'Usrl_LeaveId', 'visible': false },
                                { 'data': 'accntmail', 'visible': false },
                                { 'data': 'ProjectName' },
                                { 'data': 'userName' },
                                {
                                    'data': 'LeaveStartDate',
                                    //"type": "date ",
                                    //"render": function (value) {
                                    //    if (value === null) return "";
                                    //    return dateConversion(value);
                                    //}
                                },
                                {
                                    'data': 'LeaveEndDate',
                                    //"type": "date ",
                                    //"render": function (value) {
                                    //    if (value === null) return "";
                                    //    return dateConversion(value);
                                    //}
                                },
                                { 'data': 'No_Of_Days' },
                                {
                                    'data': "LeaveApprovalStatus",
                                    "data": function (data) {
                                        return '<span class="badge badge-radius" data-toggle="tooltip" title="' + data.Leavestatus + '"></span>'
                                    }
                                },
                                //{
                                //    "render": function (Usrl_LeaveId, type, full, meta) {
                                //        if ((full.Leavestatus === 'Approved') || (full.Leavestatus === 'Rejected') || (full.Leavestatus === 'On Hold')) {

                                //            return '<i  class="fa fa-edit" title="Edit"></i>';
                                //        }
                                //        else {
                                //            return '<i id="ApproveID" class="fa fa-check" title="Approve"></i> <i id="RejectId" title="Reject" class="fa fa-times" ></i> ';
                                           
                                //        }
                                //    },
                                //},
                            
                            ]
                        });
                    }


                }

            }



        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        },

        error: function (data) {
            //  alert(data.responseText);
        }
    });
    function dateConversion(value) {
        if (value === null) return "";
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));

        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();

    }

});


function calculateSum() {

    var sum = 0;
    $(".uc1txtHours").each(function () {
        //add only if the value is number
        if (!isNaN(this.value) && this.value.length !== 0) {
            sum += parseFloat(this.value);
        }
    });
    //.toFixed() method will roundoff the final sum to 2 decimal places
    $("#TotalHoursCount").html(sum.toFixed(2));
}


function userleaveconsumed(id) {
    debugger;
    var Userid = id.getAttribute("data-UserId");
    var LeaveStartDate = id.getAttribute("data-LeaveStartDate");    
    var LeaveEndDate = id.getAttribute("data-LeaveEndDate");
    var LeaveId = id.getAttribute("data-LeaveId");
    var accountmail = id.getAttribute("data-accountmail");
    var Leavestatusid = id.getAttribute("data-status");
    var mgrid = id.getAttribute("data-mngrid");
    var mgrname = id.getAttribute("data-mngrname");
    var mgrmail = id.getAttribute("data-mngrmail");
    var usrmail = id.getAttribute("data-usermail");

   // Leavstatusid = Leavestatus;

    $.ajax({
       // contentType: "application/json",
        type: "POST",
        url: "/LeaveApplicationManagement/WebLeaveApproval",
        data: {
            "Userid": Userid,
            "LeaveStartDate": LeaveStartDate,
            "LeaveEndDate": LeaveEndDate,
            "leaveid": LeaveId,
            "accntmail": accountmail,
            "Leavestatus": Leavestatusid,
            "ManagerId": mgrid,
            "ManagerName": mgrname,
            "ManagerMail": mgrmail,
            "UserMail": usrmail
        },
        success: function (data) {

            alert(data);
            window.location = '/LeaveApplicationManagement/PreviewLeaves';

        }
    });


}



