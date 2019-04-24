var delid;
$(document).ready(function () {
    var AccountId;
    loadUsers();
    change();

    $("#btnAddModel").click(function () {

        // bindRoleNames();

        $("#edit").hide();
        $("#add").show();
        $("#AccountName").val(0).removeAttr('disabled');

        $("#Usr_Username").val('');
        $("#Usr_LoginId").val('');
        $("#Usr_Password").val('');
        $("#profile-image").prop('src', 'empty');
       // $("#UsrP_EmployeeID").empty();
        $("#Usr_ActiveStatus").val(1);
        $("#UsrP_FirstName").val('');
        $("#UsrP_LastName").val('');
        $("#Usrp_DOJ").val('');
        $("#Usr_UserTypeID").val('');
        $("#RoleName").val('');
        $("#Managername2").val('');
        $("#ManagerName").empty();
        $("#Usr_TaskID").val('');
        // bindAccountNames();
        ShowAddEmployeeModalPopUp();
        //change();
        //  bindUserTypes();
        bindManger2();
        bindManger1();
    });

    $("#profile-image").click(function () {

        $("#fileUpload").trigger('click');


    });

   $("#btnAdd").on("click", function () {
       $('#myform').validate({
           rules: {
               AccountName: {
                   required: true
               },
               Usr_UserTypeID: {
                   required: true
               },
               RoleName: {
                   required: true
               },
               Usr_Titleid: {
                   required: {
                       depends: function () {
                           $(this).val($.trim($(this).val()));
                           return true;
                       }
                   }
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

               //UsrP_EmployeeID: {
               //    required: {
               //        depends: function () {
               //            $(this).val($.trim($(this).val()));
               //            return true;
               //        }
               //    },
               //    regex:/^[a-zA-Z0-9_.-]*$/

               //},
               Usr_Username: {
                   required: {
                       depends: function () {
                           $(this).val($.trim($(this).val()));
                           return true;
                       }
                   },
                   regx: /^[a-zA-Z]+$/
               },
               Usr_LoginId: {
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
                   reg:/^([a-zA-Z0-9@*#]{8,15})$/
               },
               cnf_Password: {
                   required: true,
                   reg: /^([a-zA-Z0-9@*#]{8,15})$/,
                   equalTo: "#Usr_Password"
               },
               Usrp_DOJ: {
                   required: true,
                   date: true,
               }
           },
           submitHandler: function (form) {
               AddUser();
               return false;
           }
       });
       $.validator.addMethod("regx", function (value, element, regexpr) {
           return regexpr.test(value);
       }, "Please enter  Only Alphabets .");  
       $.validator.addMethod("regex", function (value, element, regexpr) {
           return regexpr.test(value);
       }, "Please enter Only Alphabets and Numbers.");  
       $.validator.addMethod("reg", function (value, element, regexpr) {
           return regexpr.test(value);
       }, "Please enter valid Password");  
      

       });
   
    $("#btnUpdate").on("click", function () {
        $('#myform').validate({
            rules: {
                AccountName: {
                    required: true
                },
                Usr_UserTypeID: {
                    required: true
                },
                RoleName: {
                    required: true
                },
                Usr_Titleid: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    }
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

                //UsrP_EmployeeID: {
                //    required: {
                //        depends: function () {
                //            $(this).val($.trim($(this).val()));
                //            return true;
                //        }
                //    },
                //    regex:/^[a-zA-Z0-9_.-]*$/

                //},
                Usr_Username: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    },
                    regx: /^[a-zA-Z]+$/
                },
                Usr_LoginId: {
                    required: {
                        depends: function () {
                            $(this).val($.trim($(this).val()));
                            return true;
                        }
                    },
                    email: true
                },
                //Usr_Password: {
                //    required: true,
                //    reg: /^([a-zA-Z0-9@*#]{8,15})$/
                //},
                //cnf_Password: {
                //    required: true,
                //    reg: /^([a-zA-Z0-9@*#]{8,15})$/,
                //    equalTo: "#Usr_Password"
                //},
                Usrp_DOJ: {
                    required: true,
                    date: true,
                }
            },
            submitHandler: function (form) {
                btnUpdate();
                return false;
            }
        });
        $.validator.addMethod("regx", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter  Only Alphabets .");
        $.validator.addMethod("regex", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter Only Alphabets and Numbers.");
        $.validator.addMethod("reg", function (value, element, regexpr) {
            return regexpr.test(value);
        }, "Please enter valid Password");


       

    });

    $("#Usrp_DOJ").datepicker({
        dateFormat: 'mm/dd/yyyy'
    });

    $("#user_management").addClass("active");
    $("#user_management > ul.treeview-menu > li:last-child").addClass("click");
    $("#user_management").siblings().removeClass("active");

});


function loadUsers() {

    $.ajax({
        url: "/User/GetUserCollection?acntID=" + acID + '&RoleId=' + RoleId,
        type: "Get",
        dataType: "json",
        success: function (res) {

            $('#table').DataTable({
                'data': res,
                'paginate': true,
                'sort': true,
                'Processing': true,
                'columns': [

                    { 'data': 'AccountName' },
                    { 'data': 'Usr_AccountID', 'visible': false },
                    { 'data': 'UserType' },
                    { 'data': 'RoleName' },
                    { 'data': 'Taskname' },
                    { 'data': 'ManagerName', 'visible': false },
                    { 'data': 'Managername2', 'visible': false },
                    { 'data': 'Usr_Username' },
                    { 'data': 'Usr_LoginId' },
                    { 'data': 'Usr_Password', 'visible': false },
                    //{
                    //    'data': 'Usr_ActiveStatus',
                    //    //"render": function (Usr_ActiveStatus, type, full, meta) {
                    //    //    if (Usr_ActiveStatus === true) {
                    //    //        return '<div class="statuscheck"> <input type="checkbox" id="check_01" checked onclick="UnCheckStatus(' + full.Usr_UserID + ')"> <label for="check_01"></label> </div>';
                    //    //    }
                    //    //    else {
                    //    //        return '<div class="statuscheck"> <input type="checkbox" id="check_02" onclick="CheckStatus(' + full.Usr_UserID + ')"> <label for="check_02"></label> </div>';
                    //    //    }
                    //    //}
                    //},

                    {
                        "render": function (Usr_UserID, type, full, meta, data) {

                            // var permissions = '@ViewBag.a';
                            if (permissions === "Read/Write") {
                                return '<a class="btn btn-icn btn-icn-hide  edit" data-toggle="modal"  data-target="#ContainerGridDetail" id="edit" title="Edit"  onclick="EditUser(' + full.Usr_UserID + ')" ><i class="fa fa-edit"></i></a><a class="btn btn-icn btn-icn-hide" data-target="#containerDelete" data-toggle="modal" title="Delete" data-id="' + full.Usr_UserID + '"  onclick="GetId(' + full.Usr_UserID + ')" "><i class="fa fa-trash"></i></a>';

                            }
                            else if (permissions === "Read") {
                                return '<a class="btn btn-icn btn-icn-hide  edit" data-toggle="modal"  data-target="#ContainerGridDetail" id="edit" title="Edit"  onclick="Preview(' + full.Usr_UserID + ')" ><i class="fa fa-eye"></i></a>';

                            }
                            else {
                                return '<a class="btn btn-icn" data-toggle="modal"  data-target="#ContainerGridDetail" id="edit" title="Edit"  onclick="EditUser(' + full.Usr_UserID + ')" ><i class="fa fa-edit"></i></a><a class="btn btn-icn" data-target="#containerDelete" data-toggle="modal" title="Delete" data-id="' + full.Usr_UserID + '"  onclick="DeleteSkill(' + full.Usr_UserID + ')" "><i class="fa fa-trash"></i></a>';
                            }
                        }

                    },
                    //{
                    //    data: null, render: function (data, type, row) {

                    //        return '<a class="btn btn-icn" data-target="#containerDelete" data-toggle="modal" title="Delete" data-id="' + row.Usr_UserID + '"  onclick="DeleteSkill(' + row.Usr_UserID + ')" "><i class="fa fa-trash"></i></a>';

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

function UnCheckStatus(id) {
    $.ajax({
        url: "/User/ChangeStatus",
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
        url: "/User/ChangeStatus",
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


function GetId(id) {
    delid = id;
}

function ShowAddEmployeeModalPopUp() {

    $("#btnAdd").show();

    $("#btnUpdate").hide();
    // bindAccountNames();

}

function bindAccountNames() {

    $("#AccountName").empty();
   // alert(acID);
    $.ajax({
        type: "GET",
        // contentType: "application/json; charset=utf-8",
        // url: "/User/GetUserAccounts?acntID=" + acID + '&userid=' + userId,
        url: "/User/GetUserAccounts1?acntID=" + acID + '&RoleId=' + RoleId,

        dataType: "json",
        async: false,
        success: function (Result) {

            $(Result).each(function () {

                $("#AccountName").append($("<option></option>").val(this.Usr_AccountID).html(this.AccountName));


            });
        },
        error: function (Result) {


        }

    });
}

function bindUserTypesbyEditclick() {

    $("#Usr_UserTypeID").empty();

    $.ajax({

        type: "GET",
        // contentType: "application/json; charset=utf-8",
        url: "/User/getUserTypes",
        dataType: "json",
        async: false,
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

function bindUserTypes() {
    $("#Usr_UserTypeID").empty();

    $.ajax({

        type: "GET",
        // contentType: "application/json; charset=utf-8",
        url: "/User/getUserTypes?AccountId=" + AccountId,
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

function bindRoleNames(AccountId) {
      
    $("#RoleName").empty();
    //var acID = $("#AccountName").val();
   //   
    $.ajax({

        type: "GET",
        // contentType: "application/json; charset=utf-8",
        url: "/User/GetUserRoles?AccountId=" + AccountId + '&RoleId=' + RoleId,
        dataType: "json",
        async: false,
        success: function (Result) {

            $(Result).each(function () {

                $("#RoleName").append($("<option></option>").val(this.Usr_RoleID).html(this.RoleName));


            });

        },
        error: function (Result) {

            // alert("Error");

        }

    });
}
function EditbindRoleNames(acID) {
      
    $("#RoleName").empty();
   
    $.ajax({

        type: "GET",
        // contentType: "application/json; charset=utf-8",
        url: "/User/GetUserRoles?AccountId=" + acID + '&RoleId=' + RoleId,
        dataType: "json",
        async: false,
        success: function (Result) {

            $(Result).each(function () {

                $("#RoleName").append($("<option></option>").val(this.Usr_RoleID).html(this.RoleName));


            });

        },
        error: function (Result) {

            // alert("Error");

        }

    });
}

function bindTaskNames() {

    $("#Usr_TaskID").empty();
    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/User/GetTaskNames?AccountId=" + AccountId,
        //data: "{}",
        dataType: "json",
        async: false,
        success: function (Result) {

            $(Result).each(function () {

                $("#Usr_TaskID").append($("<option></option>").val(this.Usr_TaskID).html(this.Task_Name));


            });

        },

        error: function (Result) {

            //alert("Error");

        }

    });
}

function bindManger2() {

    $("#Managername2").empty();
    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/User/GetL2ManagerNames?AccountId=" + acID,
        //data: "{}",
        dataType: "json",
        async: false,
        success: function (Result) {
            $("#Managername2").append($("<option></option>").val("").html("Select Manager"));
            $(Result).each(function () {

                $("#Managername2").append($("<option></option>").val(this.Usr_UserID).html(this.Usr_Username));
                //  $("#Managername").append($("<option></option>").val(this.Usr_UserID).html(this.Usr_Username));

            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });
}

function bindManger1() {

    $("#Managername").empty();
    $.ajax({

        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/User/GetL1ManagerNames?AccountId=" + acID,
        //data: "{}",
        dataType: "json",
        async: false,
        success: function (Result) {
            $("#ManagerName").append($("<option></option>").val("").html("Select Manager"));
            $(Result).each(function () {

                $("#ManagerName").append($("<option></option>").val(this.Usr_UserID).html(this.Usr_Username));


            });

        },

        error: function (Result) {

            // alert("Error");

        }

    });
}
function Preview(Usr_UserID) {
    $("#edit").show();
    $("#add").hide();
    $("#Usr_UserTypeID").empty();
    $("#RoleName").empty();
    $("#Usr_TaskID").empty();
    $("#Managername2").empty();
    $("#Managername").empty();
    AccountId = $("#AccountName").val();

    $("#btnAdd").hide();
    $("#btnUpdate").hide();

    $.ajax({

        url: '/User/GetUserByID?catID=' + Usr_UserID,
        type: 'Get',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
              

            AccountId = data.Usr_AccountID;
            EditbindRoleNames(AccountId);
            userid = Usr_UserID;

            var AccountName = data.AccountName;
            var Usr_UserTypeID = data.Usr_UserTypeID;
            bindUserTypes();
            var UserType = data.UserType;
            var Usr_RoleID = data.Usr_RoleID;
            var RoleName = data.RoleName;
            var Taskname = data.Task_Name;
            var ManagerName = data.Usr_Manager;
            var Managername2 = data.Usr_Manager2;
            var Usr_Username = data.Usr_Username;
            var Usr_LoginId = data.Usr_LoginId;
            var Usr_Password = data.Usr_Password;
            var Usr_ActiveStatus = data.Usr_ActiveStatus;
            var Usrp_ProfilePicture = data.Usrp_ProfilePicture;
            var UsrP_FirstName = data.UsrP_FirstName;
            var UsrP_LastName = data.UsrP_LastName;
            var Usrp_DOJ = data.Usrp_DOJ;
            var Usr_Titleid = data.Usr_Titleid;

            if (data.Usrp_DOJ !== null) {

                var dojDate = new Date(parseInt(Usrp_DOJ.substr(6)));
                $("#Usrp_DOJ").val(dojDate.format('mm/dd/yyyy'));

            }






            if (Usr_ActiveStatus === true) {
                $("#Usr_ActiveStatus").val("1");
            } else {
                $("#Usr_ActiveStatus").val("0");
            }

            $("#AccountName").val(AccountId).attr('disabled', 'disabled');

            // $("#AccountName").val(AccountId).change()/*.attr("disabled", "disabled")*/;
            //$("#AccountName").val(AccountId).trigger()/*.attr("disabled", "disabled")*/;

            //  $("#Usr_UserTypeID").val(Usr_UserTypeID).attr("disabled", "disabled");
            //console.log(Usr_UserTypeID);
            $("#Usr_UserTypeID").val(Usr_UserTypeID);

            // $("#Usr_RoleID").val(Usr_RoleID).attr("disabled", "disabled");

            $("#RoleName").val(Usr_RoleID)/*.attr("disabled", "disabled")*/;

            $("#Usr_UserID").val(userid);

            $("#Usr_TaskID").val(Taskname)/*.attr("disabled", "disabled")*/;
            $("#Usr_Titleid").val(Usr_Titleid);
            $("#ManagerName").val(ManagerName)/*.attr("disabled", "disabled")*/;

            $("#Managername2").val(Managername2)/*.attr("disabled", "disabled")*/;
            $("#Usr_Username").val(Usr_Username);
            $("#Usr_LoginId").val(Usr_LoginId);
            $("#Usr_Password").val(Usr_Password);
           // $("#UsrP_EmployeeID").val(data.UsrP_EmployeeID);

            //  $("#Usr_Password").prop('disabled', 'true');
            var folderName = "/uploadimages/images/" + Usrp_ProfilePicture;

            $("#profile-image").prop('src', folderName);

            $("#UsrP_FirstName").val(UsrP_FirstName)/*.attr("disabled", "disabled")*/;
            $("#UsrP_LastName").val(UsrP_LastName)/*.attr("disabled", "disabled")*/;
            //  $("#Usrp_DOJ").val(Usrp_DOJ)/*.attr("disabled", "disabled")*/;

        },
        error: function () {

        }
    });

}


function EditUser(Usr_UserID) {
    $("#edit").show();
    $("#add").hide();
    $("#Usr_UserTypeID").empty();
    $("#RoleName").empty();
    $("#Usr_TaskID").empty();
    $("#Managername2").empty();
    $("#Managername").empty();
    AccountId = $("#AccountName").val();

    $("#btnAdd").hide();
    $("#btnUpdate").show();

    $.ajax({

        url: '/User/GetUserByID?catID=' + Usr_UserID,
        type: 'Get',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {

            debugger;
            AccountId = data.Usr_AccountID;
            EditbindRoleNames(AccountId);
            userid = Usr_UserID;

            var AccountName = data.AccountName;
            var Usr_UserTypeID = data.Usr_UserTypeID;
            bindUserTypes();
            var UserType = data.UserType;
            var Usr_RoleID = data.Usr_RoleID;
            var RoleName = data.RoleName;
            var Taskname = data.Task_Name;
            var ManagerName = data.Usr_Manager;
            var Managername2 = data.Usr_Manager2;
            var Usr_Username = data.Usr_Username;
            var Usr_LoginId = data.Usr_LoginId;
            var Usr_Password = data.Usr_Password;
            var Usr_ActiveStatus = data.Usr_ActiveStatus;
            var Usrp_ProfilePicture = data.Usrp_ProfilePicture;
            var UsrP_FirstName = data.UsrP_FirstName;
            var UsrP_LastName = data.UsrP_LastName;
            var Usrp_DOJ = data.Usrp_DOJ;
            var Usr_Titleid = data.Usr_Titleid;

            if (data.Usrp_DOJ !== null) {

                var dojDate = new Date(parseInt(Usrp_DOJ.substr(6)));
                $("#Usrp_DOJ").val(dojDate.format('mm/dd/yyyy'));

            }
            
            




            if (Usr_ActiveStatus === true) {
                $("#Usr_ActiveStatus").val("1");
            } else {
                $("#Usr_ActiveStatus").val("0");
            }

            $("#AccountName").val(AccountId).attr('disabled', 'disabled');

          
            $("#Usr_UserTypeID").val(Usr_UserTypeID);

            // $("#Usr_RoleID").val(Usr_RoleID).attr("disabled", "disabled");

            $("#RoleName").val(Usr_RoleID)/*.attr("disabled", "disabled")*/;

            $("#Usr_UserID").val(userid);

            $("#Usr_TaskID").val(Taskname)/*.attr("disabled", "disabled")*/;
            $("#Usr_Titleid").val(Usr_Titleid);
            $("#ManagerName").val(ManagerName)/*.attr("disabled", "disabled")*/;

            $("#Managername2").val(Managername2)/*.attr("disabled", "disabled")*/;
            $("#Usr_Username").val(Usr_Username);
            $("#Usr_LoginId").val(Usr_LoginId);
           $("#Usr_Password").val();
            //$("#UsrP_EmployeeID").val(data.UsrP_EmployeeID);

            //  $("#Usr_Password").prop('disabled', 'true');
            var folderName = "/uploadimages/images/" + Usrp_ProfilePicture;

            $("#profile-image").prop('src', folderName);

            $("#UsrP_FirstName").val(UsrP_FirstName)/*.attr("disabled", "disabled")*/;
            $("#UsrP_LastName").val(UsrP_LastName)/*.attr("disabled", "disabled")*/;
            //  $("#Usrp_DOJ").val(Usrp_DOJ)/*.attr("disabled", "disabled")*/;

        },
        error: function () {

        }
    });


}

function btnUpdate() {

    if ($("#Usr_ActiveStatus").val() === "1") {
        var Usr_ActiveStatusVal = true;
    }
    else {
        Usr_ActiveStatusVal = false;
    }

    var file = document.getElementById("fileUpload").files[0];


    var formData = new FormData();

    formData.append("Usr_UserTypeID", $('#Usr_UserTypeID').val());


    formData.append("Usrp_ProfilePicture", file);

    formData.append("Usr_RoleID", $('#RoleName').val());

    formData.append("Usr_UserID", userid);
    formData.append("Usr_Titleid", $("#Usr_Titleid").val());
    formData.append("Usr_AccountID", $('#AccountName').val());

    formData.append("Usr_TaskID", $('#Usr_TaskID').val());

    formData.append("Usr_Manager", $('#ManagerName').val());

    formData.append("Usr_Manager2", $('#Managername2').val());

    formData.append("Usr_Username", $('#Usr_Username').val());

    formData.append("Usr_LoginId", $('#Usr_LoginId').val());

    formData.append("UsrP_FirstName", $('#UsrP_FirstName').val());

    formData.append("UsrP_LastName", $('#UsrP_LastName').val());

    formData.append("Usr_Password", $('#Usr_Password').val());

    formData.append("Usr_ActiveStatus", Usr_ActiveStatusVal);

    formData.append("Usrp_DOJ", $('#Usrp_DOJ').val());
    //formData.append("UsrP_EmployeeID", $('#UsrP_EmployeeID').val());

    $.ajax({

        // type: "POST",

        url: "/User/UpdateUser",
        cache: false,
        type: "POST",
        contentType: false,
        processData: false,
        data: formData,
        complete: function (res) {

            alert(res.responseText);
            window.location.href = "/User/Index";

        },

        error: function (Result) {

            //alert("Error");

        }

    });


}

function DeleteSkill(Usr_UserID) {

    $.ajax({

        type: "POST",
        url: '/User/DeleteUser?UserID=' + Usr_UserID,
        dataType: "json",
        //   cache: false,
        complete: function (res) {

            alert("Record Deleted");
            //loaddata();
            window.location.href = "/User/Index";

        },

        error: function (Result) {

            //alert("Error");

        }

    })

}



function change() {

    $("#AccountName").change(function (e) {
          
        $("#Usr_UserTypeID").empty();
        $("#RoleName").empty();
        $("#Usr_TaskID").empty();
        $("#Managername2").empty();
        $("#Managername").empty();

        AccountId = $("#AccountName").val();

        bindTaskNames();
        bindManger2();
        bindManger1();
        bindRoleNames(AccountId);
        bindUserTypes();
    });

}

function AddUser() {

    if ($("#Usr_ActiveStatus").val() === "1") {

        var Usr_ActiveStatusVal = true;
    }
    else {
        Usr_ActiveStatusVal = false;
    }
    var file = document.getElementById("fileUpload").files[0];
    var accountid = $("#AccountName").val();
    var formData = new FormData();

    formData.append("Usr_UserTypeID", $('#Usr_UserTypeID').val());


    formData.append("Usrp_ProfilePicture", file);

    formData.append("Usr_RoleID", $('#RoleName').val());

    formData.append("Usr_UserID", userId);

    formData.append("Usr_AccountID", accountid);
    formData.append("Usr_Titleid", $("#Usr_Titleid").val());

    formData.append("Usr_TaskID", $('#Usr_TaskID').val());

    formData.append("Usr_Manager", $('#ManagerName').val());

    formData.append("Usr_Manager2", $('#Managername2').val());

    formData.append("Usr_Username", $('#Usr_Username').val());

    formData.append("Usr_LoginId", $('#Usr_LoginId').val());

    formData.append("UsrP_FirstName", $('#UsrP_FirstName').val());

    formData.append("UsrP_LastName", $('#UsrP_LastName').val());

    formData.append("Usr_Password", $('#Usr_Password').val());

    formData.append("Usr_ActiveStatus", Usr_ActiveStatusVal);

    formData.append("Usrp_DOJ", $('#Usrp_DOJ').val());
    //formData.append("UsrP_EmployeeID", $('#UsrP_EmployeeID').val());


    $.ajax({

        type: "POST",

        url: "/User/CreateUser",
        data: formData,

        dataType: "json",
        // cache: false,
        complete: function (res) {
            //var result = JSON.parse(res);
            if (res.responseText === "UserName already Exists") {
                $("#Usr_Username").addClass("validate_msg");
                $("#Usr_LoginId").removeClass("validate_msg");

            } else if (res.responseText === "Loginid already Exists") {
                $("#Usr_Username").removeClass("validate_msg");
                $("#Usr_LoginId").addClass("validate_msg");
            }
            else {
                $("#Usr_Username").removeClass("validate_msg");
                $("#Usr_LoginId").removeClass("validate_msg");
                alert(res.responseText);
                window.location.href = "/User/Index";
            }


        },
        contentType: false,
        processData: false,
        error: function (Result) {

            //alert("Error");

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


var dateControler = {
    currentDate: null
};
$(document).on("change", "#Usrp_DOJ", function (event, ui) {
    var now = new Date();
    var selectedDate = new Date($(this).val());

    if (selectedDate > now) {
        $(this).val(dateControler.currentDate)
    } else {
        dateControler.currentDate = null;
    }
});  