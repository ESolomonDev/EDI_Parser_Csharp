using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace EDIX12Paser
{
    //create a class to instaniate an object into memory or whatever pretentious way you wanna say
    public class PurchaseOrder850
    {
        public string PONum;
        public string PODateText;
        public DateTime PODate;
        public string POType;
        public string VendorNumber;
        public string BuyerName;
        public string BuyerTelephone;
        public List<PurchaseOrder850LineItem> LineItems;
        //^ list of line items in the purchase order
    }
    public class PurchaseOrder850LineItem
    {
        public string lineItem;
        public int quantity;
        // ^could be decmial number if appropriate IE chain sold in ft
        
        public string uom;
        //units of measure
        public decimal price;
        public string basisOfUnitPrice;
        public string catalogNum;
        public string description;
        public string dateTextReq;
        public DateTime dateRequired;
    }

    class Program
    {
        static void Main(String[] args)
        {
            //load edi file into memory
            string ediFlename = @"c:\Users\mrhyd\Downloads\EDIClass_2021_02_17\EDIClass_2021_02_17\Samples\Sample_850_01.edi";
            string ediFileContents = File.ReadAllText(ediFlename);

            //temp variables for parsing
            string currentref01  = "";
            string currentPer01  = "";


            //write to console
            Console.WriteLine(ediFileContents);
            //get delimeter at position into memory

            //get delimeter by index in string
            string elementSeparator = ediFileContents.Substring(103,1);
            string lineSeparator = ediFileContents.Substring(105,1);
            
            //replace line ends and return char with null values
            ediFileContents = ediFileContents.Replace("\r","").Replace("\n","");

            //write delimeter to console
            Console.WriteLine("elementSeparator="+elementSeparator);
            Console.WriteLine("lineSeparator="+lineSeparator);

            //create an object
            PurchaseOrder850 po850 = new PurchaseOrder850();
            PurchaseOrder850LineItem lineitem = new PurchaseOrder850LineItem();
            //initialize list
            po850.LineItems = new List<PurchaseOrder850LineItem>();

            //create a array of lines in edi file, split along lineSeparator
            //lineSeparator is a String so needs to be cast to a char within the spilit method
            string[] lines = ediFileContents.Split(char.Parse(lineSeparator));
            Console.Write("Number of lines="+ lines.Length);

            //loop through each line in lines
            foreach(string line in lines)
            {   
                //print each line to console
                Console.WriteLine(line);
                //array of elements
                string[] elements = line.Split(char.Parse(elementSeparator));

                int loopCounter = 0;
                string segment = "";
                string elNum = "";
                string elName = "";

                foreach (string el in elements)
                {   
                    if (loopCounter == 0)
                    {
                        segment = el;
                    }
                    else
                    {
                    elNum = loopCounter.ToString("D2");
                    elName = segment + elNum;

                    //write each element on each line to console
                    //Console.WriteLine(segment+": "+ loopCounter + " value ="+el);
                    Console.WriteLine(elName + "="+el);

                    //switch case to map various elements to the po850 object 
                    switch (elName)
                    {
                        case "BEG03":
                            po850.PONum = el;
                            break;
                        case "BEG05":
                            po850.PODateText = el;
                            //TODO Handle DateTime and validation Issues for incorrect datetime ect.
                            //fancy date formatting
                            po850.PODate = DateTime.ParseExact(
                                el, "yyyyMMdd", CultureInfo.InvariantCulture);
                            break;
                        case "BEG02":
                            po850.POType = el;
                            break;
                        case "REF01":
                            currentref01 = el;
                            break;
                        case "REF02":
                            if(currentref01 == "VR")
                            {
                                po850.VendorNumber = el;
                            }
                            break;
                        case "PER01":
                            currentPer01 = el;
                            break;
                        case "PER02":
                            if(currentPer01 == "BD")
                            {
                                po850.BuyerName = el;
                            }
                            break;
                        case "PER04":
                            if(currentPer01 == "BD")
                            {
                                po850.BuyerTelephone = el;
                            }
                            break;
                        case "PO101":
                            lineitem.lineItem = el;
                            break;
                        case "PO102":
                            //note reak world needs error handling if field does not parse, as of now the program will break if they fail
                            //EDI systems will often have validation to make sure numeric fields are indeed numeric
                            lineitem.quantity = Int32.Parse(el);
                            break;
                        case "PO103":
                            lineitem.uom = el;
                            break;
                        case "PO104":
                            lineitem.price = Decimal.Parse(el);
                            break;
                        case "PO105":
                            lineitem.basisOfUnitPrice = el;
                            break;
                        case "PO107":
                            lineitem.catalogNum = el;
                            break;
                        case "PID05":
                            lineitem.description = el;
                            break;
                        case "DTM02":
                            lineitem.dateTextReq = el;
                            //fancy date formatting
                            lineitem.dateRequired = DateTime.ParseExact(
                                el, "yyyyMMdd", CultureInfo.InvariantCulture);

                            //add to list
                            po850.LineItems.Add(lineitem);
                            lineitem = new PurchaseOrder850LineItem();
                            break;

                    }//end switch

                    }
                    loopCounter++;
                }//end inner for each

                //print mapped object
                Console.WriteLine("PONum= "+ po850.PONum +" PODate= "+po850.PODate+" POType= "+po850.POType);
                Console.WriteLine("Vendor= "+ po850.VendorNumber +" BuyerName= "+po850.BuyerName+" BuyerPhone= "+po850.BuyerTelephone);
                foreach (PurchaseOrder850LineItem item in po850.LineItems)
                {
                    Console.WriteLine("***** "+
                                    item.lineItem+" "+
                                    " qty="+item.quantity+" "+
                                    " uom="+item.uom+" "+
                                    " price="+item.price+" "+
                                    " basis="+item.basisOfUnitPrice+" "+
                                    " desc="+item.description+" "+
                                    " reqDate="+item.dateRequired+" "
                                    );
                }

                //get user to hit enter on keyboard each time
                Console.ReadLine();
            }//end outter for each

            Console.WriteLine("\n\n Press enter to end:");
            Console.ReadLine();
        }
    }
}