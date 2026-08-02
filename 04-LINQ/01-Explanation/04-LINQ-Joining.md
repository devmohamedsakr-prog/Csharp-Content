# LINQ Joining Operations

## Overview
Join operations combine elements from two or more collections based on matching keys, similar to SQL JOINs.

## Join Operator

### Basic Join
```csharp
public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; }
}

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public int AuthorId { get; set; }
}

var authors = new List<Author>
{
    new Author { AuthorId = 1, Name = "Author A" },
    new Author { AuthorId = 2, Name = "Author B" }
};

var books = new List<Book>
{
    new Book { BookId = 1, Title = "Book 1", AuthorId = 1 },
    new Book { BookId = 2, Title = "Book 2", AuthorId = 1 },
    new Book { BookId = 3, Title = "Book 3", AuthorId = 2 }
};

// Inner join: Books with their authors
var joined = authors.Join(
    books,
    author => author.AuthorId,
    book => book.AuthorId,
    (author, book) => new { author.Name, book.Title }
);

// Result:
// { Name = "Author A", Title = "Book 1" }
// { Name = "Author A", Title = "Book 2" }
// { Name = "Author B", Title = "Book 3" }
```

### Query Syntax Join
```csharp
var result = from author in authors
             join book in books on author.AuthorId equals book.AuthorId
             select new { author.Name, book.Title };
```

## Group Join

### Left Join Behavior
```csharp
// Group join: Each author with all their books
var groupJoined = authors.GroupJoin(
    books,
    author => author.AuthorId,
    book => book.AuthorId,
    (author, authorBooks) => new
    {
        author.Name,
        Books = authorBooks.ToList()
    }
);

// Result:
// { Name = "Author A", Books = [Book 1, Book 2] }
// { Name = "Author B", Books = [Book 3] }
```

### Query Syntax Group Join
```csharp
var result = from author in authors
             join book in books on author.AuthorId equals book.AuthorId
             into authorBooks
             select new
             {
                 author.Name,
                 Books = authorBooks.ToList()
             };
```

## Left Join (Using DefaultIfEmpty)

### Simulating LEFT JOIN
```csharp
var leftJoin = authors.GroupJoin(
    books,
    author => author.AuthorId,
    book => book.AuthorId,
    (author, authorBooks) => new
    {
        author.Name,
        Books = authorBooks
    }
)
.SelectMany(
    x => x.Books.DefaultIfEmpty(),
    (x, book) => new
    {
        x.Name,
        BookTitle = book?.Title ?? "No Books"
    }
);
```

### Using LeftJoin Helper
```csharp
// Extension method for left join
public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
    this IEnumerable<TOuter> outer,
    IEnumerable<TInner> inner,
    Func<TOuter, TKey> outerKeySelector,
    Func<TInner, TKey> innerKeySelector,
    Func<TOuter, TInner, TResult> resultSelector)
{
    return from o in outer
           join i in inner on outerKeySelector(o) equals innerKeySelector(i) into joined
           from j in joined.DefaultIfEmpty()
           select resultSelector(o, j);
}

// Usage
var leftJoin = authors.LeftJoin(
    books,
    a => a.AuthorId,
    b => b.AuthorId,
    (a, b) => new { a.Name, BookTitle = b?.Title ?? "No Books" }
);
```

## Multiple Key Joins

### Composite Keys
```csharp
public class Sale
{
    public string ProductId { get; set; }
    public string Region { get; set; }
    public decimal Amount { get; set; }
}

public class Target
{
    public string ProductId { get; set; }
    public string Region { get; set; }
    public decimal TargetAmount { get; set; }
}

var sales = new List<Sale> { /* ... */ };
var targets = new List<Target> { /* ... */ };

// Join on multiple keys using anonymous types
var comparison = sales.Join(
    targets,
    s => new { s.ProductId, s.Region },
    t => new { t.ProductId, t.Region },
    (s, t) => new
    {
        s.ProductId,
        s.Region,
        s.Amount,
        t.TargetAmount,
        Achievement = s.Amount >= t.TargetAmount
    }
);
```

## Multiple Collection Joins

### Chaining Joins
```csharp
public class Publisher
{
    public int PublisherId { get; set; }
    public string Name { get; set; }
}

// Book extended with PublisherId
// public class Book
// {
//     public int BookId { get; set; }
//     public string Title { get; set; }
//     public int AuthorId { get; set; }
//     public int PublisherId { get; set; }
// }

var publishers = new List<Publisher> { /* ... */ };

// Join authors, books, and publishers
var complex = authors.Join(
    books,
    a => a.AuthorId,
    b => b.AuthorId,
    (a, b) => new { Author = a, Book = b }
)
.Join(
    publishers,
    ab => ab.Book.PublisherId,
    p => p.PublisherId,
    (ab, p) => new
    {
        ab.Author.Name,
        ab.Book.Title,
        PublisherName = p.Name
    }
);
```

### Using Query Syntax for Multiple Joins
```csharp
var result = from author in authors
             join book in books on author.AuthorId equals book.AuthorId
             join publisher in publishers on book.PublisherId equals publisher.PublisherId
             select new
             {
                 author.Name,
                 book.Title,
                 publisher.Name
             };
```

## Zip Operator

### Combining Two Collections
```csharp
var list1 = new List<int> { 1, 2, 3 };
var list2 = new List<string> { "A", "B", "C" };

// Combine elements at same position
var zipped = list1.Zip(list2, (num, letter) => $"{num}-{letter}");
// ["1-A", "2-B", "3-C"]

// Three collections
var list3 = new List<bool> { true, false, true };
var triplet = list1.Zip(list2, list3, (n, l, b) => $"{n}-{l}-{b}");
// ["1-A-True", "2-B-False", "3-C-True"]
```

## Cross Join

### Cartesian Product
```csharp
var colors = new List<string> { "Red", "Green", "Blue" };
var sizes = new List<string> { "S", "M", "L" };

// All combinations
var combinations = colors.SelectMany(c => sizes.Select(s => $"{c} {s}"));
// ["Red S", "Red M", "Red L", "Green S", ...]

// Using query syntax
var result = from color in colors
             from size in sizes
             select $"{color} {size}";
```

## Best Practices

1. **Use Query Syntax for Complex Joins**: More readable
```csharp
// Better readability
var result = from a in authors
             join b in books on a.AuthorId equals b.AuthorId
             select new { a.Name, b.Title };
```

2. **Index Collections Before Joining**: Performance improvement
```csharp
// For repeated joins
var booksByAuthor = books.ToLookup(b => b.AuthorId);
var joined = authors.Select(a => new
{
    Author = a,
    Books = booksByAuthor[a.AuthorId]
});
```

3. **Filter Before Joining**: Reduce data volume
```csharp
// Good: Filter first
var activeBooks = books.Where(b => b.IsActive).ToList();
var joined = authors.Join(activeBooks, ...);
```

## Common Mistakes

1. **Forgetting Key Equality Requirements**
```csharp
// Bad: Keys have different types
var joined = authors.Join(
    books,
    a => a.AuthorId, // int
    b => b.AuthorIdString, // string - won't work!
    (a, b) => new { a.Name, b.Title }
);
```

2. **N+1 Query Problem with Joins**
```csharp
// Bad: Query per author
var result = authors.Select(a => new
{
    a.Name,
    Books = books.Where(b => b.AuthorId == a.AuthorId).ToList()
}).ToList();

// Good: Single join
var result = authors.Join(books, ...).ToList();
```

3. **Not Handling Null in Left Joins**
```csharp
// Bad: Null reference exception if no match
var leftJoin = authors.GroupJoin(books, ...)
    .SelectMany(x => x.Books)
    .Select(b => b.Title.ToUpper()); // Throws if Books is empty!

// Good: Handle null/empty
var leftJoin = authors.GroupJoin(books, ...)
    .SelectMany(x => x.Books.DefaultIfEmpty())
    .Select(b => b?.Title?.ToUpper() ?? "No Book");
```

## Quick Summary
- Join combines collections on matching keys
- GroupJoin creates grouped results
- DefaultIfEmpty enables left join behavior
- Multiple keys use anonymous types
- Zip combines parallel positions
- Use query syntax for complex joins
- Index for performance with repeated joins
- Handle nulls in left joins properly

## Resources
- Join Operations (LINQ)
- Left, Right, and Full Joins in LINQ
- LINQ Performance considerations
