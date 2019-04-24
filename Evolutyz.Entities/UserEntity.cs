using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public class UserEntity : ResponseHeader
    {
        [Key]
        public int? RoleId { get; set; }
        public int? id { get; set; } 
        public int Usr_UserID { get; set; }
        public int Usr_AccountID { get; set; }
        public string AccountName { get; set; }
        public int Usr_UserTypeID { get; set; }
        public string UserType { get; set; }
        public int Usr_RoleID { get; set; }
        public string RoleName { get; set; }
        public Nullable<int> Usr_TaskID { get; set; }
        public string Taskname { get; set; }
        public string Task_Name { get; set; }
        public int? Usr_Manager { get; set; }
        public string ManagerName { get; set; }
        public int? Usr_Manager2 { get; set; }
        public string Managername2 { get; set; }
        public string Usr_Username { get; set; }
        public string UserName { get; set; }
        public string Usr_LoginId { get; set; }
        public string Usr_Password { get; set; }
        public bool Usr_ActiveStatus { get; set; }
        public short Usr_Version { get; set; }
        public System.DateTime Usr_CreatedDate { get; set; }
        public int Usr_CreatedBy { get; set; }
        public Nullable<System.DateTime> Usr_ModifiedDate { get; set; }
        public int Usr_ModifiedBy { get; set; }
        public bool Usr_isDeleted { get; set; }
        public string Usr_salt { get; set; }
        public string UsrP_FirstName { get; set; }
        public string UsrP_LastName { get; set; }
        public string Usrp_ProfilePicture { get; set; }
        public string file { get; set; }
        public string UsrP_EmailID { get; set; }
        public string cnf_Password { get; set; }
        public Nullable<System.DateTime> Usrp_DOJ { get; set; }
        public string visibility { get; set; }
        public int? Usr_Titleid { get; set; }
        public string UsrP_EmployeeID { get; set; }
        public Nullable<int> Projectid { get; set; }
        public bool? isusacc { get; set; }
        public int Genderid { get; set; }
    }

    public class History_UsersEntity : ResponseHeader
    {
        public int History_User_ID { get; set; }
        public int History_Usr_UserID { get; set; }
        public int History_Usr_AccountID { get; set; }
        public string AccountName { get; set; }
        public int History_Usr_UserTypeID { get; set; }
        public string UserType { get; set; }
        public int History_Usr_RoleID { get; set; }
        public string RoleName { get; set; }
        public Nullable<int> History_Usr_Manager { get; set; }
        public string ManagerName { get; set; }
        public string History_Usr_Username { get; set; }
        public string History_Usr_LoginId { get; set; }
        public string History_Usr_Password { get; set; }
        public bool History_Usr_ActiveStatus { get; set; }
        public short History_Usr_Version { get; set; }
        public System.DateTime History_Usr_CreatedDate { get; set; }
        public int History_Usr_CreatedBy { get; set; }
        public Nullable<System.DateTime> History_Usr_ModifiedDate { get; set; }
        public int History_Usr_ModifiedBy { get; set; }
        public bool History_Usr_isDeleted { get; set; }
    }
}
