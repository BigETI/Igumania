using Igumania.Objects;
using UnityEngine;
using UnityEngine.Events;
#if !UNITY_EDITOR
using UnitySceneLoaderManager;
#endif
using UnityTiming.Data;

namespace Igumania.Controllers
{
    public class ProductionControllerScript : MonoBehaviour, IProductionController
    {
        [SerializeField]
        private TimingData dayTiming = TimingData.One;

        [SerializeField]
        private long minimalProfitPerDay = 5L;

        [SerializeField]
        private long additionalProfitPerRobot = 20L;

        [SerializeField]
        private UnityEvent<long> onGameDayHasPassed = default;

        public TimingData DayTiming
        {
            get => dayTiming;
            set
            {
                dayTiming = value;
                if (dayTiming.TickTime <= float.Epsilon)
                {
                    dayTiming.TickTime = float.Epsilon + float.Epsilon;
                }
            }
        }

        public long MinimalProfitPerDay
        {
            get => minimalProfitPerDay;
            set => minimalProfitPerDay = value;
        }

        public long AdditionalProfitPerRobot
        {
            get => additionalProfitPerRobot;
            set => additionalProfitPerRobot = value;
        }

        public IProfile Profile { get; private set; }

        public float ElapsedProductionHaltingTime { get; private set; }

        public event GameDayHasPassedDelegate OnGameDayHasPassed;

        private void Start()
        {
            Profile = GameManager.SelectedProfile;
            if (Profile == null)
            {
#if UNITY_EDITOR
                Profile = Igumania.Profile.CreateNewFakeProfile();
#else
                SceneLoaderManager.LoadScenes("MainMenuScene");
#endif
            }
        }

        private void Update()
        {
            if (Profile != null)
            {
                int day_count = dayTiming.ProceedUpdate(false, false);
                if (day_count > 0)
                {
                    long profit_additions = 0L;
                    float profit_multiplier = 1.0f;
                    foreach (UpgradeObjectScript upgrade in Profile.Upgrades)
                    {
                        if (upgrade)
                        {
                            profit_additions += upgrade.ProfitAddition;
                            profit_multiplier += upgrade.ProfitMultiplierAddition;
                        }
                    }
                    foreach (IRobot robot in Profile.Robots)
                    {
                        if (robot != null)
                        {
                            profit_additions += additionalProfitPerRobot;
                            foreach (RobotPartObjectScript robot_part in robot.RobotParts)
                            {
                                if (robot_part)
                                {
                                    profit_additions += robot_part.ProfitAddition;
                                    profit_multiplier += robot_part.ProfitMultiplierAddition;
                                }
                            }
                        }
                    }
                    long profit = (long)(((minimalProfitPerDay * Mathf.Clamp((dayTiming.TickTime - ElapsedProductionHaltingTime) / dayTiming.TickTime, 0.0f, 1.0f)) + profit_additions) * profit_multiplier);
                    Profile.Money += profit;
                    if (onGameDayHasPassed != null)
                    {
                        onGameDayHasPassed.Invoke(profit);
                    }
                    OnGameDayHasPassed?.Invoke(profit);
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (dayTiming.TickTime <= float.Epsilon)
            {
                dayTiming.TickTime = float.Epsilon + float.Epsilon;
            }
        }
#endif
    }
}
