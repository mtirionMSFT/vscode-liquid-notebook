import * as vscode from "vscode";
import { LIQUID_LANGUAGE, SETTINGS_LANGUAGE, setVerboseMode } from "./extensionGlobals";
import { executeSettings } from "./settingsLanguageProcessor";
import { executeLiquid } from "./liquidLanguageProcessor";

/**
 * Kernel of the Liquid Notebook implementation.
 * This takes care of running the code blocks.
 */
export class LiquidNotebookKernel {
    readonly id = "liquid-notebook-kernel";
    public readonly label = "Liquid Kernel";
    readonly supportedLanguages = ["liquid", "liquid-notebook-settings"];

    private _executionOrder = 0;
    private readonly _controller: vscode.NotebookController;

    /**
     * Constructor of the kernel.
     */
    constructor() {
        this._controller = vscode.notebooks.createNotebookController(
            this.id,
            "liquid-notebook",
            this.label
        );

        this._controller.supportedLanguages = this.supportedLanguages;
        this._controller.supportsExecutionOrder = true;
        this._controller.executeHandler = this._executeAll.bind(this);
    }

    /**
     * Dispose
     */
    dispose(): void {
        this._controller.dispose();
    }

    /**
     * Execute all code blocks handling.
     * @param cells - cells to execute
     * @param _notebook - current notebook
     * @param _controller - controller
     */
    private _executeAll(
        cells: vscode.NotebookCell[],
        _notebook: vscode.NotebookDocument,
        _controller: vscode.NotebookController
    ): void {
        for (let cell of cells) {
            this._doExecution(cell);
        }
    }

    /**
     * Execute the given code block.
     * @param cell - cell to execute
     */
    private async _doExecution(cell: vscode.NotebookCell): Promise<void> {
        const execution = this._controller.createNotebookCellExecution(cell);

        // always start with verbose mode OFF
        setVerboseMode(false);

        execution.executionOrder = ++this._executionOrder;
        // set start for timing and start the progress indicator
        execution.start(Date.now());

        try {
            if (cell.document.languageId === SETTINGS_LANGUAGE) {
                executeSettings(execution, cell);
            } else if (cell.document.languageId === LIQUID_LANGUAGE) {
                executeLiquid(execution, cell);
            }
        } catch (err: any) {
            execution.replaceOutput([
                new vscode.NotebookCellOutput([
                    vscode.NotebookCellOutputItem.error(err),
                ]),
            ]);
            execution.end(false, Date.now());
        }
    }
}
