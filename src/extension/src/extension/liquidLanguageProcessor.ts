import * as vscode from "vscode";
import { dataFolder, liquidList, liquidParser, templatesFolder } from "./extensionGlobals";
import { getFullPath } from "./pathHelpers";
import { exec } from "child_process";

/**
 * Execute a Liquid code block. We will add registerd commands first
 * and then we will add the code from the block. That will be passed
 * to the Liquid Parser. Output is rendered in the notebook cell output.
 * @param execution - execution context
 * @param cell - code block
 */
export function executeLiquid(
    execution: vscode.NotebookCellExecution,
    cell: vscode.NotebookCell) {
    var codeBlock: string = "";
    // first see if we have lines to add from global settings
    for (const cmd of liquidList) {
        codeBlock += cmd + "\n";
    }
    // now add the code from the notebook cell
    codeBlock += cell.document.getText();

    // build the commandline to execute
    var base64input = Buffer.from(codeBlock, "binary").toString("base64");
    let cmd = `${liquidParser} -i "${getFullPath(dataFolder)}" -t "${getFullPath(templatesFolder)}" -c ${base64input}`;

    // execute command line and handle errors and/or output
    exec(cmd, (error, stdout, stderr) => {
        if (error) {
            // see if we have stderr or stdout. In that case, we'll show that.
            var msg = stderr;
            if (stdout) {
                msg = stdout;
            }
            // some error calling the executable
            if (msg) {
                // show the output of the tool
                execution.replaceOutput([
                    new vscode.NotebookCellOutput([
                        vscode.NotebookCellOutputItem.error(new Error(msg)),
                    ]),
                ]);
            } else {
                // show the error from exec
                execution.replaceOutput([
                    new vscode.NotebookCellOutput([
                        vscode.NotebookCellOutputItem.error(error),
                    ]),
                ]);
            }
            execution.end(false, Date.now());
            return;
        }

        if (stderr) {
            // some error from the executable output
            execution.replaceOutput([
                new vscode.NotebookCellOutput([
                    vscode.NotebookCellOutputItem.text(stderr),
                ]),
            ]);
            execution.end(false, Date.now());
            return;
        }

        handleParserOutput(execution, stdout);
        execution.end(true, Date.now());
    });
}

/**
 * Handle the output from the parser command.
 * We will try to detect JSON or HTML output to properly format it in the UI.
 * Fallback is always plain text.
 * @param execution - execution context
 */
function handleParserOutput(
    execution: vscode.NotebookCellExecution,
    output: string) {
    var processed: boolean = false;
    try {
        if (output.startsWith("{")) {
            // JSON
            // try to parse the JSON and output as JSON
            execution.replaceOutput([
                new vscode.NotebookCellOutput([
                    vscode.NotebookCellOutputItem.json(
                        JSON.parse(output),
                        "application/json"
                    ),
                ]),
            ]);
            processed = true;
        } else if (output.indexOf("<html>") >= 0) {
            // HTML
            execution.replaceOutput([
                new vscode.NotebookCellOutput([
                    vscode.NotebookCellOutputItem.text(output, "text/html"),
                ]),
            ]);
            processed = true;
        }
    } catch (error) {
        output =
            "Process output error: " +
            error +
            ". Fallback to plain text\n\n" +
            output;
        console.log(
            "Process output error: " + error + ". Fallback to plain text"
        );
    } finally {
        if (!processed) {
            // all other: plain text
            execution.replaceOutput([
                new vscode.NotebookCellOutput([
                    vscode.NotebookCellOutputItem.text(output, "text/plain"),
                ]),
            ]);
        }
    }
}
