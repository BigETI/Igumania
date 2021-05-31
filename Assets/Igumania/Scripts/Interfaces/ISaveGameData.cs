using Igumania.Data;
using System.Collections.Generic;

namespace Igumania
{
    public interface ISaveGameData
    {
        IReadOnlyList<ProfileData> Profiles { get; }

        bool IsProfileAvailable(byte profileIndex);

        IProfileData CreateNewProfile(byte profileIndex, string name);

        IProfileData LoadProfile(byte profileIndex);

        void WriteProfile(byte profileIndex, string name, byte productionLevel, long money, IReadOnlyList<string> upgrades, IReadOnlyList<RobotData> robots, IEnumerable<string> passedDialogEvents);

        bool RemoveProfile(byte profileIndex);
    }
}
