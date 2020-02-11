#if UNITY_PURCHASING
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using TMPro;
namespace UnityEngine.Purchasing
{
    [RequireComponent(typeof(Button))]
    [AddComponentMenu("Unity IAP/IAP Button")]
    [HelpURL("https://docs.unity3d.com/Manual/UnityIAP.html")]
    public class P10IAPButton : IAPButton
    {
        [Tooltip("[Optional] Displays the localized title from the app store")]
        public TextMeshProUGUI titleTextTMP;

        [Tooltip("[Optional] Displays the localized description from the app store")]
        public TextMeshProUGUI descriptionTextTMP;

        [Tooltip("[Optional] Displays the localized price from the app store")]
        public TextMeshProUGUI priceTextTMP;

        void Start()
        {
            Button button = GetComponent<Button>();

            if (buttonType == ButtonType.Purchase)
            {
                if (button)
                {
                    button.onClick.AddListener(PurchaseProduct);
                    onPurchaseComplete.AddListener(OnCompletePurchase);
                }

                if (string.IsNullOrEmpty(productId))
                {
                    Debug.LogError("IAPButton productId is empty");
                }

                if (!CodelessIAPStoreListener.Instance.HasProductInCatalog(productId))
                {
                    Debug.LogWarning("The product catalog has no product with the ID \"" + productId + "\"");
                }
            }
            else if (buttonType == ButtonType.Restore)
            {
                if (button)
                {
                    button.onClick.AddListener(Restore);
                }
            }
        }

        void OnEnable()
        {
            if (buttonType == ButtonType.Purchase)
            {
                CodelessIAPStoreListener.Instance.AddButton(this);
                if (CodelessIAPStoreListener.initializationComplete)
                {
                    UpdateText();
                }
            }
        }

        void OnDisable()
        {
            if (buttonType == ButtonType.Purchase)
            {
                CodelessIAPStoreListener.Instance.RemoveButton(this);
            }
        }
        public void OnCompletePurchase(Product product)
        {
            float packPrice = IAPPackHelper.GetPackPrice(product.definition.id);
            float totalMoneyPay = PlayerPrefs.GetFloat("user_iap_value", 0);
            totalMoneyPay += packPrice;
            PlayerPrefs.SetFloat("user_iap_value", totalMoneyPay);
            if (totalMoneyPay == 0)
                Firebase.Analytics.FirebaseAnalytics.SetUserProperty("paid_user", "free");
            else if (totalMoneyPay < 5.99f)
                Firebase.Analytics.FirebaseAnalytics.SetUserProperty("paid_user", "paid");
            else if (totalMoneyPay > 5.99f)
                Firebase.Analytics.FirebaseAnalytics.SetUserProperty("paid_user", "super_paid");
        }
        void PurchaseProduct()
        {
            if (buttonType == ButtonType.Purchase)
            {
                Debug.Log("IAPButton.PurchaseProduct() with product ID: " + productId);
                Firebase.Analytics.FirebaseAnalytics.LogEvent("shop", "click_buy", productId);
                CodelessIAPStoreListener.Instance.InitiatePurchase(productId);
            }
        }

        void Restore()
        {
            if (buttonType == ButtonType.Restore)
            {
                if (Application.platform == RuntimePlatform.WSAPlayerX86 ||
                    Application.platform == RuntimePlatform.WSAPlayerX64 ||
                    Application.platform == RuntimePlatform.WSAPlayerARM)
                {
                    CodelessIAPStoreListener.Instance.ExtensionProvider.GetExtension<IMicrosoftExtensions>()
                        .RestoreTransactions();
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer ||
                         Application.platform == RuntimePlatform.OSXPlayer ||
                         Application.platform == RuntimePlatform.tvOS)
                {
                    CodelessIAPStoreListener.Instance.ExtensionProvider.GetExtension<IAppleExtensions>()
                        .RestoreTransactions(OnTransactionsRestored);
                }
                else if (Application.platform == RuntimePlatform.Android &&
                         StandardPurchasingModule.Instance().appStore == AppStore.SamsungApps)
                {
                    CodelessIAPStoreListener.Instance.ExtensionProvider.GetExtension<ISamsungAppsExtensions>()
                        .RestoreTransactions(OnTransactionsRestored);
                }
                else if (Application.platform == RuntimePlatform.Android &&
                         StandardPurchasingModule.Instance().appStore == AppStore.CloudMoolah)
                {
                    CodelessIAPStoreListener.Instance.ExtensionProvider.GetExtension<IMoolahExtension>()
                        .RestoreTransactionID((restoreTransactionIDState) =>
                        {
                            OnTransactionsRestored(
                                restoreTransactionIDState != RestoreTransactionIDState.RestoreFailed &&
                                restoreTransactionIDState != RestoreTransactionIDState.NotKnown);
                        });
                }
                else
                {
                    Debug.LogWarning(Application.platform.ToString() +
                                     " is not a supported platform for the Codeless IAP restore button");
                }
            }
        }

        void OnTransactionsRestored(bool success)
        {
            Debug.Log("Transactions restored: " + success);
        }

        internal void UpdateText()
        {
            var product = CodelessIAPStoreListener.Instance.GetProduct(productId);
            if (product != null)
            {
                if (titleTextTMP != null)
                {
                    titleTextTMP.text = product.metadata.localizedTitle;
                }

                if (descriptionTextTMP != null)
                {
                    descriptionTextTMP.text = product.metadata.localizedDescription;
                }
                if (priceTextTMP != null)
                {
                    priceTextTMP.text = product.metadata.localizedPriceString.Trim();
                }
            }
        }
    }
}
#endif
