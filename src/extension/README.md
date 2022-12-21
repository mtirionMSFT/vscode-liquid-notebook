# VSCode Extension for Liquid Notebooks

This is a Visual Studio Code extension to support [Liquid](https://shopify.github.io/liquid/) Notebooks.

[Liquid](https://shopify.github.io/liquid/) is an open-source template language created by [Shopify](https://www.shopify.com/). The input is text based combined with Liquid commands, indicated by the use of braces {}. The text can be anything, like HTML, CSS, JSON and more. Shopify provided a full [reference to the Liquid language](https://shopify.github.io/liquid/basics/introduction/). It's a secure template language that is also very accessible for non-programmer audiences.

A Liquid Notebook can contain markdown blocks (text) and code blocks. Code blocks support (HTML) Liquid, but also offers a built-in `Settings` language. The extension contains a Liquid parser based on  the [open source Fluid framework](https://github.com/sebastienros/fluid). The parser is compiled for Windows and Linux.

This extension depends on these extensions which are automatically installed whit this extension:

* [Liquid](https://marketplace.visualstudio.com/items?itemName=sissel.shopify-liquid) for liquid syntax highlighting, auto formatting.
* [Shopify Liquid Template Snippets](https://marketplace.visualstudio.com/items?itemName=killalau.vscode-liquid-snippets) for easy snippets typing Liquid commands

To learn more about this extension, we have made available a Liquid Notebook called [Getting Started with Liquid Notebooks](https://github.com/mtirionMSFT/vscode-liquid-notebook/blob/main/docs/getting-started-with-liquid.liquidbook). This is a getting started guide on using the extension. If you want to learn more about using the Liquid language, another Liquid Notebook called [Getting Started with Liquid](https://github.com/mtirionMSFT/vscode-liquid-notebook/blob/main/docs/getting-started-with-liquid.liquidbook) is available as well. All sources, including these notebooks can be found in [the open source GitHub repo](https://github.com/mtirionMSFT/vscode-liquid-notebook).

## Data

The notebook will combine data and the provided Liquid to produce output in the Notebook. The following formats are supported:

* [Parquet format](https://parquet.apache.org/docs/). Files with extension .parquet.
* [Comma Separated Value (CSV) format](https://en.wikipedia.org/wiki/Comma-separated_values). Files with extension .csv.
* [JSON format](https://en.wikipedia.org/wiki/JSON). Files with extension .json.
* [.env files](https://docs.docker.com/compose/environment-variables/). Files with extension .env.

 These files should be placed in one folder. Parquet files and CSV files are treated as tables accessible via the `_model` object or directly accessible by the filename without extension. All other types are accessible directly by the filename without extension. This means that sample.json is accessible as `sample`.

The data model also make the `_env` available which provides all environment variables from your environment. A variable can be accessed using `{{ _env.COMPUTERNAME }}` or `{{ _env['COMPUTERNAME'] }}`.

### CSV Files

For a CSV file, every column is separated by a comma. The first line of the file is used as the list of field names. A sample of a well formed CSV file for Patients is this:

```shell
id,identifier,firstName,lastName,gender,birthDate,street,city,zipcode,phone
1000,1000.1234.1000,Benjamin,Morris,M,21-11-1973,3 Kensington Close,Widnes,WA8 3BA,7700 109091
1001,1001.1234.1001,Kimberly,Hall,F,22-11-1975,55 Kintore Drive,Great Sankey,WA5 3NP,7281 381935
1002,1002.1234.1002,Nicole,Parker,F,23-11-1976,21 Martham Close,Grappenhall,WA4 2LU,7911 228350
1003,1003.1234.1003,Heather,Williams,F,24-11-1977,429 Stockport Road,Timperley,WA15 7XR,7457 583736
1004,1004.1234.1004,Michael,Smith,M,25-11-1978,45 Littlegate,Halton Brook,WA7 2EE,7911 046649
1005,1005.1234.1005,Edward,Morgan,M,26-11-2001,16 Cowdell Street,Warrington,WA2 7PP,7700 188943
1006,1006.1234.1006,Mia,Anderson,F,27-11-2011,8 Tobermory Close,Haydock,WA11 0YP,7199 284153
1007,1007.1234.1007,Ava,Brown,F,28-11-2012,21 Yewdale Avenue,St Helens,WA11 7EY,7213 072284
1008,1008.1234.1008,Paul,Clark,M,29-11-2013,23 Primrose Close,Orford,WA2 9BS,7454 662873
1009,1009.1234.1009,Harry,Thompson,M,30-11-2014,88 Blundell Road,Widnes,WA8 8SN,7911 031431
```

### JSON Files

JSON files are accessible by using the name of the file without extension and where spaces are replace by a dash (e.g. 'sample file' is 'sample-file'). The JSON structure can then be navigated using properties by name and using indexers for arrays. So something like `sample-file.questions[0].question`.

> NOTE: if you use **model.json**, it will interfere with the standard `model` object for access to the tables. So don't use that as a name for a JSON file to prevent errors.

### .env Files

A .env file has a environment variable setting per line, where the format is `[variable name]=[value]`. An example:

```shell
DatabaseSettings__ConnectionString=server=(localdb)\MSSQLLocalDB;database=TEST;Integrated Security=true
SwaggerUI__ClientId=11111111-1111-1111-1111-111111111111
```

You can reference the variable either with `DatabaseSettings__ConnectionString` or `DatabaseSettings:ConnectionString`. We'll do a replacement for the search in the converter if needed.

### Sample data

Sample data can be found [in the repo as well](https://github.com/mtirionMSFT/vscode-liquid-notebook/blob/main/DemoContent/Data). The data model is explained there as well. It is a simplified model for FHIR HL7 Healthcare Claims. You can download that demo set and place it anywhere on your disk and configure it in your Liquid Notebook using Settings](#Settings).

## Templates

When you are developing (partial) Liquid Templates, you can place them in a folder as well and reuse in the notebook as well.

## Settings

To have the settings for your notebook, include a Settings language block at the top of your file and run it. You can use commands like this:

```yaml
DATA [path to the folder containing the data]
TEMPLATES [path to the folder containing templates for includes/render tags]
```
