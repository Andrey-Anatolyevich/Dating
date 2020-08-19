import * as React from "react";
import { MergeTool } from "../tool/mergeTool";
import { LocalizationSerice } from "./service/localizationService";

export class ViewComponent<P, S> extends React.Component<P, S>{
    constructor(props) {
        super(props);

        this._localizationService = LocalizationSerice.Single();
    }

    private _localizationService: LocalizationSerice;

    protected Localize(code: string): string {
        if (code == null || code == '')
            return code;

        let result = this._localizationService.Localize(code);
        return result;
    }

    protected applyStateChanges(stateChanges: S) {
        let newState = MergeTool.mergeDeep({}, this.state, stateChanges);
        this.setState(newState);
    }
}