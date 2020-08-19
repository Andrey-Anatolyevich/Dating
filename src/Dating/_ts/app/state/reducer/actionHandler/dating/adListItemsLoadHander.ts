import { Http } from "../../../../../tool/http";
import { AdsListFilterRequest } from "../../../../../_cs_to_ts/dating/adsListFilterRequest";
import { GetAdsModelReponse } from "../../../../../_cs_to_ts/dating/getAdsModelReponse";
import { ActionFactory } from "../../../action/actionFactory";
import { ActionKey } from "../../../action/actionKey";
import { AdListItemsLoadAction } from "../../../action/interfaces/dating/adListItemsLoadAction";
import { AppState } from "../../../appState";
import { ReduxAppStateStore } from "../../../reduxAppStateStore";
import { IActionHandler } from "../../iActionHandler";

export class AdListItemsLoadHandler implements IActionHandler {
    actionType: string = ActionKey.adListItemsLoad;

    getStateDeltaFromAction(lastState: AppState, action: AdListItemsLoadAction): AppState {
        new Http()
            .onUrlPost('/Api/Dating/GetListItems')
            .onSuccess(responseText => {
                let response = JSON.parse(responseText) as GetAdsModelReponse;
                let adsFiltered = response.ads
                    .filter(ad => ad.age >= lastState.dating.filter.age.currentMin
                        && ad.age <= lastState.dating.filter.age.currentMax);

                let action = ActionFactory.AdListItemsLoadedAction(adsFiltered);
                ReduxAppStateStore.dispatch(action);
            })
            .onError(responseText => {
                let action = ActionFactory.AdListItemsLoadedAction([]);
                ReduxAppStateStore.dispatch(action);
            })
            .send({
                placeId: lastState.dating.filter.city.Id
            } as AdsListFilterRequest);

        let newStateDelta = {
            dating: {
                ads: {
                    isLoading: true
                }
            }
        } as AppState;
        return newStateDelta;
    }
}