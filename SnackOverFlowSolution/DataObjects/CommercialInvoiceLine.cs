﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    /// <summary>
    /// Ariel Sigo
    /// 
    /// Created:
    /// 2017/04/29
    /// 
    /// DTO for Commercial Invoice Line
    /// 
    /// </summary>
    public class CommercialInvoiceLine
    {
        public int CommercialInvoiceId {get;set;}
        public int ProductLotId {get;set;}
        public int QuantitySold {get;set;}
        public decimal PriceEach {get;set;}
        public decimal ItemDiscount {get;set;}
        public decimal ItemTotal { get; set; }
        public string ProductName { get; set; }
    }
}
