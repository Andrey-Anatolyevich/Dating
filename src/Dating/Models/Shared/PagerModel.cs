namespace Dating.Models.Shared
{
    public class PagerModel
    {
        public PagerModel(int itemsTotal, int perPage)
        {
            PageMax = (int)decimal.Ceiling((decimal)itemsTotal / (decimal)perPage);
            PageCurrent = 1;
            ItemsTotal = itemsTotal;
            ItemsPerPage = perPage;
        }

        public int PageMax;
        public int PageCurrent;
        public int ItemsTotal;
        public int ItemsPerPage;
    }
}
