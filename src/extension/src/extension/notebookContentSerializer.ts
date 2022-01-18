import * as vscode from 'vscode';
import { TextDecoder, TextEncoder } from "util";
import { dataFolder, liquidList, setDataFolder, setLiquidList, setTemplatesFolder, templatesFolder } from './extensionGlobals';

/**
 * Interface defining all notebook data
 */
interface RawNotebookData {
    dataFolder: string,
    templatesFolder: string,
    liquidList: string[],
    cells: RawNotebookCell[]
}

/**
 * Interface defining each code block.
 * We're not storing output in this notebook.
 */
interface RawNotebookCell {
  language: string;
  value: string;
  kind: vscode.NotebookCellKind;
}

/**
 * Serializer class with a serialize and deserialize method.
 */
export class LiquidNotebookSerializer implements vscode.NotebookSerializer {
  public readonly label: string = 'Liquid Notebook Serializer';

 /**
   * Deserialize the notebook contents on read.
   * @param data - content from the file.
   * @param token - cancellation token
   * @returns Deserialized data.
   */
  public async deserializeNotebook(data: Uint8Array, token: vscode.CancellationToken): Promise<vscode.NotebookData> {
    var contents = new TextDecoder().decode(data);    // convert to String to make JSON object

    // Read file contents
    let raw: RawNotebookData = { dataFolder: '.', templatesFolder: '.', liquidList: [], cells: [] };
    try {
      raw = <RawNotebookData>JSON.parse(contents);
    } catch {
      raw = { dataFolder: '.', templatesFolder: '.', liquidList: [], cells: [] };
    }

    if (raw.cells === undefined) {
        raw.cells = [];
    }

    // set the global vars in extension.ts
    setDataFolder(raw.dataFolder);
    setTemplatesFolder(raw.templatesFolder);
    setLiquidList(raw.liquidList);

    // Create array of Notebook cells for the VS Code API from file contents
    const cells = raw.cells.map(item => new vscode.NotebookCellData(
      item.kind, item.value, item.language
    ));

    // Pass read and formatted Notebook Data to VS Code to display Notebook with saved cells
    return new vscode.NotebookData(
      cells
    );
  }

  /**
   * Serialize the VS Code notebook with blocks to data for file storage.
   * @param data - data from VSC Code
   * @param token - cancellation token
   * @returns serialized data.
   */
  public async serializeNotebook(data: vscode.NotebookData, token: vscode.CancellationToken): Promise<Uint8Array> {
    // Map the Notebook data into the format we want to save the Notebook data as
    let contents: RawNotebookData = { dataFolder: '.', templatesFolder: '.', liquidList: [], cells: []};

    // general settings
    contents.dataFolder = dataFolder;
    contents.templatesFolder = templatesFolder;
    contents.liquidList = liquidList;

    // all blocks
    for (const cell of data.cells) {
      contents.cells.push({
        kind: cell.kind,
        language: cell.languageId,
        value: cell.value
      });
    }

    // Give a string of all the data to save and VS Code will handle the rest
    return new TextEncoder().encode(JSON.stringify(contents));
  }
}