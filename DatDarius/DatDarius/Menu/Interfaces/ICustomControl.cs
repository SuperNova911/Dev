using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace DatDarius.Example.Menu.Interfaces
{
    // ReSharper disable once TypeParameterCanBeVariant
    public interface ICustomControl<TValueBase>
    {
        ValueBase<TValueBase> GetValueBase();
    }
}