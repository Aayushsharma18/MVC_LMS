using System;
using System.Collections.Generic;

namespace firstmvc.Models;

public partial class BorrowedBook
{
    public int BorrowedId { get; set; }

    public int BookId { get; set; }

    public int UserId { get; set; }

    public DateTime BorrowDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual UserLib User { get; set; } = null!;
}
