using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class Relation
    {
        [DataMember]
        public int From { get; set; }

        [DataMember]
        public List<int> To { get; set; }
    }
}
