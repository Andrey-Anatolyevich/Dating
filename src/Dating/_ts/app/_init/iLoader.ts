export interface ILoader {
    load(): Promise<string>;
}