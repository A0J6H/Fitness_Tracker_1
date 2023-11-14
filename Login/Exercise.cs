using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    internal class Exercise
    {
        string owner_id { get; set; }
        public string etype { get; set; }
        public string cals { get; set; }
        public string duration { get; set; }
        public string date_ {  get; set; }
        string description { get; set; }

        public Exercise(string Owner, string Etype, string Cals, string Duration, string Date_, string Description)
        {
            this.owner_id = Owner;
            this.etype = Etype;
            this.cals = Cals;
            this.duration = Duration;
            this.date_ = Date_;
            this.description = Description;
        }
    }
}
