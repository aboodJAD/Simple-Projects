using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingSystem
{
    class PassengerInterface:IUserInterface
    {
        readonly string PassengerList = "Please, choose one of the following services :\n" +
                "1- Book for a flight\n" +
                "2- Cancle a flight's booking\n" +
                "3- View my bookings\n" +
                "4- Logout";
        Passenger passenger;

        public PassengerInterface()
        {
            passenger = null;
        }
        public void login()
        {
            while (true)
            {
                string name, password;
                Console.WriteLine("Please, enter your name (-1 to go back): ");
                name = Console.ReadLine();
                if (name == "-1") return;
                Console.WriteLine("Please, enter your password number (-1 to go back): ");
                password = Console.ReadLine();
                if (password == "-1") return;
                passenger = (Passenger)FileHandler.FindUser(ObjectChoices.Passenger, name, password);
                if (passenger == null)
                {
                    Console.WriteLine("Wrong user name or password");
                }else
                {
                    interact();
                    return;
                }
            }
        }
        public void register()
        {
            string name, password, passport, address;
            double balance;
            int day, month, year;
            string number;
            Console.WriteLine("Please, enter your name (-1 to go back): ");
            name = Console.ReadLine();
            if (name == "-1") return;
            Console.WriteLine("Please, enter your address (-1 to go back): ");
            address = Console.ReadLine();
            if (address == "-1") return;

            Console.WriteLine("Please, enter your passport number (-1 to go back): ");
            passport = Console.ReadLine();
            if (passport == "-1") return;
            if (FileHandler.Find(ObjectChoices.Passenger, passport) != null)
            {
                Console.WriteLine("You have already an account in the system" +
                    " please call us to help you restore your account");
                return;
            }
            Console.WriteLine("Please, enter your password number (-1 to go back): ");
            password = Console.ReadLine();
            if (password == "-1") return;
            Console.WriteLine("Please, enter your credit card information : ");
            while (true)
            {
                Console.WriteLine("Balance (-1 to go back): ");
                string tr = Console.ReadLine();
                if (tr == "-1") return;
                if (!double.TryParse(tr, out balance))
                {
                    Console.WriteLine("Wrong value,enter again..");
                }
                else break;
            }
            while (true)
            {
                Console.WriteLine("enter the credit card information : ");
                Console.WriteLine("the expiry date\nday (-1 to exit): ");
                string tr = Console.ReadLine();
                if (tr == "-1") return;
                if (!int.TryParse(tr, out day) || day < 1 || day > 30)
                {
                    Console.WriteLine("Wrong value,enter again..");
                }
                else break;
            }
            while (true)
            {
                Console.WriteLine("Please, enter month (1-12) (-1 to exit): ");
                string tr = Console.ReadLine();
                if (tr == "-1") return;
                if (!int.TryParse(tr, out month) || month < 1 || month > 12)
                {
                    Console.WriteLine("Wrong value,enter again..");
                }
                else break;
            }
            while (true)
            {
                Console.WriteLine("Please, enter year: ");
                string tr = Console.ReadLine();
                if (tr == "-1") return;
                if (!int.TryParse(tr, out year))
                {
                    Console.WriteLine("Wrong value,enter again..");
                }
                else break;
            }
            Console.WriteLine("Please, enter the credit number : ");
            number = Console.ReadLine();
            CreditCard credit = new CreditCard(new DateTime(year, month, day), number, balance);
            passenger = new Passenger(name, address, password, passport, credit);
            FileHandler.Add(ObjectChoices.Passenger, passenger);
            interact();
        }
        public void interact()
        {
            while (true)
            {
                Console.WriteLine(PassengerList);
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        bool yes = false;
                        string origin, dest, travelClass, flightType;
                        DateTime dept = new DateTime();
                        DateTime ret = new DateTime();
                        string day, month, year;
                        FlightType p1 = FlightType.OneWay;
                        FlightSeatClass p2 = FlightSeatClass.Economy;
                        Console.WriteLine("Please, enter the origin city : ");
                        origin = Console.ReadLine();
                        Console.WriteLine("Please, enter the destination city : ");
                        dest = Console.ReadLine();
                        Console.WriteLine("Please, enter the travel class : \n" +
                            "(1 : First class) (2 : Economy class) (3 : Buinsness class)");
                        travelClass = Console.ReadLine();
                        if (!(SystemHandler.checkInt(travelClass) && int.Parse(travelClass) >= 1 && int.Parse(travelClass) <= 3)) goto label;
                        Console.WriteLine("Please, enter the flight type : \n" +
                            "(1 : one way) (2 : return)");
                        flightType = Console.ReadLine();
                        Console.WriteLine("Please, enter the departur date : ");
                        Console.WriteLine("day : ");
                        day = Console.ReadLine();
                        Console.WriteLine("month : ");
                        month = Console.ReadLine();
                        Console.WriteLine("year : ");
                        year = Console.ReadLine();
                        if(SystemHandler.checkDate(day,month,year))
                            dept = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                        else
                        {
                            goto label;
                        }
                        if (flightType == "2")
                        {
                            Console.WriteLine("Please, enter the return date : ");
                            Console.WriteLine("day : ");
                            day = Console.ReadLine();
                            Console.WriteLine("month : ");
                            month = Console.ReadLine();
                            Console.WriteLine("year : ");
                            year = Console.ReadLine();
                            if (SystemHandler.checkDate(day, month, year))
                            {
                                ret = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                                if (ret <= dept)
                                {
                                    goto label;
                                }
                            }
                            else
                            {
                                goto label;
                            }                           
                        }
                        if (flightType == "1") p1 = FlightType.OneWay;
                        else if (flightType == "2") p1 = FlightType.Return;
                        else  goto label; 
                        
                        if (travelClass == "1") p2 = FlightSeatClass.First;
                        else if (travelClass == "2") p2 = FlightSeatClass.Economy;
                        else if (travelClass == "3") p2 = FlightSeatClass.Buisness;
                        else goto label; 
                        if(SystemHandler.viewMatchedFlight(origin, dest,p2, p1, dept, ret))
                            yes = true;
                        label:
                        if (yes)
                        {
                            Console.WriteLine("Please, enter the number of the flight you want to book in : ");
                            string number = Console.ReadLine();
                            
                            if (!passenger.MakeFlightBooking(int.Parse(number), p2))
                            {
                                Console.WriteLine("wrong choice for flight number or you don't have enough money to book");
                            }
                        }
                        break;
                    case "2":
                        passenger.ViewPassengerBookings();
                        Console.WriteLine("Please, choose the teckit code you want to cancel");
                        string code=Console.ReadLine();
                        if (!passenger.CancelFlightBooking(code))
                        {
                            Console.WriteLine("No teckit with such a code");
                        }
                        break;
                    case "3":
                        passenger.ViewPassengerBookings();
                        break;
                    case "4":
                        FileHandler.Add(ObjectChoices.Passenger, passenger);
                        return;
                    default:
                        break;
                }
            }
        }
    }
}
