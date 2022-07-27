using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.DomainModels
{
    public class EmailMessage : BaseEntity
    {
        public string mailTo { get; set; }
        public string subject { get; set; }
        public string content { get; set; }
        public bool status { get; set; }

    }
}
