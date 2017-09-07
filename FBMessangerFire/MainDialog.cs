using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using FacebookMessageSender;
using Facebook;
using FBMessangerFire.Target;
using System.Threading;

namespace FBMessangerFire
{
    public partial class MainDialog : MetroForm
    {
        AuthFaceBook realLogin = null;
        FacebookClient fbclient = null;
        MessageContent MessagesContect = null;
        TargetingandMessaging TargetingBuilder = null;
        public MainDialog()
        {
            InitializeComponent();
            MessagesContect = new MessageContent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(320, 517);
            this.MinimumSize = new System.Drawing.Size(320, 517);
            this.MaximumSize = new System.Drawing.Size(320, 517);
            AcsesstokenPanel.Hide();
            keywork_txt.AutoCompleteCustomSource.Add("FB Messagner");
            keywork_txt.AutoCompleteCustomSource.Add("Ahmed Korwash");
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void metroLabel3_Click(object sender, EventArgs e)
        {

        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Accsesstoken_txt.Text))
            {
                try
                {
                    fbclient = new FacebookClient(Accsesstoken_txt.Text);
                    dynamic user = fbclient.Get("me?fields=name,email");
                    if (!string.IsNullOrEmpty(user.email) && !string.IsNullOrEmpty(user.name))
                    {
                        _AcsesstokenEmail.Text = user.email;
                        _AcsesstokenUsername.Text = user.name;
                        this.Size = new System.Drawing.Size(918, 517);
                        this.MinimumSize = new System.Drawing.Size(918, 517);
                        this.MaximumSize = new System.Drawing.Size(918, 517);
                        MessageBox.Show("Welcome sir, Your Account Fully Connected Sucsessfully with Facebook",
                                "Login Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        metroButton3.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("invalid Token", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (FacebookOAuthException)
                {
                    MessageBox.Show("invalid Token please insert valid one..", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("UnExpected Error, Maybe the connection was lost..." + Environment.NewLine + ex.Message,
                        "UnExpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please Give me The Accsesstoken for your Account..", "Fields Requiered", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_UsernameLogin.Text) && !string.IsNullOrEmpty(_PasswordLogin.Text))
            {
                try
                {
                    realLogin = new AuthFaceBook(_UsernameLogin.Text, _PasswordLogin.Text);
                    if (realLogin.IsLogin)
                    {
                        AcsesstokenPanel.Show();
                        MessageBox.Show("Welcome sir, Your Account Connected Sucsessfully with Facebook",
                            "Login Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Login_btn.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Username or Password invalid", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("UnExpected Error, Maybe the connection was lost..." + Environment.NewLine + ex.Message,
                        "UnExpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please Write Username and Password", "Fields Requiered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (string.IsNullOrEmpty(_UsernameLogin.Text))
                    this._UsernameLogin.WithError = true;
                if (string.IsNullOrEmpty(_PasswordLogin.Text))
                    this._PasswordLogin.WithError = true;
            }   
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void keywords_list_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void keywork_remove_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(keywork_txt.Text))
                {
                    if (keywork_txt.AutoCompleteCustomSource.Contains(keywork_txt.Text))
                    {
                        keywork_txt.AutoCompleteCustomSource.Remove(keywork_txt.Text);
                        KeywordCounts.Text = keywork_txt.AutoCompleteCustomSource.Count.ToString();
                        keywords_list.Items.Clear();
                        foreach (var item in keywork_txt.AutoCompleteCustomSource)
                        {
                            keywords_list.Items.Add(item.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please insure that you write keyword name correctly",
                            "Miss Match - Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please Select one of Keyword List to Remove it..", "Fields Requiered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("UnExpected Error, Contact me ASAP please..." + Environment.NewLine + ex.Message,
                    "UnExpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void keywork_add_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(keywork_txt.Text))
                {
                    if (!keywork_txt.AutoCompleteCustomSource.Contains(keywork_txt.Text))
                    {
                        keywork_txt.AutoCompleteCustomSource.Add(keywork_txt.Text);
                        KeywordCounts.Text = keywork_txt.AutoCompleteCustomSource.Count.ToString();
                        keywords_list.Items.Clear();
                        foreach (var item in keywork_txt.AutoCompleteCustomSource)
                        {
                            keywords_list.Items.Add(item.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please insure that you enter new keyword",
                            "Keyword Entered before", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter Keyword..", "Fields Requiered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("UnExpected Error, Contact me ASAP please..." + Environment.NewLine + ex.Message,
                    "UnExpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroTabPage2_Click(object sender, EventArgs e)
        {

        }

        private void metroTabPage3_Click(object sender, EventArgs e)
        {

        }

        private async void messages_select_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Multiselect = true;
                op.Filter = "Text Files Only|*.txt";
                if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var files = op.FileNames;
                   await MessageContent.PrepateMessages(MessagesContect, files);
                   MessagesCount.Text = MessagesContect.Messages.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("UnExpected Error, Contact me ASAP please..." + Environment.NewLine + ex.Message,
                    "UnExpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessagesCount.Text = MessagesContect.Messages.Count.ToString();
        }

        private void Start_target_Click(object sender, EventArgs e)
        {
            try
            {
                if (keywork_txt.AutoCompleteCustomSource.Count > 0)
                {
                    if (small_camp.Checked)
                    {
                        if (TargetingBuilder == null)
                        {
                            TargetingBuilder = new TargetingandMessaging(fbclient, keywork_txt.AutoCompleteCustomSource.Cast<string>().ToList<string>(), Campaign.Small);
                            TargetingBuilder.SetController(metroProgressBar1, TargetUserCounts).Start();
                        }
                        else
                        {
                            TargetingBuilder.operationalThread = new Thread(TargetingBuilder.ExtractTargetUser);
                            TargetingBuilder.SetKeywords(keywork_txt.AutoCompleteCustomSource.Cast<string>().ToList<string>());
                            TargetingBuilder.ChangeCampaign(Campaign.Small);
                            TargetingBuilder.SetController(metroProgressBar1, TargetUserCounts).Start();
                        }
                    }
                    else
                    {
                        if (mid_camp.Checked)
                        {
                            if (TargetingBuilder == null)
                            {
                                TargetingBuilder = new TargetingandMessaging(fbclient, keywork_txt.AutoCompleteCustomSource.Cast<string>().ToList<string>(), Campaign.Mid);
                                TargetingBuilder.SetController(metroProgressBar1, TargetUserCounts).Start();
                            }
                            else
                            {
                                TargetingBuilder.operationalThread = new Thread(TargetingBuilder.ExtractTargetUser);
                                TargetingBuilder.SetKeywords(keywork_txt.AutoCompleteCustomSource.Cast<string>().ToList<string>());
                                TargetingBuilder.ChangeCampaign(Campaign.Mid);
                                TargetingBuilder.SetController(metroProgressBar1, TargetUserCounts).Start();
                            }
                        }
                        else
                        {
                            if (TargetingBuilder == null)
                            {
                                TargetingBuilder = new TargetingandMessaging(fbclient, keywork_txt.AutoCompleteCustomSource.Cast<string>().ToList<string>(), Campaign.Large);
                                TargetingBuilder.SetController(metroProgressBar1, TargetUserCounts).Start();
                            }
                            else
                            {
                                TargetingBuilder.operationalThread = new Thread(TargetingBuilder.ExtractTargetUser);
                                TargetingBuilder.SetKeywords(keywork_txt.AutoCompleteCustomSource.Cast<string>().ToList<string>());
                                TargetingBuilder.ChangeCampaign(Campaign.Large);
                                TargetingBuilder.SetController(metroProgressBar1, TargetUserCounts).Start();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Feed me more keywords", "Keywords List Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ThreadStateException ex)
            {
                MessageBox.Show("Threading Error, Contact me ASAP please..." + Environment.NewLine + ex.Message,
                    "Threading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("UnExpected Error, Contact me ASAP please..." + Environment.NewLine + ex.Message,
                    "UnExpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
        }

        private void Stop_target_Click(object sender, EventArgs e)
        {
            try
            {
                if (TargetingBuilder != null)
                {
                    try
                    {
                        TargetingBuilder.SetController(metroProgressBar1, TargetUserCounts).Abort();
                    }
                    catch (ThreadStateException ex)
                    {
                        MessageBox.Show("Threading Error, Contact me ASAP please..." + Environment.NewLine + ex.Message,
                            "Threading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("UnExpected Error, Contact me ASAP please..." + Environment.NewLine + ex.Message,
                    "UnExpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopFire_btn_Click(object sender, EventArgs e)
        {
            if (TargetingBuilder != null)
            {
                TargetingBuilder.StopMessageingFire();
            }
            else
            {
                MessageBox.Show("Did you Start Targeting some users !!.", "Target Users not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartFire_btn_Click(object sender, EventArgs e)
        {
            if (MessagesContect.Messages.Count > 0)
            {
                if (TargetingBuilder != null)
                {
                    if (int.Parse(TargetUserCounts.Text) > 0)
                    {
                        TargetingBuilder.IntaiteGridView(SucsessGrid, errorGrid, sucsessGridBindingSource, filedGridBindingSource, MessegesSentCount);
                        TargetingBuilder.MessageingFire(MessagesContect.Messages,realLogin);
                    }
                    else
                    {
                        MessageBox.Show("Did you Start Targeting some users !!.", "Target Users not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Did you Start Targeting some users !!.", "Target Users not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please Upload Some Messgae Files", "Message Files Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
