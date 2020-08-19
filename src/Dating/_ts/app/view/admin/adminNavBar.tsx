import * as React from "react";
import { NavLink } from "react-router-dom";
import { Unsubscribe } from "redux";
import { ObjectComparer } from "../../../tool/objectComparer";
import { AppState } from "../../state/appState";
import { ReduxAppStateStore } from "../../state/reduxAppStateStore";

interface AdminNavBarState {
    userIsAdmin: boolean;
}

export class AdminNavBar extends React.Component<{}, AdminNavBarState> {
    constructor(props) {
        super(props);

        this._stateUnsubscribe = ReduxAppStateStore.subscribe(this.onStateChanged.bind(this));

        this.state = this.createNewComponentState();
    }

    private _comparer: ObjectComparer = new ObjectComparer();
    private _stateUnsubscribe: Unsubscribe;

    public componentWillUnmount() {
        this._stateUnsubscribe();
    }

    private onStateChanged(): void {
        let componentNewState = this.createNewComponentState();
        if (this._comparer.areDeepEqual(this.state, componentNewState))
            return;

        this.setState(componentNewState);
    }

    private createNewComponentState(): AdminNavBarState {
        let appState = ReduxAppStateStore.getState();
        let componentState = this.getComponentState(appState);
        return componentState;
    }

    private getComponentState(appState: AppState): AdminNavBarState {
        let auth = appState?.auth;
        let result: AdminNavBarState = {
            userIsAdmin: (auth?.userSignedIn && auth?.currentUser?.claims?.includes('Admin')) ?? false
        };
        return result;
    }

    public render() {
        if (!this.state.userIsAdmin)
            return '';

        return (
            <div className="super-nav">
                <NavLink to="/Admin/Dashboard/Index" className="btn">Dashboard</NavLink>
                <NavLink to="/Admin/Places/All" className="btn">Places</NavLink>
                <NavLink to="/Admin/Users/UsersList" className="btn">Users</NavLink>
                <NavLink to="/Admin/Locales/LocalesList" className="btn">Locales</NavLink>
                <NavLink to="/Admin/ObjectTypes" className="btn">Object types</NavLink>
                <NavLink to="/Admin/Basics/Objects" className="btn">Objects</NavLink>
                <NavLink to="/Admin/Translation/TranslationsList" className="btn">Objects Translations</NavLink>
            </div>
        );
    }
}