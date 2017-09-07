using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using System.Threading;
using MetroFramework.Forms;
using MetroFramework.Controls;
using System.Windows.Forms;

namespace FBMessangerFire.Target
{
   public partial class TargetingandMessaging
    {
       /// <summary>
       /// Facebook Client we Use to Interact with Facebook API to gathring Infromation about our Target.
       /// </summary>
       private FacebookClient fbc = null;
       /// <summary>
       /// this is the number of users will search for per keyword
       /// comes from this Equation total-slots / count-keywods
       /// the total-slots determine by the type of campaign [Small,Mid,Large]
       /// and the count-keywords determine by the Keywords.Count
       /// </summary>
       private int numberOfSlotPerKeyword = 0;
       private Campaign CampaignType;
       /// <summary>
       /// List of Keywords needed By Extract Method to search on Facebook about some users realted by this keywords.
       /// </summary>
       private List<string> Keywords = null;
       /// <summary>
       /// Final Results of Extraction Process will be stay here , waiting for Messaging them.
       /// </summary>
       private List<string> TargetUsers = null;
       private MetroProgressBar progressPar = null;
       private MetroLabel targetUserCount = null;
       /// <summary>
       /// This is Main Operational Thread we control all the operation through it, we can Start , Abort etc..
       /// </summary>
       public Thread operationalThread = null;
       /// <summary>
       /// intiate the Object of class or Update it
       /// at the first time all of Opject data will Intaite , when we try to create new Instance the object will not dropped but will updated.
       /// when we update the alrady object OpertionalThread will not effect and Keywords list also.
       /// </summary>
       /// <param name="client"> Facebook Client to interact with FB API</param>
       /// <param name="keywords">List of Keywords</param>
       /// <param name="type">type of Campaign</param>
       public TargetingandMessaging(FacebookClient client, List<string> keywords, Campaign type)
       {
           // if the Operational Thread don't intait = null then we Intaite them and send it thier Method
           if (operationalThread == null)
           {
               operationalThread = new Thread(ExtractTargetUser);
               operationalThread.IsBackground = true;
           }
           

           fbc = client;
           CampaignType = type;

           if (Keywords == null)
           { Keywords = new List<string>(); }
           if (TargetUsers == null)
           { TargetUsers = new List<string>(); }

           Keywords.AddRange(keywords);
           Keywords.Distinct();

           // Determine number of slots per keywords
           switch (CampaignType)
           {
               case Campaign.Small:
                   numberOfSlotPerKeyword = (int)CampaignSlots.Small / Keywords.Count;
                   break;
               case Campaign.Mid:
                   numberOfSlotPerKeyword = (int)CampaignSlots.Mid / Keywords.Count;
                   break;
               case Campaign.Large:
                   numberOfSlotPerKeyword = (int)CampaignSlots.Large / Keywords.Count;
                   break;
           }
       }
       /// <summary>
       /// This Method Make a Delay Between Send Multiple Messages.
       /// </summary>
       /// <param name="CampaignType"> Type of Your Campaign [Small , Mid , Large]</param>
       private void SetDelay(Campaign CampaignType)
       {
           Random random = new Random();
           int time = 0;
           switch (CampaignType)
           {
               case Campaign.Small:
                   time = random.Next((int)MinDelayOptions.Small, (int)MaxDelayOptions.Small);
                   break;
               case Campaign.Mid:
                   time = random.Next((int)MinDelayOptions.Small, (int)MaxDelayOptions.Small);
                   break;
               case Campaign.Large:
                   time = random.Next((int)MinDelayOptions.Small, (int)MaxDelayOptions.Small);
                   break;
           }
           Thread.Sleep(time);
       }
       /// <summary>
       /// using this Method to Point at Controls in the Main form so we can Invoke those Controls and updates there values
       /// </summary>
       /// <param name="MetroProgressBarCONTROL"> ProgressPar of Main Form</param>
       /// <param name="MetroLabelCONTROL">Lable of User Target Count at Mentoring tap</param>
       /// <returns>Main Thread that Run all of this Operation</returns>
       public Thread SetController(MetroProgressBar MetroProgressBarCONTROL, MetroLabel MetroLabelCONTROL)
       {
           // Set the Constrain Slots for Progess to be ready.
           progressPar = MetroProgressBarCONTROL;
           progressPar.Minimum = 0;
           switch (CampaignType)
           {
               case Campaign.Small:
                   progressPar.Maximum = (int)CampaignSlots.Small;
                   break;
               case Campaign.Mid:
                   progressPar.Maximum = (int)CampaignSlots.Mid;
                   break;
               case Campaign.Large:
                   progressPar.Maximum = (int)CampaignSlots.Large;
                   break;
           }
            
           targetUserCount = MetroLabelCONTROL;
           return operationalThread;
       }
       /// <summary>
       /// This is Method the Core for Extract the Users Related to Keywords list
       /// Fristly we Parallel our Operation for each Keyword in the List
       /// then we gethering using Facebook API all events realted to the keyword
       /// then scraping the attending for this events
       /// finally we update the Main form Controlls
       /// </summary>
       public void ExtractTargetUser()
       {
           Parallel.ForEach(Keywords, keyword =>
           {
               dynamic eventsdata = fbc.Get(string.Format("search?q={0}&type=event", keyword));
               int Count = eventsdata.data.Count;
               for (int i = 0; i < Count; i++)
               {
                   try
                   {
                       dynamic attending = fbc.Get(string.Format("{0}?fields=attending.limit({1})", eventsdata.data[i].id, numberOfSlotPerKeyword));
                       if ((int)attending.attending.data.Count < numberOfSlotPerKeyword)
                       {
                           for (int j = 0; j < (int)attending.attending.data.Count; j++)
                           {
                               TargetUsers.Add(attending.attending.data[j].id);
                               progressPar.Invoke((MethodInvoker)delegate
                               {
                                   // Running on the UI thread
                                   progressPar.Value++;
                               });
                               targetUserCount.Invoke((MethodInvoker)delegate
                               {
                                   // Running on the UI thread
                                   targetUserCount.Text = TargetUsers.Count.ToString();
                               });
                           }
                       }
                       else
                       {
                           for (int j = 0; j < numberOfSlotPerKeyword; j++)
                           {
                               if (TargetUsers.Count >= numberOfSlotPerKeyword)
                               {
                                   break;
                               }
                               TargetUsers.Add(attending.attending.data[j].id);
                               progressPar.Invoke((MethodInvoker)delegate
                               {
                                   // Running on the UI thread
                                   progressPar.Value++;
                               });
                               targetUserCount.Invoke((MethodInvoker)delegate
                               {
                                   // Running on the UI thread
                                   targetUserCount.Text = TargetUsers.Count.ToString();
                               });
                           }
                       }
                       TargetUsers.Distinct();
                       if (TargetUsers.Count >= numberOfSlotPerKeyword)
                       {
                           break;
                       }
                   }
                   catch
                   {
                       //Failed
                   }
               }
           });
       }
       /// <summary>
       /// Change the List of Keywords
       /// </summary>
       /// <param name="list">List of Keywords</param>
       public void SetKeywords(List<string> list)
       {
           Keywords.AddRange(list);
           Keywords.Distinct();
       }
       /// <summary>
       /// Change the Type of Your Campaign [Small , Mid , Large]
       /// </summary>
       /// <param name="campaign">Type of Your Campaign [Small , Mid , Large]</param>
       public void ChangeCampaign(Campaign campaign)
       {
           CampaignType = campaign;
           progressPar.Minimum = 0;
           
           if (progressPar.InvokeRequired)
           {
               progressPar.Invoke((MethodInvoker)delegate
               {
                   // Running on the UI thread
                   switch (CampaignType)
                   {
                       case Campaign.Small:
                           progressPar.Maximum = (int)CampaignSlots.Small;
                           break;
                       case Campaign.Mid:
                           progressPar.Maximum = (int)CampaignSlots.Mid;
                           break;
                       case Campaign.Large:
                           progressPar.Maximum = (int)CampaignSlots.Large;
                           break;
                   }
               });
           }
           else
           {
               switch (CampaignType)
               {
                   case Campaign.Small:
                       progressPar.Maximum = (int)CampaignSlots.Small;
                       break;
                   case Campaign.Mid:
                       progressPar.Maximum = (int)CampaignSlots.Mid;
                       break;
                   case Campaign.Large:
                       progressPar.Maximum = (int)CampaignSlots.Large;
                       break;
               }
           }           
       }
    }
}
