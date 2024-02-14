using System;
using System.Collections.Generic;

namespace firstmvc.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string Isbn { get; set; } = null!;

    public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();

    public virtual ICollection<UserLib> UserLibs { get; set; } = new List<UserLib>();
}
