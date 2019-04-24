using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolutyz.Entities
{
    public class ProjectAllocationEntity : ResponseHeader
    {
        //public int Ufp_UsersForProjectsID { get; set; }
        //public int Ufp_ProjectID { get; set; }
        public string ProjectName { get; set; }
        //public int Ufp_UserID { get; set; }
        public string Username { get; set; }
        //public Nullable<System.DateTime> Ufp_StartDate { get; set; }
        //public Nullable<System.DateTime> Ufp_EndDate { get; set; }
        //public Nullable<int> Ufp_ParticipationPercentage { get; set; }
        //public bool Ufp_ActiveStatus { get; set; }
        //public short Ufp_Version { get; set; }
        //public Nullable<System.DateTime> Ufp_CreatedDate { get; set; }
        //public int Ufp_CreatedBy { get; set; }
        //public Nullable<System.DateTime> Ufp_ModifiedDate { get; set; }
        //public Nullable<int> Ufp_ModifiedBy { get; set; }
        //public bool Ufp_isDeleted { get; set; }
        public string LoginId { get; set; }
        public int Proj_AccountID { get; set; }
        public int UProj_UserProjectID { get; set; }
        public int UProj_ProjectID { get; set; }
        public int UProj_UserID { get; set; }
        public System.DateTime UProj_StartDate { get; set; }
        public System.DateTime? UProj_EndDate { get; set; }
        public byte UProj_ParticipationPercentage { get; set; }
        public bool UProj_ActiveStatus { get; set; }
        public short UProj_Version { get; set; }
        public System.DateTime UProj_CreatedDate { get; set; }
        public int UProj_CreatedBy { get; set; }
        public Nullable<System.DateTime> UProj_ModifiedDate { get; set; }
        public Nullable<int> UProj_ModifiedBy { get; set; }
        public bool UProj_isDeleted { get; set; }

        public Nullable<int> UProj_L1_ManagerId { get; set; }
        public Nullable<int> UProj_L2_ManagerId { get; set; }
        public bool? Is_L1_Manager { get; set; }
        public bool? Is_L2_Manager { get; set; }
        public string Proj_ProjectCode { get; set; }
    }
}
