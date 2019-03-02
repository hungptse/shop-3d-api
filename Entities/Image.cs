using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ShopAPI.Entities
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public partial class Image
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Url { get; set; }
        [JsonIgnore]
        public int? ProId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Product Pro { get; set; }
    }
}
