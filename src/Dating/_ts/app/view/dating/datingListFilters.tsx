import * as React from "react";
import { Unsubscribe } from "redux";
import { MergeTool } from "../../../tool/mergeTool";
import { ActionFactory } from "../../state/action/actionFactory";
import { ReduxAppStateStore } from "../../state/reduxAppStateStore";
import { DatingListFiltersLocation } from "./datingListFiltersLocation";
import { ViewComponent } from "../../ViewComponent";

interface DatingListFiltersProps {
    applyFilters: () => void;
}

interface DatingListFiltersState {
    countryCurrent: LocationInfo,
    cityCurrent: LocationInfo,
    age: {
        range: AgeRange,
        currentMin: number,
        currentMax: number
    }
}

export class DatingListFilters extends ViewComponent<DatingListFiltersProps, DatingListFiltersState> {
    constructor(params) {
        super(params);

        this._stateStore = ReduxAppStateStore;

        let appState = this._stateStore.getState();

        let datingFilter = appState.dating.filter;
        this.state = {
            countryCurrent: datingFilter.country,
            cityCurrent: datingFilter.city,
            age: {
                range: datingFilter.age.range,
                currentMin: datingFilter.age.currentMin,
                currentMax: datingFilter.age.currentMax
            }
        };

        this.onAppStateChanged = this.onAppStateChanged.bind(this);
        this.countryPicked = this.countryPicked.bind(this);
        this.cityPicked = this.cityPicked.bind(this);
        this.onAgeMinChanged = this.onAgeMinChanged.bind(this);
        this.onAgeMaxChanged = this.onAgeMaxChanged.bind(this);
        this.onApplyFilter = this.onApplyFilter.bind(this);

        this._stateUnsubscribe = this._stateStore.subscribe(this.onAppStateChanged);
    }

    private _stateStore: typeof ReduxAppStateStore;
    private _stateUnsubscribe: Unsubscribe;

    public componentWillUnmount() {
        this._stateUnsubscribe();
    }

    private onAppStateChanged(): void {
        let appState = this._stateStore.getState();
        let datingFilter = appState.dating.filter;
        let newControlState = MergeTool.mergeDeep({}, this.state, {
            countryCurrent: datingFilter.country,
            cityCurrent: datingFilter.city,
            age: {
                currentMin: datingFilter.age.currentMin,
                currentMax: datingFilter.age.currentMax
            }
        } as DatingListFiltersState);
        this.setState(newControlState);
    }

    private countryPicked(country: LocationInfo): void {
        let action = ActionFactory.EscortFilterNewCountry(country);
        this._stateStore.dispatch(action);
    }

    private cityPicked(city: LocationInfo): void {
        let action = ActionFactory.EscortFilterNewCity(city);
        this._stateStore.dispatch(action);
    }

    private onAgeMinChanged(e: React.ChangeEvent<HTMLSelectElement>) {
        let newAgeMin = parseInt(e.target.value);
        var action = ActionFactory.EscortFilterNewAgeMin(newAgeMin);
        this._stateStore.dispatch(action);
    }

    private onAgeMaxChanged(e: React.ChangeEvent<HTMLSelectElement>) {
        let newAgeMax = parseInt(e.target.value);
        var action = ActionFactory.EscortFilterNewAgeMax(newAgeMax);
        this._stateStore.dispatch(action);;
    }

    private onApplyFilter(e: React.MouseEvent<HTMLButtonElement, MouseEvent>) {
        this.props.applyFilters();
    }

    public render() {
        var ageRangeOptions = [];
        for (let age = this.state.age.range.ageMin; age <= this.state.age.range.ageMax; age++) {
            ageRangeOptions.push(<option key={age}>{age}</option>);
        }

        return (
            <div id="Filters" className="escort-filters">
                <div className="data-row">
                    <DatingListFiltersLocation
                        currentCountry={this.state.countryCurrent}
                        currentCity={this.state.cityCurrent}
                        setCountry={this.countryPicked}
                        setCity={this.cityPicked}
                    />
                </div>
                <div className="data-row">
                    <span>{this.Localize('Age')}:</span>
                    <select id="FiltersAgeFrom" className="escort-filters-age"
                        onChange={this.onAgeMinChanged}
                        value={this.state.age.currentMin}
                    >{ageRangeOptions}</select>
                    <span> - </span>
                    <select id="FiltersAgeTo" className="escort-filters-age"
                        onChange={this.onAgeMaxChanged}
                        value={this.state.age.currentMax}
                    >{ageRangeOptions}</select>
                </div>
                <div className="data-row">
                    <button className='btn' onClick={this.onApplyFilter}>{this.Localize('Filter')}</button>
                </div>
            </div>
        );
    }
}
