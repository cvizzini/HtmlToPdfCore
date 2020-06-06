# HtmlToPdfCore
A .Net Core Solution that parses an html template and converts to pdf document.

## Download the latest .NET Core SDK

* [.NET Core 3.1 SDK](release-notes/3.1/README.md)

## Description

Generate a pdf document from an html page generated with [Materialize Css](https://materializecss.com/). 

The PDF document is generated using the [wkhtmltopdf](https://wkhtmltopdf.org/) library. There are also linux and mac os versions of this library.

## Features

The template.html provide is an example of a quote which illustrate the following features:
- Basic Quote Layout
- Placeholders for actual data
- Table layout
- Signature fields
- Watermark

## Usage

1. Edit the template.html as required.
2. Update the Dictionary as per the placeholder fields within the template.html
3. Update the logo.png in images.
4. Run the solution.
5. View pdf output in the Output Directory in the Bin folder.

## License

[GNU GENERAL PUBLIC LICENSE](LICENSE.TXT)
