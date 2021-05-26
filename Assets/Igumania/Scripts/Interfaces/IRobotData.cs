using Igumania.Objects;
using System.Collections.Generic;

namespace Igumania
{
    public interface IRobotData
    {
        float ElapsedTimeSinceLastLubrication { get; set; }

        float ElapsedTimeSinceLastRepair { get; set; }

        IEnumerable<string> Parts { get; }

        void SetParts(IEnumerable<RobotPartObjectScript> robotParts);
    }
}
