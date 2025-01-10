using System;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ArcheBuddy.Bot.Classes;

namespace YourNamespace
{
    public class Magebot : Core
   {
            public static string GetPluginAuthor()
            {return "```by - elec```";}
            public static string GetPluginVersion()
            {return "1.0.0.0";}
            public static string GetPluginDescription()
            {return "Руками"; }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        public void PluginRun()    
        {   
            SetGroupStatus("hand", true); //Add checkbox to our character widget
           while (true)      
            { 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
                if (GetGroupStatus("hand") && me.isAlive() == true)
                {
               {   
                try
                   { 
                       if (me.laborPoints < 250)
                 UseItem("Большая бутыль с имбирным напитком");
                       if (me.laborPoints < 250)
                 UseItem("Бутыль с имбирным напитком");
                         UseItem("Потускневшее старинное оружие");
                         UseItem("Старинный кожаный доспех"); 
                         UseItem("Старинный латный доспех");
                         UseItem("Старинный матерчатый доспех");
                         UseItem("Неизвестное оружие");
                         UseItem("Предмет одеяний добродетели");
                         UseItem("Легкий сверток странника");
                         UseItem("Крошки лунного акхиума");
                         UseItem("Крошки солнечного акхиума");
                         UseItem("Крошки звездного акхиума");
                         UseItem("Пыль лунного акхиума");
                         UseItem("Пыль солнечного акхиума");
                         UseItem("Пыль звездного акхиума");
                         UseItem("Слиток лунного акхиума");
                         UseItem("Слиток солнечного акхиума");
                         UseItem("Слиток звездного акхиума");
                         UseItem("Резной сундучок");
                         UseItem("Котомка авантюриста");
                         UseItem("Окованный сундучок");
                         UseItem("Котомка странствующего алхимика");
                     } 
                     catch {}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
                            }

                        }} 
                    }                  
} }           