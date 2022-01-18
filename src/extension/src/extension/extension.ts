/**
 * Main logic of the extension.
 */
import path = require('path');
import * as vscode from 'vscode';
import { setLiquidParser } from './extensionGlobals';
import { SettingsCompletionProvider } from './languageCompletionProvider';
import { LiquidNotebookSerializer } from './notebookContentSerializer';
import { LiquidNotebookKernel } from './notebookExecutionKernel';

/**
 * Activation method called when extension is activated.
 * @param context - the visual studio code context for the extension.
 */
export function activate(context: vscode.ExtensionContext) {
  setLiquidParser(path.join(
    context.extensionPath, 
    "dist", 
    "LiquidParser", 
    "LiquidParser.exe"));

  context.subscriptions.push(
    vscode.workspace.registerNotebookSerializer(
      'liquid-notebook', new LiquidNotebookSerializer(), { transientOutputs: true }
    ),
    new LiquidNotebookKernel(),

    vscode.languages.registerCompletionItemProvider({ language: 'settings' }, 
      new SettingsCompletionProvider())
  );
  
}

/**
 * This method is called when your extension is deactivated
 */
export function deactivate() { }
