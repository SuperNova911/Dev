﻿using DatDarius.Example.Menu.Interfaces;
using EloBuddy.SDK.Menu.Values;

namespace DatDarius.Example.Menu.Controls
{
    public class DynamicKeyBind : ICustomControl<bool>
    {
        /// <summary>
        /// The internal <see cref="KeyBind"/> used. Use <seealso cref=""/> to get the <see cref="KeyBind"/>.
        /// </summary>
        public KeyBind KeyBind;
        private readonly string _configKey;

        public DynamicKeyBind(string key, string displayName, bool defaultValue, KeyBind.BindTypes type, uint defaultKey1 = 27, uint defaultKey2 = 27)
        {
            _configKey = key;
            KeyBind = new KeyBind(displayName, defaultValue, type, defaultKey1, defaultKey2);
            Properties.SetKey(_configKey, KeyBind);
            KeyBind.OnValueChange += KeyBind_OnValueChange;
        }

        private void KeyBind_OnValueChange(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            Properties.SetKey(_configKey, KeyBind);
        }

        public ValueBase<bool> GetValueBase()
        {
            return KeyBind;
        }
    }
}