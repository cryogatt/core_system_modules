using System;

namespace ContainerLevels
{
    public interface IContainerLevelRule
    {
        // Evaluate if container is certain level
        bool DoesApply(Int32 type);

        // Returns the level of the rule
        ContainerLevelTypes GetLevel();
    }
}