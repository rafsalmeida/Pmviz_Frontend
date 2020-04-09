﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pmviz_Frontend.Models
{
    [DataContract]
    public class ActivityFreq
    {
        [DataMember]
        public int Frequency { get; set; }

        [DataMember]
        public Duration MeanDuration { get; set; }

        [DataMember]
        public Duration MedianDuration { get; set; }

        [DataMember]
        public Duration MinDuration { get; set; }

        [DataMember]
        public Duration MaxDuration { get; set; }

        [DataMember]
        public string Activity { get; set; }

        [DataMember]
        public decimal RelativeFrequency { get; set; }
        [DataMember]
        public string MeanActivityFormatted { get; set; }

        [DataMember]
        public string MedianActivityFormatted { get; set; }

        [DataMember]
        public string MinActivityFormatted { get; set; }

        [DataMember]
        public string MaxActivityFormatted { get; set; }
    }
}
