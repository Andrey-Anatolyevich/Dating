import { AdsListItem } from "../../_cs_to_ts/dating/AdsListItem";
import { TranslationInfo } from "../../_cs_to_ts/data/translationInfo";

export interface AppState {
    initialLoadingComplete: boolean,
    locations: LocationInfo[],
    dating: {
        filter: {
            country: LocationInfo,
            city: LocationInfo,
            age: {
                range: AgeRange,
                currentMin: number,
                currentMax: number
            }
        },
        ads: {
            items: AdsListItem[],
            isLoading: boolean
        }
    },
    auth: {
        signInInProgress: boolean,
        userSignedIn: boolean,
        currentUser: {
            email: string,
            login: string,
            claims: string[],
            localeCode: string,
            chosenPlaceId: number
        }
    },
    localization: {
        chosenLocaleId: number,
        loadedTranslations: {
            translationsLocaleId: number,
            translations: TranslationInfo[]
        }
    }
}

export class AppStateDefault implements AppState {
    initialLoadingComplete: boolean = false;
    locations: LocationInfo[] = [];
    dating: {
        filter: {
            country: LocationInfo,
            city: LocationInfo,
            age: {
                range: AgeRange,
                currentMin: 0,
                currentMax: 0
            }
        },
        ads: {
            items: [],
            isLoading: false
        }
    };
    auth: {
        signInInProgress: false,
        userSignedIn: false,
        currentUser: {
            email: '',
            login: '',
            claims: [],
            localeCode: '',
            chosenPlaceId: 0
        }
    };
    localization: {
        chosenLocaleId: 0,
        loadedTranslations: {
            translationsLocaleId: 0,
            translations: []
        }
    };
}