import { Http } from "../../../tool/http";
import { ApiResponse } from "../../../_cs_to_ts/apiResponse";
import { ActionFactory } from "../../state/action/actionFactory";
import { ReduxAppStateStore } from "../../state/reduxAppStateStore";

export class CurrentUserLoader {
    public load(): Promise<string> {
        let loadDataPromise = new Http()
            .onUrlGet('/Api/Auth/GetCurrentUser')
            .onSuccess((data) => {
                let response = JSON.parse(data) as ApiResponse<SignInResponseModel>;
                if (response.success) {
                    let userSignedInAction = ActionFactory.UserSignedIn(response.value);
                    ReduxAppStateStore.dispatch(userSignedInAction);
                }
            })
            .onError((response) => {
                console.error(response);
            })
            .send();

        return loadDataPromise;
    }
}