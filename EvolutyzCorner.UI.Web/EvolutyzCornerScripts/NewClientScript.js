var managerFlag;
var ManagerFName;
var ManagerLName;
var uid;
var perms;
var Rolid;
var Proj_AccountID = '@ViewBag.AccountId';
var projid;
var editprojectid;
editprojectid = $("#editpid").val();
var Proj_ProjectID = "";
var projectspecifictaskid;
var deluserid;
var delHoliday;
var delprotaskid;
var delproid;
var roleids;
$(document).ready(function () {

    $("#HolidayDate").datepicker({
        
        format: 'mm/dd/yyyy'
    });
    loaddata();
    loadproject(editprojectid);
    GetUpdateClient(editprojectid);
    loadholidays(editprojectid);
    loadtaks(editprojectid);
    usergriddata($("#editpid").val());
    $("#btnSingleUpdate").hide();
    $("#Proj_ActiveStatus").val(1);
   
    $("#Proj_StartDate").datepicker({
        dateFormat: 'mm/dd/yyyy'
    });
    $("#Usrp_DOJ").datepicker({
        dateFormat: 'mm/dd/yyyy'
    });
    $("#Proj_StartDate").datepicker({
        dateFormat: 'mm/dd/yyyy'
    });

    $("#Proj_EndDate").datepicker({
        dateFormat: 'mm/dd/yyyy'
    });

    $("#UProj_StartDate").datepicker({
        dateFormat: 'mm/dd/yyyy'
    });

    $("#Usrp_DOJ").datepicker({
        dateFormat: 'mm/dd/yyyy'
    });
    $("#UProj_EndDate").datepicker({

        dateFormat: 'mm/dd/yyyy'
    });
    $("#Actual_StartDate").datepicker({
        dateFormat: 'mm/dd/yyyy'
    });
    $("#Actual_EndDate").datepicker({
        dateFormat: 'mm/dd/yyyy'
    });
    $("#Plan_StartDate").datepicker({
        dateFormat: 'mm/dd/yyyy'
    });
    $("#Plan_EndDate").datepicker({
        dateFormat: 'mm/dd/yyyy'
       
    });



    $("#profile-image").click(function () {

        $("#fileUpload").trigger('click');

    });
    $("#btnAddModel").click(function () {
        window.location.href = "/Client/Index?proid=" + 0;
        //SequenceClientCode();
        //  loadproject();
        return false;

    });

    $("#btnAdd").on("click", function () {
        $('#clientform').validate({
            rules: {
                Proj_ProjectName: {
                    required: true
                },
                CountryId: {
                    required: true
                },
                StateId: {
                    required: true
                },
                WebUrl: {
                    required: true
                },
                Proj_ActiveStatus: {
                    required: true

                },
                
            },
            submitHandler: function (form) {
                add();
                $("#btnAdd").hide();
                $("#UpdatebtnEdit").hide();
                $("#btnSingleUpdate").show();
                $("#btnManModel").show();
                $("#btnEmpModel").show();
                $("#btnHoliday").show();
                return false;
            }
        });
        $.validator.addMethod("regx", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter  Only Alphabets .");
        $.validator.addMethod("regex", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter 10 Digit Number.");
        $.validator.addMethod("reg", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter valid Password");




    });

    $("#CountryId").change(function () {
        countryChange();
    });
    $("#RoleName").change(function () {
        // 
        //   
        var roledata = $("#RoleName").val();

        console.log(roledata);
        GetAlltasknames(this.value);

        //$("#Managername2 option[value=" + ManagerID + "]").remove();
        // $("#Managername2").remove($("<option></option>").val(u_id));
    });
    $("#btnSingleUpdate").on("click", function () {
        $('#clientform').validate({
            rules: {
                Proj_ProjectName: {
                    required: true
                },
                CountryId: {
                    required: true
                },
                StateId: {
                    required: true
                },
                WebUrl: {
                    required: true
                },
                Proj_ActiveStatus: {
                    required: true

                },

            },
            submitHandler: function (form) {
                UpdateSingleRecord();
               
                return false;
            }
        });
        
    });

    $("#btnManModel").click(function () {

        var roledata = $("#RoleName").val();
        $("#ManagerName").empty();
        $("#manager1").hide();
        $("#Managername2").empty();
        $("#manager2").hide();
        $("#btnManAddModel").show();
        $("#btnEmpAddModel").hide();
        $("#btnUpdateModel").hide();
        $("#btnassociate").hide();

        bindProjectNames();
        bindRoleNames();
        bindUserNames();
        bindUserTypes();
        GetAlltasknames(roledata);
        $("#add").text('Add Manager');
        //  GetRoleNamesbyemp();
        Manger2();
        modelclick();
        getclientforproject();

        // $("#btnassociateemp").hide();
        $("#profile-image").prop('src', 'empty');
    });
    $("#btnManAddModel").on("click", function () {
        $('#EmpForm').validate({
            rules: {
                Usr_TaskID: {
                    required: true
                },
                Usr_UserTypeID: {
                    required: true
                },
                RoleName: {
                    required: true
                },
                project: {
                    required: true
                },
                Usr_Titleid: {
                    required: true

                },
                UsrP_FirstName: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    },
                    regx: /^[a-zA-Z]+$/

                },
                UsrP_LastName: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    },
                    regx: /^[a-zA-Z]+$/
                },

                Usrp_MobileNumber: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    },
                    regex: /^[0-9]{10}$/

                },
                Usr_Username: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    },
                    regx: /^[a-zA-Z]+$/
                },
                Email: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    },
                    email: true
                },
                Usr_Password: {
                    required: true,
                    reg: /^([a-zA-Z0-9@*#]{8,15})$/
                },
                Usr_ConfirmPassword: {
                    required: true,
                    reg: /^([a-zA-Z0-9@*#]{8,15})$/,
                    equalTo: "#Usr_Password"
                },
                Usr_GenderId: {
                    required: true,
                    //date: true,
                },
                //ManagerName: {
                //    required: true
                //},
                TimesheetMode_id: {
                    required: true
                },
                UProj_ActiveStatus: {
                    required: true
                },
                Usrp_CountryCode: {
                    required: true
                }
            },
            submitHandler: function (form) {
                ManagerSaveData();
                return false;
            }
        });
        $.validator.addMethod("regx", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter  Only Alphabets .");
        $.validator.addMethod("regex", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter 10 Digit Number.");
        $.validator.addMethod("reg", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter valid Password");



        
        
       
       
    });

    $("#btnEmpModel").click(function () {
        $("#ManagerName").empty();
        $("#Managername2").empty();
        var roledata = $("#RoleName").val();
        modelclick();
        bindProjectNames();
        GetRoleNamesbyemp();
        bindUserNames();
        GetAlltasknames(roledata);
        bindUserTypes();
        Manger2();
        getclientforproject();
        $("#btnManAddModel").hide();
        $("#btnEmpAddModel").show();
        $("#btnUpdateModel").hide();
        $("#btnassociate").hide();
        $("#add").text('Add Employee');
        $("#profile-image").prop('src', 'empty');
    });


    $("#btnEmpAddModel").click(function () {
        $('#EmpForm').validate({
            rules: {
                Usr_TaskID: {
                    required: true
                },
                Usr_UserTypeID: {
                    required: true
                },
                RoleName: {
                    required: true
                },
                project: {
                    required:true
                },
                Usr_Titleid: {
                    required: true
                    
                },
                UsrP_FirstName: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    },
                    regx: /^[a-zA-Z]+$/

                },
                UsrP_LastName: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    },
                    regx: /^[a-zA-Z]+$/
                },

                Usrp_MobileNumber: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    },
                    regex:/^[0-9]{10}$/

                },
                Usr_Username: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    },
                    regx: /^[a-zA-Z]+$/
                },
                Email: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    },
                    email: true
                },
                Usr_Password: {
                    required: true,
                    reg: /^([a-zA-Z0-9@*#]{8,15})$/
                },
                Usr_ConfirmPassword: {
                    required: true,
                    reg: /^([a-zA-Z0-9@*#]{8,15})$/,
                    equalTo: "#Usr_Password"
                },
                Usr_GenderId: {
                    required: true
                },
                ManagerName: {
                    required:true
                },
                TimesheetMode_id: {
                    required:true
                },
                UProj_ActiveStatus: {
                    required:true
                },
                Usrp_CountryCode: {
                    required:true
                }
            },
            submitHandler: function (form) {
                AddEmployee();
                return false;
            }
        });
        $.validator.addMethod("regx", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter  Only Alphabets .");
        $.validator.addMethod("regex", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter 10 Digit Number.");
        $.validator.addMethod("reg", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter valid Password");


     
       

    });

    $("#btnclose").click(function () {
        $('#ContainerGridDetail').hide();
        $(".modal-backdrop").hide();
        window.location.reload();
        
    });
    $("#closetask").click(function () {
        $('#Projectspecifictaskgrid').hide();
        window.location.reload();
        $(".modal-backdrop").hide();
    });
    $("#Closegrid").click(function () {
        $('#AssociateEmpGrid').hide();
        window.location.reload();
        $(".modal-backdrop").hide();
    });

    $("#Back").click(function () {
        window.location.href = "/Project/Index";
        return false;
    });

    $("#btnUpdateModel").on("click", function () {
           
        var role = $("#RoleName option:selected").text();
        if (role === "Manager") {
            $('#EmpForm').validate({
                rules: {
                    Usr_TaskID: {
                        required: true
                    },
                    Usr_UserTypeID: {
                        required: true
                    },
                    RoleName: {
                        required: true
                    },
                    project: {
                        required: true
                    },
                    Usr_Titleid: {
                        required: true

                    },
                    UsrP_FirstName: {
                        required: {
                            depends: function () {
                                $(this).val($.trim($(this).val()));
                                return true;
                            }
                        },
                        regx: /^[a-zA-Z]+$/

                    },
                    UsrP_LastName: {
                        required: {
                            depends: function () {
                                $(this).val($.trim($(this).val()));
                                return true;
                            }
                        },
                        regx: /^[a-zA-Z]+$/
                    },

                    Usrp_MobileNumber: {
                        required: {
                            depends: function () {
                                $(this).val($.trim($(this).val()));
                                return true;
                            }
                        },
                        regex: /^[0-9]{10}$/

                    },
                    Usr_Username: {
                        required: {
                            depends: function () {
                                $(this).val($.trim($(this).val()));
                                return true;
                            }
                        },
                        regx: /^[a-zA-Z]+$/
                    },
                    Email: {
                        required: {
                            depends: function () {
                                $(this).val($.trim($(this).val()));
                                return true;
                            }
                        },
                        email: true
                    },
                    Usr_Password: {
                        required: true,
                        reg: /^([a-zA-Z0-9@*#]{8,15})$/
                    },
                    Usr_ConfirmPassword: {
                        required: true,
                        reg: /^([a-zA-Z0-9@*#]{8,15})$/,
                        equalTo: "#Usr_Password"
                    },
                    Usr_GenderId: {
                        required: true,
                        //date: true,
                    },
                    //ManagerName: {
                    //    required: true
                    //},
                    TimesheetMode_id: {
                        required: true
                    },
                    UProj_ActiveStatus: {
                        required: true
                    },
                    Usrp_CountryCode: {
                        required: true
                    }
                },
                submitHandler: function (form) {
                    updategrid();
                    return false;
                }
            });
            $.validator.addMethod("regx", function (value, element, regexpr) {
                return regexpr.test(value);
            }, "Please enter  Only Alphabets .");
            $.validator.addMethod("regex", function (value, element, regexpr) {
                return regexpr.test(value);
            }, "Please enter 10 Digit Number.");
            $.validator.addMethod("reg", function (value, element, regexpr) {
                return regexpr.test(value);
            }, "Please enter valid Password");

        } else {
            $('#EmpForm').validate({
                rules: {
                    Usr_TaskID: {
                        required: true
                    },
                    Usr_UserTypeID: {
                        required: true
                    },
                    RoleName: {
                        required: true
                    },
                    project: {
                        required: true
                    },
                    Usr_Titleid: {
                        required: true

                    },
                    UsrP_FirstName: {
                        required: {
                            depends: function () {
                                $(this).val($.trim($(this).val()));
                                return true;
                            }
                        },
                        regx: /^[a-zA-Z]+$/

                    },
                    UsrP_LastName: {
                        required: {
                            depends: function () {
                                $(this).val($.trim($(this).val()));
                                return true;
                            }
                        },
                        regx: /^[a-zA-Z]+$/
                    },

                    Usrp_MobileNumber: {
                        required: {
                            depends: function () {
                                $(this).val($.trim($(this).val()));
                                return true;
                            }
                        },
                        regex: /^[0-9]{10}$/

                    },
                    Usr_Username: {
                        required: {
                            depends: function () {
                                $(this).val($.trim($(this).val()));
                                return true;
                            }
                        },
                        regx: /^[a-zA-Z]+$/
                    },
                    Email: {
                        required: {
                            depends: function () {
                                $(this).val($.trim($(this).val()));
                                return true;
                            }
                        },
                        email: true
                    },
                    //Usr_Password: {
                    //    //required: true,
                    //    reg: /^([a-zA-Z0-9@*#]{8,15})$/
                    //},
                    //Usr_ConfirmPassword: {
                    //  //  required: true,
                    //    reg: /^([a-zA-Z0-9@*#]{8,15})$/,
                    //    equalTo: "#Usr_Password"
                    //},
                    Usr_GenderId: {
                        required: true,
                        //date: true,
                    },
                    ManagerName: {
                        required: true
                    },
                    TimesheetMode_id: {
                        required: true
                    },
                    UProj_ActiveStatus: {
                        required: true
                    },
                    Usrp_CountryCode: {
                        required: true
                    }
                },
                submitHandler: function (form) {
                    updategrid();
                    return false;
                }
            });
            $.validator.addMethod("regx", function (value, element, regexpr) {
                return regexpr.test(value);
            }, "Please enter  Only Alphabets .");
            $.validator.addMethod("regex", function (value, element, regexpr) {
                return regexpr.test(value);
            }, "Please enter 10 Digit Number.");
            $.validator.addMethod("reg", function (value, element, regexpr) {
                return regexpr.test(value);
            }, "Please enter valid Password");

        }
      

      
    });

    $("#HolidaybtnAdd").click(function () {
        var holiday = [];
        
        var holidayname = $("#HolidayName").val();
        var holidatydate = $("#HolidayDate").val();
        var financialyear = $("#FinancialYearId").val();
        var optionalholiday = $("#isOptionalHoliday").val();
        var status = $("#isActive").val();
        holiday.push({ "HolidayName": holidayname, "HolidayDate": holidatydate, "FinancialYearId": financialyear, "isOptionalHoliday": optionalholiday, "isActive": status, "ProjectID": projid });

        holiday = JSON.stringify({ 'holiday': holiday });
        $('#holidayform').validate({
            rules: {
                HolidayName: {
                    required: true,
                    regx: /^[a-zA-Z]+$/
                },
                HolidayDate: {
                    required: true,
                    date:true
                },
                FinancialYearId: {
                    required: true
                },
                isOptionalHoliday: {
                    required: true
                },
                isActive: {
                    required: true

                },

            },
            submitHandler: function (form) {
                $.ajax({

                    type: 'POST',
                    url: '/HolidayCalendar/CreateHolidayforclient',
                    data: {
                        "HolidayName": holidayname, "HolidayDate": holidatydate, "FinancialYearId": financialyear, "isOptionalHoliday": optionalholiday, "isActive": status, "ProjectID": projid
                    },
                    success: function (data) {
                        if (data === "HolidayName Already Exist In this Client") {
                            $("#HolidayName").addClass("validate_msg");
                            $("#HolidayDate").removeClass("validate_msg");
                        } else if (data === "HolidayDate Already Exist In this Client") {
                            $("#HolidayName").removeClass("validate_msg");
                            $("#HolidayDate").addClass("validate_msg");
                        } else {
                            alert(data);
                            $("#HolidayName").removeClass("validate_msg");
                            $("#HolidayDate").removeClass("validate_msg");
                            $('#Holidaycalendergrid').hide();
                            window.location.reload();
                            $(".modal-backdrop").hide();
                            $('#usertable').dataTable();
                            $('#holidaytable').dataTable();
                            $('#tasktable').dataTable();
                            $('#Projecttable').dataTable();

                            if (projid === "") {
                                usergriddata($("#editpid").val());
                                loadholidays($("#editpid").val());
                                loadtaks($("#editpid").val());
                                loadproject($("#editpid").val());
                            } else {
                                usergriddata(projid);
                                loadholidays(projid);
                                loadtaks(projid);
                                loadproject(projid);
                            }
                        }

                    },
                    complete: function () {
                        $('#loading-image').attr("style", "display: none;");
                    },
                    error: function (error) {
                        console.log(error);
                    }

                });
                return false;
            }
        });
        $.validator.addMethod("regx", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter  Only Alphabets .");
        $.validator.addMethod("regex", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter 10 Digit Number.");
        $.validator.addMethod("reg", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter valid Password");
          
      
        
        
    });

    $("#HolidaybtnUpdate").click(function () {
        // // 
        var holiday = [];
        var hcid = $("#HolidayCalendarID").val();
        var holidayname = $("#HolidayName").val();
        var holidatydate = $("#HolidayDate").val();
        var financialyear = $("#FinancialYearId").val();
        var optionalholiday = $("#isOptionalHoliday").val();
        var status = $("#isActive").val();
        holiday.push({ "HolidayName": holidayname, "HolidayDate": holidatydate, "FinancialYearId": financialyear, "isOptionalHoliday": optionalholiday, "isActive": status });

        holiday = JSON.stringify({ 'holiday': holiday });
        $('#holidayform').validate({
            rules: {
                HolidayName: {
                    required: true,
                    regx: /^[a-zA-Z]+$/
                },
                HolidayDate: {
                    required: true,
                    date: true
                },
                FinancialYearId: {
                    required: true
                },
                isOptionalHoliday: {
                    required: true
                },
                isActive: {
                    required: true

                },

            },
            submitHandler: function (form) {
                $.ajax({
                    //contentType: 'application/json; charset=utf-8',
                    type: 'POST',
                    url: '/HolidayCalendar/UpdateCalenderControl',
                    data: {
                        "HolidayCalendarID": hcid,
                        "HolidayName": holidayname, "HolidayDate": holidatydate, "FinancialYearId": financialyear, "isOptionalHoliday": optionalholiday, "isActive": status
                    },
                    success: function (data) {
                        if (data === "HolidayName Already Exist") {
                            $("#HolidayName").addClass("validate_msg");
                            $("#HolidayDate").removeClass("validate_msg");
                        } else if (data === "HolidayDate Already Exist") {
                            $("#HolidayName").removeClass("validate_msg");
                            $("#HolidayDate").addClass("validate_msg");
                        } else {
                            alert(data);
                            $("#HolidayName").removeClass("validate_msg");
                            $("#HolidayDate").removeClass("validate_msg");
                            $('#Holidaycalendergrid').hide();
                            window.location.reload();
                            $(".modal-backdrop").hide();
                            $('#usertable').dataTable();
                            $('#holidaytable').dataTable();
                            $('#tasktable').dataTable();
                            $('#Projecttable').dataTable();
                            if (projid === "") {
                                usergriddata($("#editpid").val());
                                loadholidays($("#editpid").val());
                                loadtaks($("#editpid").val());
                                loadproject($("#editpid").val());
                            } else {
                                usergriddata(projid);
                                loadholidays(projid);
                                loadtaks(projid);
                                loadproject(projid);
                            }
                        }
                    },

                    error: function (error) {
                        console.log(error);
                    }

                });
                return false;
            }
        });
        $.validator.addMethod("regx", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter  Only Alphabets .");
        $.validator.addMethod("regex", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter 10 Digit Number.");
        $.validator.addMethod("reg", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter valid Password");
           
        
    });

    //$('#Usr_Password, #Usr_ConfirmPassword').on('keyup', function () {
    //    if ($('#Usr_Password').val() === $('#Usr_ConfirmPassword').val()) {
    //        $('#message').html('Matching').css('color', 'green');
    //    } else
    //        $('#message').html('Not Matching').css('color', 'red');
    //});

    $("#ManagerName").change(function () {
        // 
        var ManagerID = $("#ManagerName").val();
        GetManagerOnChange(ManagerID);

        //$("#Managername2 option[value=" + ManagerID + "]").remove();
        // $("#Managername2").remove($("<option></option>").val(u_id));
    });

    $("#btntasks").click(function () {
        $("#Addprojecttask").show();
        $("#Updatetask").hide();
    });

    $("#Addprojecttask").click(function () {
        addtasks();
    });

    $("#Updatetask").click(function () {
        updatetasks();
    });
    var managertype;
    $('input[type=radio][name=optradio]').change(function () {
        if (this.value === 'rad1') {
            $("#managers").show();
            $("#employees").hide();
            $("#managertype").show();

            $('input[type=radio][name=optradio1]').change(function () {
                if (this.value === 'rad3') {
                    managertype = 1;
                } else {
                    managertype = 2;
                }

            });
            getmanagerlist();
        }
        else if (this.value === 'rad2') {
            $("#managers").hide();
            $("#employees").show();
            $("#managertype").hide();
            //$("#employeeslist").empty();
            getemployeelist();
        }
    });


    $("#empbtnassociate").click(function () {

        var UProj_ActiveStatusVal = "";
        if ($("#UProj_ActiveStatus").val() === "1") {

            UProj_ActiveStatusVal = true;
        }
        else {
            UProj_ActiveStatusVal = false;
        }
        var file = document.getElementById("fileUpload").files[0];

        var formdata = new FormData();
        var projectid = projid;
        formdata.append("Usrp_ProfilePicture", file);
        formdata.append("Proj_ProjectID", projid);
        formdata.append("Email", $("#Email").val());
        formdata.append("Usr_UserID", Employeeid);
        formdata.append("Usr_Password", $("#Usr_Password").val());
        formdata.append("Usr_Username", $("#Usr_Username").val());
        formdata.append("Usr_ConfirmPassword", $("#Usr_ConfirmPassword").val());
        formdata.append("Usrp_MobileNumber", $("#Usrp_MobileNumber").val());
        formdata.append("Usr_Titleid", $("#Usr_Titleid").val());
        formdata.append("Usr_GenderId", $("#Usr_GenderId").val());
        formdata.append("UsrP_FirstName", $("#UsrP_FirstName").val());
        formdata.append("UsrP_LastName", $("#UsrP_LastName").val());
        formdata.append("UsrP_EmployeeID", $("#UsrP_EmployeeID").val());
        formdata.append("Usrp_DOJ", $("#Usrp_DOJ").val());
        formdata.append("Usr_UserTypeID", $("#Usr_UserTypeID").val());
        formdata.append("RoleName", $("#RoleName").val());
        formdata.append("ManagerName", $("#ManagerName").val());
        formdata.append("Managername2", $("#Managername2").val());
        formdata.append("Usr_TaskID", $("#Usr_TaskID").val());
        formdata.append("UProj_ParticipationPercentage", $("#UProj_ParticipationPercentage").val());
        formdata.append("UProj_StartDate", $("#UProj_StartDate").val());
        formdata.append("UProj_EndDate", $("#UProj_EndDate").val());
        formdata.append("UProj_ActiveStatus", UProj_ActiveStatusVal);

        $.ajax({
            type: "POST",
            url: "/Client/AssociateEmployee",
            data: formdata,

            dataType: "json",
            complete: function (res) {
                // // 
                alert(res.responseText);
                // $('#ContainerGridDetail').modal('toggle');
                $('#ContainerGridDetail').hide();
                $(".modal-backdrop").hide();
                $('#usertable').dataTable();
                $('#holidaytable').dataTable();
                $('#tasktable').dataTable();
                $('#Projecttable').dataTable();
                if (projid === "") {
                    usergriddata($("#editpid").val());
                    loadholidays($("#editpid").val());
                    loadtaks($("#editpid").val());
                    loadproject($("#editpid").val());
                } else {
                    usergriddata(projid);
                    loadholidays(projid);
                    loadtaks(projid);
                    loadproject(projid);
                }
            },
            contentType: false,
            processData: false,
            error: function (Result) {

            }

        });
    });

    $("#manbtnassociate").click(function () {

        var UProj_ActiveStatusVal = "";
        if ($("#UProj_ActiveStatus").val() === "1") {

            UProj_ActiveStatusVal = true;
        }
        else {
            UProj_ActiveStatusVal = false;
        }
        var file = document.getElementById("fileUpload").files[0];

        var formdata = new FormData();
        var projectid = projid;
        formdata.append("Usrp_ProfilePicture", file);
        formdata.append("Proj_ProjectID", projid);
        formdata.append("Email", $("#Email").val());
        formdata.append("Usr_UserID", Employeeid);
        formdata.append("Usr_Password", $("#Usr_Password").val());
        formdata.append("managertype", managertype);
        formdata.append("Usr_Username", $("#Usr_Username").val());
        formdata.append("Usr_ConfirmPassword", $("#Usr_ConfirmPassword").val());
        formdata.append("Usrp_MobileNumber", $("#Usrp_MobileNumber").val());
        formdata.append("Usr_Titleid", $("#Usr_Titleid").val());
        formdata.append("Usr_GenderId", $("#Usr_GenderId").val());
        formdata.append("UsrP_FirstName", $("#UsrP_FirstName").val());
        formdata.append("UsrP_LastName", $("#UsrP_LastName").val());
        formdata.append("UsrP_EmployeeID", $("#UsrP_EmployeeID").val());
        formdata.append("Usrp_DOJ", $("#Usrp_DOJ").val());
        formdata.append("Usr_UserTypeID", $("#Usr_UserTypeID").val());
        formdata.append("RoleName", $("#RoleName").val());
        formdata.append("ManagerName", $("#ManagerName").val());
        formdata.append("Managername2", $("#Managername2").val());
        formdata.append("Usr_TaskID", $("#Usr_TaskID").val());
        formdata.append("UProj_ParticipationPercentage", $("#UProj_ParticipationPercentage").val());
        formdata.append("UProj_StartDate", $("#UProj_StartDate").val());
        formdata.append("UProj_EndDate", $("#UProj_EndDate").val());
        formdata.append("UProj_ActiveStatus", UProj_ActiveStatusVal);
        formdata.append("CL_ProjectID", $("#project").val());

        $.ajax({
            type: "POST",
            url: "/Client/AssociateManager",
            data: formdata,

            dataType: "json",
            complete: function (res) {
                // // 
                alert(res.responseText);
                // $('#ContainerGridDetail').modal('toggle');
                $('#ContainerGridDetail').hide();
                $(".modal-backdrop").hide();
                $('#usertable').dataTable();
                $('#holidaytable').dataTable();
                $('#tasktable').dataTable();
                $('#Projecttable').dataTable();
                if (projid === "") {
                    usergriddata($("#editpid").val());
                    loadholidays($("#editpid").val());
                    loadtaks($("#editpid").val());
                    loadproject($("#editpid").val());
                } else {
                    usergriddata(projid);
                    loadholidays(projid);
                    loadtaks(projid);
                    loadproject(projid);
                }
            },
            contentType: false,
            processData: false,
            error: function (Result) {

            }

        });
    });

    $("#btnProject").click(function () {
        $("#btnupdateProj").hide();
        $("#btnAddProj").show();
        $("#editproj").hide();
        $("#addproj").show();
        $("#clientprojecttitle").val("");
        $("#clientprojectdescription").val("");
    });


    $('#btnAddProj').click(function () {

        var clientprojecttitle = $("#clientprojecttitle").val();

        var clientprojdescription = $("#clientprojectdescription").val();
        //if (clientprojecttitle === "") {
        //    alert("Please Enter Project Name");
        //} else {
        $('#proForm').validate({
            rules: {
                clientprojecttitle: {
                    required: true
                }
               

            },
            submitHandler: function (form) {
                $.ajax({

                    type: 'POST',
                    url: '/Client/CreateClientProject?client=' + projid,
                    data: {
                        "clientprojecttitle": clientprojecttitle, "clientprojdescription": clientprojdescription
                    },
                    success: function (data) {
                        if (data === "Project Name Already Existed") {
                            $("#clientprojecttitle").addClass("validate_msg");
                        } else {
                            alert(data);
                            // $('#ContainerGridDetail').modal('toggle');
                            $('#ContainerGridProjectDetail').hide();
                            $(".modal-backdrop").hide();
                            $("body").removeClass('modal-open');
                            $('#usertable').dataTable();
                            $('#holidaytable').dataTable();
                            $('#tasktable').dataTable();
                            $('#Projecttable').dataTable();
                            if (projid === "") {
                                usergriddata($("#editpid").val());
                                loadholidays($("#editpid").val());
                                loadtaks($("#editpid").val());
                                loadproject($("#editpid").val());
                            } else {
                                usergriddata(projid);
                                loadholidays(projid);
                                loadtaks(projid);
                                loadproject(projid);
                            }
                        }




                    },
                    complete: function () {
                        //  $('#loading-image').attr("style", "display: none;");
                    },
                    error: function (error) {
                        console.log(error);
                    }

                });
                return false;
            }
        });
           
        //}
        // var client = editprojectid;

      

    });

    $("#Usr_Titleid").change(function () {

        var titleid = $(this).val();

        if (titleid === "1") {
            $("#Usr_GenderId").val(1);
        } else if (titleid === "2" || titleid === "3") {
            $("#Usr_GenderId").val(2);
        } else {
            $("#Usr_GenderId").val("");
        }

    });


    $("#btnupdateProj").click(function () {


        var id = $("#Clientproid").val();

        var clientprojecttitle = $("#clientprojecttitle").val();

        var clientprojdescription = $("#clientprojectdescription").val();

        var accid = $("#accid").val();

        var projid = $("#projid").val();
        $('#proForm').validate({
            rules: {
                clientprojecttitle: {
                    required: true
                }


            },
            submitHandler: function (form) {
                $.ajax({

                    type: 'POST',
                    url: '/Client/updatecp?client=' + id,
                    data: {
                        "clientprojecttitle": clientprojecttitle, "clientprojdescription": clientprojdescription, "projid": projid, "accid": accid
                    },
                    success: function (data) {

                        alert(data);
                        // $('#ContainerGridDetail').modal('toggle');
                        $('#ContainerGridProjectDetail').hide();
                        $(".modal-backdrop").hide();
                        $("body").removeClass('modal-open');
                        $('#usertable').dataTable();
                        $('#holidaytable').dataTable();
                        $('#tasktable').dataTable();
                        $('#Projecttable').dataTable();
                        if (projid === "") {
                            usergriddata($("#editpid").val());
                            loadholidays($("#editpid").val());
                            loadtaks($("#editpid").val());
                            loadproject($("#editpid").val());
                        } else {
                            usergriddata(projid);
                            loadholidays(projid);
                            loadtaks(projid);
                            loadproject(projid);
                        }



                    },
                    complete: function () {
                        //  $('#loading-image').attr("style", "display: none;");
                    },
                    error: function (error) {
                        console.log(error);
                    }

                });
                return false;
            }
        });
            
    });

});
function loaddata() {

    $.ajax({
        url: "/Project/GetProjectCollection",
        type: "Get",
        dataType: "json",
        success: function (res) {

            $('#table').DataTable({
                'data': res,
                'paginate': true,
                'sort': true,
                'Processing': true,
                'columns': [

                    { 'data': 'Proj_ProjectID', 'visible': false },
                    { 'data': 'Proj_ProjectCode' },
                    { 'data': 'Proj_ProjectName' },
                    {
                        'data': 'Proj_ProjectDescription', 'visible': false,
                        'render': function (data, type, row) {

                            if (data !== null) {
                                return data.substr(0, 15);
                            }
                            else {
                                return "";
                            }
                            //return data.substr(0, 15);
                        }
                    },

                    {
                        'data': 'Proj_StartDate',
                        "type": "date ",
                        "render": function (value) {
                            if (value === null) return "";

                            //var pattern = /Date\(([^)]+)\)/;
                            //var results = pattern.exec(value);
                            // dt = new Date(parseFloat(results[1]));

                            //return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                            return dateConversion(value);
                        }
                    },

                    {
                        'data': 'Proj_EndDate',
                        "type": "date ",
                        "render": function (value) {
                            if (value === null) return "";
                            //var pattern = /Date\(([^)]+)\)/;
                            //var results = pattern.exec(value);
                            // dt1 = new Date(parseFloat(results[1]));

                            //return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                            return dateConversion(value);
                        }
                    },
                    
                    //{
                    //    'data': 'Proj_ActiveStatus', 'visible': false,
                    //    //"render": function (Proj_ActiveStatus, type, full, meta) {
                          
                    //    //    if (Proj_ActiveStatus === true) {

                    //    //        return '<div class="statuscheck"> <input type="checkbox" id="check_01" checked   onclick="UnCheckStatus(' + full.Proj_ProjectID + ')"> <label for="check_01"></label> </div>';
                    //    //    }
                    //    //    else {
                    //    //        return '<div class="statuscheck"> <input type="checkbox" id="check_02"   onclick="CheckStatus(' + full.Proj_ProjectID + ')"> <label for="check_02"></label> </div>';
                    //    //    }
                    //    //}
                    //},

                    {
                        "render": function (Proj_ProjectID, type, full, meta, data) {
                            // var permissions = '@ViewBag.a';
                            // perms = permissions;
                            if (permissions === "Read") {
                                // return '<a class="btn btn-icn" data-toggle="modal"  title="Edit"  id="edit"  onclick="EditUser(' + full.Proj_ProjectID + ')" ><i class="fa fa-edit"></i></a><a class="btn btn-icn" title="Delete" data-target="#containerDelete" data-toggle="modal" data-id="' + full.Proj_ProjectID + '"  onclick="DeleteSkill(' + full.Proj_ProjectID + ')" "><i class="fa fa-trash"></i></a>';
                                return '<a class="btn btn-icn btn-icn-hide " data-toggle="modal"  title="Edit"  id="edit"  onclick="Preview(' + full.Proj_ProjectID + ')" ><i class="fa fa-eye"></i></a>';

                            }
                            else if (permissions === "Read/Write") {
                                return '<a class="btn btn-icn btn-icn-hide  edit" data-toggle="modal"  title="Edit"  id="edit"  onclick="EditUser(' + full.Proj_ProjectID + ')" ><i class="fa fa-edit"></i></a><a class="btn btn-icn btn-icn-hide" title="Delete" data-target="#procontainerDelete" data-toggle="modal" data-id="' + full.Proj_ProjectID + '"  onclick="GetProId(' + full.Proj_ProjectID + ')" "><i class="fa fa-trash"></i></a>';

                            } else {
                                return '<a class="btn btn-icn  " data-toggle="modal"  title="Edit"  id="edit"  onclick="Preview(' + full.Proj_ProjectID + ')" ><i class="fa fa-eye"></i></a>';

                            }

                        },

                    },
                    //{
                    //    data: null, render: function (data, type, row) {

                    //        return '<a class="btn btn-icn" title="Delete" data-target="#containerDelete" data-toggle="modal" data-id="' + row.Proj_ProjectID + '"  onclick="DeleteSkill(' + row.Proj_ProjectID + ')" "><i class="fa fa-trash"></i></a>';

                    //    }

                    //},

                ]
            });

        },
        error: function (msg) {
            // alert(msg.responseText);
        }
    });
}

function loadproject(id) {
    $.ajax({
        url: "/Client/getclientprojects?projid=" + id,
        type: "Get",
        dataType: "json",
        success: function (res) {

            $('#Projecttable').DataTable({
                'data': res,
                'paginate': true,
                'sort': true,
                'Processing': true,
                'destroy': true,
                'columns': [

                    { 'data': 'Proj_ProjectID', visible: false, },
                    { 'data': 'Accountid', visible: false, },
                    { 'data': 'ClientProjTitle' },
                    { 'data': 'ClientProjDesc' },
                    {
                        "render": function (CL_ProjectID, type, full, meta, data) {
                            if (usrpermission === "Read") {
                                return '<a class="btn btn-icn " data-toggle="modal"  data-target="#ContainerGridProjectDetail" id="edit" data-rolID=' + full.Usr_RoleID + ' title="Edit" onclick="EditClientProj(' + full.CL_ProjectID + ')" ><i class="fa fa-edit" aria-hidden="true"></i></a>';

                            } else if (usrpermission === "Read/Write") {
                                return '<a class="btn btn-icn btn-icn-hide" data-toggle="modal"  data-target="#ContainerGridProjectDetail" id="edit" data-rolID=' + full.Usr_RoleID + ' title="Edit" onclick="EditClientProj(' + full.CL_ProjectID + ')" ><i class="fa fa-edit" aria-hidden="true"></i></a>';

                            } else {
                                return '<a class="btn btn-icn " data-toggle="modal"  data-target="#ContainerGridProjectDetail" id="edit" data-rolID=' + full.Usr_RoleID + ' title="Edit" onclick="EditClientProj(' + full.CL_ProjectID + ')" ><i class="fa fa-edit" aria-hidden="true"></i></a>';

                            }


                        }
                    },

                ]
            });

        },
        error: function (msg) {
            // alert(msg.responseText);
        }
    });



}

function EditClientProj(id) {
    $("#btnupdateProj").show();
    $("#btnAddProj").hide();
    $("#editproj").show();
    $("#addproj").hide();

    $.ajax({

        url: '/Client/UpdateClientProject?id=' + id,
        type: 'Get',
        dataType: "Json",
        success: function (data) {
            //alert("OK");
            //   $("#ContainerGridProjectDetail").show();
            $("#accid").val(data.Accid);

            $("#projid").val(data.ProjectID)

            $("#Clientproid").val(data.Cl_projid);

            //   $("#accid").val(data.acc)
            $("#clientprojecttitle").val(data.clientprojecttitle);

            $("#clientprojectdescription").val(data.clientprojectdescription);

            //   $("#btnupdateProj").text('Update');


        },

        error: function () {
            //alert("No");
            alert(Response.text);
        }

    });

}


function UnCheckStatus(id) {
    $.ajax({
        url: "/Project/ChangeStatus",
        type: "POST",
        data: {
            'id': id,
            'status': false
        },

        //dataType: "json",
        success: function (data) {

            alert(data);
            window.location.reload();
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        },
    });
}

function CheckStatus(id) {
    $.ajax({
        url: "/Project/ChangeStatus",
        type: "POST",
        data: {
            'id': id,
            'status': true
        },

        //dataType: "json",
        success: function (data) {

            alert(data);
            window.location.reload();
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        },
    });
}

function GetProId(id) {
    delproid = id;
}

function SequenceClientCode() {
    var sequenceCode;
    $.ajax({
        type: "GET",
        url: "/Client/SequenceCode",
        dataType: "json",
        success: function (res) {
            $("#Proj_ProjectCode").val(res);
            $("#Proj_AccountID").val(Proj_AccountID);
        },

        error: function (Result) {

        }

    });
}

function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            //$('#profile-image').css('background-image', 'url(' + e.target.result + ')');
            $('#profile-image').prop('src', e.target.result);
            //css('background-image', 'url(' + e.target.result + ')');
            $('#profile-image').hide();
            $('#profile-image').fadeIn(650);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

function countryChange() {

    SelectedCountryId = $("#CountryId").val();
    $("#StateId").empty();
    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Client/GetStates?CountryId=" + SelectedCountryId,
        //data: "{}",
        dataType: "json",
        async: false,
        success: function (Result) {
            console.log(Result);
            $(Result).each(function () {

                $("#StateId").append($("<option></option>").val(this.StateId).html(this.StateName));

            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });
}

function statebindfunction(countryid) {

    SelectedCountryId = $("#CountryId").val();
    $("#StateId").empty();
    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Client/GetStates?CountryId=" + countryid,
        //data: "{}",
        dataType: "json",
        async: false,
        success: function (Result) {
            console.log(Result);
            $(Result).each(function () {

                $("#StateId").append($("<option></option>").val(this.StateId).html(this.StateName));

            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });
}

function dateConversion(value) {
    if (value === null) return "";
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));

    return dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();

}

function add() {

    //var Proj_ProjectCode = $("#Proj_ProjectCode").val();
    Proj_ProjectName = $("#Proj_ProjectName").val();
    var Proj_ProjectDescription = $("#Proj_ProjectDescription").val();
    // var Proj_StartDate = $("#Proj_StartDate").val();
    //  var Proj_EndDate = $("#Proj_EndDate").val();
    var Proj_ActiveStatus = $("#Proj_ActiveStatus").val();
    //var AccountName = $("#AccountName").val();
    var CountryId = $("#CountryId").val();
    var StateId = $("#StateId").val();
    var WebUrl = $("#WebUrl").val();
    //var Is_Timesheet_ProjectSpecific = $("#Is_Timesheet_ProjectSpecific").val();
    if ($('input[name="Is_Timesheet_ProjectSpecific"]').is(":checked")) {
        var Is_Timesheet_ProjectSpecific = true;
    }
    else if ($('input[name="Is_Timesheet_ProjectSpecific"]').is(":not(:checked)")) {
        Is_Timesheet_ProjectSpecific = false;
    }
    if (Proj_ActiveStatus === "1") {

        var Proj_ActiveStatusVal = true;
    }
    else {
        Proj_ActiveStatusVal = false;
    }
    $.ajax({
        type: 'POST',
        //contentType: "application/json; charset=utf-8",
        url: "/Client/CreateClient",
        data: {
            Proj_ProjectName: Proj_ProjectName, Proj_ProjectDescription: Proj_ProjectDescription,
            Proj_AccountID: Proj_AccountID, Proj_ActiveStatus: Proj_ActiveStatusVal,
            CountryId: CountryId, StateId: StateId, WebUrl: WebUrl, Is_Timesheet_ProjectSpecific: Is_Timesheet_ProjectSpecific
        },
        dataType: "json",

        success: function (res) {
            console.log(res.ProjectId);
            if (res.ProjectId === 0) {
                $("#Proj_ProjectName").addClass();
            } else {
                alert("Successfully Added");

                projid = res.ProjectId;
                usergriddata(projid);
                loadholidays(projid);
                loadtaks(projid);
                loadproject(projid);
            }
           
        },


        error: function (Result) {

            //alert("Error");

        }

    });

}

function UpdateSingleRecord() {

  //  var Proj_ProjectCode = $("#Proj_ProjectCode").val();
    Proj_ProjectName = $("#Proj_ProjectName").val();
    var Proj_ProjectDescription = $("#Proj_ProjectDescription").val();
    //var Proj_StartDate = $("#Proj_StartDate").val();
    // var Proj_EndDate = $("#Proj_EndDate").val();
    var Proj_ActiveStatus = $("#Proj_ActiveStatus").val();

    var CountryId = $("#CountryId").val();
    var StateId = $("#StateId").val();
    var WebUrl = $("#WebUrl").val();
    if ($('input[name="Is_Timesheet_ProjectSpecific"]').is(":checked")) {
        var Is_Timesheet_ProjectSpecific = true;
    }
    else if ($('input[name="Is_Timesheet_ProjectSpecific"]').is(":not(:checked)")) {
        Is_Timesheet_ProjectSpecific = false;
    }
    if (Proj_ActiveStatus === "1") {
        var Proj_ActiveStatusVal = true;
    }
    else {
        Proj_ActiveStatusVal = false;
    }
    $.ajax({

        type: "POST",
        // contentType: "application/json; charset=utf-8",
        url: "/Client/UpdateClient",
        data: {
            Proj_ProjectID: projid, Proj_ProjectName: Proj_ProjectName, Proj_ProjectDescription: Proj_ProjectDescription,
            Proj_AccountID: Proj_AccountID, Proj_ActiveStatus: Proj_ActiveStatusVal,
            CountryId: CountryId, StateId: StateId, WebUrl: WebUrl, Is_Timesheet_ProjectSpecific: Is_Timesheet_ProjectSpecific
        },
        dataType: "json",
        // cache: false,
        complete: function (res) {
            alert(res.responseText);
            //  window.location.href = "/UserType/Index";
            //loaddata();
        },

        error: function (Result) {

        }

    });
}

function bindProjectNames() {

    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Project/GetProjectNames",
        //data: "{}",
        dataType: "json",
        success: function (Result) {

            $(Result).each(function () {

                $("#ProjectName").append($("<option></option>").val(this.Proj_ProjectID).html(this.Proj_ProjectName));

            });
        },
        error: function (Result) {

            //alert("Error");

        }

    });
}

function bindUserNames() {

    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/ProjectAllocation/GetUserNames",
        //data: "{}",
        dataType: "json",
        success: function (Result) {

            $(Result).each(function () {

                $("#Username").append($("<option></option>").val(this.UProj_UserID).html(this.Username));

            });
        },
        error: function (Result) {



        }

    });
}

function bindRoleNames() {
    $("#RoleName").empty();
    $.ajax({

        type: "GET",
        // contentType: "application/json; charset=utf-8",
        url: "/Client/GetRoles"/*+ '&RoleId=' + RoleId*/,
        dataType: "json",
        async: false,
        success: function (Result) {

            $(Result).each(function () {

                $("#RoleName").append($("<option></option>").val(this.GenericRoleID).html(this.Title));


            });

        },
        error: function (Result) {

            // alert("Error");

        }

    });

    GetAlltasknames($("#RoleName").val());
}

function bindAllRoleNames() {
    $("#RoleName").empty();
    $.ajax({

        type: "GET",
        // contentType: "application/json; charset=utf-8",
        url: "/Client/GetAllRoles"  /*+ '&RoleId=' + RoleId*/,
        dataType: "json",
        async: false,
        success: function (Result) {

            $(Result).each(function () {

                $("#RoleName").append($("<option></option>").val(this.GenericRoleID).html(this.Title));


            });

        },
        error: function (Result) {

            // alert("Error");

        }

    });
}

function bindUserTypes() {

    $("#Usr_UserTypeID").empty();

    $.ajax({

        type: "GET",
        // contentType: "application/json; charset=utf-8",
        url: "/User/GetUserTypesByAccountid",
        dataType: "json",
        async: true,
        success: function (Result) {

            $(Result).each(function () {

                $("#Usr_UserTypeID").append($("<option></option>").val(this.Usr_UserTypeID).html(this.UserType));

            });

        },
        error: function (Result) {

            // alert("Error");

        }

    });
}

function GetManagerByRole(userid) {
    debugger;
    uid = userid;
    $("#ManagerName").empty();
    $("#Managername2").empty();
    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Client/GetManagerByRole?projid=" + projid + "&userid=" + userid,
        //data: "{}",
        dataType: "json",
        success: function (Result) {
            $("#ManagerName").append($("<option></option>").val("").html("Select Manager"));
            $("#Managername2").append($("<option></option>").val("").html("Select Manager"));
            $(Result).each(function () {

                $("#ManagerName").append($("<option></option>").val(this.Usr_UserID).html(this.UsrP_FirstName));
                $("#Managername2").append($("<option></option>").val(this.Usr_UserID).html(this.UsrP_FirstName));


            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });



}

function GetManagerOnChange(ManagerID) {


    $("#Managername2").empty();

    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Client/GetManagerOnChange?projid=" + projid + "&ManagerID=" + ManagerID /*+ "&userid=" + uid*/,
        //data: "{}",
        dataType: "json",
        success: function (Result) {

            $(Result).each(function () {

                // $("#ManagerName").append($("<option></option>").val(this.Usr_UserID).html(this.UsrP_FirstName));
                $("#Managername2").append($("<option></option>").val(this.Usr_UserID).html(this.UsrP_FirstName));


            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });



}

function Manger2() {
    $("#ManagerName").empty();
        $("#Managername2").empty();
    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Client/GetL2Manager?projid=" + projid,
        //data: "{}",
        dataType: "json",
        success: function (Result) {
            $("#ManagerName").append($("<option></option>").val("").html("Select Manager"));
            $("#Managername2").append($("<option></option>").val("").html("Select Manager"));

            $(Result).each(function () {
                
                $("#ManagerName").append($("<option></option>").val(this.Usr_UserID).html(this.UsrP_FirstName));
                $("#Managername2").append($("<option></option>").val(this.Usr_UserID).html(this.UsrP_FirstName));


            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });
}
function getclientforproject() {
    $("#project").empty();
    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Client/getclientforproject?projid=" + projid,
        //data: "{}",
        dataType: "json",
        success: function (Result) {

            $(Result).each(function () {

                $("#project").append($("<option></option>").val(this.CL_ProjectID).html(this.ClientProjTitle));
                //   $("#Managername2").append($("<option></option>").val(this.Usr_UserID).html(this.UsrP_FirstName));


            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });
}

function Manger1() {

    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Client/GetL2Manager?projid=" + projid,
        //data: "{}",
        dataType: "json",
        success: function (Result) {
            $("#Managername2").append($("<option></option>").val("").html("Select Manager"));
            $(Result).each(function () {
               
                // $("#ManagerName").append($("<option></option>").val(this.Usr_UserID).html(this.UsrP_FirstName));
                $("#Managername2").append($("<option></option>").val(this.Usr_UserID).html(this.UsrP_FirstName));


            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });
}

function GetAlltasknames(id) {



    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Client/GetAlltasknames?projid=" + projid + "&Roleid=" + id,
        //data: "{}",
        dataType: "json",
        success: function (Result) {
            $("#Usr_TaskID").empty();
            $(Result).each(function () {

                $("#Usr_TaskID").append($("<option></option>").val(this.Proj_SpecificTaskId).html(this.Proj_SpecificTaskName));


            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });
}

function gettaskanames() {

    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Client/gettasknames?projid=" + projid,
        //data: "{}",
        dataType: "json",
        success: function (Result) {
            $("#Usr_TaskID").empty();
            $(Result).each(function () {

                $("#Usr_TaskID").append($("<option></option>").val(this.Proj_SpecificTaskId).html(this.Proj_SpecificTaskName));


            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });
}

function GetRoleNamesbyemp() {
    $("#RoleName").empty();
    $.ajax({

        type: "GET",
        // contentType: "application/json; charset=utf-8",
        url: "/Client/GetRoleNamesbyemp"  /*+ '&RoleId=' + RoleId*/,
        dataType: "json",
        async: false,
        success: function (Result) {

            $(Result).each(function () {

                $("#RoleName").append($("<option></option>").val(this.GenericRoleID).html(this.Title));

                //roleids = $("#RoleName").val(this.GenericRoleID);
            });

        },
        error: function (Result) {

        }

    });

    // GetAlltasknames($("#RoleName").val());
}



function modelclick() {

    $("#add").show();
    $("#UProj_L1_ManagerId").val(0);
    // $('input[type=checkbox]').prop('checked', false);
    // $("#Username option[value='option1']").remove();
    $("#Usr_Username").val(null);
    $("#Email").val(null);
    $("#Usr_Password").val(null);
    $("#Usr_ConfirmPassword").val(null);
    $("#Usr_Titleid").val(null);
    $("#UsrP_FirstName").val(null);

    $("#UsrP_LastName").val(null);
    $("#Usrp_MobileNumber").val(null);
    $("#Usrp_DOJ").val(null);
    $("#Usr_GenderId").val(null);
    $("#UsrP_EmployeeID").val(null);
    $("#UProj_ParticipationPercentage").val(null);
    $("#UProj_StartDate").val(null);
    $("#Proj_StartDate").val(null);
    $("#UProj_EndDate").val(null);
    $("#Usr_TaskID").val(null);
    $("#ProjectName").val(Proj_ProjectName);
    $("#ContainerGridDetail").show();
    $("#ContainerGridDetail").addClass('in');
    $("body").addClass('modal-open');
    $(UProj_ActiveStatus).val(1);

   

    var L1Manager = $("#UProj_L1_ManagerId").val();
    var L2Manager = $("#UProj_L2_ManagerId").val();

}




function ManagerSaveData() {
    //if ($("#UProj_ActiveStatus").val() === "" || projid === "" || $("#Email").val() === "" || $("#Usr_Password").val() === "" || $("#Usr_Username").val() === "" || $("#Usrp_MobileNumber").val() === "" || $("#Usr_Titleid").val() === "")
    //{
    //    alert("Please Fill Mandatory fields");
    //} else if ($("#Usr_GenderId").val() === "" || $("#UsrP_FirstName").val() === "" || $("#UsrP_LastName").val() === "" || $("#Usr_UserTypeID").val() === "") 
    //{
    //    alert("Please Fill Mandatory fields");
    //} else if ($("#RoleName").val() === "" || $("#project").val() === "" || $("#Usr_TaskID").val() === "" || $("#TimesheetMode_id").val() === "")
    //{
    //    alert("Please Fill Mandatory fields");
    //} else
    //{
        var UProj_ActiveStatusVal = "";
        if ($("#UProj_ActiveStatus").val() === "1") {

            UProj_ActiveStatusVal = true;
        }
        else {
            UProj_ActiveStatusVal = false;
        }
        var file = document.getElementById("fileUpload").files[0];
        var projectid = projid;
        var formdata = new FormData();

        formdata.append("Usrp_ProfilePicture", file);
        formdata.append("Proj_ProjectID", projid);
        formdata.append("Email", $("#Email").val());
        formdata.append("Usr_Password", $("#Usr_Password").val());
        formdata.append("Usr_Username", $("#Usr_Username").val());
        formdata.append("Usr_ConfirmPassword", $("#Usr_ConfirmPassword").val());
    formdata.append("Usrp_MobileNumber", $("#Usrp_MobileNumber").val());
        formdata.append("Usr_Titleid", $("#Usr_Titleid").val());
        formdata.append("Usr_GenderId", $("#Usr_GenderId").val());
        formdata.append("UsrP_FirstName", $("#UsrP_FirstName").val());
        formdata.append("UsrP_LastName", $("#UsrP_LastName").val());
        formdata.append("UsrP_EmployeeID", $("#UsrP_EmployeeID").val());
        formdata.append("Usrp_DOJ", $("#Usrp_DOJ").val());
        formdata.append("Usr_UserTypeID", $("#Usr_UserTypeID").val());
        formdata.append("RoleName", $("#RoleName").val());
        formdata.append("ManagerName", $("#ManagerName").val());
        formdata.append("Managername2", $("#Managername2").val());
        formdata.append("Usr_TaskID", $("#Usr_TaskID").val());
        formdata.append("UProj_ParticipationPercentage", $("#UProj_ParticipationPercentage").val());
        formdata.append("UProj_StartDate", $("#UProj_StartDate").val());
        formdata.append("UProj_EndDate", $("#UProj_EndDate").val());
        formdata.append("UProj_ActiveStatus", UProj_ActiveStatusVal);
        formdata.append("CL_ProjectID", $("#project").val());
    formdata.append("TimesheetMode_id", $("#TimesheetMode_id").val());
    formdata.append("Usrp_CountryCode", $("#Usrp_CountryCode").val());

        $.ajax({
            type: "POST",
            url: "/Client/ManagerSave",
            data: formdata,

            dataType: "json",
            complete: function (res) {
                if (res.responseText === "UserName already Exists") {
                    $("#Usr_Username").addClass("validate_msg");
                    $("#Email").removeClass("validate_msg");

                } else if (res.responseText === "Loginid already Exists") {
                    $("#Usr_Username").removeClass("validate_msg");
                    $("#Email").addClass("validate_msg");
                }
                else {
                    $("#Usr_Username").removeClass("validate_msg");
                    $("#Email").removeClass("validate_msg");

                    alert(res.responseText);
                    // $('#ContainerGridDetail').modal('toggle');
                    $('#ContainerGridDetail').hide();
                    $(".modal-backdrop").hide();
                    $("body").removeClass('modal-open');
                    $('#usertable').dataTable();
                    $('#holidaytable').dataTable();
                    $('#tasktable').dataTable();
                    $('#Projecttable').dataTable();
                   
                    if (projid === "") {
                        usergriddata($("#editpid").val());
                        loadholidays($("#editpid").val());
                        loadtaks($("#editpid").val());
                        loadproject($("#editpid").val());
                    } else {
                        usergriddata(projid);
                        loadholidays(projid);
                        loadtaks(projid);
                        loadproject(projid);
                    }
                    window.location.reload();
                }


            },
            contentType: false,
            processData: false,
            error: function (Result) {
                console.log(Result);
            }

        });
    
   
}


function AddEmployee() {
    //if ($("#UProj_ActiveStatus").val() === "" || projid === "" || $("#Email").val() === "" || $("#Usr_Password").val() === "" || $("#Usr_Username").val() === "" || $("#Usrp_MobileNumber").val() === "" || $("#Usr_Titleid").val() === "") {
    //    alert("Please Fill Mandatory fields");
    //} else if ($("#Usr_GenderId").val() === "" || $("#UsrP_FirstName").val() === "" || $("#UsrP_LastName").val() === "" ||$("#Usr_UserTypeID").val() === "") {
    //    alert("Please Fill Mandatory fields");
    //} else if ($("#RoleName").val() === "" || $("#project").val() === "" || $("#Usr_TaskID").val() === "" || $("#ManagerName").val() === "0" || $("#TimesheetMode_id").val() === "") {
    //    alert("Please Fill Mandatory fields");
    //} else {
        var UProj_ActiveStatusVal = "";
        if ($("#UProj_ActiveStatus").val() === "1") {

            UProj_ActiveStatusVal = true;
        }
        else {
            UProj_ActiveStatusVal = false;
        }
        var file = document.getElementById("fileUpload").files[0];

        var formdata = new FormData();
        var projectid = projid;
        formdata.append("Usrp_ProfilePicture", file);
        formdata.append("Proj_ProjectID", projid);

        formdata.append("Email", $("#Email").val());
        formdata.append("Usr_Password", $("#Usr_Password").val());
        formdata.append("Usr_Username", $("#Usr_Username").val());
        formdata.append("Usr_ConfirmPassword", $("#Usr_ConfirmPassword").val());
    formdata.append("Usrp_MobileNumber", $("#Usrp_MobileNumber").val());
        formdata.append("Usr_Titleid", $("#Usr_Titleid").val());
        formdata.append("Usr_GenderId", $("#Usr_GenderId").val());
        formdata.append("UsrP_FirstName", $("#UsrP_FirstName").val());
        formdata.append("UsrP_LastName", $("#UsrP_LastName").val());
        formdata.append("UsrP_EmployeeID", $("#UsrP_EmployeeID").val());
        formdata.append("Usrp_DOJ", $("#Usrp_DOJ").val());
        formdata.append("Usr_UserTypeID", $("#Usr_UserTypeID").val());
        formdata.append("RoleName", $("#RoleName").val());
        formdata.append("ManagerName", $("#ManagerName").val());
        formdata.append("Managername2", $("#Managername2").val());
        formdata.append("Usr_TaskID", $("#Usr_TaskID").val());
        formdata.append("UProj_ParticipationPercentage", $("#UProj_ParticipationPercentage").val());
        formdata.append("UProj_StartDate", $("#UProj_StartDate").val());
        formdata.append("UProj_EndDate", $("#UProj_EndDate").val());
        formdata.append("UProj_ActiveStatus", UProj_ActiveStatusVal);
        formdata.append("CL_ProjectID", $("#project").val());
    formdata.append("TimesheetMode_id", $("#TimesheetMode_id").val());
    formdata.append("Usrp_CountryCode", $("#Usrp_CountryCode").val());

        $.ajax({
            type: "POST",
            url: "/Client/SaveEmployee",
            data: formdata,

            dataType: "json",
            complete: function (res) {
                if (res.responseText === "UserName already Exists") {
                    $("#Usr_Username").addClass("validate_msg");
                    $("#Email").removeClass("validate_msg");

                } else if (res.responseText === "Loginid already Exists") {
                    $("#Usr_Username").removeClass("validate_msg");
                    $("#Email").addClass("validate_msg");
                } else {
                    $("#Usr_Username").removeClass("validate_msg");
                    $("#Email").removeClass("validate_msg");
                    alert(res.responseText);
                    // $('#ContainerGridDetail').modal('toggle');
                    $('#ContainerGridDetail').hide();
                    $(".modal-backdrop").hide();
                    $("body").removeClass('modal-open');
                    $('#usertable').dataTable();
                    $('#holidaytable').dataTable();
                    $('#tasktable').dataTable();
                    $('#Projecttable').dataTable();
                    if (projid === "") {
                        usergriddata($("#editpid").val());
                        loadholidays($("#editpid").val());
                        loadtaks($("#editpid").val());
                        loadproject($("#editpid").val());
                    } else {
                        usergriddata(projid);
                        loadholidays(projid);
                        loadtaks(projid);
                        loadproject(projid);
                    }
                    window.location.reload();
                }

            },
            contentType: false,
            processData: false,
            error: function (Result) {

            }

        });
    
}


function usergriddata(pid) {
    // // 
    // // 
    $.ajax({
        url: "/Client/GetProjectAllocationCollection?id=" + pid,
        type: "Get",
        dataType: "json",

        success: function (res) {


            $('#usertable').DataTable({
                'data': res,
                'paginate': true,
                'sort': true,
                'Processing': true,
                'destroy': true,
                'columns': [


                    { 'data': 'Usr_UserID', visible: false },
                    { 'data': 'Usr_RoleID', visible: false },
                    { 'data': 'UsrP_EmployeeID', visible: false },
                    { 'data': 'UsrP_FirstName' },

                    { 'data': 'project' },

                    { 'data': 'RoleName' },
                    { 'data': 'UProj_ParticipationPercentage' },
                    { 'data': 'Email', visible: false },
                    {
                        'data': 'UProj_StartDate', visible: false,
                        "type": "date ",
                        "render": function (value) {
                            if (value === null) return "";

                            //var pattern = /Date\(([^)]+)\)/;
                            //var results = pattern.exec(value);
                            // dt = new Date(parseFloat(results[1]));

                            //return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                            return dateConversion(value);
                        }
                    },

                    {
                        'data': 'UProj_EndDate', visible: false,
                        "type": "date ",
                        "render": function (value) {
                            if (value === null) return "";

                            //var pattern = /Date\(([^)]+)\)/;
                            //var results = pattern.exec(value);
                            // dt = new Date(parseFloat(results[1]));

                            //return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                            return dateConversion(value);
                        }
                    },
                    {
                        "render": function (Usr_UserID, type, full, meta, data) {
                            if (usrpermission === "Read") {
                                return '<a class="btn btn-icn btn-icn-hide" data-toggle="modal"  data-target="#ContainerGridDetail" id="edit" data-rolID=' + full.Usr_RoleID + ' title="Edit" onclick="Userpreview(' + full.Usr_UserID + ')" ><i class="fa fa-eye" aria-hidden="true"></i></a>';
                                // return '<a class="btn btn-icn btn-icn-hide " data-toggle="modal"  title="Edit"  id="edit"  onclick="Preview(' + full.Proj_ProjectID + ')" ><i class="fa fa-eye"></i></a>';

                            }
                            else if (usrpermission === "Read/Write") {
                                return '<a class="btn btn-icn btn-icn-hide" data-toggle="modal"  data-target="#ContainerGridDetail" id="edit" data-rolID=' + full.Usr_RoleID + ' title="Edit" onclick="EditUserProject(' + full.Usr_UserID + ')" ><i class="fa fa-edit" aria-hidden="true"></i></a><a class="btn btn-icn btn-icn-hide" data-target="#usercontainerDelete" data-toggle="modal"  data-id="' + full.Usr_UserID + '" title="Delete" onclick="GetUserid(' + full.Usr_UserID + ')" "><i class="fa fa-trash" aria-hidden="true"></i></a>';

                                // return '<a class="btn btn-icn btn-icn-hide  edit" data-toggle="modal"  title="Edit"  id="edit"  onclick="EditUser(' + full.Proj_ProjectID + ')" ><i class="fa fa-edit"></i></a><a class="btn btn-icn btn-icn-hide" title="Delete" data-target="#containerDelete" data-toggle="modal" data-id="' + full.Proj_ProjectID + '"  onclick="DeleteSkill(' + full.Proj_ProjectID + ')" "><i class="fa fa-trash"></i></a>';

                            }
                            else {
                                return '<a class="btn btn-icn " data-toggle="modal"  data-target="#ContainerGridDetail" id="edit" data-rolID=' + full.Usr_RoleID + ' title="Edit" onclick="Userpreview(' + full.Usr_UserID + ')" ><i class="fa fa-eye" aria-hidden="true"></i></a>';

                            }

                        },

                    },




                ]
            });
            function dateConversion(value) {
                if (value === null) return "";
                var pattern = /Date\(([^)]+)\)/;
                var results = pattern.exec(value);
                var dt = new Date(parseFloat(results[1]));

                return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();

            }
        }


    });
}

function GetUserid(userid) {
    deluserid = userid;
}

function EditUserProject(userid) {
    ////var roledata = $("#RoleName").val();
    debugger;
    ////alert(roledata);
    $("#ManagerName").empty();
    $("#Managername2").empty();
    //modelclick();
    bindProjectNames();
    bindAllRoleNames();
    gettaskanames();
    //GetAlltasknames($("#RoleName").val());
    bindUserNames();
    bindUserTypes();
    //Manger2();
    // 
    $("#add").text('Edit User');
    getclientforproject();
    GetManagerByRole(userid);
    $("#btnUpdateModel").show();
    $("#btnManAddModel").hide();
    $("#btnEmpAddModel").hide();
    $("#manbtnassociate").hide();
    $("#empbtnassociate").hide();
    $.ajax({

        url: '/Client/GetUserDetailById?id=' + userid,
        type: 'Get',
        data: {
            "proid": projid
        },
        success: function (data) {
            debugger;
            var Usr_UserID = data.Usr_UserID;
            var Email = data.Email;
            var Usr_RoleID = data.Usr_RoleID;
           
            var UsrP_EmployeeID = data.UsrP_EmployeeID;
            Proj_ProjectID = data.Proj_ProjectID;
            var Proj_ProjectName = data.Proj_ProjectName;
            var UProj_ParticipationPercentage = data.UProj_ParticipationPercentage;
            var UsrP_FirstName = data.UsrP_FirstName;
            var UsrP_LastName = data.UsrP_LastName;
            var Usr_Password = data.Usr_Password;
            var Usr_Titleid = data.Usr_Titleid;
            var Usrp_MobileNumber = data.Usrp_MobileNumber;
            var Usr_GenderId = data.Usr_GenderId;
            var Usrp_ProfilePicture = data.Usrp_ProfilePicture;
            var Usr_UserTypeID = data.Usr_UserTypeID;
            var Usr_TaskID = data.Usr_TaskID;
            var ManagerName = data.ManagerName;
            var Managername2 = data.Managername2;
            var Usr_Username = data.Usr_Username;
            var TimesheetMode_id = data.TimesheetMode_id;
            var UProj_ActiveStatus = data.UProj_ActiveStatus;
            console.log(UProj_ActiveStatus);

            if (UProj_ActiveStatus === true) {
                $("#UProj_ActiveStatus").val("1");
            } else {
                $("#UProj_ActiveStatus").val("0");
            }
            //if (data.Usrp_DOJ !== null) {
            //    var DOJ = (data.Usrp_DOJ).substr(8);
            //}

            //if (data.UProj_StartDate !== null) {
            //    var StartDate = (data.UProj_StartDate).substr(8);
            //}
            //if (data.UProj_EndDate !== null) {
            //    var EndDate = (data.UProj_EndDate).substr(8);
            //}
            //var Proj_StartDate = new Date(parseInt(StartDate));

            //var Proj_EndDate = new Date(parseInt(EndDate));

            //var doj = new Date(parseInt(DOJ));

            $("#Usr_UserID").val(Usr_UserID);
            $("#Usr_Username").val(Usr_Username);
            $("#Email").val(Email);
            $("#Usr_Password").val();
            $("#Usr_ConfirmPassword").val();
            $("#Usrp_MobileNumber").val(Usrp_MobileNumber);
            $("#Usr_Titleid").val(Usr_Titleid);
            $("#Usr_GenderId").val(Usr_GenderId);
            $("#UsrP_FirstName").val(UsrP_FirstName);
            $("#UsrP_LastName").val(UsrP_LastName);
            $("#UsrP_EmployeeID").val(UsrP_EmployeeID);
            $("#TimesheetMode_id").val(TimesheetMode_id);
            $("#Usr_UserTypeID").val(Usr_UserTypeID);
            $("#RoleName").val(Usr_RoleID);
            $("#ManagerName").val(data.ManagerName);
            $("#Managername2").val(Managername2);
            $("#Usr_TaskID").val(Usr_TaskID);
            $("#project").val(data.Cl_projid);
            $("#Usrp_CountryCode").val(data.Usrp_CountryCode);
            $("#UProj_ParticipationPercentage").val(UProj_ParticipationPercentage);
            //  $("#UProj_ActiveStatus").val(UProj_ActiveStatus);

            var folderName = "/uploadimages/images/" + Usrp_ProfilePicture;

            $("#profile-image").prop('src', folderName);
            // $("#Usrp_DOJ").val(doj.format("yy-mm-dd"));
            //$("#Usrp_DOJ").val(doj.format('mm/dd/yyyy'));


            //$("#UProj_StartDate").val(Proj_StartDate.format('mm/dd/yyyy'));
            //$("#UProj_EndDate").val(Proj_EndDate.format('mm/dd/yyyy'));



            //$("#UProj_StartDate").val(UProj_StartDate);
            //$("#UProj_EndDate").val(UProj_EndDate);

        },
        error: function () {
            alert(Response.text);
        }
    });
}

function Userpreview(userid) {

    $("#ManagerName").empty();
    $("#Managername2").empty();
    //modelclick();
    bindProjectNames();
    bindAllRoleNames();
    GetAlltasknames(roledata);
    bindUserNames();
    bindUserTypes();
    //Manger2();

    GetManagerByRole(userid);
    // $("#btnUpdateModel").show();
    $("#btnManAddModel").hide();
    $("#btnEmpAddModel").hide();
    // $("#btnassociateemp").hide();
    $.ajax({

        url: '/Client/GetUserDetailById?id=' + userid,
        type: 'Get',
        data: {
            "proid": projid
        },
        success: function (data) {

            var Usr_UserID = data.Usr_UserID;
            var Email = data.Email;
            var Usr_RoleID = data.Usr_RoleID;
            // alert(Usr_RoleID);
            var UsrP_EmployeeID = data.UsrP_EmployeeID;
            Proj_ProjectID = data.Proj_ProjectID;
            var Proj_ProjectName = data.Proj_ProjectName;
            var UProj_ParticipationPercentage = data.UProj_ParticipationPercentage;
            var UsrP_FirstName = data.UsrP_FirstName;
            var UsrP_LastName = data.UsrP_LastName;
            var Usr_Password = data.Usr_Password;
            var Usr_Titleid = data.Usr_Titleid;
            var Usrp_MobileNumber = data.Usrp_MobileNumber;
            var Usr_GenderId = data.Usr_GenderId;
            var Usrp_ProfilePicture = data.Usrp_ProfilePicture;
            var Usr_UserTypeID = data.Usr_UserTypeID;
            var Usr_TaskID = data.Usr_TaskID;
            var ManagerName = data.ManagerName;
            var Managername2 = data.Managername2;
            var Usr_Username = data.Usr_Username;
            var UProj_ActiveStatus = data.UProj_ActiveStatus;

            if (data.Usrp_DOJ !== null) {
                var DOJ = (data.Usrp_DOJ).substr(8);
            }

            if (data.UProj_StartDate !== null) {
                var StartDate = (data.UProj_StartDate).substr(8);
            }
            if (data.UProj_EndDate !== null) {
                var EndDate = (data.UProj_EndDate).substr(8);
            }
            var Proj_StartDate = new Date(parseInt(StartDate));

            var Proj_EndDate = new Date(parseInt(EndDate));

            var doj = new Date(parseInt(DOJ));

            $("#Usr_UserID").val(Usr_UserID);
            $("#Usr_Username").val(Usr_Username);
            $("#Email").val(Email);
            $("#Usr_Password").val(Usr_Password);
            $("#Usr_ConfirmPassword").val(Usr_Password);
            $("#Usrp_MobileNumber").val(Usrp_MobileNumber);
            $("#Usr_Titleid").val(Usr_Titleid);
            $("#Usr_GenderId").val(Usr_GenderId);
            $("#UsrP_FirstName").val(UsrP_FirstName);
            $("#UsrP_LastName").val(UsrP_LastName);
            $("#UsrP_EmployeeID").val(UsrP_EmployeeID);
            $("#Usrp_CountryCode").val(data.Usrp_CountryCode);
            $("#Usr_UserTypeID").val(Usr_UserTypeID);
            $("#RoleName").val(Usr_RoleID);
            $("#ManagerName").val(ManagerName);
            $("#Managername2").val(Managername2);
            $("#Usr_TaskID").val(Usr_TaskID);
            $("#UProj_ParticipationPercentage").val(UProj_ParticipationPercentage);
            $("#UProj_ActiveStatus").val(UProj_ActiveStatus);

            var folderName = "/uploadimages/images/" + Usrp_ProfilePicture;

            $("#profile-image").prop('src', folderName);
            // $("#Usrp_DOJ").val(doj.format("yy-mm-dd"));
            $("#Usrp_DOJ").val(doj.toLocaleDateString.format('mm/dd/yyyy'));


            $("#UProj_StartDate").val(Proj_StartDate.format('mm/dd/yyyy'));
            $("#UProj_EndDate").val(Proj_EndDate.format('mm/dd/yyyy'));



            //$("#UProj_StartDate").val(UProj_StartDate);
            //$("#UProj_EndDate").val(UProj_EndDate);

        },
        error: function () {
            alert(Response.text);
        }
    });
}

function updategrid() {

    var UProj_ActiveStatusVal = "";
    if ($("#UProj_ActiveStatus").val() === "1") {

        UProj_ActiveStatusVal = true;
    }
    else {
        UProj_ActiveStatusVal = false;
    }
    var file = document.getElementById("fileUpload").files[0];
    var enddate = $("#UProj_EndDate").val();
    var formdata = new FormData();
    formdata.append("Usr_UserID", $("#Usr_UserID").val());
    formdata.append("Usrp_ProfilePicture", file);
    formdata.append("Proj_ProjectID", Proj_ProjectID);
    formdata.append("Email", $("#Email").val());
    formdata.append("Usr_Password", $("#Usr_Password").val());
    formdata.append("Usr_Username", $("#Usr_Username").val());
    formdata.append("Usr_ConfirmPassword", $("#Usr_ConfirmPassword").val());
    formdata.append("Usrp_MobileNumber", $("#Usrp_MobileNumber").val());
    formdata.append("Usr_Titleid", $("#Usr_Titleid").val());
    formdata.append("Usr_GenderId", $("#Usr_GenderId").val());
    formdata.append("UsrP_FirstName", $("#UsrP_FirstName").val());
    formdata.append("UsrP_LastName", $("#UsrP_LastName").val());
    formdata.append("UsrP_EmployeeID", $("#UsrP_EmployeeID").val());
    formdata.append("Usrp_DOJ", $("#Usrp_DOJ").val());
    formdata.append("Usr_UserTypeID", $("#Usr_UserTypeID").val());
    formdata.append("RoleName", $("#RoleName").val());
    formdata.append("ManagerName", $("#ManagerName").val());
    formdata.append("Managername2", $("#Managername2").val());
    formdata.append("Usr_TaskID", $("#Usr_TaskID").val());
    formdata.append("UProj_ParticipationPercentage", $("#UProj_ParticipationPercentage").val());
    formdata.append("UProj_StartDate", $("#UProj_StartDate").val());
    formdata.append("UProj_EndDate", enddate);
    formdata.append("UProj_ActiveStatus", UProj_ActiveStatusVal);
    formdata.append("CL_ProjectID", $("#project").val());
    formdata.append("TimesheetMode_id", $("#TimesheetMode_id").val());
    formdata.append("Usrp_CountryCode", $("#Usrp_CountryCode").val());
   $('#Usr_Password, #Usr_ConfirmPassword').on('keyup', function () {
        if ($('#Usr_Password').val() === $('#Usr_ConfirmPassword').val()) {
            $('#message').html('Matching').css('color', 'green');
        } else
            $('#message').html('Not Matching').css('color', 'red');
    });
        $.ajax({
            type: "POST",
            url: "/Client/updateuserprojectbyid",
            data: formdata,

            dataType: "json",
            complete: function (res) {
                alert(res.responseText);
                $('#ContainerGridDetail').hide();
                //  window.location.reload();
                //$('#ContainerGridDetail').modal("toggle");
                $(".modal-backdrop").hide();
                $("body").removeClass('modal-open');
                $('#usertable').dataTable();
                usergriddata(projid);
                loadholidays(projid);
                loadtaks(projid);
                loadproject(projid);
            },
            contentType: false,
            processData: false,
            error: function (Result) {

            }

        });
    
}

function EditUser(projectid) {
    //  // 
    editprojectid = projectid;
    window.location.href = "/Client/Index?proid=" + projectid;
    //  window.location.href = "/Client/EditIndex?proid=" + projectid;
    return false;
}
function Preview(projectid) {
    //  // 
    editprojectid = projectid;
    window.location.href = "/Client/Index?proid=" + projectid;

    //  window.location.href = "/Client/EditIndex?proid=" + projectid;

    return false;
}



function GetUpdateClient(editprojectid) {


    var currentprojectid = $("#editpid").val();
    projid = editprojectid;
    $.ajax({

        type: "POST",

        url: '/Client/GetClientByID?catID=' + currentprojectid,
        dataType: "json",
        success: function (data) {

            statebindfunction(data.CountryId);
            if (perms === "Read") {
                $("#btnSingleUpdate").hide();
            } else if (perms === "Read/Write") {
                $("#btnSingleUpdate").show();
            } else {
                $("#btnSingleUpdate").hide();
            }

            $("#btnAdd").hide();
            $("#UpdatebtnEdit").show();
            $("#Proj_StartDate").val("");

            $("#Proj_EndDate").val("");

            var Proj_AccountID = data.Proj_AccountID;

            var AccountName = data.AccountName;

            var Proj_ProjectID = data.Proj_ProjectID;

           // var Proj_ProjectCode = data.Proj_ProjectCode;

            var Proj_ProjectName = data.Proj_ProjectName;

            var Proj_ProjectDescription = data.Proj_ProjectDescription;

            var CountryId = data.CountryId;

            var StateId = data.StateId;

            var WebUrl = data.WebUrl;
            var Is_Timesheet_ProjectSpecific = data.Is_Timesheet_ProjectSpecific;
            if (Is_Timesheet_ProjectSpecific === true) {
                $("#Is_Timesheet_ProjectSpecific").prop('checked', true);
            } else {
                $("#Is_Timesheet_ProjectSpecific").prop('checked', false);
            }
            if (data.Proj_StartDate !== null) {
                var StartDate = (data.Proj_StartDate).substr(6);
            }

            if (data.Proj_EndDate !== null) {
                var EndDate = (data.Proj_EndDate).substr(6);
            }




            var Proj_StartDate = new Date(parseInt(StartDate));

            var Proj_EndDate = new Date(parseInt(EndDate));


            var Proj_ActiveStatus = data.Proj_ActiveStatus;


            if (Proj_ActiveStatus === true) {
                $("#Proj_ActiveStatus").val("1");
            } else {
                $("#Proj_ActiveStatus").val("0");
            }


            $("#AccountName").val(AccountName);

            $("#Proj_AccountID").val(Proj_AccountID);

            $("#Proj_ProjectID").val(Proj_ProjectID);

           // $("#Proj_ProjectCode").val(Proj_ProjectCode);

            $("#Proj_ProjectName").val(Proj_ProjectName).attr("disabled", "disabled");

            $("#Proj_ProjectDescription").val(Proj_ProjectDescription);

            $("#Proj_StartDate").val(Proj_StartDate.format('mm/dd/yyyy'));

            $("#Proj_EndDate").val(Proj_EndDate.format('mm/dd/yyyy'));

            $("#CountryId").val(CountryId);

            $("#StateId").val(StateId);

            $("#WebUrl").val(WebUrl);

        }
    });

}

function DeleteSkill(Proj_ProjectID) {

    $.ajax({

        type: "POST",
        url: '/Project/DeleteProject?ProjectID=' + Proj_ProjectID,
        dataType: "json",

        complete: function (res) {
            alert(res.responseText);

        },

        error: function (Result) {

        }

    });

}
function Deleteuser(Usr_UserID) {

    $.ajax({

        type: "POST",
        url: '/Client/DeleteUser?userid=' + Usr_UserID,
        dataType: "json",
        //   cache: false,
        complete: function (res) {

            alert(res.responseText);
            //loaddata();
            //window.location.href = "/Project/Index";
            window.location.href = "/Client/Index?proid=" + editprojectid;

        },

        error: function (Result) {

            //alert("Error");

        }

    })

}




function loadholidays(id) {

    $.ajax({
        url: "/Client/GetHolidays?projectid=" + id,
        type: "Get",
        dataType: "json",

        success: function (res) {
              
            if (res.length===0) {
                $("#clientholidays").hide();
                $("#accountholidays").show();
            }else if (res[0].ProjectID === null) {
                $("#clientholidays").hide();
                $("#accountholidays").show();
            } else {
                $("#clientholidays").show();
                $("#accountholidays").hide();
            }

            $("#optionalholidays").val(res[0].optionalholidays);
            $("#useroptionalholidays").val(res[0].useroptionalholidays);
            $('#holidaytable').DataTable({
                'data': res,
                'paginate': true,
                'sort': true,
                'Processing': true,
                'destroy': true,
                'columns': [


                    { 'data': 'HolidayCalendarID', visible: false },
                    { 'data': 'AccountID', visible: false },
                    { 'data': 'HolidayName' },



                    {
                        'data': 'HolidayDate',
                        "type": "date ",
                        "render": function (value) {
                            if (value === null) return "";

                            var pattern = /Date\(([^)]+)\)/;
                            var results = pattern.exec(value);
                            var dt = new Date(parseFloat(results[1]));

                            return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                        }
                    },
                    { 'data': 'financialyear' },

                    {
                        'data': 'isOptionalHoliday',
                        "render": function (isOptionalHoliday, type, full, meta) {
                            if (isOptionalHoliday === true) {
                                return "<span >Yes</span>";
                            }
                            else {
                                return "<span >No</span>";
                            }
                        }
                    },

                    {
                        'data': 'isActive',
                        "render": function (isActive, type, full, meta) {
                            // if (hlpermission === "Read/Write") {
                            if (isActive === true) {

                                return '<div class="statuscheck"> <input type="checkbox" id="check_01" checked onclick="HolidayUnCheckStatus(' + full.HolidayCalendarID + ')"> <label for="check_01"></label> </div>';
                            }
                            else {
                                return '<div class="statuscheck"> <input type="checkbox" id="check_02" onclick="HolidayCheckStatus(' + full.HolidayCalendarID + ')"> <label for="check_02"></label> </div>';
                            }

                        }
                    },


                    {
                        "render": function (Usr_UserID, type, full, meta, data) {
                            if (hlpermission === "Read") {
                                return '<a class="btn btn-icn btn-icn-hide" data-toggle="modal"  data-target="#Holidaycalendergrid" id="edit" title="Edit" onclick="PreviewHoliday(' + full.HolidayCalendarID + ')" ><i class="fa fa-eye" aria-hidden="true"></i></a>';

                            }
                            else if (hlpermission === "Read/Write") {
                                return '<a class="btn btn-icn btn-icn-hide" data-toggle="modal"  data-target="#Holidaycalendergrid" id="edit" title="Edit" onclick="EditHoliday(' + full.HolidayCalendarID + ')" ><i class="fa fa-edit" aria-hidden="true"></i></a><a class="btn btn-icn btn-icn-hide"  data-target="#holidaycontainerDelete" data-toggle="modal"  onclick="GetHc(' + full.HolidayCalendarID + ')" "><i class="fa fa-trash" aria-hidden="true"></i></a>';


                            } else {
                                return '<a class="btn btn-icn btn-icn-hide" data-toggle="modal"  data-target="#Holidaycalendergrid" id="edit" title="Edit" onclick="PreviewHoliday(' + full.HolidayCalendarID + ')" ><i class="fa fa-eye" aria-hidden="true"></i></a>';

                            }

                        }

                    }




                ]
            });

        }


    });
}

function HolidayUnCheckStatus(id) {
    $.ajax({
        url: "/HolidayCalendar/ChangeStatus",
        type: "POST",
        data: {
            'id': id,
            'status': false
        },

        //dataType: "json",
        success: function (data) {

            alert(data);
            window.location.reload();
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        },
    });
}
function HolidayCheckStatus(id) {
    $.ajax({
        url: "/HolidayCalendar/ChangeStatus",
        type: "POST",
        data: {
            'id': id,
            'status': true
        },

        //dataType: "json",
        success: function (data) {

            alert(data);
            window.location.reload();
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        },
    });
}


function GetHc(holidayid) {
    delHoliday = holidayid;
}
function EditHoliday(hcid) {
    $("#HolidaybtnAdd").hide();
    $("#HolidaybtnUpdate").show();
    $("#HolidayDate").datepicker({
        
        format: 'yyyy/mm/dd',

    });

    $.ajax({
        url: "/HolidayCalendar/GetCalenderDetailByID",
        type: "POST",
        data: {
            'id': hcid
        },

        dataType: "json",
        success: function (data) {
           
            $("#HolidayCalendarID").val(data.HolidayCalendarID);
            $("#HolidayName").val(data.HolidayName);

            var holiday = data.HolidayDate;
            var exnowdate = new Date(parseInt(holiday.substr(6)));
            $("#HolidayDate").val(exnowdate.format('mm/dd/yyyy'));
            $("#HolidayDate").val();
            $("#FinancialYearId").val(data.FinancialYearId);
           // var date = data.HolidayDate;
            var status = data.isActive;

            if (status === true) {
                $("#isActive").val("True");
            } else {
                $("#isActive").val("False");
            } 
            var optional = data.isOptionalHoliday;
            if (optional === true) {
                $("#isOptionalHoliday").val("True");
            } else {
                $("#isOptionalHoliday").val("False");
            }
        }


    });
}

function PreviewHoliday(hcid) {
    $("#HolidaybtnAdd").hide();
    $("#HolidayDate").datepicker({
        format: 'yyyy/mm/dd'

    });

    $.ajax({
        url: "/HolidayCalendar/GetCalenderDetailByID",
        type: "POST",
        data: {
            'id': hcid
        },

        dataType: "json",
        success: function (data) {
            $("#HolidayCalendarID").val(data.HolidayCalendarID);
            $("#HolidayName").val(data.HolidayName);

            var holiday = data.HolidayDate;
            var exnowdate = new Date(parseInt(holiday.substr(6)));
            $("#HolidayDate").val(exnowdate.format('mm/dd/yyyy'));
            $("#HolidayDate").val();
            $("#FinancialYearId").val(data.FinancialYearId);
            var date = data.HolidayDate;
            var status = data.isActive;

            if (status === true) {
                $("#isActive").val("True");
            } else {
                $("#isActive").val("False");
            }
            var optional = data.isOptionalHoliday;
            if (optional === true) {
                $("#isOptionalHoliday").val("True");
            } else {
                $("#isOptionalHoliday").val("False");
            }



        }


    });
}

function Deletehc(hcid) {

    $('#loading-image').attr("style", "display: block;");

    $.ajax({
        url: "/HolidayCalendar/DeleteHoliday",
        type: "POST",
        data: {
            'id': hcid
        },

        success: function (data) {

            alert(data);
            $('#holidaycontainerDelete').hide();
            $(".modal-backdrop").hide();
            $("body").removeClass('modal-open');
            $('#usertable').dataTable();
            $('#holidaytable').dataTable();
            $('#tasktable').dataTable();
            $('#Projecttable').dataTable();
            if (projid === "") {
                usergriddata($("#editpid").val());
                loadholidays($("#editpid").val());
                loadproject($("#editpid").val());
                loadtaks($("#editpid").val());
            } else {
                usergriddata(projid);
                loadholidays(projid);
                loadtaks(projid);
                loadproject(projid);
            }
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        }
    });
}

function DeleteTask(protaskid) {

    $('#loading-image').attr("style", "display: block;");

    $.ajax({
        url: "/Client/DeleteProjecttask",
        type: "POST",
        data: {
            'id': protaskid
        },

        success: function (data) {

            alert(data);
            $('#taskcontainerDelete').hide();
            $(".modal-backdrop").hide();
            $("body").removeClass('modal-open');
            $('#usertable').dataTable();
            $('#holidaytable').dataTable();
            $('#tasktable').dataTable();
            $('#Projecttable').dataTable();
            if (projid === "") {
                usergriddata($("#editpid").val());
                loadholidays($("#editpid").val());
                loadtaks($("#editpid").val());
                loadproject($("#editpid").val());
            } else {
                usergriddata(projid);
                loadholidays(projid);
                loadtaks(projid);
                loadproject(projid);
            }
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        }
    });
}


function loadtaks(id) {
    $.ajax({
        url: "/Client/Getprotasks?projectid=" + id,
        type: "Get",
        dataType: "json",

        success: function (res) {

            $('#tasktable').DataTable({
                'data': res,
                'paginate': true,
                'sort': true,
                'Processing': true,
                'destroy': true,
                'columns': [


                    { 'data': 'Proj_SpecificTaskId', visible: false },
                    { 'data': 'Acc_SpecificTaskName' },
                    { 'data': 'Proj_SpecificTaskName' },
                    { 'data': 'RTMId' },

                    // { 'data': 'ProjectId', visible: false },


                    {
                        'data': 'Actual_StartDate',
                        "type": "date ",
                        "render": function (value) {
                            if (value === null) return "";

                            var pattern = /Date\(([^)]+)\)/;
                            var results = pattern.exec(value);
                            var dt = new Date(parseFloat(results[1]));

                            return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                        }
                    },
                    {
                        'data': 'Actual_EndDate',
                        "type": "date ",
                        "render": function (value) {
                            if (value === null) return "";

                            var pattern = /Date\(([^)]+)\)/;
                            var results = pattern.exec(value);
                            var dt = new Date(parseFloat(results[1]));

                            return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                        }
                    },
                    {
                        'data': 'Plan_StartDate',
                        "type": "date ",
                        "render": function (value) {
                            if (value === null) return "";

                            var pattern = /Date\(([^)]+)\)/;
                            var results = pattern.exec(value);
                            var dt = new Date(parseFloat(results[1]));

                            return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                        }
                    },

                    {
                        'data': 'Plan_EndDate',
                        "type": "date ",
                        "render": function (value) {
                            if (value === null) return "";

                            var pattern = /Date\(([^)]+)\)/;
                            var results = pattern.exec(value);
                            var dt = new Date(parseFloat(results[1]));

                            return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                        }
                    },


                    {
                        'data': 'StatusId',
                        "render": function (StatusId, type, full, meta) {
                            if (StatusId === true) {

                                return '<div class="statuscheck"> <input type="checkbox" id="check_01" checked onclick="TaskUnCheckStatus(' + full.Proj_SpecificTaskId + ')"> <label for="check_01"></label> </div>';
                            }
                            else {
                                return '<div class="statuscheck"> <input type="checkbox" id="check_02" onclick="TaskCheckStatus(' + full.Proj_SpecificTaskId + ')"> <label for="check_02"></label> </div>';
                            }
                        }
                    },


                    {
                        "render": function (Proj_SpecificTaskId, type, full, meta, data) {
                            if (taskpermission === "Read") {
                                return '<a class="btn btn-icn btn-icn-hide" data-toggle="modal"  data-target="#Projectspecifictaskgrid" id="edit" title="Edit" onclick="PreviewTasks(' + full.Proj_SpecificTaskId + ')" ><i class="fa fa-eye" aria-hidden="true"></i></a>';

                            }
                            else if (taskpermission === "Read/Write") {
                                return '<a class="btn btn-icn btn-icn-hide" data-toggle="modal"  data-target="#Projectspecifictaskgrid" id="" title="Edit" onclick="EditProjecttask(' + full.Proj_SpecificTaskId + ')" ><i class="fa fa-edit" aria-hidden="true"></i></a><a class="btn btn-icn btn-icn-hide" data-target="#taskcontainerDelete" data-toggle="modal"   onclick="GetProid(' + full.Proj_SpecificTaskId + ')" "><i class="fa fa-trash" aria-hidden="true"></i></a>';


                            } else {
                                return '<a class="btn btn-icn" data-toggle="modal"  data-target="#Projectspecifictaskgrid" id="edit" title="Edit" onclick="PreviewTasks(' + full.Proj_SpecificTaskId + ')" ><i class="fa fa-eye" aria-hidden="true"></i></a>';

                            }

                        },

                    },




                ]
            });

        }


    });
}

function TaskUnCheckStatus(id) {

    $.ajax({
        url: "/Client/ChangeStatus",
        type: "POST",
        data: {
            'id': id,
            'status': false
        },

        success: function (data) {

            alert(data);
            window.location.reload();
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        }
    });
}
function TaskCheckStatus(id) {
 
    $.ajax({
        url: "/Client/ChangeStatus",
        type: "POST",
        data: {
            'id': id,
            'status': true
        },

        //dataType: "json",
        success: function (data) {

            alert(data);
            window.location.reload();
        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        },
    });
}



function GetProid(protaskid) {
    delprotaskid = protaskid;
}

function addtasks() {

    var Acc_SpecificTaskName = $("#Acc_SpecificTaskName").val();
    var Proj_SpecificTaskName = $("#Proj_SpecificTaskName").val();
    // Proj_ProjectName = $("#Proj_ProjectName").val();
    var RTMId = $("#RTMId").val();
    var Actual_StartDate = $("#Actual_StartDate").val();
    var Actual_EndDate = $("#Actual_EndDate").val();
    var Plan_StartDate = $("#Plan_StartDate").val();
    var Plan_EndDate = $("#Plan_EndDate").val();
    var StatusId = $("#StatusId").val();
    var projectid = projid;
    var Proj_ActiveStatusVal;
   
    if ($("#StatusId").val() === "1") {

         Proj_ActiveStatusVal = true;
    }
    else {
        Proj_ActiveStatusVal = false;
    }
    $.ajax({
        type: 'POST',
        //contentType: "application/json; charset=utf-8",
        url: "/Client/Addprotasks",
        data: {

            "Acc_SpecificTaskName": Acc_SpecificTaskName, "ProjectID": projectid, "Proj_SpecificTaskName": Proj_SpecificTaskName,
            "RTMId": RTMId, "Actual_StartDate": Actual_StartDate, "Actual_EndDate": Actual_EndDate, "Plan_StartDate": Plan_StartDate,
            "Plan_EndDate": Plan_EndDate, "StatusId": Proj_ActiveStatusVal
        },
        //dataType: "json",

        success: function (data) {

            alert(data);
            // window.location.reload();
            $('#Projectspecifictaskgrid').hide();
            $(".modal-backdrop").hide();
            $("body").removeClass('modal-open');
            $('#usertable').dataTable();
            $('#holidaytable').dataTable();
            $('#tasktable').dataTable();
            $('#Projecttable').dataTable();

            if (projid === "") {
                usergriddata($("#editpid").val());
                loadholidays($("#editpid").val());
                loadtaks($("#editpid").val());
                loadproject($("#editpid").val());
            } else {
                usergriddata(projid);
                loadholidays(projid);
                loadtaks(projid);
                loadproject(projid);
            }

        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        },
        error: function (error) {
            console.log(error);
        }


    });

}

function EditProjecttask(hcid) {
    $("#Addprojecttask").hide();
    $("#Updatetask").show();

    $.ajax({
        url: "/Client/getprojecttaskbyid?id=" + hcid,
        type: "Get",

        dataType: "json",
        success: function (data) {

            $("#Acc_SpecificTaskName").val(data[0].Acc_SpecificTaskId);
            $("#Proj_SpecificTaskName").val(data[0].Proj_SpecificTaskName);
            $("#RTMId").val(data[0].RTMId);
            $("#Proj_SpecificTaskName").val(data[0].Proj_SpecificTaskName);

            var actualstartdate = data[0].Actual_StartDate;
            var actualenddate = data[0].Actual_EndDate;
            var planstartdate = data[0].Plan_StartDate;
            var planenddate = data[0].Plan_EndDate;
            actualstartdate = new Date(parseInt(actualstartdate.substr(6)));
            actualenddate = new Date(parseInt(actualenddate.substr(6)));
            planstartdate = new Date(parseInt(planstartdate.substr(6)));
            planenddate = new Date(parseInt(planenddate.substr(6)));
            $("#Actual_StartDate").val(actualstartdate.format('mm/dd/yyyy'));
            $("#Actual_EndDate").val(actualenddate.format('mm/dd/yyyy'));
            $("#Plan_StartDate").val(planstartdate.format('mm/dd/yyyy'));
            $("#Plan_EndDate").val(planenddate.format('mm/dd/yyyy'));

            // projid = data[0].ProjectId;
            projectspecifictaskid = data[0].Proj_SpecificTaskId;
            var status = data[0].StatusId;

            if (status === true) {
                $("#StatusId").val(1);
            } else {
                $("#StatusId").val(0);
            }





        },


    });
}

function PreviewTasks(id) {
    EditProjecttask(id);
    $("#Addprojecttask").hide();
    $("#Updatetask").hide();
}


function updatetasks() {
    var Acc_SpecificTaskName = $("#Acc_SpecificTaskName").val();
    var Proj_SpecificTaskName = $("#Proj_SpecificTaskName").val();
    // Proj_ProjectName = $("#Proj_ProjectName").val();
    var RTMId = $("#RTMId").val();
    var Actual_StartDate = $("#Actual_StartDate").val();
    var Actual_EndDate = $("#Actual_EndDate").val();
    var Plan_StartDate = $("#Plan_StartDate").val();
    var Plan_EndDate = $("#Plan_EndDate").val();
    var StatusId = $("#StatusId").val();




    $.ajax({
        type: 'POST',
        //contentType: "application/json; charset=utf-8",
        url: "/Client/updatetasks?id=" + projectspecifictaskid,
        data: {

            "Acc_SpecificTaskName": Acc_SpecificTaskName, "ProjectID": projid, "Proj_SpecificTaskName": Proj_SpecificTaskName,
            "RTMId": RTMId, "Actual_StartDate": Actual_StartDate, "Actual_EndDate": Actual_EndDate, "Plan_StartDate": Plan_StartDate,
            "Plan_EndDate": Plan_EndDate, "StatusId": StatusId
        },
        //dataType: "json",

        success: function (data) {
            // 
            alert(data);
            // window.location.reload();
            $('#Projectspecifictaskgrid').hide();
            $(".modal-backdrop").hide();
            $('#usertable').dataTable();
            $('#holidaytable').dataTable();
            $('#tasktable').dataTable();
            $('#Projecttable').dataTable();

            if (projid === "") {
                usergriddata($("#editpid").val());
                loadholidays($("#editpid").val());
                loadtaks($("#editpid").val());
                loadproject($("#editpid").val());
            } else {
                usergriddata(projid);
                loadholidays(projid);
                loadtaks(projid);
                loadproject(projid);
            }

        },
        complete: function () {
            $('#loading-image').attr("style", "display: none;");
        },
        error: function (error) {
            console.log(error);
        }


    });

}


function getmanagerlist() {
    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Client/Associatemanagers?projectid=" + projid,

        dataType: "json",
        success: function (Result) {

            $("#managerslist").empty();
            $("#managerslist").append($("<option></option>").val("").html("Select Manager"));
            $(Result).each(function () {

                //// $("#ManagerName").append($("<option></option>").val(this.Usr_UserID).html(this.UsrP_FirstName));
                $("#managerslist").append($("<option></option>").val(this.Usr_UserID).html(this.Usr_Username));


            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });
}

function getemployeelist() {
    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Client/AssociateEmployees?projectid=" + projid,

        dataType: "json",
        success: function (Result) {

            $("#employeeslist").empty();
            $("#employeeslist").append($("<option></option>").val(0).html("Select Employee"));
            $(Result).each(function () {

                //// $("#ManagerName").append($("<option></option>").val(this.Usr_UserID).html(this.UsrP_FirstName));
                $("#employeeslist").append($("<option></option>").val(this.Usr_UserID).html(this.Usr_Username));


            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });
}
var Employeeid;
function Selectmanager(value) {


    var projectid = projid;
    $("#ContainerGridDetail").show();
    $("#manbtnassociate").show();
    $("#empbtnassociate").hide();
    Associateuser(value);

}

function SelectEmployee(empvalue) {

    var projectid = projid;
    $("#ContainerGridDetail").show();
    $("#manbtnassociate").hide();
    $("#empbtnassociate").show();
    Associateuser(empvalue);
    //$("#btnUpdateModel").hide();
    //$("#btnassociateemp").show();
}

function Associateuser(userid) {
    var roledata = $("#RoleName").val();
    $("#ManagerName").empty();
    $("#Managername2").empty();
    bindProjectNames();
    bindAllRoleNames();
    GetAlltasknames(roledata);
    bindUserNames();
    bindUserTypes();
    GetManagerByRole(userid);
    $("#btnManAddModel").hide();
    $("#btnEmpAddModel").hide();
    $("#btnUpdateModel").hide();
    Employeeid = userid;

    $.ajax({

        url: '/Client/GetassUserDetailById?id=' + userid,
        type: 'Get',

        success: function (data) {

            Employeeid = data.Usr_UserID;
            var Email = data.Email;
            var Usr_RoleID = data.Usr_RoleID;
            var UsrP_EmployeeID = data.UsrP_EmployeeID;
            Proj_ProjectID = data.Proj_ProjectID;
            var Proj_ProjectName = data.Proj_ProjectName;
            var UProj_ParticipationPercentage = data.UProj_ParticipationPercentage;
            var UsrP_FirstName = data.UsrP_FirstName;
            var UsrP_LastName = data.UsrP_LastName;
            var Usr_Password = data.Usr_Password;
            var Usr_Titleid = data.Usr_Titleid;
            var Usrp_MobileNumber = data.Usrp_MobileNumber;
            var Usr_GenderId = data.Usr_GenderId;
            var Usrp_ProfilePicture = data.Usrp_ProfilePicture;
            var Usr_UserTypeID = data.Usr_UserTypeID;
            var Usr_TaskID = data.Usr_TaskID;
            var ManagerName = data.ManagerName;
            var Managername2 = data.Managername2;
            var Usr_Username = data.Usr_Username;

            var UProj_ActiveStatus = data.UProj_ActiveStatus;
            if (UProj_ActiveStatus === true) {
                $("#UProj_ActiveStatus").val("1");
            } else {
                $("#UProj_ActiveStatus").val("0");
            }
            if (data.Usrp_DOJ !== null) {
                var DOJ = (data.Usrp_DOJ).substr(8);
            }

            if (data.UProj_StartDate !== null) {
                var StartDate = (data.UProj_StartDate).substr(8);
            }
            if (data.UProj_EndDate !== null) {
                var EndDate = (data.UProj_EndDate).substr(8);
            }
            var Proj_StartDate = new Date(parseInt(StartDate));

            var Proj_EndDate = new Date(parseInt(EndDate));

            var doj = new Date(parseInt(DOJ));

            $("#Usr_UserID").val(Usr_UserID);
            $("#Usr_Username").val(Usr_Username);
            $("#Email").val(Email);
            $("#Usr_Password").val();
            $("#Usr_ConfirmPassword").val();
            $("#Usrp_MobileNumber").val(Usrp_MobileNumber);
            $("#Usr_Titleid").val(Usr_Titleid);
            $("#Usr_GenderId").val(Usr_GenderId);
            $("#UsrP_FirstName").val(UsrP_FirstName);
            $("#UsrP_LastName").val(UsrP_LastName);
            $("#UsrP_EmployeeID").val(UsrP_EmployeeID);
            $("#Usr_UserTypeID").val(Usr_UserTypeID);
            $("#RoleName").val(Usr_RoleID);
            $("#ManagerName").val(ManagerName);
            $("#Managername2").val(Managername2);
            $("#Usr_TaskID").val(Usr_TaskID);
            $("#UProj_ParticipationPercentage").val(UProj_ParticipationPercentage);

            var folderName = "/uploadimages/images/" + Usrp_ProfilePicture;

            $("#profile-image").prop('src', folderName);
            $("#Usrp_DOJ").val(doj.format('mm/dd/yyyy'));


            $("#UProj_StartDate").val(Proj_StartDate.format('mm/dd/yyyy'));
            $("#UProj_EndDate").val(Proj_EndDate.format('mm/dd/yyyy'));

        },
        error: function () {
            alert(Response.text);
        }
    });
}

