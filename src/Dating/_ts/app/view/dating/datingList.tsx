import * as React from "react";
import { NavLink } from "react-router-dom";
import { Store, Unsubscribe } from "redux";
import { MergeTool } from "../../../tool/mergeTool";
import { ObjectComparer } from "../../../tool/objectComparer";
import { AdsListItem } from "../../../_cs_to_ts/dating/AdsListItem";
import { ActionFactory } from "../../state/action/actionFactory";
import { ActionTypes } from "../../state/action/actionTypes";
import { AppState } from "../../state/appState";
import { ReduxAppStateStore } from "../../state/reduxAppStateStore";
import { ViewComponent } from "../../ViewComponent";
import { DatingAdListItem } from "./datingAdListItem";
import { DatingAdListPager } from "./datingAdListPager";
import { DatingListFilters } from './datingListFilters';

interface EscortListState {
    isLoggedIn: boolean;
    adsListLoading: boolean;
    adsList: AdsListItem[];
    pageNumber: number;
}

const defaultState: EscortListState = {
    isLoggedIn: false,
    adsListLoading: false,
    adsList: [],
    pageNumber: 1
}

export class DatingList extends ViewComponent<{}, EscortListState> {
    constructor(props) {
        super(props);

        this._stateStore = ReduxAppStateStore;
        this.state = this.createNewComponentState();
        this._stateUnsubscribe = this._stateStore.subscribe(this.onStateChanged.bind(this));
    }

    private _itemsPerPage = 8;
    private _comparer: ObjectComparer = new ObjectComparer();
    private _stateStore: Store<AppState, ActionTypes>;
    private _stateUnsubscribe: Unsubscribe;

    public componentDidMount() {
        this.requestAds();
    }

    public componentWillUnmount() {
        this._stateUnsubscribe();
    }

    private onStateChanged(): void {
        let componentNewState = this.createNewComponentState();

        if (this._comparer.areDeepEqual(this.state, componentNewState))
            return;

        this.setState(componentNewState);
    }

    private createNewComponentState(): EscortListState {
        let appState = this._stateStore.getState();
        let componentState = this.getComponentState(appState);
        return componentState;
    }

    private getComponentState(appState: AppState): EscortListState {
        let newState: EscortListState = MergeTool.mergeDeep({}, defaultState);

        if (appState.dating?.ads != null) {
            newState.adsListLoading = appState.dating.ads.isLoading;
            newState.adsList = appState.dating.ads.items ?? [];
        }

        if (appState.auth != null)
            newState.isLoggedIn = appState.auth.userSignedIn;

        return newState;
    }

    private requestAds() {
        let loadItemsAction = ActionFactory.AdListItemsLoadAction();
        ReduxAppStateStore.dispatch(loadItemsAction);
    }

    private openPageNumber(newPageNumber: number): void {
        let newState = MergeTool.mergeDeep({}, this.state, {
            pageNumber: newPageNumber
        } as EscortListState);
        this.setState(newState);
    }

    public render() {
        let itemsToSkip = (this.state.pageNumber - 1) * this._itemsPerPage;
        let firstItemToTake = itemsToSkip;
        let lastItemToTake = firstItemToTake + this._itemsPerPage
        let adsToShow = this.state.adsList.slice(firstItemToTake, lastItemToTake);

        return <React.Fragment>
            {this.state.isLoggedIn && (
                <div className="data-row">
                    <div className="escort_user_panel">
                        <NavLink to="/Ads/MyAds" className="btn">{this.Localize('My_ads')}</NavLink>
                    </div>
                </div>
            )}
            <div className="data-row">
                <DatingListFilters applyFilters={this.requestAds.bind(this)} />
            </div>
            <main id="ads-list" className="data-block">
                <div id="ad-list-container" className="ad-list-container">
                    {
                        this.state.adsListLoading
                            ? <span>Loading...</span>
                            : adsToShow.map(ad => <DatingAdListItem key={ad.adId} ad={ad} />)
                    }
                </div>
                <DatingAdListPager
                    pageCurrent={this.state.pageNumber}
                    itemsTotal={this.state.adsList.length}
                    itemsPerPage={this._itemsPerPage}
                    openPageNumber={this.openPageNumber.bind(this)} />
            </main>
        </React.Fragment>
    }
}
