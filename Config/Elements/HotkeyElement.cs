using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Configuration
{
    public class HotkeyElement : ConfigurationElement
    {
        public HotkeyElement()
        {
        }

        public HotkeyElement(string key, string modifier)
        {
            Key = key;
            Modifiers = modifier;
        }

        [ConfigurationProperty("Key", IsRequired = false)]
        public string Key
        {
            get { return (string)this["Key"]; }
            set { this["Key"] = value; }
        }

        [ConfigurationProperty("Modifiers")]
        public string Modifiers
        {
            get { return (string)this["Modifiers"]; }
            set { this["Modifiers"] = value; }
        }
    }
}
