import * as React from "react";
import { NavLink } from "react-router-dom";
import { AdsListItem } from "../../../_cs_to_ts/dating/AdsListItem";

interface DatingAdListProps {
    ad: AdsListItem
}

export class DatingAdListItem extends React.Component<DatingAdListProps, {}> {
    public render() {
        let ad = this.props.ad;
        let lastOnlineUtc = new Date(ad.lastOnline + 'Z');
        let nowUtc = new Date(new Date().toUTCString());
        let lastOnlineDiff = nowUtc.getTime() - lastOnlineUtc.getTime();
        var lastOnlineDays = Math.floor(lastOnlineDiff / (1000 * 60 * 60 * 24));
        var detailsPageUrl = `/Dating/ViewAd/${ad.adId}`

        return (
            <div data-ad-id={ad.adId} className="ad-list-item" >
                <NavLink to={detailsPageUrl}>
                    <img className="ad-list-item-pic-box-img" src={ad.picRelativeUrl} />
                </NavLink>
                <div className="ad-list-info-box-data ad-list-info-box-top" >
                    <span>{ad.name}</span>
                    <span className="ad-list-info-box-top-right">{`${ad.age} / ${ad.heightCm} / ${ad.weightGr / 1000}`}</span>
                </div>
                <div className="ad-list-info-box-data ad-list-info-box-btm" >
                    <span>Last Online: {lastOnlineDays} day{lastOnlineDays != 1 ? "s" : ""} ago </span>
                </div>
            </div>)
    }
}
