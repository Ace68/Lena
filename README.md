# Lena
Lena is a C# library providing functional programming primitives such as `Option` and `Result` types, inspired by languages like F# and Rust.  
It helps manage optional values and error handling in a more expressive and type-safe way.

## Core

- `Option<T>`: Represents an optional value, with methods for mapping, binding, and pattern matching.
- `Result<T>`: Represents a value or an error, for robust error handling.
- LINQ support for `Option<T>`.
- Unit tests for core laws and behaviors.

## Project Structure

- `Lena/`: Main library source code.
  - `Core/`: Core types (`Option`, `Result`, etc.).
  - `Extensions/`, `Functions/`, `Asyncs/`: Additional utilities.
- `Lena.Tests/`: Unit tests for the library.

## Getting Started

1. Clone the repository.
2. Open `Lena.sln` in JetBrains Rider.
3. Build the solution.
4. Run tests using the built-in test runner.

## Usage Example

```csharp
using Lena.Core;

var some = Option<int>.Some(42);
var none = Option<int>.None();

var result = some.Map(x => x * 2); // Option<int> with value 84
