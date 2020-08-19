export class MergeTool {
    private static isObject(item): boolean {
        return (item && typeof item === 'object' && !Array.isArray(item));
    }

    public static mergeDeep<T>(target: any, ...sources: T[]): T {
        if (!sources.length)
            return target;

        const source = sources.shift();

        if (this.isObject(target) && this.isObject(source)) {
            for (const key in source) {
                if (this.isObject(source[key])) {
                    if (JSON.stringify(source[key]) == JSON.stringify({})) {
                        if (key in target)
                            delete target[key];
                    }
                    else {
                        if (!(key in target))
                            Object.assign(target, { [key]: {} });
                        if (target[key] == null)
                            target[key] = {};

                        target[key] = this.mergeDeep<T>(target[key] as unknown as T, source[key] as unknown as T);
                    }
                } else {
                    Object.assign(target, { [key]: source[key] });
                }
            }
        }

        return this.mergeDeep<T>(target, ...sources);
    }
}