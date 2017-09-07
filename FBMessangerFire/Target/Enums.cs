using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBMessangerFire.Target
{
    public enum UpdateGridOptions
    {
        Sucsess,
        Failed
    }
    public enum CampaignSlots
    {
        Small = 500,
        Mid = 1000,
        Large = 3000
    }
    public enum MaxDelayOptions
    {
        Small = 15000,
        Mid = 20000,
        Large = 30000
    }
    public enum MinDelayOptions
    {
        Small = 10000,
        Mid = 15000,
        Large = 20000
    }
    public enum Campaign
    {
        Small,
        Mid,
        Large
    }
}
