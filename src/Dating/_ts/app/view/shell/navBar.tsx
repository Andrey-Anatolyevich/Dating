import * as React from "react";
import { NavLink } from "react-router-dom";
import { Unsubscribe } from "redux";
import { ObjectComparer } from "../../../tool/objectComparer";
import { AppState } from "../../state/appState";
import { ReduxAppStateStore } from "../../state/reduxAppStateStore";
import { ViewComponent } from "../../ViewComponent";

interface NavBarState {
    isLoggedIn: boolean;
    userLogin: string;
}

const defaultState: NavBarState = {
    isLoggedIn: false,
    userLogin: ''
}

export class NavBar extends ViewComponent<{}, NavBarState> {
    constructor(params) {
        super(params);

        this._stateStore = ReduxAppStateStore;
        this.state = this.createNewComponentState();
        this._stateUnsubscribe = this._stateStore.subscribe(this.onStateChanged.bind(this));
    }

    private _comparer: ObjectComparer = new ObjectComparer();
    private _stateUnsubscribe: Unsubscribe;
    private _stateStore: typeof ReduxAppStateStore;

    public componentWillUnmount() {
        this._stateUnsubscribe();
    }

    private onStateChanged(): void {
        let componentNewState = this.createNewComponentState();
        if (this._comparer.areDeepEqual(this.state, componentNewState))
            return;

        this.setState(componentNewState);
    }

    private createNewComponentState(): NavBarState {
        let appState = this._stateStore.getState();
        let componentState = this.getComponentState(appState);
        return componentState;
    }

    private getComponentState(appState: AppState): NavBarState {
        let newState: NavBarState = defaultState;
        if (appState.auth != null) {
            newState = {
                isLoggedIn: appState.auth.userSignedIn,
                userLogin: appState.auth.currentUser?.login ?? ''
            };
        }
        return newState;
    }

    public render() {
        return (
            <nav className="block-navbar">
                <div className="content-center block-narrowed">
                    <div className="nav-main-flex">
                        <div className="nav-left-part">
                            <div>
                                <NavLink to='/'>[LOGO]</NavLink>
                            </div>
                        </div>
                        <div className="nav-right-part">
                            <div className="nav-buttons-container">
                                <div>
                                    <NavLink to='/'>{this.Localize('Home')}</NavLink>
                                </div>
                                <div>
                                    <NavLink to='/Dating'>{this.Localize('Dating')}</NavLink>
                                </div>
                            </div>
                            <div className="user-info-container">
                                <div>
                                    {
                                        this.state.isLoggedIn ? (
                                            <React.Fragment>
                                                <span>{this.Localize('My_Account')}: </span>
                                                <NavLink to='/Account/Management/MyAccount'>
                                                    <span>{this.state.userLogin}</span>
                                                </NavLink>
                                                <span> / </span>
                                                <NavLink to='/SignOut'>{this.Localize('Sign_out')}</NavLink>
                                            </React.Fragment>
                                        ) : (
                                            <React.Fragment>
                                                <NavLink to='/SignIn'>{this.Localize('Sign_in')}</NavLink>
                                                <span> / </span>
                                                <NavLink to='/Account/Management/Register'>{this.Localize('Register')}</NavLink>
                                            </React.Fragment>
                                        )
                                    }
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </nav>
        )
    }
}
