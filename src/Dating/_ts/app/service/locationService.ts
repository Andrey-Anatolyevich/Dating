import { Http } from "../../tool/http";

class LocationService {
    private _config = {
        enabledPlacesApiUrl: '/DataApi/GetEnabledPlaces'
    };

    private _state: {
        places: LocationInfo[],
        onPlacesLoaded: Function
    } = {
            places: null,
            onPlacesLoaded: null
        };

    public Init(onPlacesLoaded: Function) {
        this._state.onPlacesLoaded = onPlacesLoaded;

        this.loadPlaces();
    }

    private loadPlaces(): void {
        new Http()
            .onUrlGet(this._config.enabledPlacesApiUrl)
            .onSuccess((data) => {
                this._state.places = JSON.parse(data) as LocationInfo[];

                if (this._state.onPlacesLoaded != null)
                    this._state.onPlacesLoaded(this._state.places);
            })
            .onError((response) => {
                console.error(response);
            })
            .send();
    }
}