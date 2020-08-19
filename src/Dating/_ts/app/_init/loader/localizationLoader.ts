import { Http } from "../../../tool/http";
import { ActionFactory } from "../../state/action/actionFactory";
import { ReduxAppStateStore } from "../../state/reduxAppStateStore";
import { GetLocaleWithTranslationsResponse } from "../../../_cs_to_ts/data/getLocaleWithTranslationsResponse";

export class LocalizationLoader {
    public load(): Promise<string> {

        let loadDataPromise = new Http()
            .onUrlGet('/DataApi/GetLocaleWithTranslations')
            .onSuccess((data) => {
                let response = JSON.parse(data) as GetLocaleWithTranslationsResponse;
                let action = ActionFactory.GetLocaleWithTranslationsLoaded(response.localeId, response.translations);
                ReduxAppStateStore.dispatch(action);

                return Promise.resolve();
            })
            .onError((response) => {
                console.error(response);

                return Promise.reject(response);
            })
            .send();

        return loadDataPromise;
    }
}