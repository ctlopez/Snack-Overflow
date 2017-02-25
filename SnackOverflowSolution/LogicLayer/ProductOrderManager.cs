﻿using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{

    public class ProductOrderManager : IProductOrderManager
    {
        public List<ProductOrder> RetrieveProductOrdersByStatus(String Status)
        {
            List<ProductOrder> ProductOrderList = ProductOrderAccessor.RetrieveProductOrdersByStatus(Status);
            foreach (ProductOrder ProductOrderFound in ProductOrderList)
            {
                ProductOrderFound.BackorderPreorderList = BackorderPreorderAccessor.RetrieveBackorderPreorder(
                    new BackorderPreorder
                    {
                        OrderId = ProductOrderFound.OrderId
                    }
                );

                ProductOrderFound.DeliveryList = DeliveryAccessor.RetrieveDelivery(
                    new Delivery
                    {
                        OrderId = ProductOrderFound.OrderId
                    }
                );

                ProductOrderFound.EmployeeOrderResponsibilityList = EmployeeOrderResponsibilityAccessor.RetrieveEmployeeOrderResponsibility(
                    new EmployeeOrderResponsibility
                    {
                        OrderId = ProductOrderFound.OrderId
                    }
                );

                ProductOrderFound.PackageList = PackageAccessor.RetrievePackage(
                    new Package
                    {
                        OrderId = ProductOrderFound.OrderId
                    }
                );
            }
            return ProductOrderList;
	
		}





        /// <summary>
        /// Victor Algarin
        /// Created 2017/02/08
        /// 
        /// Retrieves the details for a specific order through an order id
        /// </summary>
        public ProductOrder retrieveProductOrderDetails(int orderID)
        {
		ProductOrder order = null;
            try
            {
                order = ProductOrderAccessor.RetrieveOrderByID(orderID);
            }
            catch (Exception)
            {

                throw new ApplicationException("There was a problem retrieving the product order details.");
            }

            return order;
        }

        /// <summary>
        /// Robert Forbes
        /// 2017/02/16
        /// 
        /// updates the status of the order
        /// </summary>
        /// <param name="productOrderID">The ProductOrderID of the order to be updated</param>
        /// <param name="status">The status to assign to the order</param>
        /// <returns>bool representing whether the update was successful</returns>
        public bool UpdateProductOrderStatus(int productOrderID, string status)
        {
            bool result = false;

            try
            {
                if (ProductOrderAccessor.UpdateProductOrderStatus(productOrderID, status) > 0)
                {
                    result = true;
                }

            }
            catch
            {
                throw;
            }

            return result;
        }
    }
}