using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Evolutyz.Data
{
    public class NewBoardDAC : DataAccessComponent
    {
        
        public List<NewsboardEntity> GetNewsCollection()
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.NewsBoards
                                join u in db.Users on q.CreatedBy equals u.Usr_UserID
                                join up in db.UsersProfiles on u.Usr_UserID equals up.UsrP_UserID
                                 select new NewsboardEntity
                                 {
                                     NewsBoardId = q.NewsBoardId,
                                     Title = q.Title,
                                     Description = q.Description,
                                     Image = q.Image,
                                     Name = up.UsrP_FirstName+" "+up.UsrP_LastName,
                                     CreatedDate= q.CreatedDate
                                    
                                 }).ToList();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public NewsboardEntity GetNewsById(int id)
        {
            using (var db = new EvolutyzCornerDataEntities())
            {
                try
                {
                    var query = (from q in db.NewsBoards
                                 join u in db.Users on q.CreatedBy equals u.Usr_UserID
                                 join up in db.UsersProfiles on u.Usr_UserID equals up.UsrP_UserID
                                 where q.NewsBoardId == id
                                 select new NewsboardEntity
                                 {
                                     NewsBoardId = q.NewsBoardId,
                                     Title = q.Title,
                                     Description = q.Description,
                                     Image = q.Image,
                                     Name = up.UsrP_FirstName + " " + up.UsrP_LastName,
                                     CreatedDate = q.CreatedDate,
                                     

                                 }).FirstOrDefault();

                    return query;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }



        public int AddNews(NewsboardEntity news)
        {

            NewsBoard orgCheck = new NewsBoard();
            int retVal = 0;
            string response = string.Empty;
           
            using (var db = new DbContext(CONNECTION_NAME))
            {
                try
                {
                    orgCheck = db.Set<NewsBoard>().Where(s => s.NewsBoardId == news.NewsBoardId).FirstOrDefault<NewsBoard>();
                    if (orgCheck != null)
                    {
                        return retVal;
                    }
                    NewsBoard OrgCheck = new NewsBoard();
                    OrgCheck.NewsBoardId = news.NewsBoardId;
                    OrgCheck.Title = news.Title;
                    OrgCheck.Description = news.Description;
                    OrgCheck.Image = news.Image;
                    OrgCheck.CreatedBy = news.CreatedBy;
                    OrgCheck.CreatedDate = DateTime.Now;
                    db.Set<NewsBoard>().Add(OrgCheck);

                    db.SaveChanges();
                 



                    retVal = 1;
                }
                catch (Exception ex)
                {
                    retVal = -1;
                }
            }
            return retVal;
        }

    }
}
