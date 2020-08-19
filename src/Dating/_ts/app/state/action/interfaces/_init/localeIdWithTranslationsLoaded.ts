import { ActionKey } from "../../actionKey";
import { TranslationInfo } from "../../../../../_cs_to_ts/data/translationInfo";

export interface LocaleIdWithTranslationsLoadedAction {
    type: ActionKey.localeIdWithTranslationsLoaded,
    localeId: number,
    translations: TranslationInfo[]
}