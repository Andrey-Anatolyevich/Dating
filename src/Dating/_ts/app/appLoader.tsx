import * as React from "react";
import { MergeTool } from "../tool/mergeTool";
import { InitialLoader } from './initialLoader';
import { AppState } from "./state/appState";
import { ReduxAppStateStore } from "./state/reduxAppStateStore";
import { AppShell } from './view/shell/appShell';

interface AppLoaderState {
    initialDataLoaded: boolean;
}

export class AppLoader extends React.Component<{}, AppLoaderState> {
    constructor(props) {
        super(props);

        this.state = { initialDataLoaded: false };
    }

    public componentDidMount() {
        ReduxAppStateStore.subscribe(this.globalStateUpdated.bind(this));
    }

    private globalStateUpdated() {
        let newAppState = ReduxAppStateStore.getState() as AppState;
        let newControlState = MergeTool.mergeDeep({}, this.state, { initialDataLoaded: newAppState.initialLoadingComplete } as AppLoaderState);
        this.setState(newControlState);
    }

    public render() {
        return (
            this.state.initialDataLoaded ? <AppShell /> : <InitialLoader />
        );
    }
}
