import { ReduxAppStateStore } from "../state/reduxAppStateStore";
import { Unsubscribe } from "redux";
import { TranslationInfo } from "../../_cs_to_ts/data/translationInfo";


export class LocalizationSerice {
    constructor() {
        this.translations = [];
        this.unsubscribeFromAppStateChanged = ReduxAppStateStore.subscribe(this.AppStateChanged.bind(this));
        this.AppStateChanged();
    }

    private unsubscribeFromAppStateChanged: Unsubscribe;
    private currentLocaleId: number;
    private translations: TranslationInfo[];

    private static _instance: LocalizationSerice;
    public static Single(): LocalizationSerice {
        if (this._instance == null)
            this._instance = new LocalizationSerice();

        return this._instance;
    }

    private AppStateChanged() {
        let newAppState = ReduxAppStateStore.getState();
        this.currentLocaleId = newAppState.localization.loadedTranslations.translationsLocaleId;
        this.translations = newAppState.localization.loadedTranslations.translations;
    }

    public Localize(code: string): string {
        if (code == null || code == '')
            return '';

        let translationByCode = this.translations.find((translation) => translation.objectCode === code);
        if (translationByCode == null)
            return `[${code}]`;

        return translationByCode.value;
    }
}