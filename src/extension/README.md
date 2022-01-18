# VSCode Extension for Liquid Notebooks

It's advisable to install these extensions as well:

* [Liquid](https://marketplace.visualstudio.com/items?itemName=sissel.shopify-liquid) for liquid syntax highlighting, auto formatting.
* [Shopify Liquid Template Snippets](https://marketplace.visualstudio.com/items?itemName=killalau.vscode-liquid-snippets) for easy snippets typing Liquid commands

⚠️ Follow the steps below, as the node_modules and LiquidParser.exe are required. ⚠️

## Running this Sample

1. npm install
2. In PowerShell run `.\script\build.ps1` to get the latest LiquidParser
3. Hit `F5` to build+debug

## Settings

To have the settings for your notebook, include a Settings language block at the top of your file and run it. You can use commands like this:

```yaml
DATA [path to the folder containing the data]
TEMPLATES [path to the folder containing templates for includes/render tags]
```

## Parser

This extension enables a Notebook that can contain Markdown blocks in combination with Code blocks. The Code blocks support [Liquid](https://shopify.github.io/liquid/) and the private Settings language to control notebook settings.

Parsing Liquid is done using a Console Application. That application can import Parquet files and CSV files which will be in memory in tables with the name of the file without the extension. This model is accessible in the Liquid code blocks.

The console app has to know where the data files are and where your templates files are located (in case you want to use the [include-](https://shopify.github.io/liquid/tags/template/#include) or [render-tag](https://shopify.github.io/liquid/tags/template/#render).
