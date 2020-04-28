using Pmviz_Frontend.Models.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class Statistic
    {
        [DataMember]
        public List<Nodes> Nodes { get; set; }

        [DataMember]
        public List<Relations> Relations { get; set; }

        [DataMember]
        public string Info { get; set; }
    }
}
