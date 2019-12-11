using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment.Models;

namespace Assignment.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        private static string SHOPPING_CART_NAME = "shoppingCart";
        private MyDBContext db = new MyDBContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddCart(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid Quantity");
            }

            var product = db.Products.Find(productId);
            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Product's' not found");
            }

            var sc = LoadShoppingCart();
            sc.AddCart(product, quantity);
            SaveShoppingCart(sc);
            return Redirect("/ShoppingCart/ShowCart");
        }

        public ActionResult RemoveCart(int productId)
        {
            var product = db.Products.Find(productId);
            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Product's' not found");
            }

            var sc = LoadShoppingCart();

            sc.RemoveFromCart(product.Id);

            SaveShoppingCart(sc);
            return Redirect("/ShoppingCart/ShowCart");
        }

        public ActionResult CreateOrder()
        {
            var transaction = db.Database.BeginTransaction();
            try
            {
                var shoppingCart = LoadShoppingCart();
                if (shoppingCart.GetCartItems().Count <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad request");
                }

                var order = new Order
                {
                    TotalPrice = shoppingCart.GetTotalPrice(),
                    MemberId = 1,
                    OrderDetails = new List<OrderDetail>()
                };

                foreach (var cartItem in shoppingCart.GetCartItems())
                {
                    var orderDetail = new OrderDetail()
                    {
                        ProductId = cartItem.Value.ProductId,
                        OrderId = order.Id,
                        Quantity = cartItem.Value.Quantity,
                        UnitPrice = cartItem.Value.UnitPrice
                    };
                    order.OrderDetails.Add(orderDetail);
                }

                db.Orders.Add(order);
                db.SaveChanges();
                ClearCart();
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
            }

            return Redirect("/Products");
        }

        private void ClearCart()
        {
            Session.Remove(SHOPPING_CART_NAME);
        }

        private void SaveShoppingCart(ShoppingCart shoppingCart)
        {
            Session[SHOPPING_CART_NAME] = shoppingCart;
        }

        public ActionResult ShowCart()
        {
            ViewBag.shoppingCart = LoadShoppingCart();
            return View();
        }

        private ShoppingCart LoadShoppingCart()
        {
            if (!(Session[SHOPPING_CART_NAME] is ShoppingCart sc))
            {
                sc = new ShoppingCart();
            }

            return sc;
        }
    }
}