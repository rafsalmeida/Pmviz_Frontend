using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class Event
    {
        [DataMember]
        [JsonProperty("Id")]
        public long Id { get; set; }

        [DataMember]
        [JsonProperty("CaseId")]
        public long CaseId { get; set; }

        [DataMember]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty("Start")]
        public DateTime Start { get; set; }

        [DataMember]
        [JsonProperty("End")]
        public DateTime End { get; set; }

        [DataMember]
        [JsonProperty("Duration")]
        public long Duration { get; set; }

        [DataMember]
        [JsonProperty("Resource")]
        public string Resource { get; set; }

        [DataMember]
        [JsonProperty("Role")]
        public string Role { get; set; }

        [DataMember]
        [JsonProperty("Id_log")]
        public long Id_log { get; set; }

        [DataMember]
        [JsonProperty("Items")]
        public List<Event> Items { get; set; }

        public Event(long Id, long CaseId, string Name, DateTime Start, DateTime End, long Duration, string Resource, string Role, long Id_log)
        {
            this.Id = Id;
            this.CaseId = CaseId;
            this.Name = Name;
            this.Start = Start;
            this.End = End;
            this.Duration = Duration;
            this.Resource = Resource;
            this.Role = Role;
            this.Id_log = Id_log;
            this.Items = new List<Event>();
        }

        public static List<Event> Diagram()
        {
            List<Event> result = new List<Event>();
            DateTime date1 = new DateTime(2020, 03, 31, 00, 00, 00);
            DateTime date2 = new DateTime(2020, 03, 31, 08, 30, 00);
            Event event1 = new Event(1, 1, "Desbaste", date1, date2, 30, "x", "x", 1);
            result.Add(event1);

            DateTime date3 = new DateTime(2020, 03, 31, 11, 00, 00);
            Event event2 = new Event(1, 2, "Furação", date2, date3, 30, "x", "x", 1);
            event1.Items.Add(event2);

            DateTime date4 = new DateTime(2020, 03, 31, 14, 00, 00);
            Event event3 = new Event(1, 3, "Maquinação", date3, date4, 30, "x", "x", 1);
            event2.Items.Add(event3);

            DateTime date5 = new DateTime(2020, 03, 31, 18, 00, 00);
            Event event4 = new Event(1, 4, "Testes", date4, date5, 30, "x", "x", 1);
            event3.Items.Add(event4);

            return result;
        }
    }
}
