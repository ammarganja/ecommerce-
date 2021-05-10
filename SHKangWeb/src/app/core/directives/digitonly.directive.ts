import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  selector: 'input[digitOnly]'
})

/**
 * Digit Only Directive
 */
export class DigitonlyDirective {
  
  //#region Variables
  private navigationKeys = [
    'Backspace',
    'Delete',
    'Tab',
    'Escape',
    'Enter',
    'Home',
    'End',
    'ArrowLeft',
    'ArrowRight',
    'Clear',
    'Copy',
    'Paste'
  ];
  inputElement: HTMLElement;
  //#endregion

  //#region Constructor
  constructor(public el: ElementRef) {
    this.inputElement = el.nativeElement;
  }
  //#endregion

  //#region Listener
  @HostListener('keydown', ['$event'])
  /**
   * On Key Down
   */
  onKeyDown(e: KeyboardEvent) {
    if (
      this.navigationKeys.indexOf(e.key) > -1 || // Allow: navigation keys: backspace, delete, arrows etc.
      (e.ctrlKey === true && e.key === 'a') || // Allow: Ctrl+A
      (e.ctrlKey === true && e.key === 'c') || // Allow: Ctrl+C
      (e.ctrlKey === true && e.key === 'v') || // Allow: Ctrl+V
      (e.ctrlKey === true && e.key === 'x') || // Allow: Ctrl+X
      (e.metaKey === true && e.key === 'a') || // Allow: Cmd+A (Mac)
      (e.metaKey === true && e.key === 'c') || // Allow: Cmd+C (Mac)
      (e.metaKey === true && e.key === 'v') || // Allow: Cmd+V (Mac)
      (e.metaKey === true && e.key === 'x') // Allow: Cmd+X (Mac)
    ) {
      // let it happen, don't do anything
      return;
    }
    // Ensure that it is a number and stop the keypress
    if (
      (e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) &&
      (e.keyCode < 96 || e.keyCode > 105)
    ) {
      e.preventDefault();
    }
  }

  @HostListener('paste', ['$event'])
  /**
   * On Paste
   */
  onPaste(event: ClipboardEvent) {
    event.preventDefault();
    const pastedInput: string = event.clipboardData
      .getData('text/plain')
      .replace(/\D/g, ''); // get a digit-only string
    document.execCommand('insertText', false, pastedInput);
  }
  //#endregion
}