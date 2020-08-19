import { ActionKey } from "../../actionKey";

export interface AdFilterCitySelectAction {
    type: ActionKey.adFilterCitySelect,
    newCity: LocationInfo
}