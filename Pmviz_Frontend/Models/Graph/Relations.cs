using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models.Graph
{
    [DataContract]
    public class Relations
    {
        [DataMember]
        public int From { get; set; }

        [DataMember]
        public List<Nodes> To { get; set; }
    }
}
