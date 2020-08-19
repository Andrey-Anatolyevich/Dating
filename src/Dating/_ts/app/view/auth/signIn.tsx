import * as React from "react";
import { ReduxAppStateStore } from "../../state/reduxAppStateStore";
import { Http } from "../../../tool/http";
import { SignInRequestModel } from "../../../_cs_to_ts/areas/api/auth/signInRequestModel";
import { ApiResponse } from "../../../_cs_to_ts/apiResponse";
import { BrowserRouter as Router, Route, Switch, Redirect, NavLink } from "react-router-dom";
import { ActionFactory } from "../../state/action/actionFactory";
import { MergeTool } from "../../../tool/mergeTool";
import { Unsubscribe } from "redux";
import { ViewComponent } from "../../ViewComponent";


interface SignInState {
    login: string;
    pass: string;
    signInProcessing: boolean;
    signedIn: boolean;
    errorHappened: boolean;
    errorCode: string;
}

export class SignIn extends ViewComponent<{}, SignInState> {

    constructor(props) {
        super(props);

        this.state = {
            login: '',
            pass: '',
            signInProcessing: false
        } as SignInState;

        this.onStateChanged = this.onStateChanged.bind(this);
        this.onLoginChange = this.onLoginChange.bind(this);
        this.onPassChange = this.onPassChange.bind(this);
        this.onSignIn = this.onSignIn.bind(this);

        this._stateUnsubscribe = ReduxAppStateStore.subscribe(this.onStateChanged);
    }

    private _stateUnsubscribe: Unsubscribe;

    public componentWillUnmount() {
        this._stateUnsubscribe();
    }

    private onStateChanged() {
        let newAppState = ReduxAppStateStore.getState();
        let newState = MergeTool.mergeDeep({}, this.state, {
            signInProcessing: newAppState.auth.signInInProgress,
            signedIn: newAppState.auth.userSignedIn
        } as SignInState);
        this.setState(newState);
    }

    private onLoginChange(e: React.ChangeEvent<HTMLInputElement>) {
        let newLogin = e.target.value;
        let newState = MergeTool.mergeDeep({}, this.state, { login: newLogin } as SignInState);
        this.setState(newState);
    }

    private onPassChange(e: React.ChangeEvent<HTMLInputElement>) {
        let newPass = e.target.value;
        let newState = MergeTool.mergeDeep({}, this.state, { pass: newPass } as SignInState);
        this.setState(newState);
    }

    private onSignIn(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();

        new Http()
            .onUrlPost('/Api/Auth/SignIn')
            .onSuccess(data => {
                let response = JSON.parse(data) as ApiResponse<SignInResponseModel>;
                if (response.success) {
                    let userSignedInAction = ActionFactory.UserSignedIn(response.value);
                    ReduxAppStateStore.dispatch(userSignedInAction);
                }
                else {
                    let newState = MergeTool.mergeDeep({}, this.state, {
                        errorHappened: true,
                        errorCode: response.errorCode
                    } as SignInState);
                    this.setState(newState);
                }
            })
            .send(new SignInRequestModel(this.state.login, this.state.pass));
    }

    public render() {
        if (this.state.signedIn)
            return <Redirect to='/' />

        return (
            <div className="content-center">
                <form onSubmit={this.onSignIn}>
                    {
                        this.state.errorHappened &&
                        <div className="data-row">
                            <div>
                                <span>{this.state.errorCode}</span>
                            </div>
                        </div>
                    }
                    <div className="data-row">
                        <div className="data-row-label-box">
                            <span>{this.Localize('Login')}</span>
                        </div>
                        <div className="data-row-value-box">
                            <input type="text" onChange={this.onLoginChange} value={this.state.login} autoFocus />
                        </div>
                    </div>
                    <div className="data-row">
                        <div className="data-row-label-box">
                            <span>{this.Localize('Password')}</span>
                        </div>
                        <div className="data-row-value-box">
                            <input type="password" onChange={this.onPassChange} value={this.state.pass} />
                        </div>
                    </div>
                    <div className="data-row">
                        <div className="data-row-label-box">
                        </div>
                        <div className="data-row-value-box">
                            <button className='btn'
                                disabled={this.state.signInProcessing}
                                type='submit'
                            >{this.Localize('Submit')}</button>
                            {
                                this.state.signInProcessing && <span>Signing in...</span>
                            }
                        </div>
                    </div>
                </form>
            </div>
        );
    }
}