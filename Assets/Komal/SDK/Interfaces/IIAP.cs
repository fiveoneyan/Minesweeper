/* Brief: IAP Interface
 * Author: Komal
 * Date: "2019-07-19"
 */
namespace komal.sdk
{
    public interface IIAP
    {
        void Purchase(string productKey);
        bool IsPurchased(string productKey);
        void RestorePurchases();
        bool IsSupportRestorePurchases();
        void RemoveAds();
        bool IsAdsRemoved();
        bool IsPurchasing();
    }
}
