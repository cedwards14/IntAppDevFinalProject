using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ToolsRUs.Data.Entities;
using ToolsRUs.Data.POCOs;
using ToolsRUsSystem.DAL;
using System.ComponentModel;
using DMIT2018Common.UserControls;
#endregion

namespace ToolsRUsSystem.BLL
{
	[DataObject]
	public class PurchaseOrderDetailController
	{
		public List<PurchaseOrderStockDetail> List_OutstandingOrderDetail(int purchaseorderid)
		{
			using (var context = new ToolsRUsContext())
			{
				var results = from x in context.PurchaseOrderDetails
							  orderby x.StockItem.Description
							  where x.PurchaseOrderID == purchaseorderid
							  select new PurchaseOrderStockDetail
							  {
								  PurchaseOrderDetailID = x.PurchaseOrderDetailID,
								  StockItemID = x.StockItemID,
								  VendorStockNumber = x.StockItem.VendorStockNumber,
								  StockItemDescription = x.StockItem.Description,
								  QuantityOnOrder = x.Quantity,
								  QuantityReceived = (from y in x.ReceiveOrderDetails select y.QuantityReceived).Sum().Equals(null) ? 0 : (from y in x.ReceiveOrderDetails select y.QuantityReceived).Sum()
							  };
				return results.ToList();
			}
		}

		public List<PurchasingOrderDetail> List_PurchaseOrder_Get(int vendorid)
		{
			using (var context = new ToolsRUsContext())
			{
				var exists = from x in context.PurchaseOrderDetails
							 where x.PurchaseOrder.VendorID == vendorid && x.PurchaseOrder.PurchaseOrderNumber == null && x.PurchaseOrder.OrderDate == null
							 orderby x.PurchaseOrderDetailID
							 select new PurchasingOrderDetail
							 {
								 PurchaseOrderDetailID = x.PurchaseOrderDetailID,
								 SID = x.StockItemID,
								 Description = x.StockItem.Description,
								 QoH = x.StockItem.QuantityOnHand,
								 QoO = x.StockItem.QuantityOnOrder,
								 RoL = x.StockItem.ReOrderLevel,
								 QtO = x.Quantity,
								 Price = x.PurchasePrice
							 };

				return exists.ToList();
			}
		}

		public void Add_ItemToOrder(int purchaseorderid, int stockid, decimal price)
		{
			List<string> reasons = new List<string>();

			using (var context = new ToolsRUsContext())
			{
				PurchaseOrder exists = context.PurchaseOrders
					.Where(x => x.PurchaseOrderID == purchaseorderid)
					.Select(x => x).FirstOrDefault();

				PurchaseOrderDetail newPurchaseOrderDetail = null;

				if (exists == null)
				{
					reasons.Add("Purchase Order does not exist");
				}
				else
				{
					if (reasons.Count() > 0)
					{
						throw new BusinessRuleException("Adding item to order", reasons);
					}
					else
					{
						newPurchaseOrderDetail = new PurchaseOrderDetail();
						newPurchaseOrderDetail.StockItemID = stockid;
						newPurchaseOrderDetail.PurchasePrice = price;
						newPurchaseOrderDetail.Quantity = 1;

						decimal newTotal = exists.SubTotal + price;
						exists.SubTotal = newTotal;
						exists.TaxAmount = (decimal)0.05 * newTotal;

						exists.PurchaseOrderDetails.Add(newPurchaseOrderDetail);
						context.SaveChanges();
					}
				}
			}
		}

		public void Remove_ItemFromOrder(int purchaseorderid, int podetailid)
		{
			List<string> reasons = new List<string>();

			using (var context = new ToolsRUsContext())
			{
				PurchaseOrder exists = context.PurchaseOrders
					.Where(x => x.PurchaseOrderID == purchaseorderid)
					.Select(x => x).FirstOrDefault();

				if (exists == null)
				{
					reasons.Add("Purchase Order does not exist");
				}
				else
				{

					PurchaseOrderDetail removeitem = (from x in exists.PurchaseOrderDetails
													  where x.PurchaseOrderDetailID == podetailid
													  select x).FirstOrDefault();
					if (removeitem == null)
					{
						reasons.Add("Item does not exist or has been removed already");
					}
					else
					{
						if (reasons.Count() > 0)
						{
							throw new BusinessRuleException("Deleting item from order", reasons);
						}
						else
						{
							decimal newTotal = exists.SubTotal - removeitem.PurchasePrice;
							exists.SubTotal = newTotal;
							exists.TaxAmount = (decimal)0.05 * newTotal;
							context.PurchaseOrderDetails.Remove(removeitem);
							context.SaveChanges();
						}
					}
				}

			}
		}

		public void Update_PurchaseOrderDetail(int orderid, List<PurchasingOrderDetail> purchaseorderdetails)
		{
			List<string> reasons = new List<string>();

			using (var context = new ToolsRUsContext())
			{
				PurchaseOrder exists = context.PurchaseOrders
					.Where(x => x.PurchaseOrderID == orderid)
					.Select(x => x).FirstOrDefault();

				if (exists == null)
				{
					reasons.Add("Purchase Order no longer exists.");
				}
				else
				{
					if (reasons.Count > 0)
					{
						throw new BusinessRuleException("Update Order", reasons);
					}
					else
					{
						List<PurchaseOrderDetail> updateList = (from x in exists.PurchaseOrderDetails
																where x.PurchaseOrderID == orderid
																orderby x.PurchaseOrderDetailID
																select x).ToList();

						if (updateList == null)
						{
							reasons.Add("Purchase Order Detail no longer exists.");
						}
						else
						{
							decimal newTotal = 0;
							for (int index = 0; index < updateList.Count; index++)
							{
								updateList[index].Quantity = purchaseorderdetails[index].QtO;
								updateList[index].PurchasePrice = purchaseorderdetails[index].Price;
								context.Entry(updateList[index]).Property(y => y.Quantity).IsModified = true;
								context.Entry(updateList[index]).Property(z => z.PurchasePrice).IsModified = true;
								newTotal += updateList[index].PurchasePrice;
							}
							exists.SubTotal = newTotal;
							exists.TaxAmount = (decimal)0.05 * newTotal;

							context.SaveChanges();
						}
					}
				}
			}
		}
	}
}
