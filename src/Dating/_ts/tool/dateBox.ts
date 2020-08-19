export class DateBox {
    private constructor() { }

    public utcDateString: string;
    public utcDate: Date;
    public localDate: Date;

    public static FromUtcDateString(utcDateString: string): DateBox {
        var result = new DateBox();
        if (utcDateString == null || utcDateString.length <= 0)
            return result;

        if (!utcDateString.endsWith('Z'))
            utcDateString += 'Z';

        result.utcDateString = utcDateString;
        result.utcDate = new Date(utcDateString);
        result.localDate = new Date(Date.UTC(result.utcDate.getFullYear(),
            result.utcDate.getMonth(), result.utcDate.getDate(),
            result.utcDate.getHours(), result.utcDate.getMinutes(), result.utcDate.getSeconds()));
        return result;
    }

    public ToLocalFullString(): string {
        var result = '' + this.localDate.getUTCFullYear()
            + '-' + (this.localDate.getUTCMonth() + 1).toString().padStart(2, '0')
            + '-' + (this.localDate.getUTCDate()).toString().padStart(2, '0')
            + ' ' + (this.localDate.getUTCHours()).toString().padStart(2, '0')
            + ':' + (this.localDate.getUTCMinutes()).toString().padStart(2, '0')
            + ':' + (this.localDate.getUTCSeconds()).toString().padStart(2, '0');
        return result;
    }
}