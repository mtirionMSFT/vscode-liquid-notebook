import * as vscode from "vscode";
import { 
    addToLiquidList,
    dataFolder,
    liquidList,
    removeFromLiquidList,
    setDataFolder,
    setLiquidList,
    setTemplatesFolder,
    setVerboseMode,
    templatesFolder,
    verbose,
} from "./extensionGlobals";
import { getFullPath, pathExists } from "./pathHelpers";


/**
 * Execute a Settings code block. Most commands are to set a value
 * to be reused. Only LIST is just for showing the internal data.
 * All commands however will return information to the output for validation.
 * @param execution - execution context
 * @param cell - code block
 */
export function executeSettings(
    execution: vscode.NotebookCellExecution,
    cell: vscode.NotebookCell
) {
    var lines: string[] = cell.document.getText().split(/\r?\n/);
    var output: string = "";
    var result: boolean = true;

    for (const line of lines) {
        if (line.trim() && !line.trim().startsWith("#")) {
            // not commented, determine verb and paramter.
            var verb: string = "";
            var parameter: string = "";
            if (line.indexOf(" ") >= 0) {
                verb = line
                    .substring(0, line.indexOf(" "))
                    .trim()
                    .toUpperCase();
                parameter = line.substring(line.indexOf(" ") + 1).trim();
            } else {
                verb = line;
            }

            // handle verb
            switch (verb) {
                case "VERBOSE":
                    switch (parameter.toUpperCase()) {
                        case "TRUE":
                        case "ON":
                            setVerboseMode(true);
                            break;
                        case "FALSE":
                        case "OFF":
                            setVerboseMode(false);
                            break;
                    }
                    break;

                case "DATA":
                    setDataFolder(parameter);
                    if (verbose) {
                        output += 'Data folder set to: "' + dataFolder + '"\n';
                    }
                    break;
                case "TEMPLATES":
                    setTemplatesFolder(parameter);
                    if (verbose) {
                        output +=
                        'Templates folder set to: "' +
                        templatesFolder +
                        '"\n';
                    }
                    break;
                case "VALIDATE":
                    var folder: string = getFullPath(dataFolder);
                    if (! pathExists(folder)) {
                        output += "Error: Data folder '" + dataFolder + "' (" + folder + ") not found.\n";
                        result = false;
                    }
                    folder = getFullPath(templatesFolder);
                    if (! pathExists(folder)) {
                        output += "Error: Templates folder '" + templatesFolder + "' (" + folder + ") not found.";
                        result = false;
                    }
                    break;

                case "ADD":
                    addToLiquidList(parameter);
                    if (verbose) {
                        output += 'Command added: "' + parameter + '"\n';
                    }
                    break;
                case "REMOVE":
                    const index = parseInt(parameter);
                    if (index === NaN) {
                        throw new Error(
                            "Cannot remove item from list at index that does not exist!"
                        );
                    }
                    removeFromLiquidList(index);
                    if (verbose) {
                        output += "Command removed at index " + index + "\n";
                    }
                    break;
                case "CLEAR":
                    setLiquidList([]);
                    if (verbose) {
                        output += "Cleared all commands.\n";
                    }
                    break;
                case "LIST":
                    output += "Registered commands:\n";
                    for (const cmd of liquidList) {
                        output += cmd + "\n";
                    }
                    setVerboseMode(true);
                    break;
                default:
                    output += 'ERROR: Unknown command "' + verb + '"';
                    result = false;
                    break;
            }
        }
    }

    execution.replaceOutput([
        new vscode.NotebookCellOutput([
            vscode.NotebookCellOutputItem.text(output, "text/plain"),
        ]),
    ]);

    execution.end(result, Date.now());
}

