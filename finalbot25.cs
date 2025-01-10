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
//////////////////////////////////////////////////Считаем количество мобов, которые держат нас или нашего маунта в таргете////////////////////////////////////////////////
                public int mobsCountThatAttackUs()
                {
            int count = 0;
                foreach (var obj in getCreatures())
                    {
                if (obj.type == BotTypes.Npc && isAttackable(obj) && isAlive(obj) && (obj.target == me || (obj.target != null && obj.target == getMount())))
                count++;
                }
                return count;
            }  
//////////////////////////////////////////////////Получаем ближайшего моба, которого можно атаковать, которого не атаковал кто-либо другой, он находится в зоне////////////////////////////////////////////////   
        public Creature GetBestNearestMob(Zone zone)
        {
            Creature mob = null;
            double dist = 999999;
            foreach (var obj in getCreatures())
            {
                if (obj.type == BotTypes.Npc && isAttackable(obj) && (obj.firstHitter == null || obj.firstHitter == me) && isAlive(obj) && me.dist(obj) < dist && zone.ObjInZone(obj)
                    && (hpp(obj) == 100 || obj.target == me || (obj.target != null && obj.target == getMount())))
                {
                    mob = obj;
                    dist = me.dist(obj);
                }
            }
            return mob;
        } 
//////////////////////////////////////////////////Нитка, контролирует, если мы что-либо кастуем в моба, и пока мы это делаем - его ударил ктото другой - отменяем каст.///////////////////////////////////////
        public void CancelAttacksOnAnothersMobs()
        {
            while (true)
            {
                if (me.isCasting && me.target != null && me.target.firstHitter != null && me.target.firstHitter != me && me.target.aggroTarget != me)
                {
                   CancelSkill();
                   break;
                }
                Thread.Sleep(100);
            }
        }           
//////////////////////////////////////////////////Проверка бафа////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void CheckBuffs()
        {  
            onFoundGameMaster += onGmFound;            
            if (buffTime("Магический щит II") == 0 && 
            buffTime("Магический щит III") == 0 && 
            buffTime("Магический щит IV") == 0 && 
            buffTime("Магический щит V") == 0 && skillCooldown("Магический щит") == 0 && (hpp() < 55))
              UseSkillAndWait("Магический щит", true);
             if (buffTime("Второе дыхание I") == 0 && 
                   buffTime("Второе дыхание II") == 0 && 
                   buffTime("Второе дыхание III") == 0 && 
                   buffTime("Второе дыхание IV") == 0 && 
                   buffTime("Второе дыхание V") == 0 &&
                   buffTime("Второе дыхание VI") == 0 && skillCooldown("Второе дыхание") == 0 )
                   UseSkillAndWait("Второе дыхание", true);
             if (buffTime("Заживление ран I") == 0 && 
                buffTime("Заживление ран II") == 0 && 
               buffTime("Заживление ран III") == 0 && 
               buffTime("Заживление ран IV") == 0 && skillCooldown("Заживление ран") == 0 )
                 UseSkillAndWait("Заживление ран", true);
             if (buffTime("Свиток скорости") == 0)
                 UseItem("Свиток скорости");
             if (buffTime("Свиток стойкости") == 0)
                 UseItem("Свиток стойкости");
                 DeleteItem("Полновесный мешочек с серебром", 100);
                 DeleteItem("Увесистый краденый кошелек", 100);
                 DeleteItem("Позвякивающий краденый кошелек", 100);
                 DeleteItem("Украшение лугового прайда");    //украшения
                 DeleteItem("Старинное эльфийское украшение");    //украшения
                 DeleteItem("Сверкающее украшение");
                 DeleteItem("Украшение моряка");
                 DeleteItem("Предмет снаряжения королевского егеря");
                 DeleteItem("Предмет бронзовых доспехов");
             if (me.laborPoints > 4900)
                 UseItem("Котомка авантюриста");
             if (me.laborPoints > 1900 && buffTime("Премиум-подписка") == 0)
                 UseItem("Котомка авантюриста");
             UseItem("Свиток быстрого обучения IV");
             UseItem("Фолиант боевой подготовки IV");
             UseItem("Свиток быстрого обучения III");
             UseItem("Фолиант боевой подготовки III");
             UseItem("Свиток быстрого обучения II");
             UseItem("Фолиант боевой подготовки II");

       }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
        private void onGmFound(Creature ob)
         {
                Log(" Обнаружен ГМ, не палимся");
             CloseGame();
          }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////              
        public void UseSkillAndWait(string skillName, bool selfTarget = false)
        {
            //wait cooldowns first, before we try to cast skill
            while (me.isCasting || me.isGlobalCooldown)
                Thread.Sleep(50);
            if (!UseSkill(skillName, true, selfTarget))
            {
                if (me.target != null && GetLastError() == LastError.NoLineOfSight)
                {
                    //No line of sight, try come to target.
                    if (dist(me.target) <= 5)
                        ComeTo(me.target, 2);
                    else if (dist(me.target) <= 10)
                        ComeTo(me.target, 3);
                    else if (dist(me.target) < 20)
                        ComeTo(me.target, 8);
                    else
                        ComeTo(me.target, 8);
                }
            }
            while (me.isCasting || me.isGlobalCooldown)
                Thread.Sleep(50);
        } 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
        public void PluginRun()
           
        {   
            new Task(() => { CancelAttacksOnAnothersMobs(); }).Start();
            RoundZone zone = new RoundZone(me.X, me.Y, 250);             
           while (true)  
            {        
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
                    if (isAlive() == false) //воскрешение в случае гибели
                        {
                          Log(DateTime.Now + "```Я умер```");
                          while(me.resurrectionWaitingTime>0)              
                          Thread.Sleep(100);
                          Thread.Sleep(4000);
                          ResToRespoint();                        
                          Thread.Sleep(40000);
                          RestoreExp();
                          RessurectMount(true); 
                                         {   
                                                {   
                                                    UseItem("Степной лев");
                                                    Thread.Sleep(2000); 
                                                    var m = getMount();
                                                    if (m != null)  
                                                    SitToMount();
                                                    if (isMounted() && skillCooldown("Рывок") == 0)
                                                    UseSkill("Рывок");   
                                                { 
                                                    Gps gps = new Gps(this); 
                                                    gps.LoadDataBase(Application.StartupPath + "\\plugins\\all.db3");
                                                    gps.GpsMove("Спот3");
                                                }
                                                    DespawnMount();               
                                            };
                                      }}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
                    if (getAggroMobs().Count < 1)
                    CheckBuffs();
                    onFoundGameMaster += onGmFound;
                    var bestMob = GetBestNearestMob(zone);
                    if (bestMob != null) 
                    {
                            while (bestMob != null && isAlive(bestMob) && isExists(bestMob) && isAlive())
                            {      
                                if (bestMob.aggroTarget != me && bestMob.firstHitter != null && bestMob.firstHitter != me)
                                {
                                    bestMob = null;
                                    break;
                                } 
                                if (bestMob.firstHitter == null && mobsCountThatAttackUs() > 0 && bestMob != GetBestNearestMob(zone))
                                    bestMob = GetBestNearestMob(zone);
                                if (me.target != bestMob)
                                    SetTarget(bestMob);  
                                if (angle(bestMob, me) > 45 && angle(bestMob, me) < 315)
                                        TurnDirectly(bestMob);
////////////////////////////////////////////////Skills////////////////////////////////////////////////////////////////////////////////////////////////  
                                if (skillCooldown("Победный клич") == 0 && isAlive(bestMob) && me.dist(bestMob) <= 10 && hpp(bestMob) > 30 && (mpp() > 13))
                                   UseSkillAndWait("Победный клич");
                                if(skillCooldown("Обледенение") == 0 && hpp(me.target) > 30 && me.dist(me.target) <= 7 && (mpp() > 13) && getAggroMobs().Count > 1) 
                                    UseSkillAndWait("Обледенение");
                                if (skillCooldown("Магический круг") == 0 && isAlive(bestMob) && hpp(bestMob) > 20 && (mpp() > 13) && me.dist(me.target) <= 20) 
                                    UseSkillAndWait("Магический круг");
                                if((hpp() < 65)  && (skillCooldown("Сокрушение разума") == 0) && (skillCooldown("Хватка земли") == 0) && hpp(bestMob) > 35 && me.dist(bestMob) <= 20 && (mpp() > 13)) 
                                    {
                                          Thread.Sleep(750);
                                              UseSkillAndWait("Сокрушение разума");
                                             Thread.Sleep(100);
                                           UseSkillAndWait("Хватка земли");
                                       Thread.Sleep(50);
                                    };
                                   if (skillCooldown("Клич жизни") == 0 && (mpp() > 13) && (hpp() < 70))
                                   UseSkillAndWait("Клич жизни"); 
                                 if(hpp() < 65)
                                 { 
                                 UseItem("Флакон целебной настойки"); UseItem("Галеты"); UseItem("Завтрак героя"); UseItem("Пикантные булочки"); 
                                 UseItem("Ячменный хлеб"); UseItem("Золотистый рогалик"); UseItem("Малый флакон с целебной микстурой"); UseItem("Средний флакон с целебной микстурой");  UseItem("Завтрак тролля");
                                    };
                                 if(mpp() < 65)
                                 { 
                                     UseItem("Флакон травяного отвара"); UseItem("Ячменная похлебка"); UseItem("Сытная солянка"); UseItem("Горячий бульон"); 
                                     UseItem("Золотистый тыквенный суп"); UseItem("Пряный суп Ост-Терры"); UseItem("Малый флакон с микстурой маны"); UseItem("Средний флакон с микстурой маны"); UseItem("Суп императрицы Фарвати");
                                    };          
                                   if (skillCooldown("Ледяная стрела") == 0 && isAlive(bestMob) && hpp(bestMob) > 30 && (mpp() > 13)) 
                                    UseSkillAndWait("Ледяная стрела");
                                Thread.Sleep(10);
                                onFoundGameMaster += onGmFound; 
                                for (int i=0;i<2;i++)
                                    UseSkillAndWait("Сгустки пламени");
                                Thread.Sleep(10);
                                if (skillCooldown("Имитация смерти") == 0 && (((mpp() < 15) && hpp(bestMob) > 30) || (hpp() < 30))) 
                                    {   
                                       UseSkill("Имитация смерти");
                                           if (buffTime("Имитация смерти") > 0 && mpp() > 95 && hpp() > 95)
                                            {
                                                    MoveForward (true);
                                                    Thread.Sleep (100);
                                                    MoveForward (false);
                                             }
                                       };
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////                                 
                            }
                            while (bestMob != null && !isAlive(bestMob) && isExists(bestMob) && bestMob.type == BotTypes.Npc && ((Npc)bestMob).dropAvailable && isAlive())
                            {
                                if (me.dist(bestMob) > 0)
                                   ComeTo(bestMob, 1);
                                PickupAllDrop(bestMob); 
                            }         
                    }
                Thread.Sleep(10);
            }
        }
    }
}         