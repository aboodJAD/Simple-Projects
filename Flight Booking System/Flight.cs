using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingSystem
{
    enum FlightSatus
    {
        Scheduled, Canclled, Arrived
    }
    enum FlightType
    {
        OneWay, Return
    }

    enum FlightSeatClass
    {
        First,Economy,Buisness
    }

    [Serializable]
    class Flight
    {
        int flightNumber,
            availSeatFirstClass, availSeatBuisnessClass,
            availSeatEconomyClass,
            bookedEconomyClass=0, bookedFirstClass = 0, 
            bookedBuisnessClass = 0;

        DateTime departueDate, retDate;
        string origin,destination;
        FlightSatus flightState;
        double baseFlightFare;
        FlightType type;
        List<Eticket> flightBookings;

        public Flight(string org,string dest,FlightSatus state,
            double baseFare,
            int first,int buisness,int economy,
            FlightType type,
            DateTime dept,DateTime ret)
        {
            flightNumber = int.Parse(DateTime.Now.Day+""+DateTime.Now.Month+""+DateTime.Now.Hour+""+DateTime.Now.Second);
            origin = org;
            destination = dest;
            flightState = state;
            baseFlightFare = baseFare;
            availSeatFirstClass = first;
            availSeatBuisnessClass = buisness;
            availSeatEconomyClass = economy;
            this.type = type;
            departueDate = dept;
            retDate = ret;
            flightBookings = new List<Eticket>();
            Console.WriteLine("Flight number is : " + flightNumber);
        }
        public int FlightNumber
        {
            get
            {
                return flightNumber;
            }
        }
        public int EconomyClassSeat
        {
            get
            {
                return availSeatEconomyClass;
            }
            set
            {
                availSeatEconomyClass = value;
            }
        }
        public int FirstClassSeat
        {
            get
            {
                return availSeatFirstClass;
            }
            set
            {
                availSeatFirstClass = value;
            }
        }
        public int BuisnessClassSeat
        {
            get
            {
                return availSeatBuisnessClass;
            }
            set
            {
                availSeatBuisnessClass = value;
            }
        }
        public DateTime DepartureDate
        {
            get
            {
                return departueDate;
            }
            set
            {
                if (value >= retDate) {
                    Console.WriteLine("The departure date should not come after the return date");
                    return;
                }
                departueDate = value;
            }
        }
        public DateTime ReturnDate
        {
            get
            {
                return retDate;
            }
            set
            {
                if (departueDate >= value)
                {
                    Console.WriteLine("The departure date should not come after the return date");
                    return;
                }
                retDate = value;
            }
        }
        public double BaseFlightFare
        {
            get
            {
                return baseFlightFare;
            }
            set
            {
                if (value < 0) return;
                baseFlightFare = value;
            }
        }
        public string OriginCity
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
            }
        }

        public string DestinationCity
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
            }
        }
        public FlightSatus FlightState
        {
            get
            {
                return flightState;
            }
            set
            {
                flightState = value;
            }
        }
        public FlightType Type{
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public List<Eticket> getFlightBookings()
        {
            return flightBookings;
        }
        public List<Eticket> getFlightBookings(FlightSeatClass seat)
        {
            List<Eticket> et=new List<Eticket>();
            foreach(Eticket item in flightBookings)
            {
                if (item.Code[0] == 'E' && seat == FlightSeatClass.Economy)
                    et.Add(item);
                if (item.Code[0] == 'F' && seat == FlightSeatClass.First)
                    et.Add(item);
                if (item.Code[0] == 'B' && seat == FlightSeatClass.Buisness)
                    et.Add(item);
            }
            return et;
        }

        public int getNumOfBookedSeats(FlightSeatClass fst)
        {
            
            if (fst == FlightSeatClass.Buisness) return bookedBuisnessClass;
            if (fst == FlightSeatClass.Economy) return bookedEconomyClass;
            return bookedFirstClass;
        }
        public bool setDate(DateTime dept,DateTime ret)
        {
            if (dept >= ret) return false;
            departueDate = dept;
            retDate = ret;
            return true;
        }

        public double getTeckitPrice(FlightSeatClass fbb)
        {
            bool isInOccasionDate = SystemHandler.IsInOccasions(DepartureDate);
            double price=baseFlightFare;
            if (fbb == FlightSeatClass.First)
            {
                price += 100;
                if (isInOccasionDate) return price - price * 10 / 100.0;
                return price;
            }

            if (fbb == FlightSeatClass.Buisness)
            {
                price += 50;
                if (isInOccasionDate) return price - price * 10 / 100.0;
                return price;
            }
            if (isInOccasionDate) return price - price * 10 / 100.0;
            return price;
        }

        public Eticket BookSeat(Passenger pass, FlightSeatClass type)
        {
            if (DateTime.Now > departueDate) FlightState = FlightSatus.Arrived;
            if (FlightState == FlightSatus.Arrived || FlightState == FlightSatus.Canclled) return null;
            char tp=' ';
            if (type == FlightSeatClass.First && bookedFirstClass < availSeatFirstClass)
            {
                tp = 'F';
                bookedFirstClass++;
            }
            else if (type == FlightSeatClass.Economy && bookedEconomyClass < availSeatEconomyClass)
            {
                tp = 'E';
                bookedEconomyClass++;
            }
            else if (type == FlightSeatClass.Buisness && bookedBuisnessClass < availSeatBuisnessClass)
            {
                tp = 'B';
                bookedBuisnessClass++;
            }
            else return null;
            Eticket et= new Eticket(pass.Name, pass.PassportNumber, flightNumber, getTeckitPrice(type), tp);
            flightBookings.Add(et);
            //Console.WriteLine("Congrats ! Succesfull booking..");
            return et;
        }
        public void EraseBooking(Eticket et)
        {
            Eticket rem=null;
            foreach(Eticket at in flightBookings)
            {
                if (at.Equals(et))
                {
                    if (at.Code[0] == 'E') bookedEconomyClass--;
                    else if(at.Code[0]=='B') bookedBuisnessClass--;
                    else bookedFirstClass--;
                    rem = at;
                    break;
                }
            }
            if (rem == null) return;
            flightBookings.Remove(rem);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            return ((Flight)obj).flightNumber == flightNumber;
        }
        public override int GetHashCode()
        {
            return flightNumber;
        }
        public override string ToString()
        {
            string ret =
                "Flight number : " + flightNumber + "\n" +
                "origin city : " + origin + "\n" +
                "Destination : " + destination + "\n" +
                "Departure Date : " + departueDate.ToShortDateString() + "\n"+
                "Flight status : "+FlightState.ToString()+"\n";
            if (retDate != new DateTime()) ret += "Return Date : " + retDate.ToShortDateString()+"\n";
            ret += "Base Flight Fare : " + baseFlightFare;
            return ret;
        }
    }
}
