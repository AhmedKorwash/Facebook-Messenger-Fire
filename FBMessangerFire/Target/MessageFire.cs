using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using FacebookMessageSender;
using System.Threading;
using System.Data;

namespace FBMessangerFire.Target
{
    public partial class TargetingandMessaging
    {
        private AuthFaceBook auth = null;
        private OptionMessage option = null;
        private FacebookHttpRequest request = null;
        private List<SucsessGrid> dtsucess = null;
        private List<FiledGrid> dterror = null;
        private int SentCount = 0;
        /// <summary>
        /// This is CancellationToken sent with task that make us able to stop this task or cancel it
        /// </summary>
        private CancellationToken token;
        /// <summary>
        /// this is Source for CancellationToken through it we can Canceling it.
        /// </summary>
        private CancellationTokenSource source;
        /// <summary>
        /// Here the list of message content to be sent randomaly
        /// </summary>
        private List<string> messagesContent = null;
        /// <summary>
        /// here the Control of Error Grid view
        /// </summary>
        private MetroGrid error = null;
        /// <summary>
        /// Here the Control of Sucesseded Grid view
        /// </summary>
        private MetroGrid sucess = null;
        private BindingSource errorSource = null;
        private BindingSource sucessSource = null;
        private MetroLabel MessegesSentCount = null;
        public void IntaiteGridView(MetroGrid _sucess, MetroGrid _error, BindingSource _sucessSource, BindingSource _errorSource, MetroLabel MessegesSentCount)
        {
            this.error = _error;
            this.sucess = _sucess;
            this.errorSource = _errorSource;
            this.sucessSource = _sucessSource;
            this.MessegesSentCount = MessegesSentCount;
        }
        public void MessageingFire(List<string> mess,AuthFaceBook Login)
        {
            //dtsucess = new List<FBMessangerFire.SucsessGrid>();
            //dterror = new List<FiledGrid>();

            if (auth == null)
                auth = Login;

            if (messagesContent == null)
            messagesContent = new List<string>();
            messagesContent.AddRange(mess);

            //be able to cancel the operation at any time we need
            source = new CancellationTokenSource();
            token = source.Token;

            // Set Progresspar value = 0
            if (this.progressPar.InvokeRequired) progressPar.Invoke((MethodInvoker)delegate { progressPar.Value = 0; }); else progressPar.Value = 0;
            request = new FacebookHttpRequest();
            Task.Factory.StartNew(() =>
            {
                foreach (var user in TargetUsers)
                {
                    SendMessage(user);
                    progressPar.Invoke((MethodInvoker)delegate
                    {
                        // Running on the UI thread
                        progressPar.Value++;
                    });
                    SetDelay(CampaignType);
                }
            }, token);
        }
        public void StopMessageingFire()
        {
            if (source != null)
            {
                source.Cancel();
            }
        }
        /// <summary>
        /// This Method used to send Message for this Person indetify with ID
        /// </summary>
        /// <param name="id">User id</param>
        private void SendMessage(string id)
        {
            try
            {
                option = new OptionMessage(id, auth);
                switch (request.SendMessage(auth, GetMessageContent(), option))
                {
                    case "Message Sent":
                        if (sucess.InvokeRequired)
                        {
                            sucess.Invoke((MethodInvoker)delegate
                            {
                                UpdateGrid(UpdateGridOptions.Sucsess, new SucsessGrid() { Entity = id, Message = "Message was Sent", CreateAt = DateTime.Now });
                            });
                        }
                        else
                        {
                            UpdateGrid(UpdateGridOptions.Sucsess, new SucsessGrid() { Entity = id, Message = "Message was Sent", CreateAt = DateTime.Now });
                        }
                        SentCount++;
                        MessegesSentCount.Invoke((MethodInvoker)delegate
                        {
                            // Running on the UI thread
                            MessegesSentCount.Text = SentCount.ToString();
                        });
                        break;
                    case "Facebook Auth Error":
                        if (error.InvokeRequired)
                        {
                            error.Invoke((MethodInvoker)delegate
                            {
                                UpdateGrid(UpdateGridOptions.Failed, new FiledGrid() { Entity = id, Errors = "Canet Send Message", CreateAt = DateTime.Now });
                            });
                        }
                        else
                        {
                            UpdateGrid(UpdateGridOptions.Failed, new FiledGrid() { Entity = id, Errors = "Canet Send Message", CreateAt = DateTime.Now });
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                if (error.InvokeRequired)
                {
                    error.Invoke((MethodInvoker)delegate
                    {
                        UpdateGrid(UpdateGridOptions.Failed, new FiledGrid() { Entity = id, Errors = ex.Message, CreateAt = DateTime.Now });
                    });
                }
                else
                {
                    UpdateGrid(UpdateGridOptions.Failed, new FiledGrid() { Entity = id, Errors = ex.Message, CreateAt = DateTime.Now });
                }
            }
        }
        /// <summary>
        /// Get Randome Message to sent
        /// </summary>
        /// <returns></returns>
        private string GetMessageContent()
        {
            Random r = new Random();
           return messagesContent[r.Next(messagesContent.Count)];
        }
        /// <summary>
        /// Here we Update the MetroGrid View
        /// </summary>
        /// <param name="option">Give me Options of Update you need jsut like Sucsess or Failed</param>
        /// <param name="view">Which Grid view you need sucess or error , this is Controll at Main form</param>
        private void UpdateGrid(UpdateGridOptions option, StructureGrid view)
        {
            switch (option)
            {
                case UpdateGridOptions.Sucsess:
                    sucessSource.Add((SucsessGrid)view);
                    break;
                case UpdateGridOptions.Failed:
                    errorSource.Add((FiledGrid)view);
                    break;
            }
        }
    }
}
