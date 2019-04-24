using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Entities;
using Evolutyz.Data;


namespace Evolutyz.Business
{
    public class SkillComponent
    {
       SkillDAC  skilldac = new SkillDAC();
        
        public List<SkillEntity> GetAccountSkills(int accid)
        {
            return skilldac.GetAccountSkills(accid);
        }
        public SkillEntity Getskillbyid(int id)
        {
            return skilldac.Getskillbyid(id);
        }
        public string UpdateSkills(int id, string skillTitle, string Description, string status)
        {
            return skilldac.UpdateSkills(id, skillTitle, Description, status);
        }

        public int DeleteSkill(int skillid)
        {
            return skilldac.DeleteSkill(skillid);
        }

    }
}
