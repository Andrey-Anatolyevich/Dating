/*
 @{
    var ad = Model.ViewAdModel;
    var localizer = Model.Localizer;

    var pics = ad.AdMedia.Where(x => x.FileType == ViewFileType.Jpeg).ToList();
    var mainMedia = pics.First(x => x.IsPrimary) ?? pics.FirstOrDefault();
    var mainPicScaled = mainMedia.ScaledPics.Where(x => x.PicType == ViewImageType.Preview_M).FirstOrDefault();
    var mainPicUrl = "";
    @if (pics.Any())
    {
        if (mainPicScaled != null)
        {
            mainPicUrl = mainPicScaled.RelativePath;
        }
    }
}
@section Scripts{
    <script asp-src-include="~/js/escort/ad-view.js"></script>
    <script type="text/javascript">
        var picsManager;
        (function () {
            let allPics = @Html.Raw(ad.PicsJson);
            picsManager = new AdViewPicsManager('CurrentPicImg', 'AdViewMediaPreviewPicsBox', allPics);
            picsManager.selectPic(@mainPicScaled?.ScaledPicId);
        })();
    </script>
}

@if (Model.CurrentUser.Success && Model.CurrentUser.Value.Id == ad.UserId)
{
    <div class="data-row">
        <a class="btn" asp-area="" asp-controller="Ads" asp-action="EditAd" asp-route-adId="@ad.AdId">Edit</a>
    </div>
}

<div class="data-block">
    <div class="data-row-half">
        <div class="ad-view-media-main-pic-box">
            <img id="CurrentPicImg" class="ad-view-media-main-pic-item" @*src="@mainPicUrl" *@ />
        </div>
        <div id="AdViewMediaPreviewPicsBox" class="ad-view-media-preview-box">
            @foreach (var pic in pics)
            {
                var smallestPic = pic.ScaledPics.FirstOrDefault(x => x.PicType == ViewImageType.Preview_M);
                if (smallestPic != null)
                {
                    <img class="ad-view-media-preview-item" onclick="picsManager.selectPic(@smallestPic.ScaledPicId)" src="@smallestPic.RelativePath"
                         data-pic-id="@smallestPic.ScaledPicId" />
                }
            }
        </div>
    </div>
    <div class="data-row-rest ad-view-bio-box">
        <div class="data-row">
            <span>@ad.Name</span>
        </div>
        <div class="data-row">
            <span class="ad-view-bio-key">@Model.GetLocaledString("Age"):</span>
            <span class="ad-view-bio-value">@ad.AgeYears</span>
        </div>
        @if (ad.GenderId.HasValue)
        {
            <div class="data-row">
                <span class="ad-view-bio-key">@Model.GetLocaledString("Gender"):</span>
                <span class="ad-view-bio-value">@Model.GetLocaledObject(ad.GenderId.Value)</span>
            </div>
        }
        @if (ad.HeightCm.HasValue)
        {
            <div class="data-row">
                <span class="ad-view-bio-key">@Model.GetLocaledString("Height"):</span>
                <span class="ad-view-bio-value">@ad.HeightCm.Value cm</span>
            </div>
        }
        @if (ad.WeightGr.HasValue)
        {
            <div class="data-row">
                <span class="ad-view-bio-key">@Model.GetLocaledString("Weight"):</span>
                <span class="ad-view-bio-value">@(ad.WeightGr / 1000) kg</span>
            </div>
        }
        @if (ad.HairColorId.HasValue)
        {
            <div class="data-row">
                <span class="ad-view-bio-key">@Model.GetLocaledString("Hair_color"):</span>
                <span class="ad-view-bio-value">@Model.GetLocaledObject(ad.HairColorId.Value)</span>
            </div>
        }
        @if (ad.HairLengthId.HasValue)
        {
            <div class="data-row">
                <span class="ad-view-bio-key">@Model.GetLocaledString("Hair_length"):</span>
                <span class="ad-view-bio-value">@Model.GetLocaledObject(ad.HairLengthId.Value)</span>
            </div>
        }
        @if (ad.PublicHairTypeId.HasValue)
        {
            <div class="data-row">
                <span class="ad-view-bio-key">@Model.GetLocaledString("Public_hair"):</span>
                <span class="ad-view-bio-value">@Model.GetLocaledObject(ad.PublicHairTypeId.Value)</span>
            </div>
        }
        @if (ad.BreastSizeId.HasValue)
        {
            <div class="data-row">
                <span class="ad-view-bio-key">@Model.GetLocaledString("Breast_size"):</span>
                <span class="ad-view-bio-value">@Model.GetLocaledObject(ad.BreastSizeId.Value)</span>
            </div>
        }
        @if (ad.BreastTypeId.HasValue)
        {
            <div class="data-row">
                <span class="ad-view-bio-key">@Model.GetLocaledString("Breast_type"):</span>
                <span class="ad-view-bio-value">@Model.GetLocaledObject(ad.BreastTypeId.Value)</span>
            </div>
        }
        @if (!string.IsNullOrWhiteSpace(ad.About))
        {
            <div class="data-row">
                <small>About:</small><br />
                <textarea class="ad-view-bio-textarea" readonly>@ad.About</textarea>
            </div>
        }
        @if (!string.IsNullOrWhiteSpace(ad.Pricing))
        {
            <div class="data-row">
                <small>Pricing:</small><br />
                <textarea class="ad-view-bio-textarea" readonly>@ad.Pricing</textarea>
            </div>
        }
        @if (!string.IsNullOrWhiteSpace(ad.Contacts))
        {
            <div class="data-row">
                <small>Contacts:</small><br />
                <textarea class="ad-view-bio-textarea" readonly>@ad.Contacts</textarea>
            </div>
        }
    </div>
</div>

 */

import * as React from "react";

interface DatingAdDetailsViewProps {
}

interface DatingAdDetailsViewState {
    adId: string
}

export class DatingAdDetailsView extends React.Component<DatingAdDetailsViewProps, DatingAdDetailsViewState> {
    constructor(params) {
        super(params);

        this.state = {
            adId: (this.props as any).match.params.adId
        } as DatingAdDetailsViewState
    }

    public render() {
        return (
            <React.Fragment>
                <p>Hello. Ad ID: {this.state.adId}</p>
            </React.Fragment>
        )
    }
}
