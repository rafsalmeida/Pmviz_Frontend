using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]

    public class Resource
    {
        [DataMember]
        public string Name { get; set; }
    }
}
