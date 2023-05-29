using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletService.Models.Requests
{
    public class ChargeRequest
    {
       
            [JsonProperty("userId")]
            public int UserId;

            [JsonProperty("amount")]
            public double Amount;
        
    }
}
