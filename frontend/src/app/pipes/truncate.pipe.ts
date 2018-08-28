import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncate'
})
export class TruncatePipe implements PipeTransform {

  transform(value: string, limit?: number, trail?: string): any {
    const lim = limit ? limit : 20;
    const tr = trail ? trail : '...';

    if (value && value.length) {
      return value.length > lim ? value.substring(0, lim) + tr : value;
    }

    return '';
  }

}
