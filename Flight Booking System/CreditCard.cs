using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingSystem
{
    [Serializable]
    class CreditCard
    {
        DateTime expireDate;
        double balance;
        string creditNumber;
        public CreditCard(DateTime exp,string number,double balance)
        {
            expireDate = exp;
            creditNumber = number;
            this.balance = balance;
        }

        public bool Withdraw(double money)
        {
            if (DateTime.Now > expireDate)
            {
 //             Console.WriteLine("cannot withdraw money, the credit has reached it's expiry date");
                return false;
            }else if (money > balance)
            {
                //Console.WriteLine("cannot withdraw money, the credit has balance less than "+money);
                return false;
            }
            balance -= money;
            return true;
        }

        public double getBalance()
        {
            return balance;
        }
        
        public bool verifyCreditNumber(string ent)
        {
            return creditNumber.Equals(ent);
        }
    }
}
