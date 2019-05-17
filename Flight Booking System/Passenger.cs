using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingSystem
{
    [Serializable]
    class Passenger
    {
        string name, password, address, passportNumber;
        List<Eticket> BookingList;
        CreditCard credit;
        public Passenger(string name,string address,string password,string passport,CreditCard credit)
        {
            this.name = name;
            this.address = address;
            this.passportNumber = passport;
            this.password = password;
            BookingList = new List<Eticket>();
            this.credit = credit;
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string PassportNumber
        {
            get
            {
                return passportNumber;
            }
            set
            {
                passportNumber = value;
            }
        }
        public bool VerifyPassword(string ent)
        {
            return password.Equals(ent,StringComparison.OrdinalIgnoreCase);
        }
        public void ChangePassword(string ent)
        {
            password = ent;
        }

        public void ViewPassengerBookings()
        {
            if (BookingList.Count == 0)
            {
                Console.WriteLine("The passenger has no bookings");
            }else
            {
                int counter = 1;
                foreach(Eticket et in BookingList)
                {
                    Console.WriteLine("#" + counter+":");
                    Console.WriteLine(et);
                    Console.WriteLine();
                    counter++;
                }
            }
        }
        public bool MakeFlightBooking(int flightNumber,FlightSeatClass seat)
        {
            Flight BookIn = (Flight)FileHandler.Find(ObjectChoices.Flight,flightNumber.ToString());
            if (BookIn == null) return false;//write response
            if (credit.getBalance() < BookIn.getTeckitPrice(seat)) return false;//
            Eticket et = BookIn.BookSeat(this,seat);
            if (et == null) return false;//
            credit.Withdraw(BookIn.getTeckitPrice(seat));
            BookingList.Add(et);
            FileHandler.Add(ObjectChoices.Flight, BookIn);
            return true;
        }

        public bool CancelFlightBooking(string ticketCode)
        {
            Eticket found = null;
            foreach(Eticket et in BookingList)
            {
                if (et.Code == ticketCode)
                {
                    found = et;
                    break;
                }
            }
            if (found == null) return false;
            Flight associatedFlight = (Flight)FileHandler.Find(ObjectChoices.Flight, found.FlightNumber.ToString());
            associatedFlight.EraseBooking(found);
            BookingList.Remove(found);
            FileHandler.Add(ObjectChoices.Flight, associatedFlight);
            return true;
        }
    }
}
