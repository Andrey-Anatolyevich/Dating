import { ActionKey } from "../../actionKey";

export interface AgeRangeLoadedAction {
    type: ActionKey.ageRangeLoaded,
    ageRange: AgeRange
}
