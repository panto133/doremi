using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public void BuyCoins()
    {
        IAPManager.instance.Buy10000Coins();
    }

    public void BuyCesh()
    {
        IAPManager.instance.Buy100Cesh();
    }

    public void BuyNoAds()
    {
        IAPManager.instance.BuyNoAds();
    }
}
