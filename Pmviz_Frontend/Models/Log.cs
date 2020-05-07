using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class Log
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public string Filename { get; set; }
        [DataMember]
        public int NumberOfActivities { get; set; }
        [DataMember]
        public int NumberOfCases { get; set; }
        [DataMember]
        public string StartDate { get; set; }

        public List<Log> childs { get; set; }

        /*public Log(int Id, string Description, string EndDate, string FileName, int NumberActivities, int NumberCases, string StartDate)
        {
            this.Id = Id;
            this.Description = Description;
            this.EndDate = EndDate;
            this.Filename = Filename;
            this.NumberActivities = NumberActivities;
            this.NumberCases = NumberCases;
            this.StartDate = StartDate;
        }*/
    }
}
