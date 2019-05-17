using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingSystem
{
    [Serializable]
    class Admin
    {
        string name;
        int ID;
        public Admin(string name)
        {
            ID = int.Parse(DateTime.Now.Day+""+ DateTime.Now.Minute+""+DateTime.Now.Second);
            this.name = name;
        }

        public int  getId() { return ID; }
        public string getName() { return name; }
    }
}
