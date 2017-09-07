using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FBMessangerFire
{
    public class MessageContent
    {
        public MessageContent()
        {
            Messages = new List<string>();
        }
        public List<string> Messages { get; set; }
        public async static Task PrepateMessages(MessageContent messages, string[] files)
        {
            Parallel.ForEach(files, file =>
            {
                FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string full = sr.ReadToEnd();
                if (full.Contains("//end message//"))
                    messages.Messages.AddRange(full.Split(new string[] { "//end message//" }, StringSplitOptions.RemoveEmptyEntries).ToList());
                else
                {
                    messages.Messages.Add(full);
                }
            });
        }
    }
}
