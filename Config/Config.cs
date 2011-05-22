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

        public Config(string configPath)
        {
            CreateConfigDir(configPath);

            var configFile = Path.Combine(configPath, "config.xml");

            _config = GetFileConfig(configFile);
        }

        private void CreateConfigDir(string configPath)
        {
            if (!Directory.Exists(configPath))
                Directory.CreateDirectory(configPath);
        }

        private static System.Configuration.Configuration GetFileConfig(string path)
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
    }
}
