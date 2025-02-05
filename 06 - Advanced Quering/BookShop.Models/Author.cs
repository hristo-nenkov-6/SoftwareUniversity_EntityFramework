namespace BookShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Author
    {
        public Author()
        {
            this.Books = new HashSet<Book>();
        }

        [Key]
        public int AuthorId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50), Required]
        public string LastName { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}