import * as React from "react";
import { ActionFactory } from "./state/action/actionFactory";
import { ReduxAppStateStore } from "./state/reduxAppStateStore";
import { InitialLoaderService } from "./_init/initialLoaderService";

interface InitialLoaderState {
    failed: boolean,
    errorReason: string
}

export class InitialLoader extends React.Component<{}, InitialLoaderState> {

    constructor(params) {
        super(params);

        this.state = {
            failed: false
        } as InitialLoaderState;
    }

    public componentDidMount() {
        let loaderService = new InitialLoaderService();
        let loaderPromise = loaderService.load();

        loaderPromise.then(v => {
            let dataLoadedAction = ActionFactory.InitialDataLoaded();
            ReduxAppStateStore.dispatch(dataLoadedAction);
        }).catch(reason => {
            this.setState({
                failed: true,
                errorReason: reason
            } as InitialLoaderState);
        });
    };

    public render() {
        if (this.state.failed)
            return (
                <React.Fragment>
                    <span>Initial data loading failed...</span>
                    <p>{this.state.errorReason}</p>
                </React.Fragment>
            );

        return (
            <span>Loading...</span>
        );
    }
}
