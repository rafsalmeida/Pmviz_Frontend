using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PmvizFrontend.Models
{
    [DataContract]
    public class Log
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public string Filename { get; set; }
        [DataMember]
        public int NumberActivities { get; set; }
        [DataMember]
        public int NumberCases { get; set; }
        [DataMember]
        public string StartDate { get; set; }
    }
}
