﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Finance.Web
{
    public class HistoricalPrice
    {
        public DateTime Date { set; get; }
        public double Open { set; get; }
        public double High { set; get; }
        public double Low { set; get; }
        public double Close { set; get; }
        public int Volume { set; get; }

        public HistoricalPrice()
        {
        }
    }

    public class DateRange
    {
        //startdate=Oct+7%2C+2004&enddate=Oct+6%2C+2009
        
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }

        public string StartDateAsParameter
        {
            get
            {
                return string.Format("startdate={0:MMM}+{1}%2C+{0:yyyy}", StartDate, StartDate.Day);
            }
        }

        public string EndDateAsParameter
        {
            get
            {
                return string.Format("enddate={0:MMM}+{1}%2C+{0:yyyy}", EndDate, EndDate.Day);
            }
        }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        public DateRange()
        {
        }
    }

    public enum HistoricalPeriod
    {
        Daily,
        Weekly
    }

    // TODO: add some exceptions and debugging.
    public class HistoricalPrices
    {
        string googleQueryHistoricalPrices = "http://www.google.com/finance/historical?q=";

        // http://www.google.com/finance/historical?q=NASDAQ:CSCO&histperiod=daily
        // http://www.google.com/finance/historical?q=NASDAQ:CSCO&histperiod=weekly
        // http://www.google.com/finance/historical?cid=99624&startdate=Oct+7%2C+2004&enddate=Oct+6%2C+2009&histperiod=weekly
        // http://www.google.com/finance/historical?cid=99624&startdate=Oct+7%2C+2004&enddate=Oct+6%2C+2009

        // csv output file.
        // http://www.google.com/finance/historical?cid=99624&startdate=Oct+7%2C+2004&enddate=Oct+6%2C+2009&output=csv

        /*
cid = Company Id
startdate = Start date of the historical prices
enddate = End date of the historical prices
histperiod = weekly or daily history periods
start = index on which to display the historical price
num = number of historical prices to display (this has some max like 100 or 200)
output = output the data in a format (I think it currently supports CSV only)

http://www.google.com/finance/historical?cid=700146&startdate=Oct+10%2C+2008&enddate=Oct+9%2C+2009&output=csv

http://www.google.com/finance/historical?cid=700146&startdate=Oct+9%2C+2008&enddate=Oct+9%2C+2009&histperiod=weekly&output=csv

http://www.google.com/finance/historical?cid=700146&startdate=Oct%209%2C%202008&enddate=Oct%209%2C%202009&histperiod=weekly&start=0&num=20
         */

//        
//          
//          .historical_chart {
//background: no-repeat url(http://www.google.com/chart?cht=lfe&chc=&chs=270x160&chm=B,EDF7FF,0,0,0&chd=s:hdRYZZZfZWVQQQObZYYYYVcWTXXXXVRVSSSPRJFJJJQLQQRRRIKOKOOOVVWTUUUTWTSSSSQPQQQQQOPQQUUUUYWXSSSQRMNNNNNIMKNNNQSTOIIIKMNQUUUTOPPPPPPKKIJJJEIHFGGGEFKFDDAGILLLLLPRPNNNUSRWUUUQSXabbbXTVZZZZZZXaaaaWYWXccccehhjjjijjfeeedeaaZZZefdaZZZZdbddddijijllllmlmlllighffffcddgffffdedddddbabcccdekmooosvuxwwwwxvxxxx00yzyyyvvuutttqrtwyyyxxxwxxxvsuuwwwwxy333323555557521111756311135779998&chco=3f8ce5) 
         
//         * Some Interesting things to look at
//         * cht = Chart Type
//         * chs = Chart Scale
//         * chm = Chart Members, Chart Variables?
//         * chd = Chart Data
//         * chco = Chart Color
//         

        public HistoricalPeriod HistoricalPeriod { private set; get; }

        public string CompanyId { set; get; }
        public DateRange DateRange { private set; get; }
        public int StartIndex { private set; get; }
        public int NumberOfHistoricalPrices { private set; get; }

        public List<HistoricalPrice> Prices { set; get; }

        public Uri HistoricalChart { set; get; }

        public string ExportCSV
        {
            get
            {
                return "";
            }
        }

        private HtmlDocument htmlDocument;

        // <title /> of the web page. 
        public string Name { private set; get; }

        // <meta name="Description" />
        public string Description { private set; get; }

        private string stock;
        private string exchange;

        public HistoricalPrices(string stock, string exchange) 
        {
            this.stock = stock;
            this.exchange = exchange;

            HtmlWeb htmlWeb = new HtmlWeb();
            this.htmlDocument = htmlWeb.Load(googleQueryHistoricalPrices + exchange + ":" + stock);
            Prices = new List<HistoricalPrice>();
            Parse();
        }
        
        public HistoricalPrices(HtmlDocument htmlDocument) : this(htmlDocument, new DateRange(), 0, HistoricalPeriod.Daily)
        {
            
        }
        public HistoricalPrices(HtmlDocument htmlDocument, DateRange dateRange, int startIndex, HistoricalPeriod historicalPeriod)
        {
            // TODO: Fixup this constructor so it does the proper things.
            this.htmlDocument = htmlDocument;
            Prices = new List<HistoricalPrice>();
            Parse();
        }

        public void Refresh()
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            string url =
            googleQueryHistoricalPrices + exchange + ":" + stock;
            this.htmlDocument = htmlWeb.Load(url);
            Prices = new List<HistoricalPrice>();
            Parse();
        }

        public void Refresh(DateRange dateRange, int startIndex, HistoricalPeriod historicalPeriod, int number)
        {
            this.HistoricalPeriod = historicalPeriod;
            HtmlWeb htmlWeb = new HtmlWeb();
            string url =
            googleQueryHistoricalPrices + exchange + ":" + stock +
                "&" + dateRange.StartDateAsParameter +
                "&" + dateRange.EndDateAsParameter + 
                "&" + "histperiod=" + Enum.GetName(typeof(HistoricalPeriod), historicalPeriod) +
                "&" + "start=" + startIndex + 
                "&" + "num=" + number;

            this.htmlDocument = htmlWeb.Load(url);
            Prices = new List<HistoricalPrice>();
            Parse();
        }

        private void Parse()
        {
            HtmlNode description = null;

            // Parse the <title /> of the web page and set it to Name
            // Parse the <meta name="Description" />
            foreach (var n in htmlDocument.DocumentNode.ChildNodes)
            {
                if (n.Name == "html")
                {
                    try
                    {
                        HtmlNode name = n.FirstChild.FirstChild.NextSibling.NextSibling;
                        Name = name.InnerText;
                    }
                    catch
                    {
                        Name = "";
                        try
                        {
                            HtmlNodeCollection names = n.FirstChild.NextSibling.ChildNodes;
                            foreach (var v in names)
                            {
                                if (v.Name == "title")
                                {
                                    Name = v.InnerText;
                                    description = v.NextSibling;
                                    break;
                                }
                            }
                            
                        }
                        catch
                        {
                            Name = "";
                        }
                    }

                    

                    try
                    {
                        HtmlNode description2 = n.FirstChild.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling;
                        if (description2.Name == "meta" && description2.Attributes["name"].Value == "Description")
                        {
                            Description = description2.Attributes["content"].Value;
                        }
                        else
                        {
                            Description = "";
                        }
                    }
                    catch
                    {
                        Description = "";

                        try
                        {
                            //description = n.FirstChild.ChildNodes;
                            if (description != null)
                            {
                                //foreach (var i in description)

                                HtmlNode i = description.NextSibling;
                                    if (i.Name == "meta" && i.HasAttributes && i.Attributes["name"].Value == "Description")
                                    {
                                        Description = i.Attributes["content"].Value;
                                    }
                                    else
                                    {
                                        Description = "";
                                    }
                                
                            }
                            else
                            {
                                Description = "";
                            }
                        }
                        catch
                        {
                            Description = "";
                        }
                    }
                    break;
                }
            }

            // For parsing the prices the first two tags we need to look for are.
            // <div id="prices" class="gf-table-wrapper sfe-break-bottom-16">
            // <table id="historical_price" class="gf-table">

            HtmlNode nodePrices = null;

            HtmlNode historicalPricesNode = htmlDocument.GetElementbyId("historical_price");
            
            HtmlNodeCollection historicalPricesNode_ChildNodes = historicalPricesNode.ChildNodes;
            
            foreach (var tbodyChild in historicalPricesNode_ChildNodes)
            {
                if (tbodyChild.Name == "tbody")
                {
                    // We know that we have now found the table's body.
                    nodePrices = tbodyChild;
                    break;
                }
            }

            // if tbody element was not found then we must be getting the htmldocument
            // from the web. 
            if (nodePrices == null)
            {
                nodePrices = htmlDocument.GetElementbyId("historical_price"); 
            }

            foreach (var p in nodePrices.ChildNodes)
            {
                if (p.Name == "tr" && !p.HasAttributes) 
                {
                    // TODO: we could do some cleaning up here.
                    string inner = p.InnerText.Trim(new char[] { '\n' }).Replace("\n\n","\n");
                    string[] items = inner.Split(new char[] { '\n' });
                    
                    DateTime d = DateTime.Parse(items[0]);
                    double o = double.Parse(items[1]);
                    double h = double.Parse(items[2]);
                    double l = double.Parse(items[3]);
                    double c = double.Parse(items[4]);
                    int v = int.Parse(items[5].Trim(new char[] { '\n' }).Replace(",",""));

                    HistoricalPrice historicalPrice = new HistoricalPrice()
                    {
                        Date = d,
                        Open = o,
                        High = h,
                        Low = l,
                        Close = c,
                        Volume = v
                    };

                    Prices.Add(historicalPrice);
                }
            }
        }
    }
}
