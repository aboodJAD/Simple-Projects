using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingSystem
{

    class MainMenu
    {
        readonly string Welcome ="\t\t\tWelcome to AYT Airline\n\n"+
            "AYT Airline will make you able to reach more than 250 countries around The WORLD\n"+
            "Discounts offers availble All the time ^^\n\n";
        readonly string userChoice = "Please, choose one of the following :\n"
            + "1- Admin\t" + "2- Visitor\t" + "3- exit";
        readonly string logreg = "Please, choose one of the following :\n"
             + "1- log in\t" + "2- register\t" + "3- go back";
        public MainMenu()
        {
            determineUser();
        }
        private void determineUser()
        {    
            while (true)
            {
                Console.WriteLine(Welcome);
                Console.WriteLine(userChoice);
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    int gg = getEntering();
                    if (gg == 1) new AdminInterface().login();
                    else if(gg==2) new AdminInterface().register();
                }
                else if (choice == "2")
                {
                    int gg = getEntering();
                    if (gg == 1) new PassengerInterface().login();
                    else if (gg == 2) new PassengerInterface().register();
                }
                else if (choice == "3")
                    return ;
                else
                    Console.WriteLine("Invalid choice,choose again : ");
            }
        }
        public int  getEntering()
        {
            while (true)
            {
                Console.WriteLine(logreg);
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    return 1 ;
                }
                else if (choice == "2")
                {
                    return 2;
                }
                else if (choice == "3")
                {
                    return 3;
                }
                else
                {
                    Console.WriteLine("Wrong input, try again..");
                }
            }
        }
    }
}
