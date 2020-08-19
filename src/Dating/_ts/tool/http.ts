export class Http {
    constructor() {
        this._method = 'GET';
        this._timeoutSec = 60;
    }

    protected _url: string;
    protected _method: string;
    protected _timeoutSec: number;

    protected _onSuccess: (responseText: string) => void;
    protected _onError: (responseText: string) => void;
    protected _onAll: () => void;

    public onUrlGet(url: string): Http {
        this._url = url;
        this._method = 'GET';
        return this;
    }

    public onUrlPost(url: string): Http {
        this._url = url;
        this._method = 'POST';
        return this;
    }

    public withTimeout(timeoutSec: number): Http {
        this._timeoutSec = timeoutSec;
        return this;
    }

    public onSuccess(func: (responseText: string) => void): Http {
        this._onSuccess = func;
        return this;
    }

    public onError(func: (responseText: string) => void): Http {
        this._onError = func;
        return this;
    }

    public onAll(func: () => void): Http {
        this._onAll = func;
        return this;
    }

    public send(data: object = null): Promise<string> {
        let result = new Promise<string>((resolve, reject) => {

            let fData = this.GetFormDataFromObject(data);

            let r = new XMLHttpRequest();
            r.onerror = () => {
                if (typeof (this._onError) == 'function') {
                    this._onError(r.responseText);
                }
            };
            r.abort = () => {

            };
            r.onload = () => {
            };
            r.onreadystatechange = () => {
                if (r.readyState == 4) {

                    let onAll = () => {
                        if (typeof (this._onAll) == 'function') {
                            this._onAll();
                        }
                    };


                    if (r.status == 200) {
                        if (typeof (this._onSuccess) == 'function') {
                            this._onSuccess(r.responseText);
                        }
                        onAll();
                        resolve(r.responseText);
                    }
                    else {
                        if (typeof (this._onError) == 'function') {
                            this._onError(r.responseText);
                        }
                        onAll();
                        reject(r.responseText);
                    }
                }
            };

            r.open(this._method, this._url);
            r.send(fData);
        });

        return result;
    }

    private GetFormDataFromObject(data: object): FormData {
        let fData = new FormData();
        if (data == null)
            return fData;

        let dataProps = Object.getOwnPropertyNames(data);
        for (let index = 0; index < dataProps.length; index++) {
            let propName = dataProps[index];

            let propValue = data[propName];
            if (propValue != null && typeof (propValue) != 'function') {
                fData.append(propName, propValue);
            }
        }
        return fData;
    }
}