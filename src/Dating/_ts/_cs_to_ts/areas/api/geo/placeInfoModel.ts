import { PlaceType } from "./placeType";

export class PlaceInfoModel {
    public id: number;
    public parentPlaceId: number;
    public placeCode: string;
    public placeType: PlaceType;
    public isEnabled: boolean;
}