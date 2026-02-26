# HtmlToPdfCore

A .NET 10 console application demonstrating how to generate a PDF document from an HTML template in C#.

## Overview

The app renders a [Mustache](https://mustache.github.io/) HTML template against a strongly-typed C# model, then converts the result to a PDF using [DinkToPdf](https://github.com/rdvojmoc/DinkToPdf) — a managed wrapper around the native `libwkhtmltox` library. No external process is spawned; conversion happens entirely in-process.

The included template is a quotation document built with [Materialize CSS](https://materializecss.com/) and demonstrates:

- Dynamic data binding via Mustache placeholders
- Table layout (line items)
- Conditions / terms section
- Signature fields
- Optional watermark

## Project Structure

```
HtmlToPdfCore/
├── HtmlToPdfCore.sln
└── src/
    ├── HtmlToPdfCore.csproj
    ├── Program.cs                  # Entry point
    ├── Services/
    │   ├── HtmlParser.cs           # Mustache template renderer
    │   └── HtmlToPdfService.cs     # HTML → PDF conversion via DinkToPdf
    ├── Models/
    │   ├── QuoteItem.cs            # Single line-item record
    │   └── QuoteTemplateData.cs    # Full quote view model
    ├── Data/
    │   └── SampleDataFactory.cs    # Hardcoded demo data (replace with your source)
    └── Templates/
        ├── quote.html              # Mustache HTML template
        ├── css/
        ├── images/
        └── js/
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run

```bash
cd src
dotnet run
```

The generated PDF is written to `bin/Debug/net10.0/Output/`.

## Customisation

1. Edit `Templates/quote.html` — use `{{Placeholder}}` for scalar values and `{{#Items}}...{{/Items}}` for lists.
2. Update `Models/QuoteTemplateData.cs` and `Models/QuoteItem.cs` to match your data shape.
3. Replace `Data/SampleDataFactory.cs` with your own data source (database, API, etc.).
4. Swap `Templates/images/logo.png` with your own logo.
5. Adjust paper size, margins, and orientation in `Services/HtmlToPdfService.cs` → `BuildPdfDocument()`.

## Dependencies

| Package | Purpose |
|---|---|
| `Haukcode.DinkToPdf` | HTML → PDF via native libwkhtmltox |
| `Stubble.Core` | Mustache template rendering |
| `Serilog` + `Serilog.Sinks.Console` | Structured logging |
| `Microsoft.Extensions.Hosting` | DI / hosting abstractions |

## License

[GNU General Public License](LICENSE)
