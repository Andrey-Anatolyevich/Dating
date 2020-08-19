import { SignInRequestAction } from "./interfaces/auth/signInRequestAction";
import { UserSignedInAction } from "./interfaces/auth/userSignedInAction";
import { UserSignedOutAction } from "./interfaces/auth/userSignedOutAction";
import { AdFilterCitySelectAction } from "./interfaces/dating/adFilterCitySelectAction";
import { AdFilterCountrySelectAction } from "./interfaces/dating/adFilterCountrySelectAction";
import { AdFilterNewAgeMaxAction } from "./interfaces/dating/adFilterNewAgeMaxAction";
import { AdFilterNewAgeMinAction } from "./interfaces/dating/adFilterNewAgeMinAction";
import { AdListItemsLoadAction } from "./interfaces/dating/adListItemsLoadAction";
import { AdListItemsLoadedAction } from "./interfaces/dating/adListItemsLoadedAction";
import { OtherAction } from "./interfaces/otherAction";
import { AgeRangeLoadedAction } from "./interfaces/_init/ageRangeLoadedAction";
import { InitialDataLoadedAction } from "./interfaces/_init/initialDataLoadedAction";
import { LocaleIdWithTranslationsLoadedAction } from "./interfaces/_init/localeIdWithTranslationsLoaded";
import { LocationsLoadedAction } from "./interfaces/_init/locationsLoadedAction";

export type ActionTypes =
    OtherAction
    | InitialDataLoadedAction
    | LocationsLoadedAction
    | LocaleIdWithTranslationsLoadedAction
    | SignInRequestAction
    | UserSignedInAction
    | UserSignedOutAction
    | AgeRangeLoadedAction
    | AdFilterNewAgeMinAction
    | AdFilterNewAgeMaxAction
    | AdFilterCountrySelectAction
    | AdFilterCitySelectAction
    | AdListItemsLoadAction
    | AdListItemsLoadedAction
    ;