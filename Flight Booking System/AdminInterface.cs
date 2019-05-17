using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingSystem
{
    class AdminInterface:IUserInterface
    {
        readonly string speacialPasswordAdmin = "12345678";
        readonly string AdminList = "Please, choose one of the following services :\n" +
                "1- Introduce a new flight\n" +
                "2- View Flight's Bookings\n" +
                "3- View all flights\n"+
                "4- View flights for a specified status\n" +
                "5- View Transaction report\n" +
                "6- View all Economy bookings for specified flight\n" +
                "7- View information for specified flight\n" +
                "8- Remove existing flight information\n" +
                "9- Define occasion dates\n"+
                "10- modify flight information\n"+
                "11- Logout";

        Admin admin;
        public AdminInterface()
        {
            admin = null;

        }
        public void login()
        {
            while (true)
            {
                string userName, id;
                Console.WriteLine("Please, enter your username (-1 to go back): ");
                userName = Console.ReadLine();
                if (userName == "-1") return;

                Console.WriteLine("Please, enter your ID Number (-1 to go back): ");
                id = Console.ReadLine();
                if (id == "-1") return;
                admin = (Admin)FileHandler.Find(ObjectChoices.Admin, userName + id);
                if (admin == null)
                {
                    Console.WriteLine("Wrong username or id");
                }
                else {
                    interact();
                    return;
                }
            }
        }
        public void register()
        {
            string name, password;
            Console.WriteLine("Please, enter your username (-1 to go back): ");
            name = Console.ReadLine();
            if (name == "-1") return;
            Console.WriteLine("Please, enter password (-1 to go back): ");
            password = Console.ReadLine();
            if (password == "-1") return;
            if (password.Equals(speacialPasswordAdmin))
            {
                admin = new Admin(name);
                Console.WriteLine("your id is " + admin.getId());
                FileHandler.Add(ObjectChoices.Admin, admin);
                interact();
            }
            else
            {
                Console.WriteLine("You entered wrong password, please check the contract");
            }
        }
        public void interact()
        {
            while (true)
            {
                Console.WriteLine(AdminList);
                string choice = Console.ReadLine();
                DateTime strt = new DateTime();
                DateTime end = new DateTime();
                bool yes = false;
                string origin, dest, flightType;
                DateTime dept = new DateTime();
                DateTime ret = new DateTime();
                string fare,tp;
                string firstNum = "", economyNum = "", buisnessNum = "";
                FlightType type = FlightType.OneWay;
                double baseFare = 0;
                int temp;
                string flightNumber;
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("enter the origin city : ");
                        origin = Console.ReadLine();

                        Console.WriteLine("enter the destination city : ");
                        dest = Console.ReadLine();

                        Console.WriteLine("enter the flight type (1 for one way, 2 for return) : ");
                        flightType = Console.ReadLine();
                        if (flightType == "1") type = FlightType.OneWay;
                        else if (flightType == "2") type = FlightType.Return;
                        else  goto label;
                        Console.WriteLine("Please, enter the departure date : ");
                        if (!readDate(out dept))
                            goto label;

                        if (flightType == "2")
                        {
                            Console.WriteLine("Please, enter the return date : ");
                            if(!readDate(out ret))
                            {
                                if (ret <= dept) goto label;
                            }
                        }

                        Console.WriteLine("enter the flight base fare : ");
                        fare = Console.ReadLine();
                        if (double.TryParse(fare, out baseFare)) baseFare = double.Parse(fare);
                        else goto label;

                        Console.WriteLine("please enter the number of seats for the first class : ");
                        firstNum = Console.ReadLine();
                        if (!int.TryParse(firstNum, out temp)) goto label;

                        Console.WriteLine("please enter the number of seats for the economy class : ");
                        economyNum = Console.ReadLine();
                        if (!int.TryParse(economyNum, out temp)) goto label;

                        Console.WriteLine("please enter the number of seats for the buisness class : ");
                        buisnessNum = Console.ReadLine();
                        if (!int.TryParse(buisnessNum, out temp)) goto label;
                        yes = true;
                        label:
                        if (yes)
                        {
                            Flight neFlight = new Flight(origin, dest, FlightSatus.Scheduled, baseFare,
                                int.Parse(firstNum), int.Parse(buisnessNum), int.Parse(economyNum),
                                type, dept, ret);
                            FileHandler.Add(ObjectChoices.Flight,neFlight);
                            Console.WriteLine("the flight successfully added");
                        }else
                        {
                            Console.WriteLine("Wrong input for one of the fields");
                        }
                        break;
                    case "2":
                        Console.WriteLine("please enter the flight number:");
                        flightNumber=Console.ReadLine();
                        if (SystemHandler.checkInt(flightNumber))
                            SystemHandler.ViewFlightBooking(int.Parse(flightNumber));
                        break;
                    case "3":
                        SystemHandler.ViewAllFlights();
                        break;
                    case "4":
                        Console.WriteLine("please enter the status you want to view\n"+
                            "(1 for arrived, 2 for cancelled, 3 for scheduled): ");
                        tp = Console.ReadLine();
                        if (SystemHandler.checkInt(tp))
                        {
                            int con = int.Parse(tp);
                            if (con == 1)
                            {
                                SystemHandler.ViewAllFlights(FlightSatus.Arrived);
                            }
                            else if (con == 2)
                            {
                                SystemHandler.ViewAllFlights(FlightSatus.Canclled);
                            }
                            else if (con == 3)
                            {
                                SystemHandler.ViewAllFlights(FlightSatus.Scheduled);
                            }
                            else Console.WriteLine("wrong input");
                        }else Console.WriteLine("wrong input");
                        break;
                    case "5":
                       yes = false;
                        Console.WriteLine("Please enter the start date for the report : ");
                        if (!readDate(out strt)) goto label2;
                        Console.WriteLine("Please enter the start date for the report : ");
                        if (!readDate(out end)) goto label2;
                        yes = true;
                    label2:
                        if(yes)
                        SystemHandler.ViewStatistics(strt, end);
                        break;
                    case "6":
                        Console.WriteLine("Please, enter the flight number");
                        flightNumber = Console.ReadLine();
                        if (SystemHandler.checkInt(flightNumber))
                            SystemHandler.ViewFlightBookings(int.Parse(flightNumber), FlightSeatClass.Economy);
                        else Console.WriteLine("wrong flight number");
                        break;
                    case "7":
                        Console.WriteLine("Please, enter the flight number");
                        flightNumber = Console.ReadLine();
                        if (SystemHandler.checkInt(flightNumber))
                        {
                            Flight wh = (Flight)FileHandler.Find(ObjectChoices.Flight, flightNumber);
                            if (wh == null)
                            {
                                Console.WriteLine("No such flight exists");
                            }else
                            {
                                Console.WriteLine(wh);
                            }
                        }
                        else Console.WriteLine("wrong flight number");
                        break;
                    case "8":
                        Console.WriteLine("Please, enter the flight number");
                        flightNumber = Console.ReadLine();
                        if (SystemHandler.checkInt(flightNumber))
                        {
                            Flight me = (Flight)FileHandler.Find(ObjectChoices.Flight, flightNumber);
                            if (me.getNumOfBookedSeats(FlightSeatClass.Buisness) > 0 ||
                                me.getNumOfBookedSeats(FlightSeatClass.First) > 0||
                                me.getNumOfBookedSeats(FlightSeatClass.Economy) > 0){
                                Console.WriteLine("cannot delete the flight\n");
                            }else
                            {
                                FileHandler.Delete(ObjectChoices.Flight, flightNumber);
                                Console.WriteLine("Deleted");
                            }
                        }
                        break;
                    case "9":
                        yes = false;
                        Console.WriteLine("Please, enter the start date");
                        if(!readDate(out strt))
                        {
                            goto label3;
                        }
                        Console.WriteLine("Please, enter the end date");
                        if (!readDate(out end))
                        {
                            goto label3;
                        }
                        yes = true;
                    label3:
                        if (yes)
                        {
                            while (strt <= end)
                            {
                                SystemHandler.OccasionDates.Add(strt);
                                strt = strt.AddDays(1);
                            }
                        }
                        break;
                    case "10":
                        Console.WriteLine("Please, enter the flight number");
                        flightNumber = Console.ReadLine();
                        if (SystemHandler.checkInt(flightNumber))
                        {
                            modifyFlightInfo((Flight)FileHandler.Find(ObjectChoices.Flight, flightNumber));
                        }
                        break;
                    case "11":
                        foreach(DateTime dt in SystemHandler.OccasionDates)
                        {
                            FileHandler.Add(ObjectChoices.Occasions, dt);
                        }
                        return;
                    default:
                        break;
                }
            }
        }
        public void modifyFlightInfo(Flight flight)
        {
            Console.WriteLine(flight);
            string list = "choose the information you want to modify : \n" +
                "1- change origin city\n" +
                "2- change destination city\n" +
                "3- change teckit fare\n" +
                "4- change departure date\n" +
                "5- change return date\n" +
                "6- cancel a flight\n"+
                "7- go back"
                ;

            while (true)
            {
                Console.WriteLine(list);
                string choice = Console.ReadLine(),
                    neorigin,nedest,
                    nefare;
                DateTime dd=new DateTime();
                DateTime rr = new DateTime();
                bool yes = false;
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter the new origin city :") ;
                        neorigin = Console.ReadLine();
                        flight.OriginCity = neorigin;
                        break;
                    case "2":
                        Console.WriteLine("Enter the new origin city :");
                        nedest = Console.ReadLine();
                        flight.DestinationCity = nedest;
                        break;
                    case "3":
                        Console.WriteLine("Enter the new flight fare : ");
                        nefare = Console.ReadLine();
                        if (SystemHandler.checkDouble(nefare)) flight.BaseFlightFare = double.Parse(nefare);
                        else Console.WriteLine("nan");
                        break;
                    case "4":
                        Console.WriteLine("Enter the new departue date");
                        if (!readDate(out dd)) Console.WriteLine("Wrong date");
                        else flight.DepartureDate = dd;
                        break;
                    case "5":
                        Console.WriteLine("Enter the new return date");
                        if (!readDate(out dd)) Console.WriteLine("Wrong date");
                        else flight.ReturnDate = dd;
                        break;
                     case "6":
                        flight.FlightState = FlightSatus.Canclled;
                        break;
                    case "7":
                        FileHandler.Add(ObjectChoices.Flight, flight);
                        return;
                    default:
                        break;
                }
                FileHandler.Add(ObjectChoices.Flight, flight);
            }
        }
        public static bool readDate(out DateTime dept)
        {
            string day, month, year;
            Console.WriteLine("enter day : ");
            day = Console.ReadLine();
            Console.WriteLine("enter month : ");
            month = Console.ReadLine();
            Console.WriteLine("enter year : ");
            year = Console.ReadLine();
            if (SystemHandler.checkDate(day, month, year))
            {
                dept = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                return true;
            }
            dept = new DateTime();
            return false;
        }
    }
}
