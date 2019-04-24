using Evolutyz.Data;
using Evolutyz.Entities;
using System.Collections.Generic;

namespace Evolutyz.Business
{
    public  class NewBoardComponent
    {
       

        public List<NewsboardEntity> GetNewsCollection()
        {
            var newsboarddac = new NewBoardDAC();
            return newsboarddac.GetNewsCollection();
        }

        public int AddNews(NewsboardEntity news)
        {
            var newsboarddac = new NewBoardDAC();
            return newsboarddac.AddNews(news);
        }
        public NewsboardEntity GetNewsById(int id)
        {
            var newsboarddac = new NewBoardDAC();
            return newsboarddac.GetNewsById(id);
        }


    }
}
