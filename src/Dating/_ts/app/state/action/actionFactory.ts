import { ActionKey } from './actionKey';
import { SignInRequestAction } from './interfaces/auth/signInRequestAction';
import { UserSignedInAction } from './interfaces/auth/userSignedInAction';
import { UserSignedOutAction } from './interfaces/auth/userSignedOutAction';
import { AdFilterCitySelectAction } from './interfaces/dating/adFilterCitySelectAction';
import { AdFilterCountrySelectAction } from './interfaces/dating/adFilterCountrySelectAction';
import { AdFilterNewAgeMaxAction } from './interfaces/dating/adFilterNewAgeMaxAction';
import { AdFilterNewAgeMinAction } from './interfaces/dating/adFilterNewAgeMinAction';
import { AgeRangeLoadedAction } from './interfaces/_init/ageRangeLoadedAction';
import { InitialDataLoadedAction } from './interfaces/_init/initialDataLoadedAction';
import { LocationsLoadedAction } from './interfaces/_init/locationsLoadedAction';
import { AdListItemsLoadAction } from './interfaces/dating/adListItemsLoadAction';
import { AdsListItem } from '../../../_cs_to_ts/dating/AdsListItem';
import { AdListItemsLoadedAction } from './interfaces/dating/adListItemsLoadedAction';
import { TranslationInfo } from '../../../_cs_to_ts/data/translationInfo';
import { LocaleIdWithTranslationsLoadedAction } from './interfaces/_init/localeIdWithTranslationsLoaded';

export class ActionFactory {
    public static InitialDataLoaded(): InitialDataLoadedAction {
        return {
            type: ActionKey.initialDataLoaded
        }
    }

    public static LocationsLoaded(locations: Array<LocationInfo>): LocationsLoadedAction {
        return {
            type: ActionKey.locationsLoaded,
            locations: locations
        }
    }

    public static SignInRequested(login: string, pass: string): SignInRequestAction {
        return {
            type: ActionKey.signInRequest,
            login: login,
            pass: pass
        }
    }

    public static UserSignedIn(signInResponse: SignInResponseModel): UserSignedInAction {
        if (signInResponse == null)
            throw Error('signInResponse is null.');

        return {
            type: ActionKey.userSignedIn,
            signInResponse: signInResponse
        }
    }

    public static UserSignedOut(): UserSignedOutAction {
        return {
            type: ActionKey.userSignedOut
        }
    }

    public static AgeRangeLoaded(ageRange: AgeRange): AgeRangeLoadedAction {
        return {
            type: ActionKey.ageRangeLoaded,
            ageRange: ageRange
        };
    }

    public static EscortFilterNewAgeMin(newAgeMin: number): AdFilterNewAgeMinAction {
        return {
            type: ActionKey.adFilterNewAgeMin,
            newAgeMin: newAgeMin
        };
    }

    public static EscortFilterNewAgeMax(newAgeMax: number): AdFilterNewAgeMaxAction {
        return {
            type: ActionKey.adFilterNewAgeMax,
            newAgeMax: newAgeMax
        };
    }

    public static EscortFilterNewCity(newCity: LocationInfo): AdFilterCitySelectAction {
        return {
            type: ActionKey.adFilterCitySelect,
            newCity: newCity
        };
    }

    public static EscortFilterNewCountry(newCountry: LocationInfo): AdFilterCountrySelectAction {
        return {
            type: ActionKey.adFilterCountrySelect,
            newCountry: newCountry
        };
    }

    public static AdListItemsLoadAction(): AdListItemsLoadAction {
        return {
            type: ActionKey.adListItemsLoad
        };
    }

    public static AdListItemsLoadedAction(items: AdsListItem[]): AdListItemsLoadedAction {
        return {
            type: ActionKey.adListItemsLoaded,
            items: items
        };
    }

    public static GetLocaleWithTranslationsLoaded(localeId: number, translations: TranslationInfo[])
        : LocaleIdWithTranslationsLoadedAction {
        return {
            type: ActionKey.localeIdWithTranslationsLoaded,
            localeId: localeId,
            translations: translations
        };
    }
}