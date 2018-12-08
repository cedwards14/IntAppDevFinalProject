using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region Additional Namespaces
using ToolsRUs.Data.DTOs;
using ToolsRUsSystem.DAL;
using ToolsRUs.Data.Entities;
using ToolsRUs.Data.POCOs;
#endregion

namespace ToolsRUsSystem.BLL
{
	public class PurchaseOrderController
	{
		public List<VendorPurchaseOrder> List_OutstandingOrders()
		{
			using (var context = new ToolsRUsContext())
			{
				var results = from x in context.PurchaseOrders
							  orderby x.OrderDate
							  where x.Closed == false && x.OrderDate != null && x.PurchaseOrderNumber != null
							  select new VendorPurchaseOrder
							  {
								  PurchaseOrderID = x.PurchaseOrderID,
								  PurchaseOrderNumber = x.PurchaseOrderNumber,
								  OrderDate = x.OrderDate,
								  VendorName = x.Vendor.VendorName,
								  Phone = x.Vendor.Phone,
								  VendorOrderDetail = (from y in x.PurchaseOrderDetails
													   orderby y.StockItem.Description
													   select new PurchaseOrderStockDetail
													   {
														   PurchaseOrderDetailID = y.PurchaseOrderDetailID,
														   StockItemID = y.StockItemID,
														   VendorStockNumber = y.StockItem.VendorStockNumber,
														   StockItemDescription = y.StockItem.Description,
														   QuantityOnOrder = y.Quantity,
														   QuantityReceived = (from z in y.ReceiveOrderDetails select z.QuantityReceived).Sum().Equals(null) ?
														   0 : (from z in y.ReceiveOrderDetails select z.QuantityReceived).Sum()
													   }).ToList()
							  };
				return results.ToList();
			}
		}

		public VendorPurchaseOrder Get_VendorPurchaseOrder(int purchaseorderid)
		{
			using (var context = new ToolsRUsContext())
			{
				var result = from x in context.PurchaseOrders
							 where x.PurchaseOrderID == purchaseorderid
							 select new VendorPurchaseOrder
							 {
								 PurchaseOrderID = x.PurchaseOrderID,
								 PurchaseOrderNumber = x.PurchaseOrderNumber,
								 OrderDate = x.OrderDate,
								 VendorName = x.Vendor.VendorName,
								 Phone = x.Vendor.Phone,
								 VendorOrderDetail = (from y in x.PurchaseOrderDetails
													  orderby y.StockItem.Description
													  select new PurchaseOrderStockDetail
													  {
														  PurchaseOrderDetailID = y.PurchaseOrderDetailID,
														  StockItemID = y.StockItemID,
														  VendorStockNumber = y.StockItem.VendorStockNumber,
														  StockItemDescription = y.StockItem.Description,
														  QuantityOnOrder = y.Quantity,
														  QuantityReceived = (from z in y.ReceiveOrderDetails select z.QuantityReceived).Sum().Equals(null) ?
														  0 : (from z in y.ReceiveOrderDetails select z.QuantityReceived).Sum()
													  }).ToList()
							 };
				return result.SingleOrDefault();
			}
		}

		public void ForceClose_PurchaseOrder(int purchaseorderid, string closeReason)
		{
			using (var context = new ToolsRUsContext())
			{
				var exists = (from x in context.PurchaseOrders where x.PurchaseOrderID == purchaseorderid select x).SingleOrDefault();
				var existsDetails = (from y in exists.PurchaseOrderDetails select y).ToList();
				if (exists == null)
				{
					throw new Exception("Order was not found");
				}
				else
				{
					if (exists.Closed == true)
					{
						throw new Exception("Order has already been closed");
					}
					else
					{
						exists.Closed = true;
						exists.Notes += closeReason;
						//for each purchase order detail (item on order), subtract quantity outstanding from quantity on order
						foreach (PurchaseOrderDetail detail in existsDetails)
						{
							var existsReceived = (from z in detail.ReceiveOrderDetails select z).SingleOrDefault();
							if (existsReceived != null)
							{
								int quantityoutstanding = detail.Quantity - existsReceived.QuantityReceived;
								detail.StockItem.QuantityOnOrder -= quantityoutstanding;
							}
							else
							{
								detail.StockItem.QuantityOnOrder -= detail.Quantity;
							}
						}
						context.SaveChanges();
					}
				}
			}
		}

		public PurchasingOrder Get_PurchaseOrder(int vendorid)
		{
			using (var context = new ToolsRUsContext())
			{
				var result = from x in context.PurchaseOrders
							 where x.VendorID == vendorid && x.PurchaseOrderNumber == null && x.OrderDate == null
							 select new PurchasingOrder
							 {
								 PurchaseOrderID = x.PurchaseOrderID,
								 Subtotal = x.SubTotal,
								 TaxAmount = x.TaxAmount
							 };
				return result.FirstOrDefault();
			}
		}

		public void Delete_PurchaseOrder(int orderid)
		{
			List<string> reasons = new List<string>();
			using (var context = new ToolsRUsContext())
			{
				var exists = (from x in context.PurchaseOrders
							  where x.PurchaseOrderID == orderid
							  select x).FirstOrDefault();

				if (exists == null)
				{
					reasons.Add("Purchase order does not exist");
				}
				else
				{
					List<PurchaseOrderDetail> deletes = (from y in exists.PurchaseOrderDetails
														 where y.PurchaseOrderID == orderid
														 orderby y.PurchaseOrderDetailID
														 select y).ToList();
					foreach (PurchaseOrderDetail item in deletes)
					{
						context.PurchaseOrderDetails.Remove(item);
					}

					context.PurchaseOrders.Remove(exists);
					context.SaveChanges();
				}
			}
		}

		public void Create_PurchaseOrder(int vendorid, int employeeid)
		{
			using (var context = new ToolsRUsContext())
			{
				var exists = (from x in context.PurchaseOrders
							  where x.VendorID == vendorid && x.PurchaseOrderNumber == null && x.OrderDate == null
							  select x).FirstOrDefault();
				PurchaseOrder neworder = new PurchaseOrder();
				if (exists == null)
				{

					neworder.EmployeeID = employeeid;
					neworder.VendorID = vendorid;
					context.PurchaseOrders.Add(neworder);

					List<StockItem> vendorItems = (from x in context.StockItems
												   where x.VendorID == vendorid
												   select x).ToList();

					decimal totalPrice = 0;
					decimal taxAmount = 0;
					foreach (StockItem item in vendorItems)
					{
						if ((item.ReOrderLevel - (item.QuantityOnHand + item.QuantityOnOrder)) > 0)
						{

							PurchaseOrderDetail poDetail = new PurchaseOrderDetail();
							poDetail.StockItemID = item.StockItemID;
							poDetail.Quantity = (item.ReOrderLevel - (item.QuantityOnHand + item.QuantityOnOrder));
							poDetail.PurchasePrice = item.PurchasePrice * (item.ReOrderLevel - (item.QuantityOnHand + item.QuantityOnOrder));
							totalPrice += poDetail.PurchasePrice;
							neworder.SubTotal = totalPrice;
							neworder.PurchaseOrderDetails.Add(poDetail);
						}
					}
					neworder.TaxAmount = totalPrice * (decimal)0.05;
					context.SaveChanges();
				}
			}
		}

		public void Place_PurchaseOrder(int vendorid, int orderid)
		{
			List<string> reasons = new List<string>();

			using (var context = new ToolsRUsContext())
			{
				PurchaseOrder exists = (from x in context.PurchaseOrders
										where x.PurchaseOrderID == orderid && x.VendorID == vendorid
										select x).FirstOrDefault();

				if (exists == null)
				{
					reasons.Add("Purchase order does not exist");
				}
				else
				{
					//loop through order and update stock qty
					List<PurchaseOrderDetail> poDetails = (from x in exists.PurchaseOrderDetails
														   select x).ToList();


					for (int index = 0; index < poDetails.Count; index++)
					{
						int stockitemid = poDetails[index].StockItemID;
						StockItem updateItem = (from x in context.StockItems
												where x.VendorID == vendorid && x.StockItemID == stockitemid
												select x).FirstOrDefault();
						updateItem.QuantityOnOrder = poDetails[index].Quantity;
					}
					
					exists.OrderDate = DateTime.Now;
					exists.PurchaseOrderNumber = context.PurchaseOrders.Max(m => m.PurchaseOrderNumber + 1);
					context.SaveChanges();
				}
			}
		}
	}
}