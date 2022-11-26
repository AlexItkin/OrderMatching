
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
namespace ConsoleApp2
{
    class OrderClass
    {
        string OrderStatus;
        string OrderType;
        string OrderSide;
        string OrderPriceString;
        string OrderQtyString;
        string OrderTimeInForce;
        int OrderPriceInt = 0;
        int OrderQtyInt = 0;
        public void CSVtoOrder(int OrderNum, string[] UnparcedCSV)
        {
            OrderStatus = "NEW";
            string StringToParce = UnparcedCSV[OrderNum];
            int Iterator = 0;
            for (int i = 0; i < StringToParce.Length; i++)
            {
                switch (Iterator)
                {
                    case 0:
                        if (StringToParce[i] == ',')
                        {
                            Iterator++;
                        }
                        else
                        {
                            OrderType += StringToParce[i];
                        }
                        break;
                    case 1:
                        if (StringToParce[i] == ',')
                        {
                            Iterator++;
                        }
                        else
                        {
                            OrderSide += StringToParce[i];
                        }
                        break;
                    case 2:
                        if (StringToParce[i] == ',')
                        {
                            Iterator++;
                        }
                        else
                        {
                            OrderPriceString += StringToParce[i];
                        }
                        break;
                    case 3:
                        if (StringToParce[i] == ',')
                        {
                            Iterator++;
                        }
                        else
                        {
                            OrderQtyString += StringToParce[i];
                        }
                        break;
                    case 4:
                        if (StringToParce[i] == ',')
                        {
                            Iterator++;
                        }
                        else
                        {
                            OrderTimeInForce += StringToParce[i];
                            Console.WriteLine(OrderTimeInForce);
                        }
                        break;
                    
                }
            
            }
                

        }
        public void OrderInputValidation()
        {
            if(OrderType != "MARKET" && OrderType != "LIMIT")
            {
                    Console.WriteLine(String.Format("1! {0}",OrderType));
                    OrderStatus = "EXPIRED";

            }
            if(OrderSide != "SELL" && OrderSide != "BUY")
            {
                Console.WriteLine("2! {0}",OrderSide);
                OrderStatus = "EXPIRED";

            }
            if(int.TryParse(OrderPriceString, out OrderPriceInt) == false)
            {
                Console.WriteLine("3! {0},{1},{2}",OrderPriceInt,OrderPriceString,int.TryParse(OrderPriceString, out OrderPriceInt));
                OrderStatus = "EXPIRED";
            }
            else
            {
                int.TryParse(OrderPriceString, out OrderPriceInt);
                if(OrderPriceInt < 0)
                {
                    Console.WriteLine("4! {0},{1},{2}",OrderPriceInt,OrderPriceString,int.TryParse(OrderPriceString, out OrderPriceInt));
                    OrderStatus = "EXPIRED";
                }
                 
            }
            if (int.TryParse(OrderQtyString, out OrderQtyInt) == false)
            {
                Console.WriteLine("5! {0},{1},{2}",OrderQtyInt,OrderQtyString,int.TryParse(OrderPriceString, out OrderPriceInt));
                OrderStatus = "EXPIRED";
            }
            else
            {
                int.TryParse(OrderQtyString, out OrderQtyInt);
                if (OrderQtyInt <= 0)
                {
                    Console.WriteLine("6! {0},{1},{2}", OrderQtyInt, OrderQtyString, int.TryParse(OrderPriceString, out OrderPriceInt));
                    OrderStatus = "EXPIRED";
                }

            }
            if (OrderTimeInForce != "DAY" && OrderTimeInForce != "KOF" && OrderTimeInForce != "IOC")
            {
                Console.WriteLine("7! {0}",OrderTimeInForce);
                OrderStatus = "EXPIRED";

            }
            if (OrderType == "MARKET" && OrderPriceString != "0")
            {
                Console.WriteLine("8! {0}, {1}", OrderType, OrderPriceString);
                OrderStatus = "EXPIRED";
            }
        }
        public string OrderOutput()
        {
            return string.Format("{0},{1},{2},{3},{4},{5}",OrderStatus,OrderType,OrderSide,OrderPriceInt,OrderQtyInt,OrderTimeInForce);
        }

        public string GetOrderStatus()
        {
            return OrderStatus;
        }
        public string GetOrderType()
        {
            return OrderType;
        }
        public string GetOrderSide()
        {
            return OrderSide;
        }
        public string GetOrderTimeInForce()
        {
            return OrderTimeInForce;
        }
        public int GetOrderPrice()
        {
            return OrderPriceInt;
        }
        public int GetOrderQty()
        {
            return OrderQtyInt;
        }

        public void SetOrderStatus(string SetOrderTo)
        {
            OrderStatus = SetOrderTo;
        }
        public void SetOrderQty(int SetOrderQtyTo)
        {
            OrderQtyInt = SetOrderQtyTo;
        }
    }
    

    internal class Program
    {
        public static string[] CSVImport(string FileLocation)
        {
            string[] CSVLines = new string[2];

            if (!File.Exists(FileLocation))
            {
                CSVLines[0] = String.Format( "File: {0} does not exist", FileLocation);
                return CSVLines;
            }
            else
            {
                CSVLines = File.ReadAllLines(FileLocation);
                return CSVLines;
            }

        }
        public static async Task CSVExport(string FileLocation, string ExportOrder,bool NewLine)
        {
            using StreamWriter file = new(FileLocation, append: true);
            if(NewLine == true)
            {
                await file.WriteLineAsync("\t");
            }
            await file.WriteLineAsync(ExportOrder);
            file.Close();
        }
        
        static void Main(string[] args)
        {
            string[] UnparsedCSV = CSVImport(@"C:\CSVLOC\CSVFILE.CSV");

            OrderClass OrderOne = new OrderClass();
            OrderClass OrderTwo = new OrderClass();
            OrderOne.CSVtoOrder(0, UnparsedCSV);
            OrderTwo.CSVtoOrder(1, UnparsedCSV);
            Console.WriteLine("First order validation");
            OrderOne.OrderInputValidation();
            Console.WriteLine("Second order validation");
            OrderTwo.OrderInputValidation();
            Console.WriteLine("0");



            var OrderOneExport = CSVExport(@"C:\CSVLOC\CSVFILE.CSV", OrderOne.OrderOutput(), true);
            var OrderTwoExport = CSVExport(@"C:\CSVLOC\CSVFILE.CSV", OrderTwo.OrderOutput(), false);
        }
    }
}
