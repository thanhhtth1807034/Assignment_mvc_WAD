using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment.Models
{
    public class ShoppingCart
    {
        [Key]
        private Dictionary<int, CartItem> _cartItems = new Dictionary<int, CartItem>();
        private double _totalPrice = 0;

        public double GetTotalPrice()
        {
            this._totalPrice = 0;
            foreach (var items in _cartItems.Values)
            {
                this._totalPrice += items.UnitPrice * items.Quantity;
            }

            return this._totalPrice;
        }

        public Dictionary<int, CartItem> GetCartItems()
        {
            return _cartItems;
        }

        public void SetCartItems(Dictionary<int, CartItem> cartItems)
        {
            this._cartItems = cartItems;
        }

        public void AddCart(Product product, int quantity)
        {
            if (_cartItems.ContainsKey(product.Id))
            {
                var item = _cartItems[product.Id];
                item.Quantity += quantity;
                if (item.Quantity <= 0)
                {
                    _cartItems.Remove(product.Id);
                }
                else
                {
                    _cartItems[product.Id] = item;
                }
                return;
            }
            
            _cartItems.Add(product.Id, new CartItem(product, quantity));
        }

        public void RemoveFromCart(int productId)
        {
            _cartItems.Remove(productId);
        }
    }
}