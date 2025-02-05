namespace BookShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Category
    {
        public Category()
        {
            this.CategoryBooks = new HashSet<BookCategory>();
        }

        [Key]
        public int CategoryId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<BookCategory> CategoryBooks { get; set; }
    }
}