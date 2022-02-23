# Visual Studio Code extension for Liquid Notebooks

[Liquid](https://shopify.github.io/liquid/) is an open-source template language created by [Shopify](https://www.shopify.com/). The input is text based combined with Liquid commands, indicated by the use of braces {}. The text can be anything, like HTML, CSS, JSON and more. Shopify provided a full [reference to the Liquid language](https://shopify.github.io/liquid/basics/introduction/). It's a secure template language that is also very accessible for non-programmer audiences.

This repo contains the **Visual Studio Code Extension for Liquid Notebooks**. It is an extension using the [Visual Studio Code Notebook API](https://code.visualstudio.com/api/extension-guides/notebook) to offer a way to combine markdown and Liquid code. The use of this extension is up to you, but we used it for educating a customer on the Liquid language and the use of that language for their specific scenario.

This repo also contains two Liquid notebooks using the extension. Make sure the extension is installed before opening these notebooks.

* [Getting Started with Liquid Notebooks](./docs/getting-started-with-liquid-notebooks.liquidbook)
* [Getting Started with Liquid](./docs/getting-started-with-liquid.liquidbook)

The extension uses a console application `LiquidParser` to parse the liquid and post the results in the notebook. The console application is also included in this repo. The application makes use of the [Fluid](https://github.com/sebastienros/fluid) nuget package. The console application is compiled as a Windows executable depending on the .NET Framework 6 to be available on the platform. It is also compiled as a Linux executable with the .NET Framework 6 embedded in the executable.

## How to build

Use PowerShell to run the build.ps1 script in the root of the repo. This will build the LiquidParser for Windows and Linux, the extension and create the VSIX package in the root including the version using GitVersion.

## How to publish

TODO: add pipeline to publish to VSCode marketplace.
