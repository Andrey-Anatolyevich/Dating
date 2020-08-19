
class PlacePicker {
    private _config = {
        currentPlaceSpanId: 'nav-locale-current',
        changePlaceBtnId: 'nav-locale-change-btn',
        popupClasses: ['popup-container', 'content-center'],
        popupContentClasses: ['popup-content'],
        classOverflowHidden: 'overflow-hidden',
        classLabelBox: 'data-row-label-box',
        classDataRow: 'data-row',
        classHide: 'hide',
        classBtn: 'btn',
        textSet: 'Set',
        textCountry: 'Country',
        textCity: 'City'
    };

    private _state: {
        placesProvider: PlacesDataProvider,
        allPlaces: PlaceInfo[],
        onPlacePicked: Function,
        elementCurrentPlaceSpan: HTMLElement,
        elementChangePlaceBtn: HTMLElement,
        body: HTMLBodyElement,
        popupContainer: HTMLDivElement,
        popupContentContainer: HTMLDivElement,
        elementCountries: HTMLSelectElement,
        elementCities: HTMLSelectElement
    } = {
            placesProvider: null,
            allPlaces: null,
            onPlacePicked: null,
            elementCurrentPlaceSpan: null,
            elementChangePlaceBtn: null,
            body: null,
            popupContainer: null,
            popupContentContainer: null,
            elementCountries: null,
            elementCities: null
        };

    public Init(onPlacePicked: Function): void {
        this._state.onPlacePicked = onPlacePicked;

        this._state.placesProvider = new PlacesDataProvider();
        this._state.placesProvider.Init(this.allPlacesLoaded.bind(this));

        this._state.body = document.body as HTMLBodyElement;
        this._state.elementCurrentPlaceSpan = document.getElementById(this._config.currentPlaceSpanId);
        this._state.elementChangePlaceBtn = document.getElementById(this._config.changePlaceBtnId);

        this.initDom();
    }

    private initDom(): void {
        // Big box
        let popupBox = this._state.popupContainer = document.createElement('div') as HTMLDivElement;
        this._config.popupClasses.forEach(x => popupBox.classList.add(x));
        popupBox.classList.add(this._config.classHide);
        popupBox.onclick = ((evt) => {
            if (evt.originalTarget == popupBox) {
                evt.stopPropagation();
                this.closeChangePlacePopup();
            }
        }).bind(this);
        document.body.appendChild(popupBox);

        // Content box
        let popupContentBox = this._state.popupContentContainer = document.createElement('div') as HTMLDivElement;
        this._config.popupContentClasses.forEach(x => popupContentBox.classList.add(x));
        popupBox.appendChild(popupContentBox);

        // Country
        let rowCountry = document.createElement('div');
        rowCountry.classList.add(this._config.classDataRow);
        popupContentBox.appendChild(rowCountry);

        let rowCountryLabel = document.createElement('label');
        rowCountryLabel.textContent = this._config.textCountry;
        rowCountryLabel.classList.add(this._config.classLabelBox);
        rowCountry.appendChild(rowCountryLabel);

        this._state.elementCountries = document.createElement('select');
        this._state.elementCountries.onchange = this.countryChanged.bind(this);
        rowCountry.appendChild(this._state.elementCountries);

        // City
        let rowCity = document.createElement('div');
        rowCity.classList.add(this._config.classDataRow);
        popupContentBox.appendChild(rowCity);

        let rowCityLabel = document.createElement('label');
        rowCityLabel.textContent = this._config.textCity;
        rowCityLabel.classList.add(this._config.classLabelBox);
        rowCity.appendChild(rowCityLabel);

        this._state.elementCities = document.createElement('select');
        rowCity.appendChild(this._state.elementCities);

        // Ok
        let rowOk = document.createElement('div');
        rowOk.classList.add(this._config.classDataRow);
        popupContentBox.appendChild(rowOk);

        let rowOkLabel = document.createElement('label');
        rowOkLabel.textContent = ' ';
        rowOkLabel.classList.add(this._config.classLabelBox);
        rowOk.appendChild(rowOkLabel);

        let rowOkBtn = document.createElement('input');
        rowOkBtn.type = 'button';
        rowOkBtn.classList.add(this._config.classBtn);
        rowOkBtn.value = this._config.textSet;
        rowOkBtn.onclick = this.setPlace.bind(this);
        rowOk.appendChild(rowOkBtn);
    }

    public OpenChangePlacePopup(): void {
        this._state.body.classList.add(this._config.classOverflowHidden);
        this._state.popupContainer.classList.remove(this._config.classHide);
    }

    private allPlacesLoaded(places: PlaceInfo[]): void {
        this._state.allPlaces = places;

        this.fillDropdownCountries();
    }

    private fillDropdownCountries() {
        let sortedCountries = this._state.allPlaces.sort((a, b) => {
            if (a[1] < b[1]) return -1;
            if (a[1] > b[1]) return 1;
            return 0;
        });

        sortedCountries.forEach((country, index) => {
            let countryOption = document.createElement('option');
            countryOption.value = country.Id.toString();
            countryOption.text = country.DisplayName;
            this._state.elementCountries.options.add(countryOption);

            if (index == 0)
                this.fillDropdownCities(country.Id);
        });
    };

    private countryChanged() {
        let countryId = parseInt(this._state.elementCountries.value);
        this.fillDropdownCities(countryId);
    };

    private fillDropdownCities(countryId: number) {
        this._state.elementCities.options.length = 0;

        let country = this._state.allPlaces.find((place) => place.Id == countryId);

        let sortedCities = country.Children.sort((a, b) => {
            if (a[1] < b[1]) return -1;
            if (a[1] > b[1]) return 1;
            return 0;
        });

        sortedCities.forEach(city => {
            let countryOption = document.createElement('option');
            countryOption.value = city.Id.toString();
            countryOption.text = city.DisplayName;
            this._state.elementCities.options.add(countryOption);
        });
    };

    private closeChangePlacePopup(): void {
        this._state.body.classList.remove(this._config.classOverflowHidden);
        this._state.popupContainer.classList.add(this._config.classHide);
    }

    private setPlace(): void {
        let countryId = parseInt(this._state.elementCountries.value);
        let country = this._state.allPlaces.find(place => place.Id == countryId);

        let placeId = parseInt(this._state.elementCities.value);
        let pickedPlace = country.Children.find(place => place.Id == placeId);

        this._state.elementCurrentPlaceSpan.textContent = pickedPlace.DisplayName;
        this.closeChangePlacePopup();
        this._state.onPlacePicked(pickedPlace);
    }
}

class EscortFiltersManager {
    private _config = {
        elementAgeFromId: 'FiltersAgeFrom',
        elementAgeToId: 'FiltersAgeTo',
        elementFilterBtnId: 'FilterBtn',
        changePlaceBtnId: 'nav-locale-change-btn',
        changePlaceBtnTextSelect: 'Select',
        changePlaceBtnTextChange: 'Change',
        ageMin: 18,
        ageMax: 59
    };
    private _state: {
        placePicker: PlacePicker,

        containerId: string,
        container: HTMLDivElement,
        pickedPlace: PlaceInfo,
        elementAgeFrom: HTMLSelectElement,
        elementAgeTo: HTMLSelectElement,
        filterBtn: HTMLInputElement,
        onFilterApplied: Function,
        changePlaceBtn: HTMLElement
    } = {
            placePicker: null,

            containerId: '',
            container: null,
            pickedPlace: null,
            elementAgeFrom: null,
            elementAgeTo: null,
            filterBtn: null,
            onFilterApplied: null,
            changePlaceBtn: null
        };

    //************************************ INIT
    public Init(filtersContainerId: string, onFilterApplied: Function): void {
        this._state.placePicker = new PlacePicker();
        this._state.placePicker.Init(this.placePicked.bind(this));

        this._state.containerId = filtersContainerId;
        this._state.container = document.getElementById(this._state.containerId) as HTMLDivElement;

        this._state.onFilterApplied = onFilterApplied;

        this._state.changePlaceBtn = document.getElementById(this._config.changePlaceBtnId);
        this._state.changePlaceBtn.innerText = this._config.changePlaceBtnTextSelect;
        this._state.changePlaceBtn.onclick = this._state.placePicker.OpenChangePlacePopup.bind(this._state.placePicker);

        this.InitDom();
    }

    private InitDom() {
        this._state.elementAgeFrom = document.getElementById(this._config.elementAgeFromId) as HTMLSelectElement;
        this._state.elementAgeFrom.onchange = this.ageFromChanged.bind(this);
        this.FillAgesOptions(this._state.elementAgeFrom, 'From');

        this._state.elementAgeTo = document.getElementById(this._config.elementAgeToId) as HTMLSelectElement;
        this._state.elementAgeTo.onchange = this.ageToChanged.bind(this);
        this.FillAgesOptions(this._state.elementAgeTo, 'To');

        this._state.filterBtn = document.getElementById(this._config.elementFilterBtnId) as HTMLInputElement;
        this._state.filterBtn.onclick = this.ApplyFilterClicked.bind(this);
    }

    //************************************ Methods
    private FillAgesOptions(select: HTMLElement, nullText: string): void {
        let defaultAgeOption = document.createElement('option');
        defaultAgeOption.innerText = nullText;
        select.appendChild(defaultAgeOption);
        for (let age = this._config.ageMin; age <= this._config.ageMax; age++) {
            let ageOption = document.createElement('option');
            ageOption.value = age.toString();
            ageOption.innerText = age.toString();
            select.appendChild(ageOption);
        }
    }

    private ApplyFilterClicked() {
        if (this._state.onFilterApplied != null) {
            let values = {
                placeId: null,
                ageMin: this._state.elementAgeFrom.value,
                ageMax: this._state.elementAgeTo.value
            };
            if (this._state.pickedPlace != null)
                values.placeId = this._state.pickedPlace.Id;

            this._state.onFilterApplied.apply(this, [values]);
        }
    }

    private ageFromChanged(): void {
        let valueFrom = this._state.elementAgeFrom.value;
        if (valueFrom != '') {
            let valueTo = this._state.elementAgeTo.value;
            if (valueTo != '' && Number.parseInt(valueFrom) > Number.parseInt(valueTo)) {
                this._state.elementAgeTo.value = valueFrom;
            }
        }
    }

    private ageToChanged(): void {
        let valueTo = this._state.elementAgeTo.value;
        if (valueTo != '') {
            let valueFrom = this._state.elementAgeFrom.value;
            if (valueFrom != '' && Number.parseInt(valueFrom) > Number.parseInt(valueTo)) {
                this._state.elementAgeFrom.value = valueTo;
            }
        }
    }

    private placePicked(place: PlaceInfo) {
        this._state.pickedPlace = place;
        if (this._state.changePlaceBtn.innerText != this._config.changePlaceBtnTextChange)
            this._state.changePlaceBtn.innerText = this._config.changePlaceBtnTextChange;
    }
}

class ListViewManager {
    private _config = {
        filtersManagerBoxId: 'Filters',
        adsContainerId: 'ad-list-container',
        adsAndPagerContainerId: 'ads-list',
        pagesBtnsBoxId: 'pages-btns-box',
        getAdsUrl: '/Escort/GetListItems',
        getAdsPageUrl: '/Escort/GetListItemsPage',
    };

    private _state: {
        filtersManager: EscortFiltersManager,

        adsContainer: HTMLDivElement,
        adsAndPagerContainer: HTMLDivElement,

        pageCurrent: number,
        pageIsOpening: boolean,

        lastFilter: any
    } = {
            filtersManager: null,

            adsContainer: null,
            adsAndPagerContainer: null,

            pageCurrent: 1,
            pageIsOpening: false,

            lastFilter: null
        };

    private getLatestFilterRequestObject(): object {
        if (this._state.lastFilter == null)
            return {};

        return {
            ageMin: this._state.lastFilter.ageMin,
            ageMax: this._state.lastFilter.ageMax,
            placeId: this._state.lastFilter.placeId
        };
    }

    private loadAds(): void {
        let requestFilter = this.getLatestFilterRequestObject();

        new Http()
            .onUrlPost(this._config.getAdsUrl)
            .onSuccess((data: string) => {
                this._state.adsAndPagerContainer.innerHTML = data;
            })
            .send(requestFilter);
    }

    private selectCurrentPageButton(): void {
        let pagesBtnsBox = document.getElementById(this._config.pagesBtnsBoxId) as HTMLDivElement;
        let childrenCollection = pagesBtnsBox.children;
        for (let i = 0; i < childrenCollection.length; i++) {
            let child = childrenCollection[i];
            if (child.getAttribute('data-page-number') == this._state.pageCurrent.toString())
                child.classList.add('btn-active');
            else
                child.classList.remove('btn-active');
        }
    }

    private filterApplied(filters) {
        this._state.lastFilter = filters;
        this.loadAds();
    }

    public OpenPage(pageToOpen: number): void {
        if (this._state.pageIsOpening)
            return;
        if (pageToOpen == this._state.pageCurrent)
            return;

        this._state.pageIsOpening = true;

        let requestFilter = this.getLatestFilterRequestObject();

        let url = this._config.getAdsPageUrl + '?page=' + pageToOpen;
        new Http()
            .onUrlPost(url)
            .onSuccess((data: string) => {
                let adsContainer = document.getElementById(this._config.adsContainerId) as HTMLDivElement;
                adsContainer.innerHTML = data;
                this._state.pageCurrent = pageToOpen;
                this.selectCurrentPageButton();
            })
            .onError(() => {
                console.warn('request failed for URL: ' + url);
            })
            .onAll(() => {
                this._state.pageIsOpening = false;
            })
            .send(requestFilter);
    }

    public Init() {
        this._state.adsContainer = document.getElementById(this._config.adsContainerId) as HTMLDivElement;
        this._state.adsAndPagerContainer = document.getElementById(this._config.adsAndPagerContainerId) as HTMLDivElement;

        this._state.filtersManager = new EscortFiltersManager();
        this._state.filtersManager.Init(this._config.filtersManagerBoxId, this.filterApplied.bind(this));

        this.loadAds();
    }
}

var listViewManager: ListViewManager;
(function () {
    listViewManager = new ListViewManager();
    listViewManager.Init();
})();