namespace Portalum.Fiscalization.Middleware.Services
{
    public class AustriaTaxGroupService : ITaxGroupService
    {
        public string GetTaxGroupCode(decimal tax)
        {
            switch (tax)
            {
                case 20:
                    return "A";
                case 10:
                    return "B";
                case 13:
                    return "C";
                case 0:
                    return "D";
                case 19:
                    return "E";
                case 7:
                    return "F";
                case 5:
                    return "G";

                default:
                    break;
            }

            return "";
        }
    }
}
