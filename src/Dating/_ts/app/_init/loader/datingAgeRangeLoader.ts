import { Http } from "../../../tool/http";
import { ApiResponse } from "../../../_cs_to_ts/apiResponse";
import { ActionFactory } from "../../state/action/actionFactory";
import { ReduxAppStateStore } from "../../state/reduxAppStateStore";

export class DatingAgeRangeLoader {
    public load(): Promise<string> {
        let loadDataPromise = new Http()
            .onUrlGet('/Api/AdFilter/GetFilterAgeRange')
            .onSuccess((data) => {
                let ageRangeResponse = JSON.parse(data) as ApiResponse<AgeRange>;
                let action = ActionFactory.AgeRangeLoaded(ageRangeResponse.value);
                ReduxAppStateStore.dispatch(action);
            })
            .onError((response) => {
                console.error(response);
            })
            .send();

        return loadDataPromise;
    }
}