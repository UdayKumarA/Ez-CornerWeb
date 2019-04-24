using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public class UserProfileEntity : ResponseHeader
    {
        public int UsrP_UserProfileID { get; set; }
        public int UsrP_UserID { get; set; }
        public string UsrP_FirstName { get; set; }
        public string UsrP_LastName { get; set; }
        public string UsrP_EmployeeID { get; set; }
        public string UsrP_EmailID { get; set; }
        public string Usrp_MobileNumber { get; set; }
        public string Usrp_PhoneNumber { get; set; }
        public string Usrp_ProfilePicture { get; set; }
        public DateTime? UsrP_DOB { get; set; }
        public DateTime? Usrp_DOJ { get; set; }
        public bool UsrP_ActiveStatus { get; set; }
        public short UsrP_Version { get; set; }
        public System.DateTime UsrP_CreatedDate { get; set; }
        public int UsrP_CreatedBy { get; set; }
        public Nullable<System.DateTime> UsrP_ModifiedDate { get; set; }
        public Nullable<int> UsrP_ModifiedBy { get; set; }
        public bool UsrP_isDeleted { get; set; }
        public Nullable<int> Usr_GenderId { get; set; }
        public string Marital_Status { get; set; }
        public string PermanentAddress { get; set; }
        public string TemporaryAddress { get; set; }
        public Nullable<int> Usr_Titleid { get; set; }
        public UserEntity User { get; set; }


        public int Usr_SkillId { get; set; }
        public string SkillTitle { get; set; }
        public Nullable<int> Usr_UserId { get; set; }
        public Nullable<int> SkillId { get; set; }
        public string Experience { get; set; }
        public Nullable<System.DateTime> Usk_CreatedDate { get; set; }
        public Nullable<System.DateTime> Usk_ModifiedDate { get; set; }
        public Nullable<bool> Is_Deleted { get; set; }
        public Nullable<int> StatusID { get; set; }

        public Nullable<int> SocialMediaId { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public int SocialMediaProfileId { get; set; }
        public Nullable<int> Usmp_UserID { get; set; }
        public Nullable<int> Usmp_SocialMediaId { get; set; }
        public string Url { get; set; }
    }
}