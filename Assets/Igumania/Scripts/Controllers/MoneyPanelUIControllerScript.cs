using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Igumania.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    public class MoneyPanelUIControllerScript : MonoBehaviour, IMoneyPanelUIController
    {
        [SerializeField]
        private float widthPerCharacter = 27.375f;

        [SerializeField]
        private TextMeshProUGUI moneyText = default;

        [SerializeField]
        private UnityEvent<long> onMoneyChanged = default;

        private long lastMoney;

        public float WidthPerCharacter
        {
            get => widthPerCharacter;
            set => widthPerCharacter = value;
        }

        public TextMeshProUGUI MoneyText
        {
            get => moneyText;
            set => moneyText = value;
        }

        public float BasePanelWidth { get; private set; }

        public IProfile Profile { get; private set; }

        public long Money => (Profile == null) ? 0L : Profile.Money;

        public RectTransform RectangleTransform { get; private set; }

        public event MoneyChangedDelegate OnMoneyChanged;

        private void UpdateVisuals()
        {
            if (moneyText)
            {
                string money_string = Money.ToString("n");
                moneyText.text = money_string;
                if (RectangleTransform)
                {
                    RectangleTransform.sizeDelta = new Vector2(BasePanelWidth + (WidthPerCharacter * money_string.Length), RectangleTransform.sizeDelta.y);
                }
            }
        }

        private void Start()
        {
            if (TryGetComponent(out RectTransform rectangle_transform))
            {
                RectangleTransform = rectangle_transform;
                BasePanelWidth = RectangleTransform.sizeDelta.x;
            }
            else
            {
                Debug.LogError($"Please attach a \"{ nameof(RectTransform) }\" component to this game object.", this);
            }
            Profile = GameManager.SelectedProfile;
            lastMoney = Money;
            UpdateVisuals();
        }

        private void Update()
        {
            long money = Money;
            if (lastMoney != money)
            {
                lastMoney = money;
                if (onMoneyChanged != null)
                {
                    onMoneyChanged.Invoke(money);
                }
                OnMoneyChanged?.Invoke(money);
                UpdateVisuals();
            }
        }
    }
}
