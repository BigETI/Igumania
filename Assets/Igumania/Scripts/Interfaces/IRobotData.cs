using Igumania.Objects;
using System.Collections.Generic;

namespace Igumania
{
    public interface IRobotData
    {
        float ElapsedTimeSinceLastLubrication { get; set; }

        float ElapsedTimeSinceLastRepair { get; set; }

        IReadOnlyList<string> RobotParts { get; }

        void SetParts(IReadOnlyList<RobotPartObjectScript> robotParts);
    }
}
