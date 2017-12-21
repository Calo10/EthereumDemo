using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EthereumWeb.Models
{
    public class DataPointModel
    {
       
            public DataPointModel(double x, double y,string legendText,string indexLabel)
            {
                this.x = x;
                this.y = y;
                this.legendText = legendText;
                this.indexLabel = indexLabel;
        }

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "x")]
            public Nullable<double> x = null;

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public Nullable<double> y = null;
        public string legendText { get; set; }
        public string indexLabel { get; set; }

    }
}