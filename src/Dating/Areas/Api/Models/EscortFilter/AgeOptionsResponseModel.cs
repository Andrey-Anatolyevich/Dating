namespace Dating.Areas.Api.Models.EscortFilter
{
    public class AgeOptionsResponseModel
    {
        public AgeOptionsResponseModel(int ageMin, int ageMax)
        {
            AgeMin = ageMin;
            AgeMax = ageMax;
        }

        public int AgeMin;
        public int AgeMax;
    }
}
