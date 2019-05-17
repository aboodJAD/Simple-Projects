using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingSystem
{
    [Serializable]
    class Eticket
    {
        string passengerName, passportNumber,code;
        double flightFare;
        int flightNumber;

        public Eticket(string passengerName, string passportNumber,int flightNumber, double flightFare,char classType)
        {
            this.passengerName = passengerName;
            this.passportNumber = passportNumber;
            this.flightFare = flightFare;
            this.flightNumber = flightNumber;
            code = classType+"" +flightNumber+ generateCode();
        }
        private string generateCode()
        {
            string res=passportNumber+DateTime.Now.Millisecond;
            return res;
        }
        public string Code
        {
            get
            {
                return code;
            }
        }
        public int FlightNumber
        {
            get
            {
                return flightNumber;
            }
        }
        public double FlightFare
        {
            get
            {
                return flightFare;
            }
        }
        public String PassengerName
        {
            get
            {
                return passengerName;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            Eticket con = (Eticket)obj;
            return code.Equals(con.Code);
        }
        public override int GetHashCode()
        {
            int hash=code[0]-'A';
            for(int i = 1; i < code.Length; i++)
            {
                hash = hash +26*(code[i] - '0');
                hash %= 1234567;
            }
            return hash;
        }
        public override string ToString()
        {
            string ret= "Ticket number : "+code+"\n"
                +"Passenger name : "+passengerName+"\n"
                +"Passenger Passport Number : "+passportNumber+"\n"
                +"Ticket fare = "+flightFare+"\n"+
                "Class : ";
            if (code[0] == 'E') ret += "Economy class";
            else if (code[0] == 'F') ret += "First class";
            else ret += "Buisness class";
            ret += "\n";
            ret += ((Flight)FileHandler.Find(ObjectChoices.Flight,flightNumber.ToString())).ToString();
            return ret;
        }
    }
}
