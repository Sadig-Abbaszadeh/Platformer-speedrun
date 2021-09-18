using System.Collections.Generic;

namespace DartsGames
{
    public interface ICustomPropertyDrawable
    {
        string windowName { get; }
        string[] DrawableProperties { get; }
    }
}