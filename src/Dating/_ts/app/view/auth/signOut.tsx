import * as React from "react";
import { Redirect } from "react-router-dom";
import { Unsubscribe } from "redux";
import { Http } from "../../../tool/http";
import { MergeTool } from "../../../tool/mergeTool";
import { ApiResponse } from "../../../_cs_to_ts/apiResponse";
import { ActionFactory } from "../../state/action/actionFactory";
import { ReduxAppStateStore } from "../../state/reduxAppStateStore";
import { ViewComponent } from "../../ViewComponent";


interface SignOutState {
    signedIn: boolean;
    hasToRedirectToHomePage: boolean;
}

export class SignOut extends ViewComponent<{}, SignOutState> {

    constructor(props) {
        super(props);

        this.state = {
            signedIn: false,
            hasToRedirectToHomePage: false
        } as SignOutState;

        this.onStateChanged = this.onStateChanged.bind(this);
        this.onSignOut = this.onSignOut.bind(this);

        this._stateUnsubscribe = ReduxAppStateStore.subscribe(this.onStateChanged);
    }

    private _stateUnsubscribe: Unsubscribe;

    public componentWillUnmount() {
        this._stateUnsubscribe();
    }

    public componentDidMount() {
        let appState = ReduxAppStateStore.getState();
        let userIsSignedIn = appState.auth != null && appState.auth.userSignedIn;
        let state = {
            signedIn: userIsSignedIn,
            hasToRedirectToHomePage: !userIsSignedIn
        } as SignOutState;
        this.setState(state);
    }

    private onStateChanged() {
        let newAppState = ReduxAppStateStore.getState();
        let newState = MergeTool.mergeDeep({}, this.state, {
            signedIn: newAppState.auth.userSignedIn,
            hasToRedirectToHomePage: !newAppState.auth.userSignedIn
        } as SignOutState);
        this.setState(newState);
    }

    private onSignOut(e: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
        new Http()
            .onUrlPost('/Api/Auth/SignOut')
            .onSuccess(data => {
                let response = JSON.parse(data) as ApiResponse<void>;
                if (response.success) {
                    let action = ActionFactory.UserSignedOut();
                    ReduxAppStateStore.dispatch(action);
                }
            })
            .send();
    }

    public render() {
        if (this.state.hasToRedirectToHomePage)
            return <Redirect to='/' />

        return (
            <div className="content-center">
                <div>
                    <div className="data-row">
                        <div className="data-row-label-box">
                            <span>Press the button to sing out...</span>
                        </div>
                        <div className="data-row-value-box">
                            <button className='btn' onClick={this.onSignOut}>{this.Localize('Sign_out')}</button>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}