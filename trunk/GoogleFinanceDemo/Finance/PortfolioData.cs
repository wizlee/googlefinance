﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Extensions;

namespace Finance
{
    /// <summary>
    /// gf:portfolioData schema extension describing the portfolio. Contains an overview of a portfolio, including currency, returns, and performance statistics.
    /// </summary>
    public class PortfolioData : SimpleElement
    {
        #region Constructor
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PortfolioData() :
            base(FinanceNamespace.PORTFOLIODATA, FinanceNamespace.PREFIX, FinanceNamespace.NAMESPACE)
        {
            Attributes.Add(FinanceNamespace.CURRENCYCODE, null);
            Attributes.Add(FinanceNamespace.GAINPERCENTAGE, null);
            Attributes.Add(FinanceNamespace.RETURN1W, null);
            Attributes.Add(FinanceNamespace.RETURN4W, null);
            Attributes.Add(FinanceNamespace.RETURN3M, null);
            Attributes.Add(FinanceNamespace.RETURNYTD, null);
            Attributes.Add(FinanceNamespace.RETURN1Y, null);
            Attributes.Add(FinanceNamespace.RETURN3Y, null);
            Attributes.Add(FinanceNamespace.RETURN5Y, null);
            Attributes.Add(FinanceNamespace.RETURNOVERALL, null);
        }

        /// <summary>
        /// Constructs an PortfolioData that contains inner xml element 
        /// </summary>
        /// <param name="value">Inner Xml Value</param>
        public PortfolioData(string value) :
            base(FinanceNamespace.PORTFOLIODATA, FinanceNamespace.PREFIX, FinanceNamespace.NAMESPACE, value)
        {
            // NOTE: should this be removed? I do not think that portfolioData can contain an inner value.
        }
        #endregion 

        #region Properties
        /// <summary>
        /// Currency the portfolio is set to. Default ISO4217 currency code for the portfolio.
        /// </summary>
        public string CurrencyCode
        {
            // NOTE: would it be nice to have a currency type, that enums the total amount of currencies that google supports?
            get 
            { 
                return Attributes[FinanceNamespace.CURRENCYCODE] as string; 
            }
            set
            {
                Attributes[FinanceNamespace.CURRENCYCODE] = value as string;
            }
        }

        /// <summary>
        /// Percentage gain.
        /// </summary>
        public double GainPercentage
        {
            get
            {
                return double.Parse(Attributes[FinanceNamespace.GAINPERCENTAGE] as string);
            }
            set
            {
                Attributes[FinanceNamespace.GAINPERCENTAGE] = value.ToString();
            }

        }

        /// <summary>
        /// 1 week return (percentage).
        /// </summary>
        public double Return1Week
        {
            get
            {
                return double.Parse(Attributes[FinanceNamespace.RETURN1W] as string);
            }
            set
            {
                Attributes[FinanceNamespace.RETURN1W] = value.ToString();
            }
        }

        /// <summary>
        /// 4 week return (percentage).
        /// </summary>
        public double Return4Week
        {
            get
            {
                return double.Parse(Attributes[FinanceNamespace.RETURN4W] as string);
            }
            set
            {
                Attributes[FinanceNamespace.RETURN4W] = value.ToString();
            }
        }

        /// <summary>
        /// 3 month return (percentage).
        /// </summary>
        public double Return3Month
        {
            get
            {
                return double.Parse(Attributes[FinanceNamespace.RETURN3M] as string);
            }
            set
            {
                Attributes[FinanceNamespace.RETURN3M] = value.ToString();
            }
        }

        /// <summary>
        /// Year-to-date return (percentage).
        /// </summary>
        public double ReturnYTD
        {
            get
            {
                return double.Parse(Attributes[FinanceNamespace.RETURNYTD] as string);
            }
            set
            {
                Attributes[FinanceNamespace.RETURNYTD] = value.ToString();
            }
        }

        /// <summary>
        /// 1 year return (percentage).
        /// </summary>
        public double Return1Year
        {
            get
            {
                return double.Parse(Attributes[FinanceNamespace.RETURN1Y] as string);
            }
            set
            {
                Attributes[FinanceNamespace.RETURN1Y] = value.ToString();
            }
        }

        /// <summary>
        /// 3 year return (percentage).
        /// </summary>
        public double Return3Year
        {
            get
            {
                return double.Parse(Attributes[FinanceNamespace.RETURN3Y] as string);
            }
            set
            {
                Attributes[FinanceNamespace.RETURN3Y] = value.ToString();
            }
        }

        /// <summary>
        /// 5 year return (percentage).
        /// </summary>
        public double Return5Year
        {
            get
            {
                return double.Parse(Attributes[FinanceNamespace.RETURN5Y] as string);
            }
            set
            {
                Attributes[FinanceNamespace.RETURN5Y] = value.ToString();
            }
        }

        /// <summary>
        /// Overall return (percentage).
        /// </summary>
        public double ReturnOverall
        {
            get
            {
                return double.Parse(Attributes[FinanceNamespace.RETURNOVERALL] as string);
            }
            set
            {
                Attributes[FinanceNamespace.RETURNOVERALL] = value.ToString();
            }
        }
        #endregion 
    }
}
