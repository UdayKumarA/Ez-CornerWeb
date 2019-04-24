//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Evolutyz.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Accounts = new HashSet<Account>();
            this.HolidayCalendars = new HashSet<HolidayCalendar>();
            this.HolidayCalendars1 = new HashSet<HolidayCalendar>();
            this.UsersProfiles = new HashSet<UsersProfile>();
        }
    
        public int Usr_UserID { get; set; }
        public int Usr_AccountID { get; set; }
        public int Usr_UserTypeID { get; set; }
        public int Usr_RoleID { get; set; }
        public string Usr_Username { get; set; }
        public string Usr_LoginId { get; set; }
        public string Usr_Password { get; set; }
        public bool Usr_ActiveStatus { get; set; }
        public Nullable<int> Usr_TaskID { get; set; }
        public short Usr_Version { get; set; }
        public string Usr_salt { get; set; }
        public Nullable<int> Usr_Manager { get; set; }
        public Nullable<int> Usr_Manager2 { get; set; }
        public System.DateTime Usr_CreatedDate { get; set; }
        public Nullable<int> Usr_CreatedBy { get; set; }
        public Nullable<System.DateTime> Usr_ModifiedDate { get; set; }
        public Nullable<int> Usr_ModifiedBy { get; set; }
        public bool Usr_isDeleted { get; set; }
        public string Url_token { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual Account Account { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HolidayCalendar> HolidayCalendars { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HolidayCalendar> HolidayCalendars1 { get; set; }
        public virtual UserType UserType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsersProfile> UsersProfiles { get; set; }
    }
}
