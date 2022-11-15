namespace Portalum.Fiscalization.Middleware.Services
{
    public class GermanyTaxGroupService : ITaxGroupService
    {
        public string GetTaxGroupCode(decimal tax)
        {
            switch (tax)
            {
                case 19:
                    return "A";
                case 7:
                    return "B";
                case 10.7m:
                    return "C";
                case 5.5m:
                    return "D";
                case 0:
                    return "E";
                case 16:
                    return "H";
                case 5:
                    return "I";
                case 9.5m:
                    return "J";

                default:
                    break;
            }

            return "";
        }
    }
}
