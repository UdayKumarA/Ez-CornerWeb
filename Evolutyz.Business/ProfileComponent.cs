using Evolutyz.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evolutyz.Entities;
namespace Evolutyz.Business
{
    public class ProfileComponent
    {

        #region To call add method of Role from Data access layer
        public int AddProfile(UserProfileEntity user)
        {
            var ProfileDAC = new ProfileDAC();

            return ProfileDAC.AddProfile(user);
        }
        #endregion

        #region To call update method of ProfileAllocation Table from Data access layer
        public int UpdateProfileDetail(UserProfileEntity profile)
        {
            var ProfileDAC = new ProfileDAC();
            return ProfileDAC.UpdateProfileDetail(profile);
        }
        #endregion
        #region To call update method of ProfileAllocation Table from Data access layer
        public int UpdateProfileDetailByImage(UserProfileEntity profile)
        {
            var ProfileDAC = new ProfileDAC();
            return ProfileDAC.UpdateProfileDetailByImage(profile);
        }
        #endregion
      
        #region To get all details of Account Table from Data access layer
        public UserProfileEntity GetProfileDetailByID(int orgID)
        {
            var ProfileDAC = new ProfileDAC();
            return ProfileDAC.GetProfileDetailByID(orgID);
        }
        #endregion
        #region To get all details of Account Table from Data access layer
        public List<UserProfileEntity> GetSocialMediaByID(int orgID)
        {
            var ProfileDAC = new ProfileDAC();
            return ProfileDAC.GetSocialMediaByID(orgID);
        }
        #endregion

        #region To get all details of Account Table from Data access layer
        public UserProfileEntity GetProfileDetailid(int orgID)
        {
            var ProfileDAC = new ProfileDAC();
            return ProfileDAC.GetProfileDetailid(orgID);
        }
        #endregion

        public List<SkillEntity> GetSkillList(int id)
        {
            ProfileDAC dac = new ProfileDAC();
            return dac.GetSkillList(id);
        }

        public string DeleteSkills(int id) {
            ProfileDAC dac = new ProfileDAC();
            return dac.DeleteSkills(id);
        }

        public string CheckPassword(string oldpassword, string newpassword)
        {
            ProfileDAC dac = new ProfileDAC();
            return dac.CheckPassword(oldpassword, newpassword);
        }
        public List<UserProfileEntity> GetAllSocialMediaIcons(int accountid)
        {
            ProfileDAC dac = new ProfileDAC();
            return dac.GetAllSocialMediaIcons(accountid);
        }
        public List<UserProfileEntity> GetUserSocialMediaurls(int userid)
        {
            ProfileDAC dac = new ProfileDAC();
            return dac.GetUserSocialMediaurls(userid);
        }
        
        public string AddUserSocialMedia(List<UserProfileEntity> jsonobj)
        {
            ProfileDAC dac = new ProfileDAC();
            return dac.AddUserSocialMedia(jsonobj);
        }
        public string UpdateUserSocialMedia(List<UserProfileEntity> jsonobj)
        {
            ProfileDAC dac = new ProfileDAC();
            return dac.UpdateUserSocialMedia(jsonobj);
        }
    }
}
