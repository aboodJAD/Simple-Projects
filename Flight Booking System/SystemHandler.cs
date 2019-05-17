using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingSystem
{
    class SystemHandler
    {
        public static List<DateTime> OccasionDates;
        static SystemHandler()
        {
            OccasionDates = new List<DateTime>();
            List<Object> list = FileHandler.GetAllObj(ObjectChoices.Occasions);
            foreach(Object date in list)
            {
                OccasionDates.Add((DateTime)date);
            }
        }
        public static bool checkInt(string number)
        {
            int res;
            if (int.TryParse(number, out res)) return true;
            return false;
        }
        public static bool checkDouble(string number)
        {
            double res;
            if (double.TryParse(number, out res)) return true;
            return false;
        }
        public static bool checkDate(string day,string month,string year)
        {
            int dd,mm,yy;
            if(!int.TryParse(day,out dd)|| !int.TryParse(month, out mm)|| !int.TryParse(year, out yy))
            {
                return false;
            }
            if (dd > 31 || dd < 1 || mm > 12 || mm < 1) return false;
            return true;
        }


        public static bool viewMatchedFlight(string origin, string dest, FlightSeatClass seat,
            FlightType type, DateTime dept, DateTime ret)
        {
            int counter = 1;
            List<Object> allFlight = FileHandler.GetAllObj(ObjectChoices.Flight);
            foreach (Object obj in allFlight)
            {
                Flight me = (Flight)obj;
                if (me.FlightState == FlightSatus.Scheduled && me.OriginCity.Equals( origin,StringComparison.OrdinalIgnoreCase) 
                    && me.DestinationCity.Equals(dest,StringComparison.OrdinalIgnoreCase)
                    && me.DepartureDate == dept && me.Type == type)
                {
                    if (type == FlightType.Return && me.ReturnDate != ret) continue;
                    if (seat == FlightSeatClass.Buisness && me.BuisnessClassSeat == me.getNumOfBookedSeats(seat)) continue;
                    if (seat == FlightSeatClass.First && me.FirstClassSeat == me.getNumOfBookedSeats(seat)) continue;
                    if (seat == FlightSeatClass.Economy && me.EconomyClassSeat == me.getNumOfBookedSeats(seat)) continue;
                    Console.WriteLine("#" + counter + " : ");
                    Console.WriteLine(me);
                    Console.WriteLine();
                    counter++;
                }
            }
            if (counter == 1)
            {
                Console.WriteLine("No matched flights");
                return false;
            }
            return true;
        }
        public static void ViewFlightBooking(int flightNumber)
        {
            try
            {
                Flight flight = (Flight)FileHandler.Find(ObjectChoices.Flight, flightNumber.ToString());
                List<Eticket> list = flight.getFlightBookings();
                int counter = 0;
                foreach (Eticket et in list)
                {
                    Console.WriteLine("Ticket #" + (++counter) + ":");
                    Console.WriteLine(et);
                    Console.WriteLine();
                }
                if (counter == 0)
                    Console.WriteLine("no bookings");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("Flight not Found");
            }
        }
        public static void ViewFlightBookings(int flightNumber, FlightSeatClass seat)
        {
            try
            {
                Flight flight = (Flight)FileHandler.Find(ObjectChoices.Flight, flightNumber.ToString());
                List<Eticket> list = flight.getFlightBookings(seat);
                int counter = 0;
                foreach (Eticket et in list)
                {
                    Console.WriteLine("Booking #" + (++counter) + " : ");
                    Console.WriteLine(et);
                    Console.WriteLine();
                }
                if(counter==0)
                   Console.WriteLine("no bookings");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("Flight not Found");
            }
        }
        public static void ViewAllFlights()
        {
            List<Object> all = FileHandler.GetAllObj(ObjectChoices.Flight);
            int counter = 0;
            foreach (Object obj in all)
            {
                if (DateTime.Now > ((Flight)obj).DepartureDate)
                {
                    ((Flight)obj).FlightState = FlightSatus.Arrived;
                    FileHandler.Add(ObjectChoices.Flight, obj);
                }
                Console.WriteLine("#" + (++counter) + " : ");
                Console.WriteLine((Flight)obj);
                Console.WriteLine();
            }
            if (counter == 0) Console.WriteLine("no flights ");
        }

        public static void ViewAllFlights(FlightSatus state)
        {
            List<Object> all = FileHandler.GetAllObj(ObjectChoices.Flight);
            int counter = 0;
            foreach (Object obj in all)
            {
                if (DateTime.Now > ((Flight)obj).DepartureDate)
                {
                    ((Flight)obj).FlightState = FlightSatus.Arrived;
                    FileHandler.Add(ObjectChoices.Flight,obj);
                }
                if (((Flight)obj).FlightState == state)
                {
                    Console.WriteLine("#" + (++counter) + " : ");
                    Console.WriteLine((Flight)obj);
                    Console.WriteLine();
                }
            }
            if (counter == 0) Console.WriteLine("no flights ");
        }
        public static void ViewStatistics(DateTime strt,DateTime end)
        {
            double totalRevenu = 0;
            int scheduledFlight=0;
            int totalBookings=0;
            List<Object> all = FileHandler.GetAllObj(ObjectChoices.Flight);
            foreach(Object obj in all)
            {
                Flight now = (Flight)obj;
                if (now.DepartureDate <= end && now.DepartureDate >= strt)
                {
                    if (now.FlightState == FlightSatus.Scheduled) scheduledFlight++;
                    List<Eticket> list = now.getFlightBookings();
                    totalBookings += list.Count;
                    foreach(Eticket et in list)
                    {
                        totalRevenu += et.FlightFare;
                    }
                }
            }
            Console.WriteLine("Total Revenue = " + totalRevenu);
            Console.WriteLine("Total Bookings = " + totalBookings);
            Console.WriteLine("Number of flight scheduled = " + scheduledFlight);
        }

        public static bool IsInOccasions(DateTime dt)
        {
            
            foreach(DateTime date in OccasionDates)
            {
                if (dt == date) return true;
            }
            return false;
        }
    }
}
