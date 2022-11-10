using Portalum.Fiscalization.SimplePos.Models;
using System.Threading.Tasks;

namespace Portalum.Fiscalization.SimplePos.Services
{
    public interface IAccountingService
    {
        Task<bool> PrintReceiptAsync(ShoppingCartItem[] shoppingCartItems);
    }
}