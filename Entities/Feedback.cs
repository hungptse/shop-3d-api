using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ShopAPI.Entities
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public partial class Feedback
    {
        public int Id { get; set; }
        public int? Rate { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public int? ProId { get; set; }
        public bool isApprove { get; set; }
        public DateTime? PostedTime { get; set; }

        public Product Pro { get; set; }
        //[JsonIgnore]
        //[IgnoreDataMember]
        public Account Acc { get; set; }
    }
}
