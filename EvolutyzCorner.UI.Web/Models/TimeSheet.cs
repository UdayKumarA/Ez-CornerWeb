using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EvolutyzCorner.UI.Web.Models;
namespace EvolutyzCorner.UI.Web.Models
{
    public class TimeSheet
    {


     public    DateTime Date { get; set; }

     public    int ProjectId { get; set; }

    public     List<Task> Task {get;set;}


    }
}