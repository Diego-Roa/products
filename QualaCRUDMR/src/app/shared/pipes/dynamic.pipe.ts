import { Pipe, PipeTransform, Inject, LOCALE_ID } from '@angular/core';
import { CurrencyPipe, DatePipe, DecimalPipe, PercentPipe, TitleCasePipe } from '@angular/common';

@Pipe({
  name: 'dynamic'
})
export class DynamicPipe implements PipeTransform {

  constructor(
    @Inject(LOCALE_ID) private locale: string
  ) {}

  transform(value: any, pipeName: string | undefined, args?: any[]): any {
    if (!pipeName) return value;

    switch (pipeName) {
      case 'date':
        const datePipe = new DatePipe(this.locale);
        return datePipe.transform(value, ...(args || []));
      case 'currency':
        const currencyPipe = new CurrencyPipe(this.locale);
        return currencyPipe.transform(value, ...(args || []));
      case 'number':
        const decimalPipe = new DecimalPipe(this.locale);
        return decimalPipe.transform(value, ...(args || []));
      case 'percent':
        const percentPipe = new PercentPipe(this.locale);
        return percentPipe.transform(value, ...(args || []));
      case 'titlecase':
        const titleCasePipe = new TitleCasePipe();
        return titleCasePipe.transform(value);
      default:
        return value;
    }
  }
}
