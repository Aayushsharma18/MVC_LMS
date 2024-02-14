using System;
using System.Collections.Generic;

namespace firstmvc.Models;

public partial class UserLib
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public int BookId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();
}
