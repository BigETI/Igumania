using NaughtyAttributes;
using System;
using UnityEngine;

// TODO: Fix poor API design without breaking references

[Serializable]
public class RobotsVisualChangerTier : System.Object
{
    public GameObject[] tierObjets;
}

public class RobotsVisualsControllerScript : MonoBehaviour
{
    [Dropdown("Tier")]
    public int tier;

    private int lastTier;

    private DropdownList<int> Tier()
    {
        return new DropdownList<int>()
        {
            {"No Upgrades", 0 },
            {"Bearings", 1},
            {"Bearings + Cables", 2},
            {"Full Upgrade", 3}
        };
    }

    public RobotsVisualChangerTier[] RobotTiers;

    private void UpdateVisuals()
    {
        for (int i = 0; i <= 3; i++) foreach (GameObject part in RobotTiers[i].tierObjets) part.SetActive(false);
        foreach (GameObject part in RobotTiers[tier].tierObjets) part.SetActive(true);
    }

    public void UpgradeTier()
    {
        if (tier < 3)
        {
            foreach (GameObject part in RobotTiers[tier].tierObjets) part.SetActive(false);
            tier++;
            foreach (GameObject part in RobotTiers[tier].tierObjets) part.SetActive(true);
        }
    }

    private void Start() => UpdateVisuals();

    private void Update()
    {
        if (lastTier != tier)
        {
            lastTier = tier;
            UpdateVisuals();
        }
    }
}
