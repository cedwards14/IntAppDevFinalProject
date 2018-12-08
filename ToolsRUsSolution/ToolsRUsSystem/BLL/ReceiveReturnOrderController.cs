using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ToolsRUs.Data.Entities;
using ToolsRUs.Data.POCOs;
using ToolsRUsSystem.DAL;
#endregion

namespace ToolsRUsSystem.BLL
{
    public class ReceiveReturnOrderController
    {
        public void Add_ReceiveReturnOrders(int purchaseorderid, List<ReturnedOrderDetail> returnedorderdetails, List<ReceiveOrderDetail> receivedorderdetails)
        {
            using (var context = new ToolsRUsContext())
            {
                var exists = (from x in context.PurchaseOrders where x.PurchaseOrderID == purchaseorderid select x).SingleOrDefault();
                if (exists == null)
                {
                    throw new Exception("Order not found. Cannot be received");
                }
                else
                {
                    if (exists.Closed == true)
                    {
                        throw new Exception("Order has been closed and can no longer be received");
                    }
                    else
                    {
                        //receive order entry that will be the parent of returned order details and receive order details
                        ReceiveOrder receiveEntry = new ReceiveOrder();
                        receiveEntry.PurchaseOrderID = purchaseorderid;
                        receiveEntry.ReceiveDate = DateTime.Now;
                        receiveEntry = context.ReceiveOrders.Add(receiveEntry);

                        //add each received detail to the new receive order entry
                        for (int i = 0; i < receivedorderdetails.Count(); i++)
                        {
                            receiveEntry.ReceiveOrderDetails.Add(receivedorderdetails[i]);
                        }

                        //add unordered items to list of items to return
                        var unorderedreturns = (from y in context.UnorderedPurchaseItemCarts where y.PurchaseOrderID == purchaseorderid select y).ToList();
                        for (int i = 0; i < unorderedreturns.Count(); i++)
                        {
                            ReturnedOrderDetail unorderedreturn = new ReturnedOrderDetail();
                            unorderedreturn.ItemDescription = unorderedreturns[i].Description;
                            unorderedreturn.Quantity = unorderedreturns[i].Quantity;
                            unorderedreturn.Reason = "Unordered item";
                            unorderedreturn.VendorStockNumber = unorderedreturns[i].VendorStockNumber;
                            returnedorderdetails.Add(unorderedreturn);
                            context.UnorderedPurchaseItemCarts.Remove(unorderedreturns[i]);
                        }

                        //add each return (including unordered) entry to receive order entry
                        for (int i = 0; i < returnedorderdetails.Count(); i++)
                        {
                            receiveEntry.ReturnedOrderDetails.Add(returnedorderdetails[i]);
                        }

                        //check if all items on purchase order have been received
                        List<PurchaseOrderDetail> podetails = exists.PurchaseOrderDetails.ToList();
                        bool allReceived = true;
                        //get quantities of each item on the purchase order that have already been received
                        var receivedSum = (from x in context.ReceiveOrderDetails
                                           where x.PurchaseOrderDetail.PurchaseOrderID == purchaseorderid
                                           group x by x.PurchaseOrderDetail into grouped
                                           select new ReceivedTotal
                                           {
                                               PurchaseOrderDetailID = grouped.Key.PurchaseOrderDetailID,
                                               ReceivedSum = (from y in grouped select y.QuantityReceived).Sum()
                                           }).ToList();

                        for (int i = 0; i < podetails.Count(); i++)
                        {
                            //compares every item in the purchase order with the previous receives and current receives quantities
                            if (receivedSum.Find(x => x.PurchaseOrderDetailID.Equals(podetails[i].PurchaseOrderDetailID)) != null)
                            {
                                //items previously received 
                                if (receivedorderdetails.Find(y => y.PurchaseOrderDetailID.Equals(podetails[i].PurchaseOrderDetailID)) != null)
                                {
                                    //items currently being received
                                    //update quantities of stock items with quantity received
                                    //could be cleaned up with variables
                                    podetails[i].StockItem.QuantityOnHand += receivedorderdetails.Find(y => y.PurchaseOrderDetailID.Equals(podetails[i].PurchaseOrderDetailID)).QuantityReceived;
                                    podetails[i].StockItem.QuantityOnOrder -= receivedorderdetails.Find(y => y.PurchaseOrderDetailID.Equals(podetails[i].PurchaseOrderDetailID)).QuantityReceived;
                                    if (podetails[i].Quantity > (receivedorderdetails.Find(y => y.PurchaseOrderDetailID.Equals(podetails[i].PurchaseOrderDetailID)).QuantityReceived)
                                        + (receivedSum.Find(z => z.PurchaseOrderDetailID.Equals(podetails[i].PurchaseOrderDetailID)).ReceivedSum))
                                    {
                                        //outstanding items still exist
                                        allReceived = false;
                                    }
                                }
                                else
                                {
                                    if(podetails[i].Quantity > (receivedSum.Find(z => z.PurchaseOrderDetailID.Equals(podetails[i].PurchaseOrderDetailID)).ReceivedSum))
                                    {
                                        allReceived = false;
                                    }
                                }
                            }
                            else
                            {
                                //items not previously received
                                if (receivedorderdetails.Find(y => y.PurchaseOrderDetailID.Equals(podetails[i].PurchaseOrderDetailID)) != null)
                                {
                                    //items currently being received but none previously
                                    //update quantities of stock items with quantity received
                                    podetails[i].StockItem.QuantityOnHand += receivedorderdetails.Find(y => y.PurchaseOrderDetailID.Equals(podetails[i].PurchaseOrderDetailID)).QuantityReceived;
                                    podetails[i].StockItem.QuantityOnOrder -= receivedorderdetails.Find(y => y.PurchaseOrderDetailID.Equals(podetails[i].PurchaseOrderDetailID)).QuantityReceived;
                                    if (podetails[i].Quantity > (receivedorderdetails.Find(y => y.PurchaseOrderDetailID.Equals(podetails[i].PurchaseOrderDetailID)).QuantityReceived))
                                    {
                                        //oustanding items will still exists after receive
                                        allReceived = false;
                                    }
                                }
                                else
                                {
                                    //item is not currently being received
                                    allReceived = false;
                                }
                            }
                        }// eof
                        if (allReceived)
                        {
                            //all items have been received for this order
                            //the order may be closed
                            exists.Closed = true;
                        }
                        context.SaveChanges();
                    }
                }
                //check that there are no empty fields
                //check that the order hasn't been received
                //loop through the gridview, pulling entered data from textboxes
                //create ReceiveOrders entry, 
                //create ReceiveOrderDetail entry,
                //create ReturnedOrderDetail entry for both UnorderedPurchaseItems and for damaged / refused items update StockItems quantities, 
                //close PurchaseOrder if all quantities outstanding have been received, 
                //empty cart
            }
        }
    }
}
