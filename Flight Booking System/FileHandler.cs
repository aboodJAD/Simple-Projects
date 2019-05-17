using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FlightBookingSystem
{
    enum ObjectChoices
    {
        Passenger, Flight, Admin,Occasions
    }
    class FileHandler
    {   
        private static string IO_PASSENGER = Directory.GetCurrentDirectory() + "\\Passenger";
        private static string IO_FLIGHT = Directory.GetCurrentDirectory() + "\\Flight";
//      private static string IO_CREDITCARD = Directory.GetCurrentDirectory() + "\\CreditCard";
//      private static string IO_ETICKET = Directory.GetCurrentDirectory() + "\\Eticket";
        private static string IO_ADMIN = Directory.GetCurrentDirectory() + "\\Admin";
        private static string IO_OCCASIONS = Directory.GetCurrentDirectory() + "\\OOCASIONS";

        static FileHandler()
        {
            if (Directory.Exists(IO_PASSENGER) == false)
            {
                Directory.CreateDirectory(IO_PASSENGER);
            }
            if (Directory.Exists(IO_FLIGHT) == false)
            {
                Directory.CreateDirectory(IO_FLIGHT);
            }
            if (Directory.Exists(IO_ADMIN) == false)
            {
                Directory.CreateDirectory(IO_ADMIN);
            }
            if (Directory.Exists(IO_OCCASIONS) == false)
            {
                Directory.CreateDirectory(IO_OCCASIONS);
            }
        }

        private static string getDirectory(ObjectChoices choice)
        {
            if (choice == ObjectChoices.Passenger)
            {
                return IO_PASSENGER;
            }
            else if (choice == ObjectChoices.Flight)
            {
                return IO_FLIGHT;
            }
            else if (choice == ObjectChoices.Admin)
            {
                return IO_ADMIN;
            }else if (choice == ObjectChoices.Occasions)
            {
                return IO_OCCASIONS;
            }
            return null;
        }

        public static List<Object> GetAllObj(ObjectChoices choice)
        {
            string gt = getDirectory(choice);
            if (gt == null) return null;
            DirectoryInfo dir=new DirectoryInfo(gt);
            if (dir == null || dir.Exists == false) return null;

            FileInfo[] files = dir.GetFiles();
            List<Object> result=new List<object>();
            
            foreach(FileInfo ff in files)
            {
                FileStream stream = new FileStream(ff.FullName, FileMode.Open);
                result.Add(new BinaryFormatter().Deserialize(stream));
                stream.Close();
            }
            return result;
       }

        public static Object Find(ObjectChoices choice,string unique)
        {
            FileInfo[] files = new DirectoryInfo(getDirectory(choice)).GetFiles();
            foreach(FileInfo f in files)
            {
                if (f.Name == unique+".bin")
                {
                    FileStream stream = new FileStream(f.FullName,FileMode.Open);
                    Object ret=new BinaryFormatter().Deserialize(stream);
                    stream.Close();
                    return ret;
                }
            }
            return null;
        }

        public static bool Find(ObjectChoices choice, Object obj)
        {
            List<Object> lis = GetAllObj(choice);
            if (lis == null) return false;
            foreach (Object comp in lis)
            {
                if (choice == ObjectChoices.Passenger&& ((Passenger)obj).Equals(comp))
                {
                    return true;
                }else if(choice == ObjectChoices.Admin && ((Admin)obj).Equals(comp))
                {
                    return true;
                }else if(choice == ObjectChoices.Flight && ((Flight)obj).Equals(comp))
                {
                    return true;
                }
            }
            return false;
        }

        public static Object FindUser(ObjectChoices choice, string name,string password)
        {
            List<Object> lis = GetAllObj(choice);
            if (lis == null) return null;
            foreach (Object obj in lis)
            {
                if (choice == ObjectChoices.Passenger )
                {
                    if (((Passenger)obj).Name.Equals(name,StringComparison.OrdinalIgnoreCase) &&
                        ((Passenger)obj).VerifyPassword(password))
                        return obj;
                }
                else if (choice == ObjectChoices.Admin)
                {
                    if (((Admin)obj).getName().Equals(name,StringComparison.OrdinalIgnoreCase) && 
                        ((Admin)obj).getId().ToString().Equals(password,StringComparison.OrdinalIgnoreCase))
                        return obj;
                }
            }
            return null;
        }

        public static bool Delete(ObjectChoices choice,string unique)
        {
            FileInfo[] lis = new DirectoryInfo(getDirectory(choice)).GetFiles();
            foreach (FileInfo aa in lis)
            {
                if (unique+".bin"==aa.Name)
                {
                    aa.Delete();
                    return true;
                }
            }
            return false;
        }

        public static bool Add(ObjectChoices choice,Object obj)
        {
            string unique="";
            
            if (choice == ObjectChoices.Passenger)
            {
                unique = ((Passenger)obj).PassportNumber;
            }
            else if (choice == ObjectChoices.Flight)
            {
                unique = ((Flight)obj).FlightNumber.ToString();
            }else if (choice == ObjectChoices.Admin)
            {
                unique = ((Admin)obj).getName()+((Admin)obj).getId().ToString();
            }else if(choice == ObjectChoices.Occasions)
            {
                unique = ""+((DateTime)obj).GetHashCode();
            }
            if (unique.Length == 0) return false;
            FileStream stream = new FileStream(getDirectory(choice) + "\\" + unique+".bin", FileMode.Create);
            new BinaryFormatter().Serialize(stream, obj);
            stream.Close();
            return true;
        }
    }
}
