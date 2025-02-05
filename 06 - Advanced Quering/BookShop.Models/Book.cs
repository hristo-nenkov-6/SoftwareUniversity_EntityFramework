using BookShop.Models.Enums;

namespace BookShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Book
    {
        public Book()
        {
            this.BookCategories = new HashSet<BookCategory>();
        }

        [Key]
        public int BookId { get; set; }

        [MaxLength(50), Required]
        public string Title { get; set; }

        [MaxLength(1000), Required]
        public string Description { get; set; }

        public EditionType EditionType { get; set; }
        
        public decimal Price { get; set; }

        public int Copies { get; set; }
       
        public DateTime? ReleaseDate { get; set; }

        public AgeRestriction AgeRestriction { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public ICollection<BookCategory> BookCategories { get; set; }
    }
}
