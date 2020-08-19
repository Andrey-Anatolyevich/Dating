import { ILoader } from "./iLoader";
import { CurrentUserLoader } from "./loader/currentUserLoader";
import { PlacesLoader } from "./loader/placesLoader";
import { DatingAgeRangeLoader } from "./loader/datingAgeRangeLoader";
import { LocalizationLoader } from "./loader/localizationLoader";

export class InitialLoaderService {
    private loaders: Array<ILoader> = [
        new CurrentUserLoader(),
        new PlacesLoader(),
        new LocalizationLoader(),
        new DatingAgeRangeLoader()
    ];

    public load(): Promise<void> {

        let loadersPromises = this.loaders.map(x => x.load());

        let result = new Promise<void>((resolve, reject) => {
            Promise.all(loadersPromises)
                .then(strings => resolve())
                .catch(reason => reject(reason));
        });

        return result;
    }
}