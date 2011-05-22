using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Xml;

namespace Configuration
{
    public class Config
    {
        private System.Configuration.Configuration _config;

        public HotkeysSection Hotkeys
        {
            get { return _config.GetSection("Hotkeys") as HotkeysSection; }
        }

        public Config(string configPath)
        {
            CreateConfigDir(configPath);

            var configFile = Path.Combine(configPath, "config.xml");

            _config = GetFileConfig(configFile);

            Init(_config);

            _config.Save(ConfigurationSaveMode.Modified);
        }

        private void CreateConfigDir(string configPath)
        {
            if (!Directory.Exists(configPath))
                Directory.CreateDirectory(configPath);
        }

        private System.Configuration.Configuration GetFileConfig(string path)
        {
            if (!File.Exists(path))
            {
                using (XmlTextWriter xml = new XmlTextWriter(path, Encoding.UTF8))
                {
                    xml.WriteStartDocument();
                    xml.WriteStartElement("configuration");
                    xml.WriteStartElement("appSettings");
                    xml.WriteEndElement();
                    xml.WriteEndElement();
                    xml.WriteEndDocument();
                }
            }

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap { ExeConfigFilename = path };
            return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        private void Init(System.Configuration.Configuration config)
        {
            HotkeysSection hotkeys;
            if (config.GetSection("Hotkeys") == null)
            {
                hotkeys = new HotkeysSection();
                hotkeys.SectionInformation.ForceSave = true;
                config.Sections.Add("Hotkeys", hotkeys);
            }

            hotkeys = config.Sections["Hotkeys"] as HotkeysSection;

            if (hotkeys.Select.Key == "")
            {
                hotkeys.Select.Key = "R";
                hotkeys.Select.Modifiers = "Alt+Shift";
            }
        }
    }
}
