/**
 * Application variables and accessor methods
 */
 
 /**
  * Global constants. Most align with package.json values.
  */
 export const NOTEBOOK_ID: string = 'liquid-notebook';
 export const NOTEBOOK_KERNEL_ID: string = 'liquid-notebook-kernel';
 export const NOTEBOOK_KERNEL_NAME: string = 'Liquid Kernel';
 export const NOTEBOOK_RENDERER_ID: string = 'liquid-notebook-renderer';
 export const NOTEBOOK_SETTINGS_ID: string = 'liquid-notebook-settings';
  
 export const LIQUID_LANGUAGE: string = 'liquid';
 export const SETTINGS_LANGUAGE: string = 'liquid-notebook-settings';
 
 /**
  * LiquidParser path. set in activate from context.
  */
 export var liquidParser: string;
 
 /**
  * The folder with the data.
  * Set this property with the 'DATA' command to make
  * working against your data possible.
  */
 export var dataFolder: string = '.';
 
 /**
  * The folder with the templates.
  * Set this property with the 'TEMPLATES' command to make
  * include handling work on your templates.
  */
 export var templatesFolder: string = '.';
 
 /**
   * List of Liquid commands to add to every Liquid code-block execution.
   */
 export var liquidList: string[] = [];
 
 /**
  * Switch to show verbose mode for EACH code block.
  * This is reset per Settings code block.
  */
 export var verbose: boolean = false;
 
 /**
  * Set the dataFolder to the given string if not empty,
  * otherwise set to '.' for current directory.
  * @param dir - directory path
  */
 export function setDataFolder(value: string) {
   var dir: string = value.trim();
   if (dir.trim()) {
     dataFolder = dir;
   } else {
     dataFolder = '.';
   }
 }
 
 /**
  * Set the templatesFolder to the given string if not empty,
  * otherwise set to '.' for current directory.
  * @param dir - directory path
  */
 export function setTemplatesFolder(value: string) {
   var dir: string = value.trim();
   if (dir.trim()) {
     templatesFolder = dir;
   } else {
     templatesFolder = '.';
   }
 }
 
 /**
  * Set the liquid list to the given list if defined.
  * @param list - the list
  * @returns nothing
  */
 export function setLiquidList(list: string[]) {
     if(list === undefined) { return; }
     liquidList = list;
 }
 
 /**
  * Add given item to the liquidList.
  * @param item - command to add.
  */
 export function addToLiquidList(item: string) {
     liquidList.push(item);
 }
 
 /**
  * Remove given index from the list of Liquid commands.
  * @param index - index into the array to remove.
  */
 export function removeFromLiquidList(index: number) {
     if (index <= 0 || index > liquidList.length) {
         throw new Error('Cannot remove item from list with out of bounds index!');
     }
     liquidList.splice(index - 1, 1);
 }
 
 /**
  * Set the verbose mode.
  * @param verbose - true of false
  * @returns nothing
  */
  export function setVerboseMode(value: boolean) {
     verbose = value;
 }

  /**
  * Set liquid parser
  * @param value - liquid parser path
  * @returns nothing
  */
   export function setLiquidParser(value: string) {
    liquidParser = value;
}