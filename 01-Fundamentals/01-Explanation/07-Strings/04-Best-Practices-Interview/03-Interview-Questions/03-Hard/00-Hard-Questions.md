# Strings - Hard Interview Questions

## Q1: Design a thread-safe string builder pattern

**Answer:**
```csharp
public class ThreadSafeStringBuilder {
    private readonly StringBuilder sb = new();
    private readonly ReaderWriterLockSlim lockSlim = new();
    
    public void Append(string text) {
        lockSlim.EnterWriteLock();
        try {
            sb.Append(text);
        } finally {
            lockSlim.ExitWriteLock();
        }
    }
    
    public string ToString() {
        lockSlim.EnterReadLock();
        try {
            return sb.ToString();
        } finally {
            lockSlim.ExitReadLock();
        }
    }
}

// Or use immutable approach
public class ImmutableStringBuilder {
    private readonly ImmutableList<string> parts;
    
    public string ToString() => string.Concat(parts);
}
```

---

## Q2: Implement efficient string pooling

**Answer:**
```csharp
public class StringPool {
    private readonly Dictionary<string, string> pool = 
        new(StringComparer.Ordinal);
    private readonly object lockObj = new();
    
    public string Intern(string value) {
        if (value == null) return null;
        
        lock (lockObj) {
            if (!pool.TryGetValue(value, out var existing)) {
                pool[value] = value;
                return value;
            }
            return existing;
        }
    }
    
    public int Count => pool.Count;
}

// Usage
var pool = new StringPool();
string s1 = pool.Intern("Hello");
string s2 = pool.Intern("Hello");
bool same = ReferenceEquals(s1, s2);  // true
```

---

## Q3: Optimize large text processing

**Answer:**
```csharp
// Problem: Processing million-line file
IEnumerable<string> lines = File.ReadLines("largefile.txt");

// Inefficient - Load all into memory
List<string> allLines = lines.ToList();  // Memory spike

// Efficient - Stream processing
int processedCount = 0;
foreach (var line in lines) {
    if (line.Contains(searchTerm)) {
        Process(line);
        processedCount++;
    }
    
    // For aggregation, batch results
    if (processedCount % 1000 == 0) {
        GC.Collect();  // Allow GC to run
    }
}

// Key: Use IEnumerable for lazy evaluation
// Batch large operations
// Stream instead of loading all
```

---

## Q4: Complex regex with grouping and validation

**Answer:**
```csharp
// Match and extract structured data
string text = "Date: 2024-08-03, Time: 14:30:45, Status: Active";

string pattern = @"Date: (?<date>\d{4}-\d{2}-\d{2}),\s+Time: (?<time>\d{2}:\d{2}:\d{2}),\s+Status: (?<status>\w+)";

Match match = Regex.Match(text, pattern);
if (match.Success) {
    string date = match.Groups["date"].Value;  // "2024-08-03"
    string time = match.Groups["time"].Value;  // "14:30:45"
    string status = match.Groups["status"].Value;  // "Active"
}

// For many uses, compile
static readonly Regex StructuredRegex = 
    new Regex(pattern, RegexOptions.Compiled);
```

---

## Q5: Implement efficient string interning

**Answer:**
```csharp
public static class StringInternPool {
    private static readonly ConcurrentDictionary<string, string> internedStrings =
        new(StringComparer.Ordinal);
    
    public static string Intern(string value) {
        if (value == null) return null;
        return internedStrings.AddOrUpdate(value, value, (k, v) => v);
    }
}

// Pros: Memory savings for duplicate strings
// Cons: Lookup overhead
// Use when: Many duplicate strings in application lifetime
```

---

## Q6: Build a markup language parser

**Answer:**
```csharp
public class SimpleMarkdownParser {
    public string Parse(string markdown) {
        var sb = new StringBuilder();
        
        // Headers
        markdown = Regex.Replace(markdown, @"^### (.+)$", "<h3>$1</h3>", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, @"^## (.+)$", "<h2>$1</h2>", RegexOptions.Multiline);
        markdown = Regex.Replace(markdown, @"^# (.+)$", "<h1>$1</h1>", RegexOptions.Multiline);
        
        // Bold
        markdown = Regex.Replace(markdown, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
        
        // Italic
        markdown = Regex.Replace(markdown, @"\*(.+?)\*", "<em>$1</em>");
        
        // Links
        markdown = Regex.Replace(markdown, @"\[(.+?)\]\((.+?)\)", "<a href=\"$2\">$1</a>");
        
        return markdown;
    }
}
```

---

## Q7: Analyze and fix performance bottleneck

**Answer:**
```csharp
// Bottleneck: Multiple regex compilations
public string ProcessData(string[] items, string pattern) {
    var results = new List<string>();
    
    foreach (var item in items) {
        // WRONG - Recompiles regex each iteration!
        if (Regex.IsMatch(item, pattern)) {
            results.Add(item);
        }
    }
    
    return string.Join(",", results);
}

// FIX - Compile once
public string ProcessDataOptimized(string[] items, string pattern) {
    var regex = new Regex(pattern, RegexOptions.Compiled);
    var results = new List<string>();
    
    foreach (var item in items) {
        if (regex.IsMatch(item)) {
            results.Add(item);
        }
    }
    
    return string.Join(",", results);
}

// Improvement: 10-100x faster for many iterations
```

---

## Q8: Implement secure string comparison

**Answer:**
```csharp
// Constant-time comparison (prevents timing attacks)
public bool SecureStringCompare(string input, string expected) {
    if (input == null || expected == null) return false;
    if (input.Length != expected.Length) return false;
    
    int result = 0;
    for (int i = 0; i < input.Length; i++) {
        result |= input[i] ^ expected[i];
    }
    
    return result == 0;
}

// Usage for passwords
bool isValid = SecureStringCompare(inputPassword, storedPasswordHash);

// Why: Timing attacks can infer information from comparison time
// Normal comparison returns early on first difference
// Secure comparison always takes same time
```

---

## Q9: Design high-performance text index

**Answer:**
```csharp
public class TextIndex {
    private readonly Dictionary<string, HashSet<int>> index = new();
    private readonly string text;
    
    public TextIndex(string text) {
        this.text = text;
        BuildIndex();
    }
    
    private void BuildIndex() {
        // Simple word index
        var words = text.ToLower().Split(new[] { ' ', '\n', '\t' }, 
            StringSplitOptions.RemoveEmptyEntries);
        
        for (int i = 0; i < words.Length; i++) {
            if (!index.ContainsKey(words[i])) {
                index[words[i]] = new HashSet<int>();
            }
            index[words[i]].Add(i);
        }
    }
    
    public List<int> FindWord(string word) {
        word = word.ToLowerInvariant();
        return index.TryGetValue(word, out var positions) 
            ? positions.ToList() 
            : new List<int>();
    }
}
```

---

## Q10: Implement localization system

**Answer:**
```csharp
public class LocalizationService {
    private readonly Dictionary<string, Dictionary<string, string>> translations;
    private readonly CultureInfo defaultCulture;
    
    public LocalizationService(CultureInfo defaultCulture) {
        this.defaultCulture = defaultCulture;
        this.translations = new Dictionary<string, Dictionary<string, string>>();
    }
    
    public void Register(string key, string en, string es, string fr) {
        translations[key] = new Dictionary<string, string> {
            { "en", en },
            { "es", es },
            { "fr", fr }
        };
    }
    
    public string Get(string key, CultureInfo? culture = null) {
        culture ??= defaultCulture;
        
        if (translations.TryGetValue(key, out var variants)) {
            string cultureName = culture.TwoLetterISOLanguageName;
            if (variants.TryGetValue(cultureName, out var text)) {
                return text;
            }
        }
        
        return key;  // Fallback to key
    }
}

// Usage
var i18n = new LocalizationService(CultureInfo.GetCultureInfo("en-US"));
i18n.Register("greeting", "Hello", "Hola", "Bonjour");
string msg = i18n.Get("greeting", CultureInfo.GetCultureInfo("es-ES"));  // "Hola"
```

---

## Summary of Hard Concepts

✓ Thread-safe string operations
✓ String pooling and interning
✓ Large text processing optimization
✓ Advanced regex with groups
✓ Text indexing for search
✓ Performance analysis and fixing
✓ Secure string comparison
✓ Markup parsing
✓ Localization systems
✓ Memory and performance considerations

---

## Next Steps

1. Study production systems
2. Practice complex implementations
3. Review interview preparation
