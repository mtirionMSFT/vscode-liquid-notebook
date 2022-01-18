import * as vscode from "vscode";
import path = require("path");
import fs = require("fs");

/**
 * Get the full path for the given path. If it's already absolute it
 * will just be returned. Otherwise it will be joined with the folder
 * of the open file in the editor.
 * @param folder - folder path
 * @returns full path
 */
export function getFullPath(folder: string): string {
    if (path.isAbsolute(folder)) {
        // absolute path, so return it.
        return folder;
    }

    // NOTE: There is an issue with obtaining the path of the current
    // file all the time. Below is a fallback solution.
    // Issue is tracked at https://github.com/microsoft/vscode/issues/138523

    // we try to match the relative path in the folder where the notebook is located
    if (vscode.window.activeTextEditor) {
        console.log("Using the text editor for the path.");
        let f = path.dirname(vscode.window.activeTextEditor?.document.uri.fsPath),
            p = path.join(f, folder);
        // only return if path exists, otherwise we'll try the workspace folder
        if (pathExists(p)) {
            return p;
        }
    }

    // we try to match the relative path in the workspace folder
    if (vscode.workspace.workspaceFolders) {
        console.log("Using the workspace for the path.");
        let f = path.dirname(vscode.workspace.workspaceFolders?.[0].uri.fsPath),
            p = path.join(f, folder);
        // only return if path exists, otherwise we'll just return the input
        if (pathExists(p)) {
            return p;
        }
    }

    // default: just return the input folder
    return folder;
}

/**
 * Check if the path exists
 * @param path - path to check
 * @returns exists true or false
 */
export function pathExists(path: string): boolean {
    try {
        fs.statSync(path);
        return true;
    } catch {
        return false;
    }
}