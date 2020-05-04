using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class Activity
    {

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
