using System;

namespace ContainerLevels
{
    public interface IRuleContainerLevelCalculator
    {
        ContainerLevelTypes GetLevel(Int32 type);
    }
}