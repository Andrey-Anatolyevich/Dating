import { TranslationInfo } from "./translationInfo";

export class GetLocaleWithTranslationsResponse {
    public localeId: number;
    public translations: TranslationInfo[];
}