import * as React from "react";
import { NavLink } from "react-router-dom";
import { Http } from "../../../tool/http";

interface AdminLocalesListState {
    locales: LocaleInfo[];
    failedToLoadLocales: boolean;
}

export class AdminLocalesList extends React.Component<{}, AdminLocalesListState> {
    constructor(props) {
        super(props);

        this.state = {
            locales: [],
            failedToLoadLocales: false
        };
    }

    public componentDidMount() {
        new Http()
            .onUrlGet('/Admin/Locales/GetLocales')
            .onSuccess((response) => {
                var locales = JSON.parse(response) as LocaleInfo[];
                this.setState({ locales: locales });
            })
            .onError(response => {
                this.setState({ failedToLoadLocales: true })
            })
            .send();
    }

    public render() {
        return (
            <React.Fragment>
                <h3>LocalesList</h3>

                <div className="data-block">
                    {
                        this.state.failedToLoadLocales
                            ? <p>Failed to load locales.</p>
                            : this.state.locales.map((locale, index) => {
                                return <div className="data-row" key={locale.id}>
                                    <div className="data-row-label-box">
                                        <span>{locale.code}</span>
                                    </div>
                                    <div className="data-row-value-box">

                                        <span>{locale.comment}</span>
                                    </div>
                                </div>
                            })
                    }
                </div>
            </React.Fragment>
        );
    }
}