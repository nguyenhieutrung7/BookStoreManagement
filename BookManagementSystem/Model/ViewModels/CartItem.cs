using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Model.Resources;

namespace Model.ViewModels
{
    public class CartItem
    {
        public BookViewModel Book { get; set; }
        [Display(Name ="Quantity", ResourceType =typeof(Book))]
        public int Quantity { get; set; }
    }
    public class Cart
    {
        private List<CartItem> listCartItems = new List<CartItem>();
        public void AddItem(BookViewModel book, int quantity)
        {
            //find cart existing
            CartItem item = listCartItems.Where(c => c.Book.BookID == book.BookID).FirstOrDefault();
            if (item == null)//cart doesn't exist in list yet 
            {
                this.listCartItems.Add(new CartItem { Book = book, Quantity = quantity });
            }
            else
            {
                item.Quantity += quantity;
            }
        }
        public IEnumerable<CartItem> ListCartItems
        {
            get
            {
                return listCartItems;
            }
        }
        //remove item
        public void RemoveItem(int bookID)
        {
            this.listCartItems.RemoveAll(c => c.Book.BookID == bookID);
        }
        //compute total price
        public decimal ComputeTotalPrice()
        {
            return listCartItems.Sum(c => c.Quantity * c.Book.Price);
        }
        //update Item
        public void UpdateItem(int bookID, int quantity)
        {
            CartItem item = this.listCartItems.FirstOrDefault(c => c.Book.BookID == bookID);
            item.Quantity = quantity;
        }
        //clear cart
        public void Clear()
        {
            this.listCartItems.Clear();
        }
    }

}

