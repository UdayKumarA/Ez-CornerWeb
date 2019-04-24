///// <reference path="jquery-1.10.2.js" />
// <reference path="bootstrap.min.js" />

var TimesheetID = '', fulldate = '';
var resultDataArray; var objUserSessionId = '';
var resultHoursWorkedColour; var ApporRejprojectid;
$(document).ready(function () {

    $("#tabCon").empty();
    $("#body_ClientDetails").attr("style", "display: none;");
    $("#divEditTimesheetData").attr("style", "display: none;");
    $('#loading-image').attr("style", "display: block;");
    $.ajax({
        url: "/Timesheet/GetPreviewTimesheets",
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        // cache: false,
        success: function (resultData) {

            if (resultData.roleid) {
                objUserSessionId = resultData.roleid

                if ((objUserSessionId == '1001') || (objUserSessionId == '1002') || (objUserSessionId == '1007')) {
                    $("#UserGridData").attr("style", "display: table;");
                    $("#ManagerGridData").attr("style", "display: table;");
                    if (resultData.mytimesheets) {
                        var objUsertimesheets = resultData.mytimesheets;
                        $('#UserGridData').DataTable({
                            // 'destroy': true,
                            'data': objUsertimesheets,
                            'paginate': true,
                            'sort': true,
                            'Processing': true,
                            'columns': [
                                { 'data': 'Usr_UserID', 'visible': false, },
                                { 'data': 'TimesheetID', 'visible': false, },
                                { 'data': 'ProjectId', 'visible': false, },
                                { 'data': 'ProjectName' },
                                { 'data': 'userName' },
                                { 'data': 'Month_Year' },
                                { 'data': 'CompanyBillingHours' },
                                { 'data': 'ResourceWorkingHours' },
                                {
                                    "data": "TimesheetApprovalStatus",
                                    "data": "ManagerApprovalStatus",
                                    "data": function (data) {
                                        return '<span class="badge badge-radius" data-toggle="tooltip" id="' + data.TimesheetApprovalStatus + '" title="' + data.ManagerApprovalStatus + '" ></span>'
                                    }
                                },
                                {
                                    "render": function (TimesheetID, type, full, meta) {
                                        if ((full.TimesheetApprovalStatus == 'Saved Timesheet') || (full.TimesheetApprovalStatus == 'Rejected') || (full.TimesheetApprovalStatus == 'Rejected at Level_1 Manager') || (full.TimesheetApprovalStatus == 'Rejected at Level_2 Manager')) {

                                            return '<a class="btn btn-icn"   data-MonthYear="' + full.Month_Year + '" data-TimesheetID="' + full.TimesheetID + '"  data-Usr_UserID="' + full.Usr_UserID + '"   onclick="EditUser(this)" ><i  class="fa fa-edit" title="Edit"></i></a>';
                                        }
                                        else {
                                            return '<a class="btn btn-icn"  id="PreviewManagerUser"   data-MonthYear="' + full.Month_Year + '" data-TimesheetID="' + full.TimesheetID + '" data-TimesheetID="' + full.TimesheetID + '" data-ProjectId="' + full.ProjectId + '" data-Usr_UserID="' + full.Usr_UserID + '"  title="Preview" data-TimesheetApprovalStatus="' + full.TimesheetApprovalStatus + '"  onclick="PreviewSubmitTimesheet(this)" ><i class="fa fa-eye"></i></a>';
                                        }
                                    },
                                },
                            ]
                        });

                    }

                    if (resultData.timesheetsforapproval) {
                        var objManagertimesheets = resultData.timesheetsforapproval;

                        $('#ManagerGridData').DataTable({
                            //   'destroy': true,
                            'data': objManagertimesheets,
                            'paginate': true,
                            'sort': false,
                            'Processing': true,
                            'columns': [
                                { 'data': 'Usr_UserID', 'visible': false, },
                                { 'data': 'TimesheetID', 'visible': false, },
                                { 'data': 'ProjectId', 'visible': false, },
                                { 'data': 'ProjectName' },
                                { 'data': 'userName' },
                                { 'data': 'Month_Year' },
                                { 'data': 'CompanyBillingHours' },
                                { 'data': 'ResourceWorkingHours' },
                                {
                                    "data": "TimesheetApprovalStatus",
                                    "data": "ManagerApprovalStatus",
                                    "data": function (data) {
                                        return '<span class="badge badge-radius" data-toggle="tooltip" id="' + data.TimesheetApprovalStatus + '" title="' + data.ManagerApprovalStatus + '"></span>'
                                    }
                                },
                                {
                                    "render": function (TimesheetID, type, full, meta) {

                                        if ((full.TimesheetApprovalStatus == "Approved By Level_1 Manager") || (full.TimesheetApprovalStatus == "Approved By Level_2 Manager")
                                            || (full.TimesheetApprovalStatus == "Approved By Level_1 Manager and Pending at Level_2 Manager") || (full.TimesheetApprovalStatus == "Pending at Level_1 Manager and Approved by Level_2 Manager")
                                            || (full.TimesheetApprovalStatus == "Approved by Both Managers")) {

                                            return '<a class="btn btn-icn"   id="PreviewManager"  data-MonthYear="' + full.Month_Year + '" data-TimesheetID="' + full.TimesheetID + '"  data-Usr_UserID="' + full.Usr_UserID + '" data-TimesheetApprovalStatus="' + full.TimesheetApprovalStatus + '"  data-ProjectId="' + full.ProjectId + '" title="Preview" onclick="PreviewManagerSubmitTimesheet(this)" ><i class="fa fa-eye"></i> </a><a class="btn btn-icn"   id="Accepttimesheet"  data-MonthYear="' + full.Month_Year + '" data-TimesheetID="' + full.TimesheetID + '"  data-Usr_UserID="' + full.Usr_UserID + '" data-TimesheetApprovalStatus="' + full.TimesheetApprovalStatus + '"   data-Status="3" data-ProjectId="' + full.ProjectId + '" title="Preview" onclick="ApprovalRejectTimesheet(this)" ></a><a class="btn btn-icn"    id="Rejecttimesheet"  data-MonthYear="' + full.Month_Year + '" data-TimesheetID="' + full.TimesheetID + '"  data-Usr_UserID="' + full.Usr_UserID + '" data-TimesheetApprovalStatus="' + full.TimesheetApprovalStatus + '"  data-ProjectId="' + full.ProjectId + '"  data-Status="4" onclick="ApprovalRejectTimesheet(this)" > </a>';

                                        }

                                        else {

                                            return '<a class="btn btn-icn"   id="PreviewManager"  data-MonthYear="' + full.Month_Year + '" data-TimesheetID="' + full.TimesheetID + '"  data-Usr_UserID="' + full.Usr_UserID + '" data-TimesheetApprovalStatus="' + full.TimesheetApprovalStatus + '" data-ProjectId="' + full.ProjectId + '"  title="Preview" onclick="PreviewManagerSubmitTimesheet(this)" ><i class="fa fa-eye"></i> </a><a class="btn btn-icn"   id="Accepttimesheet" data-ProjectName="' + full.ProjectName + '"  data-userName="' + full.userName + '" data-CompanyBillingHours="' + full.CompanyBillingHours + '" data-ResourceWorkingHours="' + full.ResourceWorkingHours + '"  data-MonthYear="' + full.Month_Year + '" data-TimesheetID="' + full.TimesheetID + '"  data-Usr_UserID="' + full.Usr_UserID + '" data-TimesheetApprovalStatus="' + full.TimesheetApprovalStatus + '"  data-ProjectId="' + full.ProjectId + '"  data-Status="3" title="Preview" onclick="ManagerApprOrRej(this)" ><i id="ApproveID" class="fa fa-check" title="Approve"></i> </a><a class="btn btn-icn"    id="Rejecttimesheet" data-ProjectName="' + full.ProjectName + '"  data-userName="' + full.userName + '" data-CompanyBillingHours="' + full.CompanyBillingHours + '" data-ResourceWorkingHours="' + full.ResourceWorkingHours + '" data-MonthYear="' + full.Month_Year + '" data-TimesheetID="' + full.TimesheetID + '"  data-Usr_UserID="' + full.Usr_UserID + '" data-TimesheetApprovalStatus="' + full.TimesheetApprovalStatus + '"  data-ProjectId="' + full.ProjectId + '"  data-Status="4" onclick="ManagerApprOrRej(this)" ><i id="RejectId" title="Reject" class="fa fa-times" ></i> </a>';

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
                    if (resultData.mytimesheets) {
                        var objUsertimesheets = resultData.mytimesheets;
                        $('#UserGridData').DataTable({
                            // 'destroy': true,
                            'data': objUsertimesheets,
                            'paginate': true,
                            'sort': true,
                            'Processing': true,
                            'columns': [
                                { 'data': 'Usr_UserID', 'visible': false, },
                                { 'data': 'TimesheetID', 'visible': false, },
                                { 'data': 'ProjectId', 'visible': false, },
                                { 'data': 'ProjectName' },
                                { 'data': 'userName' },
                                { 'data': 'Month_Year' },
                                { 'data': 'CompanyBillingHours' },
                                { 'data': 'ResourceWorkingHours' },
                                {
                                    "data": "TimesheetApprovalStatus",
                                    "data": "ManagerApprovalStatus",
                                    "data": function (data) {
                                        //alert(data.TimesheetApprovalStatus);
                                        return '<span class="badge badge-radius" data-toggle="tooltip" id="' + data.TimesheetApprovalStatus + '" title="' + data.ManagerApprovalStatus + '"></span>'
                                    }
                                },
                                {
                                    "render": function (TimesheetID, type, full, meta) {
                                        if ((full.TimesheetApprovalStatus == 'Saved Timesheet') || (full.TimesheetApprovalStatus == 'Rejected') || (full.TimesheetApprovalStatus == 'Rejected at Level_1 Manager') || (full.TimesheetApprovalStatus == 'Rejected at Level_2 Manager')) {
                                            return '<a class="btn btn-icn"   data-MonthYear="' + full.Month_Year + '" data-TimesheetID="' + full.TimesheetID + '"  data-Usr_UserID="' + full.Usr_UserID + '" data-ProjectId="' + full.ProjectId + '" onclick="EditUser(this)" ><i class="fa fa-edit" title="Edit"></i></a>';
                                        }

                                        else {

                                            return '<a class="btn btn-icn" id="PreviewUser"   data-MonthYear="' + full.Month_Year + '" data-TimesheetID="' + full.TimesheetID + '" data-Usr_UserID="' + full.Usr_UserID + '" data-ProjectId="' + full.ProjectId + '" title="Preview"  onclick="PreviewSubmitTimesheet(this)" ><i class="fa fa-eye"></i></a>';

                                        }

                                    },

                                },

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
    })

});

var TaskDetailsid = ''; var MonthTimesheet = ''; var tsuserid = '', AppRejectstatus = '';
function EditUser(id) {
    $("#body_ClientDetails").attr("style", "display: table;");
    TimesheetID = id.getAttribute("data-TimesheetID");
    MonthTimesheet = id.getAttribute("data-MonthYear");
    tsuserid = id.getAttribute("data-Usr_UserID");
    $("#divPreviewTimesheet").attr("style", "display: none;");
    $("#divEditTimesheetData .btn-app").attr("style", "display: none;");
    $("#divEditTimesheetData").attr("style", "display: block;");
    $('#loading-image').attr("style", "display: block;");
    $.ajax({
        url: "/Timesheet/ViewSubmitedTimesheet",
        type: "GET",
        data: { TimesheetMonth: MonthTimesheet, TimesheetUserid: tsuserid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        async: false,
        success: function (resultData) {
            resultDataArray = resultData;
            LoadColoursonLeavesandHolidays(MonthTimesheet, tsuserid)
            ViewTimesheetByMonth(MonthTimesheet, resultData);
            LoadPreviewTasklookups();
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        }

    });
    LoadPreviewTaskData();
}


function PreviewSubmitTimesheet(id) {
    $("#body_ClientDetails").attr("style", "display: table;");
    $("#btnPrint").show();
    $("#btnpdf").show();
    $("#tabCon").empty();
    $("#MainTable").empty();
    $("#SubTable").empty();
    //LoadClientDetails();
    //$("#ClientDetails2").show(true);
    TimesheetID = id.getAttribute("data-TimesheetID");
    MonthTimesheet = id.getAttribute("data-MonthYear");
    AppRejectstatus = id.getAttribute("data-TimesheetApprovalStatus");
    $("#divEditTimesheetData .btn-app").attr("style", "display: none;");
    $("#divPreviewTimesheet").attr("style", "display: none;");
    $("#divEditTimesheetData").attr("style", "display: block;");
    tsuserid = id.getAttribute("data-Usr_UserID");
    $('#loading-image').attr("style", "display: block;");
    $.ajax({
        url: "/Timesheet/ViewSubmitedTimesheet",
        type: "GET",
        data: { TimesheetMonth: MonthTimesheet, TimesheetUserid: tsuserid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        async: false,
        // cache: false,

        success: function (resultData) {
            resultDataArray = resultData;
            LoadColoursonLeavesandHolidays(MonthTimesheet, tsuserid);
            PreviewTimesheetsByMonth(MonthTimesheet, resultData);
            LoadPreviewTasklookups();
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        }

    });
    LoadData();
}


function PreviewManagerSubmitTimesheet(id) {
    $("#body_ClientDetails").attr("style", "display: table;");
    $("#btnPrint").show();
    $("#btnpdf").show();
    $("#tabCon").empty();
    $("#MainTable").empty();
    $("#SubTable").empty();
    //LoadClientDetails();
    //$("#ClientDetails2").show(true);
    TimesheetID = id.getAttribute("data-TimesheetID");
    MonthTimesheet = id.getAttribute("data-MonthYear");
    AppRejectstatus = id.getAttribute("data-TimesheetApprovalStatus");
    $("#divEditTimesheetData .btn-app").attr("style", "display: none;");
    $("#divPreviewTimesheet").attr("style", "display: none;");
    $("#divEditTimesheetData").attr("style", "display: block;");
    tsuserid = id.getAttribute("data-Usr_UserID");
    ApporRejprojectid = id.getAttribute("data-ProjectId");
    var myvariable = $('#UserGridData a').attr('id');
    var myManagerGridData = $('#ManagerGridData a').attr('id');

    if (myManagerGridData) {

        if ((AppRejectstatus == "Approved By Level_1 Manager") || (AppRejectstatus == "Approved By Level_2 Manager")
            || (AppRejectstatus == "Approved By Level_1 Manager and Pending at Level_2 Manager") || (AppRejectstatus == "Pending at Level_1 Manager and Approved by Level_2 Manager")
            || (AppRejectstatus == "Approved by Both Managers")) {

            $("#btnApprove").attr("style", "display:none;");
            $("#btnReject").attr("style", "display:none;");

        }

        else {
            $("#btnApprove").attr("style", "display: inline-block;");
            $("#btnReject").attr("style", "display: inline-block;");
        }
    }
    $('#loading-image').attr("style", "display: block;");
    $.ajax({
        url: "/Timesheet/ViewSubmitedTimesheet",
        type: "GET",
        data: { TimesheetMonth: MonthTimesheet, TimesheetUserid: tsuserid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        async: false,
        success: function (resultData) {
            resultDataArray = resultData;
            LoadColoursonLeavesandHolidays(MonthTimesheet, tsuserid);
            PreviewTimesheetsByMonth(MonthTimesheet, resultData);

            LoadTasklookupsByUser(tsuserid);
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        }


    });
    LoadManagerData();

}


function PreviewTimesheetsByMonth(MonthTimesheet, resultData) {

    $("#tabCon").empty();
    $("#MainTable").empty();
    $("#SubTable").empty();
    var tbltask = ''; var objSelect = '';
    $('#Cmtsave').show();
    $("#ClientDetails2").show(true);
    var counter = -1;
    var counter15 = -1;

    $("#tabCon").append("<table name='maintbl' id='MainTable' width='100%' /*style='position:relative;left:10%;'*/>");


    $.each(resultData, function (k, v) {

        $("#txtDescription").html(v.Comments);

        if (v.ApprovedDate != '') {
            $("#ts_Approveid").html(v.ApprovedDate);
            $("#ts_approid").show();
        }
        else {
            $("#ts_Approveid").hide(v.ApprovedDate);
            $("#ts_approid").hide();
        }

        $("#ts_Submittedid").html(v.SubmittedDate);
        $("#ts_submited").show();
        $('#ts_ManagerNamesid1').html(v.ManagerName1);
        if (v.ManagerName2 != '0') {
            $('#ts_ManagerNamesid2').html(v.ManagerName2);
            $("b.ts_ManagerNamesid2").show();
            $("#ts_Manager2").show();
        }
        else {
            $("#ts_ManagerNamesid2").hide();
            $('#ts_ManagerNamesid2').html("");
            $("#ts_Manager2").hide();
        }

        $("#StardateToEnddate").html("<span id='StartToEnd'><b>Timesheet of " + v.UserName + " for the Period of " + v.TotalMonthName + "</b></span>");
        if (k <= 15) {
            tbltask = 'uc1_ddlTask' + k;
            if (counter15 == -1) {

                $("#MainTable").append("<td id='td1'>" +
                    "<table id='SubTable' width='100%' class='evAdminTable1'><tr style='background-color:black;color:white'><th width='160' text-align='center' class='evAdminTable'>Date </th> <th class='evAdminTable'>Task </th> <th width='50' class='evAdminTable'>Hours </th></tr>");

            }
            $("#SubTable").append("<tr><td class='colored'>" + v.MonthYearName + "</td><td class='colored' id='uc1_ddlTask" + k + "'>" + v.Taskname + "</td><div class='flex'><td class='colored' name='uc1txtHours'  id='uc1_txtHours" + k + "'><font>" + v.NoofHoursWorked + "</font><span id='HoursColours" + k + "' style='font-size: 25px;line-height: 25px;color:" + v.colour + "'>* </span></div></td></td>");
         //   $("#SubTable").append("<tr><td>" + v.MonthYearName + "</td><td><select id='uc1_ddlTask" + k + "' class='form-control lookup' selected  ></select></td><td><div class='flex'><input class='form-control uc1txtHours' name='hrs' type='number' title='Enter hours' min='0' max='24' maxlength='2' id='uc1_txtHours" + k + "' readonly value=" + v.NoofHoursWorked + "  style='width: 56px;'><span id='HoursColours" + k + "' style='font-size: 25px;line-height: 25px;color:" + v.colour + "'>* </span></div></td><td >&nbsp</td></td>");
          

            counter15 = counter15 + 1;

        }
        else {
            tbltask = 'uc1_ddlTask' + k;
            if (counter == -1) {
                $("#SubTable").append("</table>")
                $("#SubTable").append("</td>")
                $("#MainTable").append("<td id='td2'><table id='SubTable2' /*style='position:relative;top:19px;'*/ width='100%' class='evAdminTable1'><tr style='background-color:black;color:white'><th width='160' text-align='center' class='evAdminTable'>Date </th> <th class='evAdminTable'>Task </th> <th width='50'class='evAdminTable'>Hours</th></tr>");
            }
            $("#SubTable2").append("<tr><td class='colored'>" + v.MonthYearName + "</td><td class='colored' id='uc1_ddlTask" + k + "'>" + v.Taskname + "</td><div class='flex'><td class='colored' name='uc1txtHours'  id='uc1_txtHours" + k + "'><font>" + v.NoofHoursWorked + "</font><span id='HoursColours" + k + "' style='font-size: 25px;line-height: 25px;color:" + v.colour + "'>* </span></div></td></td>");
           // $("#SubTable2").append("<tr><td>" + v.MonthYearName + "</td><td><select id='uc1_ddlTask" + k + "' class='form-control lookup' selected  ></select></td><td><div class='flex'><input class='form-control uc1txtHours' name='hrs' type='number' title='Enter hours' min='0' max='24' maxlength='2' id='uc1_txtHours" + k + "' readonly value=" + v.NoofHoursWorked + "  style='width: 56px;'><span id='HoursColours" + k + "' style='font-size: 25px;line-height: 25px;color:" + v.colour + "'>* </span></div></td><td >&nbsp</td></td>");
            counter = counter + 1;

        }

    });

    HoursDataColoursPreview();
    CalculatePreview();
    "</table>" + + "<hr />"
    $("#MainTable").append("</tr></table>") + "<hr />";
    $('table[id="SubTable"] tr > td:last-child').css("display", "table-cell");
    $('table[id="SubTable2"] tr > td:last-child').css("display", "table-cell");

}

function ViewTimesheetByMonth(MonthTimesheet, resultData) {
    $("#body_ClientDetails").attr("style", "display: table;");
    $("#btnPrint").hide();
    $("#btnpdf").hide();
    $("#tabCon").empty();
    $("#MainTable").empty();
    $("#SubTable").empty();
    var tbltask = ''; var objSelect = '';
    $('#Cmtsave').show();
    var counter = -1;
    var counter15 = -1;
    $("#ClientDetails2").show(true);
    $("#tabCon").append("<table name='maintbl' id='MainTable' width='100%'/*style='position:relative;left:10%;'*/>");

    $.each(resultData, function (k, v) {

        $("#StardateToEnddate").html("<span id='StartToEnd'><b>Timesheet of " + v.UserName + " for the Period of " + v.TotalMonthName + "</b></span>");
        $("#txtDescription").val(v.Comments);
        $('#ts_ManagerNamesid1').html(v.ManagerName1);

        if (v.ManagerName2 != '0') {
            $('#ts_ManagerNamesid2').html(v.ManagerName2);
            $("b.ts_ManagerNamesid2").show();
            $("#ts_Manager2").show();
            //$('#ts_ManagerNamesid2').closest("b.ts_ManagerNamesid2").show();
        }
        else {
            $('#ts_ManagerNamesid2').html("");
            $("b.ts_ManagerNamesid2").hide();
            $("#ts_Manager2").hide();
        }
        $("#ts_Approveid").hide();
        $("#ts_Submittedid").hide();
        $("#ts_approid").hide();
        $("#ts_submited").hide();

        if (k <= 15) {
            tbltask = 'uc1_ddlTask' + k;
            if (counter15 == -1) {

                $("#MainTable").append("<td id='td1'>" +
                    "<table id='SubTable' width='100%' class='evAdminTable1'><tr style='background-color:black;color:white'><th width='160' text-align='center' class='evAdminTable'>Date </th> <th class='evAdminTable'>Task </th> <th width='50' class='evAdminTable'>Hours </th></tr>");

            }

            //$("#SubTable").append("<tr><td>" + v.MonthYearName + "</td><td><select id='uc1_ddlTask" + k + "' class='form-control lookup' selected  ></select></td><td><input class='form-control uc1txtHours' name='hrs' type='number' title='Enter hours' maxlength='2'  min='0' max='8' id='uc1_txtHours" + k + "' value=" + v.NoofHoursWorked + "  style='width: 50px;'></td><td >&nbsp</td></td>");
            $("#SubTable").append("<tr><td>" + v.MonthYearName + "</td><td><select id='uc1_ddlTask" + k + "' class='form-control lookup' selected  ></select></td><td><div class='flex'><input class='form-control uc1txtHours' name='hrs' type='number' title='Enter hours' min='0' max='24' maxlength='2' id='uc1_txtHours" + k + "' value=" + v.NoofHoursWorked + "  style='width: 56px;'><span id='HoursColours" + k + "' style='font-size: 25px;line-height: 25px;color:" + v.colour + "'>* </span></div></td><td >&nbsp</td></td>");
            counter15 = counter15 + 1;


        }
        else {

            tbltask = 'uc1_ddlTask' + k;
            if (counter == -1) {
                $("#SubTable").append("</table>")
                $("#SubTable").append("</td>")
                $("#MainTable").append("<td id='td2'><table id='SubTable2' /*style='position:relative;top:19px;'*/ width='100%' class='evAdminTable1'><tr style='background-color:black;color:white'><th width='160' text-align='center' class='evAdminTable'>Date </th> <th class='evAdminTable'>Task </th> <th width='50'class='evAdminTable'>Hours</th></tr>");
            }

            $("#SubTable2").append("<tr><td>" + v.MonthYearName + "</td><td><select id='uc1_ddlTask" + k + "' class='form-control lookup' selected  ></select></td><td><div class='flex'><input class='form-control uc1txtHours' name='hrs' type='number' title='Enter hours' min='0' max='24' maxlength='2' id='uc1_txtHours" + k + "' value=" + v.NoofHoursWorked + "  style='width: 56px;'><span id='HoursColours" + k + "' style='font-size: 25px;line-height: 25px;color:" + v.colour + "'>* </span></div></td><td >&nbsp</td></td>");
            // $('#uc1_ddlTask' + k).append('<option value="' + v.Taskid + '">' + v.Taskname + '</option>');
            counter = counter + 1;

        }

    });
    HoursDataColoursPreview();
    calculateSum();
    $(":input").bind('keyup mouseup', function () {
        calculateSum();

    });


    "</table>" + + "<hr />"
    $("#MainTable").append("</tr></table>") + "<hr />";

}

function calculateSum() {

    var sum = 0;
    $(".uc1txtHours").each(function () {
        //add only if the value is number
        if (!isNaN(this.value) && this.value.length != 0) {
            sum += parseFloat(this.value);
        }
    });
    //.toFixed() method will roundoff the final sum to 2 decimal places
    $("#TotalHoursCount").html(sum.toFixed(2));
}

function LoadPreviewTasklookups() {

    $.ajax({
        type: "GET",
        url: "/Admin/getLookUp",
        datatype: "Json",
        async: false,
        //data: { id: id },
        success: function (data) {

            $(data).each(function () {

                $(".lookup").append($("<option></option>").val(this.tsk_TaskID).html(this.tsk_TaskName));

            });


        }
    });
}


function LoadTasklookupsByUser(Userid) {

    $.ajax({
        type: "Post",
        url: "/Admin/GetLookUpByEmpId",
        datatype: "Json",
        async: false,
        data: { "Userid": Userid },
        success: function (data) {
            $(data).each(function () {
                $(".lookup").append($("<option></option>").val(this.tsk_TaskID).html(this.tsk_TaskName));
            });
        }
    });
}




//function daysInMonth(month, year) {
//    return new Date(year, month, 0).getDate();
//}


function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}

/////////////////////////////////////////////////////EditsubmitData////////////////////////////////////////////////////////

var Submittedtype = ""; var TaskLookupId = "", workingHours = "", Commentss = "";
function ConfirmSendTimesheet(id) {

    if (id == '1') {
        Submittedtype = 'Save';
    }
    if (id == '2') {

        Submittedtype = 'Submit';
    }

    Commentss = $.trim($("#txtDescription").val());


    //if (Commentss == "") {
    //    alert("Please enter comments");
    //    $("#txtDescription").focus();
    //    return false;
    //}
    var rows = $("#SubTable,#SubTable2").find("tr");

    var listtimesheetdetails = []; var rowData = {}; var Date, TaskId, hours;

    for (var rowOn = 1; rowOn < rows.length; rowOn++) {

        TaskLookupId = $(rows[rowOn]).find('.lookup').attr('id');
        workingHours = $(rows[rowOn]).find('.uc1txtHours').attr('id');

        Date = $(rows[rowOn]).find("td").eq(0).text();
        TaskId = $("#" + TaskLookupId).val();
        hours = $("#" + workingHours).val();

        if (hours == "") {
            if (!$.trim(this.value)) {
                alert("Please enter working hours");
                $(".uc1txtHours").focus();
                calculateSum();
                return false;
            }
        }

        if (hours != "") {
            if (hours < 0) {
                alert("Working hours should be not be less than zero");
                $("#" + workingHours).val('');
                $("#uc1_txtHours").focus();
                calculateSum();
                return false;
            }
        }



        if (hours != "") {
            if (hours > 24) {
                alert("Working hours should not be greater than 24 hours");
                $("#" + workingHours).val('');
                $("#uc1_txtHours").focus();
                calculateSum();
                return false;
            }
        }


        if (TaskId != undefined && hours != undefined && Date != null) {
            rowData = { taskDate: Date, taskid: TaskId, hoursWorked: hours }
        }
        listtimesheetdetails.push(rowData);
    }


    var timesheets = {
        TimesheetID: TimesheetID,
        TimeSheetMonth: MonthTimesheet,
        Comments: Commentss,
        SubmittedType: Submittedtype,


    }

    var sheetObj = {
        timesheets: timesheets,
        listtimesheetdetails: listtimesheetdetails

    };

    $('#loading-image').attr("style", "display: block;");
    $.ajax({
        type: "POST",
        url: "/Timesheet/updateTimesheetTaskDetails",
        data: JSON.stringify(sheetObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            alert(data);
            window.location.reload();
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        }
    });


}

function ManagerApprOrRej(id)
{
  
    TimesheetID = id.getAttribute("data-TimesheetID");
    MonthTimesheet = id.getAttribute("data-MonthYear");
    tsuserid = id.getAttribute("data-Usr_UserID");
    ApporRejprojectid = id.getAttribute("data-ProjectId")

    var projectname = id.getAttribute("data-ProjectName");
    var empname = id.getAttribute("data-userName");
    var Actualhrs = id.getAttribute("data-CompanyBillingHours");
    var Workedhrs = id.getAttribute("data-ResourceWorkingHours");


    var ApproveRejectstatus = id.getAttribute("data-Status");
    id = ApproveRejectstatus;

    if (id == 3) {
        $("#btnAdd").show();
        $("#btnRej").hide();
        $('#PopupAppRejId').html("Approve Timesheet");
        $("#ProjectNameid").html("Do you want to approve <b> " + empname + "'s </b>  <b>" + MonthTimesheet + "</b> timesheets within <b>" + Workedhrs + "</b> hours for <b>" + projectname + "</b>");
    }
    if (id == 4) {
        $("#btnRej").show();
        $("#btnAdd").hide();
        $('#PopupAppRejId').html("Reject Timesheet");
        $("#ProjectNameid").html("Do you want to reject <b> " + empname + "'s </b>  <b>" + MonthTimesheet + "</b> timesheets within <b>" + Workedhrs + "</b> hours for <b>" + projectname + "</b>");
    }
    $('#ContainerGridDetail').show();

    //$("#ProjectNameid").html("Do you want to approve<b>" + empname + " </b> of Project <b>" + projectname + "</b> and  Worked Hours is <b>" + Workedhrs + "</b>  for Month of <b>" + MonthTimesheet + "</b>");



}

function ApprovalRejectTimesheet(id)
{


    if (id == '3' || id == '4')
    {

        id = id;

    }
    else {
        TimesheetID = id.getAttribute("data-TimesheetID");
        MonthTimesheet = id.getAttribute("data-MonthYear");
        tsuserid = id.getAttribute("data-Usr_UserID");
        Commentss = $.trim($("#txtDescription").val());
        var ApproveRejectstatus = id.getAttribute("data-Status");
        id = ApproveRejectstatus;

    }

    var projectid = ApporRejprojectid;
    var timesheets =

        {
            TimesheetID: TimesheetID,
            SubmittedType: id,
            TimeSheetMonth: MonthTimesheet,
            Comments: Commentss,
            UserID: tsuserid,
            ProjectID: projectid,

        }
    var sheetObj = {
        timesheets: timesheets,


    };
    $('#loading-image').attr("style", "display: block;");
    $.ajax({
        type: "POST",
        url: "/Timesheet/TimeSheetManagerActionWeb",
        data: JSON.stringify(sheetObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
              sendmailsbyapp(sheetObj);
            alert(data);
            window.location.reload();
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        }
    });

}

function sendmailsbyapp(sheetObj) {
    $.ajax({
        type: "POST",
        url: "/Timesheet/SendMailsForApprovals",
        data: JSON.stringify(sheetObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //alert(data);
        }
    });
}

function LoadPreviewTaskData() {

    if (resultDataArray && resultDataArray.length > 0) {
        $.each(resultDataArray, function (index, value) {

            $('#uc1_ddlTask' + index).val(value.Taskid).attr("selected", "selected");
            $('#Timesheet_Duration').html(value.TotalMonthName);
            $('#ts_ClientNameid').html(value.ProjectName);
            $('#ts_ProjectNameid').html(value.ProjectClientName);
        });
    }
}

function LoadData() {

    if (resultDataArray && resultDataArray.length > 0) {
        $.each(resultDataArray, function (index, value) {

            //$('#uc1_ddlTask' + index).val(value.Taskid).attr("selected", "selected");//
            //$('#uc1_ddlTask' + index).val(value.Taskid).attr("disabled", "true");//
            $('#uc1_ddlTask' + index).val(value.Taskid).attr("selected", "true");
            $('#uc1_txtHours' + index).val(value.NoofHoursWorked).attr("selected", "true");
            $('#txtDescription').val(value.Comments).attr("disabled", "true");
            //$('#Timesheet_ProjectName').html(value.ProjectName);
            $('#Timesheet_Duration').html(value.TotalMonthName);
            $('#Timesheet_EmployeeName').html(value.UserName);
            $('#ts_ClientNameid').html(value.ProjectName);
            $('#ts_ProjectNameid').html(value.ProjectClientName);
            $('#btnSave').hide();
            $('#btnSend').hide();

        });
    }
}
function LoadManagerData() {

    if (resultDataArray && resultDataArray.length > 0) {
        $.each(resultDataArray, function (index, value) {

            $('#uc1_ddlTask' + index).val(value.Taskid).attr("selected", "selected");
            $('#uc1_ddlTask' + index).val(value.Taskid).attr("disabled", "true");
            // $('#txtDescription').val(value.Comments).attr("disabled", "true");
            $('#btnSave').hide();
            $('#btnSend').hide();
            //   $('#Timesheet_ProjectName').html(value.ProjectName);
            $('#ts_ClientNameid').html(value.ProjectName);
            $('#ts_ProjectNameid').html(value.ProjectClientName);
            $('#Timesheet_Duration').html(value.TotalMonthName);
            $('#Timesheet_EmployeeName').html(value.UserName);

        });
    }
}

function LoadClientDetails() {
    $.ajax({
        type: "GET",
        url: "/Timesheet/Usertimesheet",

        datatype: "Json",

        success: function (result) {


        }
    });
}

function AccSpecificTasksLoad() {

    $.ajax({
        type: "GET",
        url: "/ClientComponent/Getaccountspecifictasks",
        //contentType: "application/json; charset=utf-8",
        //dataType: "json",
        async: false,
        //data: { id: id },
        success: function (data) {

            //$(data).each(function () {

            //    $(".lookup").append($("<option></option>").val(this.Acc_SpecificTaskId).html(this.Acc_SpecificTaskName));

            //});


        }
    });
}

function LoadColoursonLeavesandHolidays(MonthTimesheet, tsuserid) {
    $.ajax({
        url: "/Timesheet/HoursWorkedTimesheet",
        type: "GET",
        data: { TimesheetMonth: MonthTimesheet, TimesheetUserid: tsuserid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        async: false,
        // cache: false,

        success: function (resultData) {
            resultHoursWorkedColour = resultData

        },

    });
}

function HoursDataColoursPreview() {
    $.each(resultHoursWorkedColour, function (index, value) {
        if (value.colour == 'black') {
            value.colour = 'white';
        }
        $('#HoursColours' + index).attr('style', 'font-size:25px;line-height:25px;color:' + value.colour);

    });

}

function print_page()
{
    window.print();
}

$("body").on("click", "#btnpdf", function ()
{
   
    html2canvas($('#divEditTimesheetData')[0], {
        onrendered: function (canvas) {
            var data = canvas.toDataURL();
            var docDefinition = {
                content: [{
                    image: data,
                    width: 500
                }]
            };
            pdfMake.createPdf(docDefinition).download("Timesheet.pdf");
        }
    });
});


function CalculatePreview() {

    var sum = 0;
    $("td.colored[name='uc1txtHours'] > font").each(function () {

      
            //sum += Number($(this).val()) || 0;
            sum += parseInt($(this).text());

   

    });

    $("#TotalHoursCount").html(sum);
}
