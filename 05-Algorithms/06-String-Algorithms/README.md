# String Algorithms

## Overview
String algorithms solve problems related to pattern matching, text processing, and string manipulation. Essential for text analysis, searching, and data processing.

## Fundamental Concepts

### String Properties
- **Immutability**: In C#, strings are immutable
- **Indexing**: 0-based access to characters
- **Substring**: Contiguous character sequence
- **Subsequence**: Not necessarily contiguous
- **Pattern**: Search target within text

## Pattern Matching Algorithms

### Naive/Brute Force Approach
- **Time**: O((n-m+1) × m) worst case, O(n) best
- **Space**: O(1)
- **Use**: Small patterns, simple implementation
- **Complexity**: Compare at each position

### Knuth-Morris-Pratt (KMP)
- **Time**: O(n + m)
- **Space**: O(m) for failure function
- **Advantage**: No backtracking in text
- **Key**: Failure function optimization

### Boyer-Moore Algorithm
- **Time**: O(n/m) best, O(nm) worst
- **Space**: O(alphabet size)
- **Advantage**: Fast practical performance
- **Key**: Bad character and good suffix heuristics

### Rabin-Karp Algorithm
- **Time**: O(n + m) average, O(nm) worst
- **Space**: O(1)
- **Use**: Multiple pattern search, plagiarism detection
- **Key**: Rolling hash optimization

### Aho-Corasick Algorithm
- **Time**: O(n + m + z) where z = output size
- **Space**: O(m × alphabet)
- **Use**: Multiple pattern matching
- **Key**: Trie-based finite automaton

## String Manipulation

### Reversal
- **Time**: O(n)
- **Space**: O(1) or O(n) depending on immutability
- **Variations**: Reverse substring, reverse words

### Rotation
- **Problem**: Is string B a rotation of string A?
- **Solution**: Check if B is substring of A+A
- **Time**: O(n) with efficient string search

### Anagram Detection
- **Approach 1**: Sort both strings, compare
- **Approach 2**: Character frequency counting
- **Time**: O(n log n) sorting, O(n) counting

### Palindrome Problems
- **Check**: Compare string with reverse
- **Longest Palindromic Substring**: DP or expand around center
- **Palindrome Partitioning**: DP or backtracking

## Advanced String Structures

### Trie (Prefix Tree)
- **Structure**: Node per character, path represents prefix
- **Time**: O(m) for insert/search/delete (m = word length)
- **Space**: O(alphabet size × average tree depth)
- **Use**: Autocomplete, spell checking, IP routing
- **Variants**: Suffix Trie, Compressed Trie

### Suffix Array
- **Definition**: Array of starting positions of all suffixes in sorted order
- **Construction**: O(n log n) simple, O(n) advanced
- **Use**: Pattern matching, longest repeated substring
- **With LCP**: Efficient substring queries

### Suffix Tree
- **Structure**: Compressed trie of all suffixes
- **Construction**: O(n) with Ukkonen's algorithm
- **Use**: All substring occurrences, longest repeated substring
- **Space**: O(n) but with large constants

### Z-Algorithm
- **Purpose**: Compute Z-array (length of longest substring matching prefix)
- **Time**: O(n)
- **Use**: Pattern matching, finding all occurrences
- **Alternative**: KMP-based approach

## String Analysis

### Longest Common Subsequence (LCS)
- **Time**: O(n × m)
- **Space**: O(n × m) or O(min(n,m)) optimized
- **Use**: Edit distance, DNA matching

### Longest Common Substring
- **Time**: O(n × m) DP, O(n + m) with suffix array
- **Space**: O(n × m)

### Edit Distance (Levenshtein)
- **Operations**: Insert, delete, replace
- **Time**: O(n × m)
- **Space**: O(n × m) or O(min(n,m)) with optimization
- **Use**: Spell checking, DNA analysis

### Minimum Window Substring
- **Time**: O(n + m)
- **Space**: O(alphabet size)
- **Use**: Find smallest window containing all pattern characters

## Character Encoding

### Unicode Handling
- **UTF-8**: Variable-length encoding
- **UTF-16**: Fixed 16-bit per character (C# default)
- **Considerations**: String length vs character count

### Case Conversion
- **Upper/Lower**: Simple character transformation
- **Considerations**: Locale-specific rules, performance

## Complexity Reference

| Algorithm | Time | Space | Best For |
|-----------|------|-------|----------|
| Naive Search | O(nm) | O(1) | Small patterns |
| KMP | O(n+m) | O(m) | Single pattern, guaranteed |
| Boyer-Moore | O(n/m) best | O(alphabet) | Practical fast search |
| Rabin-Karp | O(n+m) avg | O(1) | Multiple patterns |
| Aho-Corasick | O(n+m+z) | O(m×alphabet) | Multiple patterns |
| Trie Insert/Search | O(m) | O(alphabet×depth) | Prefix matching |
| Suffix Array | O(n log n) | O(n) | Substring queries |
| Edit Distance | O(n×m) | O(min(n,m)) | String similarity |

## Common Interview Problems

### Easy
- Palindrome checking
- Reverse string
- Anagram detection
- Valid parentheses

### Medium
- Longest palindromic substring
- Minimum window substring
- Edit distance
- Regular expression matching

### Hard
- Longest common subsequence
- Pattern matching (KMP, etc.)
- Wildcard matching
- Shortest palindrome

## Interview Tips

1. **Clarify Requirements**: Exact matching? Case-sensitive? Multiple patterns?
2. **Choose Algorithm**: Single pattern vs multiple, speed vs space
3. **Handle Edge Cases**: Empty strings, single character, entire string
4. **Test**: Various pattern positions (start, middle, end)
5. **Optimize**: Consider if multiple passes possible

## Practice Files
- **01-Explanation**: Pattern matching algorithms, string structures
- **02-Examples**: String problems with walkthroughs
- **03-Code-Implementations**: C# implementations of all algorithms
