import { createStore, Store } from "redux";
import { AppStateReducerService } from "./reducer/appStateReducerService";
import { AppState } from "./appState";
import { ActionTypes } from "./action/actionTypes";

let stateReducerStore = new AppStateReducerService();
stateReducerStore.AppStateReducer = stateReducerStore.AppStateReducer.bind(stateReducerStore);
let store = createStore(stateReducerStore.AppStateReducer);
export const ReduxAppStateStore: Store<AppState, ActionTypes> = store;