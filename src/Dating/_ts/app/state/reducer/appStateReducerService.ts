import { MergeTool } from '../../../tool/mergeTool';
import { ActionTypes } from '../action/actionTypes';
import { AppState, AppStateDefault } from '../appState';
import { SignInRequestHandler } from './actionHandler/auth/signInRequestHandler';
import { UserSignedInHandler } from './actionHandler/auth/userSignedInHandler';
import { UserSignedOutHandler } from './actionHandler/auth/userSignedOutHandler';
import { AdFilterNewAgeMaxHandler } from './actionHandler/dating/adFilterNewAgeMaxHandler';
import { AdFilterNewAgeMinHandler } from './actionHandler/dating/adFilterNewAgeMinHandler';
import { AdFilterNewCitySelectHandler } from './actionHandler/dating/adFilterNewCitySelectHandler';
import { AdFilterNewCountrySelectHandler } from './actionHandler/dating/adFilterNewCountrySelectHandler';
import { AdListItemsLoadedHandler } from './actionHandler/dating/adListItemsLoadedHander';
import { AdListItemsLoadHandler } from './actionHandler/dating/adListItemsLoadHander';
import { AgeRangeLoadedHandler } from './actionHandler/_init/ageRangeLoadedHandler';
import { InitialDataLoadedHandler } from './actionHandler/_init/initialDataLoadedHandler';
import { LocaleIdWithTranslationsLoadedHandler } from './actionHandler/_init/localeIdWithTranslationsLoadedHandler';
import { LocationsLoadedHandler } from './actionHandler/_init/locationsLoadedHandler';
import { IActionHandler } from './iActionHandler';

export class AppStateReducerService {

    constructor() {
        this.actionHandlersAll.map(handler => {
            if (this.actionHandlersLoaded[handler.actionType] != null)
                throw new Error(`Action Handler with key: ${handler.actionType} is already added to collection.`);

            this.actionHandlersLoaded[handler.actionType] = handler;
        });
    }

    private actionHandlersLoaded = {}
    private actionHandlersAll: IActionHandler[] = [
        new InitialDataLoadedHandler(),
        new LocationsLoadedHandler(),
        new LocaleIdWithTranslationsLoadedHandler(),
        new SignInRequestHandler(),
        new UserSignedInHandler(),
        new UserSignedOutHandler(),
        new AgeRangeLoadedHandler(),
        new AdFilterNewAgeMinHandler(),
        new AdFilterNewAgeMaxHandler(),
        new AdFilterNewCountrySelectHandler(),
        new AdFilterNewCitySelectHandler(),
        new AdListItemsLoadHandler(),
        new AdListItemsLoadedHandler()
    ];

    public AppStateReducer(oldState: AppState, action: ActionTypes): AppState {
        if (oldState == null) {
            return new AppStateDefault();
        }

        let actionHandler = this.actionHandlersLoaded[action.type] as IActionHandler;
        if (actionHandler == null)
            throw new Error(`Couldn't find ActionHandler for action of type: '${action.type}'.`);

        let newStateDelta = actionHandler.getStateDeltaFromAction(oldState, action);
        let newState = MergeTool.mergeDeep({} as AppState, oldState, newStateDelta);

        //this.logStates(action, oldState, newState);

        return newState;
    }

    private logStates(action: ActionTypes, oldState: AppState, newState: AppState) {
        console.log('-----------------------------------------------');
        console.log(`Action: ${action.type}:`);
        console.log(action);
        console.log(`Old state:`);
        console.log(oldState);
        console.log(`New state:`);
        console.log(newState);
    }
}