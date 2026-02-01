# Lena

Lena is a comprehensive C# library providing functional programming primitives such as `Option`, `Result`, `Either`, and `Validation` types, inspired by languages like F#, Rust, and Haskell. It helps manage optional values, error handling, and functional patterns in a more expressive and type-safe way.

## Features

### Core Types

- **`Option<T>`**: Represents an optional value, eliminating null reference exceptions
- **`Result<T>`**: Represents a value or an error, for robust error handling  
- **`Either<TLeft, TRight>`**: Represents a value that can be one of two types
- **`Validation<T>`**: Like Result but accumulates multiple errors instead of failing fast
- **`Unit`**: Represents a type with only one value, useful in generic contexts

### Functional Programming Utilities

- **LINQ support** for all core types with `Select`, `SelectMany`, and `Where`
- **Higher-order functions**: Identity, Compose, Curry, Partial application, etc.
- **Async extensions** for `Option<T>` and `Result<T>`
- **Extension methods** for common patterns like `Tap`, `Pipe`, and sequence operations
- **Comprehensive collection extensions** with `FirstOrNone`, `Choose`, `Traverse`, `Sequence`

## Installation

```bash
# Clone the repository
git clone <repository-url>
cd Lena

# Build the solution
dotnet build

# Run tests
dotnet test
```

## Usage Examples

### Option<T>

```csharp
using Lena.Core;

// Creating Options
var some = Option<int>.Some(42);
var none = Option<int>.None();

// Using static helpers for type inference
var value = Option.Some(42);        // Option<int>
var empty = Option.None<string>();  // Option<string>

// Pattern matching
var result = some.Match(
    some: x => $"Value is {x}",
    none: () => "No value"
); // "Value is 42"

// Functional operations
var doubled = some
    .Map(x => x * 2)           // Some(84)
    .Filter(x => x > 50)       // Some(84)
    .GetOrElse(0);             // 84

// Chaining with Bind (monadic bind)
Option<int> ParseInt(string s) =>
    int.TryParse(s, out var i) ? Option.Some(i) : Option.None<int>();

var chain = Option.Some("42")
    .Bind(ParseInt)
    .Map(x => x * 2);          // Some(84)

// LINQ query syntax
var query = 
    from x in Option.Some(21)
    from y in Option.Some(21)
    select x + y;              // Some(42)
```

### Result<T>

```csharp
using Lena.Core;

// Creating Results
var success = Result<int>.Success(42);
var error = Result<int>.Error("Something went wrong");

// Using static helpers
var value = Result.Success(42);
var failure = Result.Error<int>("Failed");

// Safe execution
var result = Result.Try(() => int.Parse("42"));  // Success(42)
var failed = Result.Try(() => int.Parse("abc")); // Error(FormatException)

// Pattern matching
var message = result.Match(
    success: x => $"Got {x}",
    error: ex => $"Error: {ex.Message}"
);

// Chaining operations
Result<string> ProcessValue(int x) =>
    x > 0 ? Result.Success(x.ToString()) : Result.Error<string>("Must be positive");

var chain = Result.Success(42)
    .Map(x => x * 2)
    .Bind(ProcessValue);       // Success("84")

// Convert to Option
var option = success.ToOption();  // Some(42)
```

### Either<TLeft, TRight>

```csharp
using Lena.Core;

// Creating Either values
var right = Either<string, int>.Right(42);
var left = Either<string, int>.Left("error");

// Pattern matching
var result = right.Match(
    left: error => $"Error: {error}",
    right: value => $"Value: {value}"
); // "Value: 42"

// Functional operations (works on Right values)
var doubled = right.Map(x => x * 2);  // Right(84)
var leftStayed = left.Map(x => x * 2); // Left("error")
```

### Extension Methods and Collections

```csharp
using Lena.Core;

// Working with sequences
var numbers = new[] { "1", "2", "abc", "4" };

// Parse all numbers, filter out failures
var parsed = numbers
    .Choose(s => int.TryParse(s, out var i) ? Option.Some(i) : Option.None<int>())
    .ToList(); // [1, 2, 4]

// First element as Option
var first = numbers.FirstOrNone(); // Some("1")
var empty = new int[0].FirstOrNone(); // None

// Functional utilities
var result = 42
    .Pipe(x => x * 2)                    // 84
    .Tap(x => Console.WriteLine(x))      // Side effect, prints 84
    .Where(x => x > 50)                  // Some(84)
    .GetOrElse(0);                       // 84
```

## Design Philosophy

Lena follows functional programming principles:

- **Immutability**: All types are immutable by design
- **Null safety**: Eliminates null reference exceptions through `Option<T>`
- **Error handling**: Railway-oriented programming with `Result<T>` and `Validation<T>`
- **Composability**: All operations can be chained and composed
- **Type safety**: Leverages C#'s type system to prevent runtime errors

## Testing

Run tests with:
```bash
dotnet test
```

The library includes comprehensive unit tests covering core laws, edge cases, and integration scenarios.

## Project Structure

- `Lena/Core/`: Core types (`Option`, `Result`, `Either`, `Validation`, `Unit`)
- `Lena/Extensions/`: Extension methods for functional programming patterns
- `Lena/Functions/`: Higher-order functions and utilities  
- `Lena/Asyncs/`: Async support for core types
- `Lena.Tests/`: Comprehensive unit tests

## Contributing

Contributions are welcome! Please ensure all tests pass and add tests for new features.
