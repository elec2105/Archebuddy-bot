using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using ArcheBuddy.Bot.Classes;

namespace DefaultNameSpace{
   public class DefaultClass : Core
   {
       public static string GetPluginAuthor()
       {
           return "Plugin Author";
       }

       public static string GetPluginVersion()
       {
           return "1.0.0.0";
       }

       public static string GetPluginDescription()
       {
           return "My plugin description";
       }

       //Call on plugin start
       public void PluginRun()
       { 
        List<Item> mailitems = new List<Item>();
{ //here we add all instances of itemname we possess, to the mail. Use a copy (or a function containing) this block for every different items
    List<Item> Inventory = getInvItems("");
    foreach (Item item in Inventory)
    {
            if (mailitems.Count > 8)
                    break;
            mailitems.Add(item);
    }
}
    SendMail("Богатей", "mail", "text", true, 10000000, mailitems);   
       }
       //Call on plugin stop
       public void PluginStop()
       {
       }
   }
}