using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using ArcheBuddy.Bot.Classes;

namespace DefaultNamespace{
    public class DefaultClass : Core
    {
        public void PluginRun()
        {
            RequestMailList();
            foreach (var mail in getMails())
            {
                mail.OpenMail();
                if (!mail.isSent)
                {
                    mail.ReceiveGoldFromMail();
                    foreach (var item in mail.getItems())
                        mail.ReceiveItemFromMail(item);
                    mail.DeleteMail();
                }
                Thread.Sleep(1000);
            }
        }
    }
}