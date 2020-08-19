import { ActionKey } from "../../../action/actionKey";
import { LocaleIdWithTranslationsLoadedAction } from "../../../action/interfaces/_init/localeIdWithTranslationsLoaded";
import { AppState } from "../../../appState";
import { IActionHandler } from "../../iActionHandler";

export class LocaleIdWithTranslationsLoadedHandler implements IActionHandler {
    actionType: string = ActionKey.localeIdWithTranslationsLoaded;

    getStateDeltaFromAction(lastState: AppState, action: LocaleIdWithTranslationsLoadedAction): AppState {
        let newStateDelta = {
            localization: {
                chosenLocaleId: action.localeId,
                loadedTranslations: {
                    translationsLocaleId: action.localeId,
                    translations: action.translations
                }
            }
        } as AppState;
        return newStateDelta;
    }
}