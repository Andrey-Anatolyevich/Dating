import * as React from "react";
import { MergeTool } from "../../../tool/mergeTool";
import { ReduxAppStateStore } from "../../state/reduxAppStateStore";
import { InlinePopup } from "../utils/inlinePopup";
import { ViewComponent } from "../../ViewComponent";

interface DatingListFiltersLocationProps {
    currentCountry: LocationInfo,
    currentCity: LocationInfo,
    setCountry: (country: LocationInfo) => void,
    setCity: (city: LocationInfo) => void
}

interface DatingListFiltersLocationState {
    availableLocations: LocationInfo[],
    openPopup: boolean,
    currentCountry: LocationInfo,
    currentCity: LocationInfo,
    popupCountry: LocationInfo,
    popupCity: LocationInfo
}

export class DatingListFiltersLocation
    extends ViewComponent<DatingListFiltersLocationProps, DatingListFiltersLocationState> {
    constructor(params) {
        super(params);

        var appState = ReduxAppStateStore.getState();
        this.state = {
            openPopup: false,
            availableLocations: appState.locations,
            currentCountry: this.props.currentCountry,
            currentCity: this.props.currentCity,
            popupCountry: this.props.currentCountry,
            popupCity: this.props.currentCity
        } as DatingListFiltersLocationState;

        this.handleChangeLocation = this.handleChangeLocation.bind(this);
        this.closePopup = this.closePopup.bind(this);
        this.countryPicked = this.countryPicked.bind(this);
        this.cityPicked = this.cityPicked.bind(this);
        this.confirmLocations = this.confirmLocations.bind(this);
    }

    private handleChangeLocation() {
        let newState = MergeTool.mergeDeep({}, this.state, { openPopup: !this.state.openPopup } as DatingListFiltersLocationState);
        this.setState(newState);
    }

    private closePopup() {
        let newState = MergeTool.mergeDeep({}, this.state, { openPopup: false } as DatingListFiltersLocationState);
        this.setState(newState);
    }

    private countryPicked(e: React.ChangeEvent<HTMLSelectElement>) {
        let pickedCountryId = parseInt(e.target.value);
        let pickedCountry = this.state.availableLocations.find(x => x.Id == pickedCountryId);
        let newState = MergeTool.mergeDeep({}, this.state, { popupCountry: pickedCountry } as DatingListFiltersLocationState);
        this.setState(newState);
    }

    private cityPicked(e: React.ChangeEvent<HTMLSelectElement>) {
        let pickedCityId = parseInt(e.target.value);
        let pickedCity = this.props.currentCountry.Children.find(x => x.Id == pickedCityId);
        let newState = MergeTool.mergeDeep({}, this.state, { popupCity: pickedCity} as DatingListFiltersLocationState);
        this.setState(newState);
    }

    private confirmLocations(e: React.MouseEvent<HTMLButtonElement, MouseEvent>) {
        let state = this.state;
        let newState = MergeTool.mergeDeep({}, state, {
            openPopup: false,
            currentCountry: state.popupCountry,
            currentCity: state.popupCity
        } as DatingListFiltersLocationState);
        this.props.setCountry(state.popupCountry);
        this.props.setCity(state.popupCity);
        this.setState(newState);
    }

    public render() {

        let countryLine = this.props.currentCountry != null ? this.props.currentCountry.DisplayName : '';
        let cityLine = this.props.currentCity != null ? this.props.currentCity.DisplayName : '';

        return (
            <React.Fragment>
                <span>{this.Localize('Location')}: </span>
                <span id="nav-locale-current">{cityLine}, {countryLine}</span>
                <button id="nav-locale-change-btn" className="btn" onClick={this.handleChangeLocation}>{this.Localize('change')}</button>

                <InlinePopup visible={this.state.openPopup} onClose={this.closePopup}>
                    <div>
                        <span>{this.Localize('Country')}</span>
                        <select onChange={this.countryPicked} value={this.state.popupCountry.Id}>
                            {
                                this.state.availableLocations.map(country => {
                                    return <option key={country.Id} value={country.Id}>{country.DisplayName}</option>
                                })
                            }
                        </select>
                    </div>
                    <div>
                        <span>{this.Localize('City')}</span>
                        <select onChange={this.cityPicked} value={this.state.popupCity.Id}>
                            {
                                this.state.popupCountry.Children.map(city => {
                                    return <option key={city.Id} value={city.Id}>{city.DisplayName}</option>
                                })
                            }
                        </select>
                    </div>
                    <div>
                        <button onClick={this.confirmLocations}>{this.Localize('OK')}</button>
                    </div>
                </InlinePopup>

            </React.Fragment>
        );
    }
}
