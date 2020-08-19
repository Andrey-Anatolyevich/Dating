using Dating.Models.Ads;
using DatingCode.BusinessModels.Auth;
using DatingCode.BusinessModels.Market;
using DatingCode.Core;
using System.Web;

namespace Dating.Mapping
{
    public class AdEditInfoMapper
    {
        internal Result<AdEditInfo> GetAdEditInfo(UserInfo myUser, EditAdModel model)
        {
            var result = new AdEditInfo()
            {
                AdId = model.AdId,
                PlaceId = model.PlaceId,
                UserId = myUser.Id,
                Name = HttpUtility.HtmlDecode(model.Name),
                GenderId = model.GenderId,
                DateBorn = model.DateBorn,
                EyeColorId = model.EyeColorId,
                HairColorId = model.HairColorId,
                WeightGr = model.WeightGr,
                HeightCm = model.HeightCm,
                HairLengthId = model.HairLengthId,
                MainPicId = model.MainPicId,
                PicsIds = model.PicsIds,
                AlcoholId = model.AlcoholId,
                BodyTypeId = model.BodyTypeId,
                EducationLevelId = model.EducationLevelId,
                EthnicGroupId = model.EthnicGroupId,
                HasKids = model.HasKids,
                RelationshipStatusId = model.RelationshipStatusId,
                ReligionId = model.ReligionId,
                SmokingId = model.SmokingId,
                ZodiacSignId = model.ZodiacSignId
            };

            return Result<AdEditInfo>.NewSuccess(result);
        }
    }
}
