import { ActionKey } from "../../actionKey";
import { AdsListItem } from "../../../../../_cs_to_ts/dating/AdsListItem";

export interface AdListItemsLoadedAction {
    type: ActionKey.adListItemsLoaded,
    items: AdsListItem[]
}