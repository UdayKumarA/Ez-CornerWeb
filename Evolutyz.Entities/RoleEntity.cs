using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public class RoleEntity : ResponseHeader
    {
        public int GenericRoleID { get; set; }
        public string Title { get; set; }
        public int Rol_RoleID { get; set; }
        public int Rol_AccountID { get; set; }
        public string AccountName { get; set; }
        public string Rol_RoleCode { get; set; }
        public string Rol_RoleName { get; set; }
        public string Rol_RoleDescription { get; set; }
        public bool Rol_ActiveStatus { get; set; }
        public int Rol_Version { get; set; }
        public System.DateTime Rol_CreatedDate { get; set; }
        public int Rol_createdBy { get; set; }
        public Nullable<System.DateTime> Rol_ModifiedDate { get; set; }
        public Nullable<int> Rol_ModifiedBy { get; set; }
        public bool Rol_isDeleted { get; set; }
        public Nullable<bool> IsManagerRole { get; set; }


        public int? ModuleAccessTypeID { get; set; }
        public string ModuleAccessType1 { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> StatusId { get; set; }

        public int Mod_ModuleID { get; set; }
        public string Mod_ModuleCode { get; set; }
        public string Mod_ModuleName { get; set; }
        public string Mod_Description { get; set; }
        public bool Mod_ActiveStatus { get; set; }
        public short Mod_Version { get; set; }
        public System.DateTime Mod_CreatedDate { get; set; }
        public int Mod_CreatedBy { get; set; }
        public Nullable<System.DateTime> Mod_ModifiedDate { get; set; }
        public Nullable<int> Mod_ModifiedBy { get; set; }
        public bool Mod_isDeleted { get; set; }

        public int RMod_RoleModuleID { get; set; }
        public int RMod_AccountID { get; set; }
        public int RMod_RoleID { get; set; }
        public int RMod_ModuleID { get; set; }

        public bool RMod_ActiveStatus { get; set; }

        public int Sub_ModuleID { get; set; }

        public string Sub_ModuleCode { get; set; }
        public string Sub_ModuleName { get; set; }


    }

    public class History_RoleEntity : ResponseHeader
    {
        public int History_Role_ID { get; set; }
        public int History_Rol_RoleID { get; set; }
        public int History_Rol_AccountID { get; set; }
        public string AccountName { get; set; }
        public string History_Rol_RoleCode { get; set; }
        public string History_Rol_RoleName { get; set; }
        public string History_Rol_RoleDescription { get; set; }
        public bool History_Rol_ActiveStatus { get; set; }
        public int History_Rol_Version { get; set; }
        public System.DateTime History_Rol_CreatedDate { get; set; }
        public int History_Rol_createdBy { get; set; }
        public Nullable<System.DateTime> History_Rol_ModifiedDate { get; set; }
        public Nullable<int> History_Rol_ModifiedBy { get; set; }
        public bool History_Rol_isDeleted { get; set; }
    }
}
