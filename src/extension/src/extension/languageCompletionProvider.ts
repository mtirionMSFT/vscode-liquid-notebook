import * as vscode from 'vscode';

/**
 * Class with methods for the Settings language completion provider.
 */
export class SettingsCompletionProvider implements vscode.CompletionItemProvider {
    /**
     * Provide list of items for intellisense handling in VS code for our
     * custom 'Settings' language. We support:
     * - DATA <folder>
     *   This sets the folder location containing the data
     * - TEMPLATES <folder>
     *   This sets the folder location containing liquid templates (and components)
     * - ADD <command>
     *   Add a Liquid command to each code block before execution
     * - REMOVE <index>
     *   Remove the liquid command on the given index of the list
     * - LIST
     *   List all Liquid commands
     * - CLEAR
     *   Clear all liquid commands
     * @param document - current document
     * @param position - position of the cursor
     * @param token - token
     * @param context - context
     * @returns List for intellisense with commands
     */
    provideCompletionItems(document: vscode.TextDocument, position: vscode.Position, token: vscode.CancellationToken, context: vscode.CompletionContext): vscode.ProviderResult<vscode.CompletionItem[] | vscode.CompletionList<vscode.CompletionItem>> {
        const result: vscode.CompletionItem[] = [];

        // create IntelliSense (autocompletes)
        result.push({
            label: 'VERBOSE',
            insertText: `VERBOSE `,
            detail: 'Switch verbose mode on or off for this code block.',
            kind: vscode.CompletionItemKind.Method
        });

        result.push({
            label: 'DATA',
            insertText: `DATA `,
            detail: 'Set the location of the data files.',
            kind: vscode.CompletionItemKind.Method
        });

        result.push({
            label: 'TEMPLATES',
            insertText: `TEMPLATES `,
            detail: 'Set the location of the templates.',
            kind: vscode.CompletionItemKind.Method
        });

        result.push({
            label: 'VALIDATE',
            insertText: `VALIDATE`,
            detail: 'Validate the settings.',
            kind: vscode.CompletionItemKind.Method
        });

        result.push({
            label: 'ADD',
            insertText: `ADD `,
            detail: 'Add a Liquid command to the list that will be added to all code executions.',
            kind: vscode.CompletionItemKind.Method
        });

        result.push({
            label: 'REMOVE',
            insertText: `REMOVE `,
            detail: 'Remove a Liquid command by number from the list.',
            kind: vscode.CompletionItemKind.Method
        });

        result.push({
            label: 'LIST',
            insertText: `LIST`,
            detail: 'List all Liquid commands.',
            kind: vscode.CompletionItemKind.Method
        });

        result.push({
            label: 'CLEAR',
            insertText: `CLEAR`,
            detail: 'Clear all Liquid commands.',
            kind: vscode.CompletionItemKind.Method
        });

        return result;
    }
}