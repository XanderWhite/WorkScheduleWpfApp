using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using System.Reflection;

namespace WorkScheduleWpfApp
{
    class SettingConfig
    {
        public DateTime FirstWorkDay { get; set; }
        public DateTime FindWorkDay { get; set; }
        public int Free_SelectedIndex { get; set; }
        public int Work_SelectedIndex { get; set; }
        public string TextboxText { get; set; }

        XmlDocument doc;

        public SettingConfig()
        {
            doc = LoadConfigDocument();
            ReadFromXMLorSetDefault();
        }

        private void ReadFromXMLorSetDefault()
        {
            NameValueCollection allAppSettings = ConfigurationManager.AppSettings;

            try
            {
                FirstWorkDay = DateTime.Parse(allAppSettings["FirstWorkDay"]);
            }
            catch (Exception)
            {
                FirstWorkDay = DateTime.Now.Date;
            }

            try
            {
                FindWorkDay = DateTime.Parse(allAppSettings["FindWorkDay"]);
            }
            catch (Exception)
            {
                FindWorkDay = FirstWorkDay;
            }

            try
            {
                Free_SelectedIndex = int.Parse(allAppSettings["Free_SelectedIndex"]);
            }
            catch (Exception)
            {
                Free_SelectedIndex = 0;
            }

            try
            {
                Work_SelectedIndex = int.Parse(allAppSettings["Work_SelectedIndex"]);
            }
            catch (Exception)
            {
                Work_SelectedIndex = 0;
            }

            try
            {
                TextboxText = allAppSettings["TextboxText"];
            }
            catch (Exception)
            {
                TextboxText = "";
            }
        }

        private XmlDocument LoadConfigDocument()
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                doc.Load(Assembly.GetExecutingAssembly().Location + ".config");
                return doc;
            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new Exception("No configuration file found.", e);
            }
        }

        public void SaveSettings()
        {
            XmlNode node = doc.SelectSingleNode("//appSettings");

            if (node == null)
            {
                node = doc.CreateNode(XmlNodeType.Element, "appSettings", "");
                XmlElement root = doc.DocumentElement;
                root.AppendChild(node);
            }

            Dictionary<string, string> keyValueDict = new Dictionary<string, string>() {
                {"FirstWorkDay", FirstWorkDay.ToString()},
                {"FindWorkDay", FindWorkDay.ToString() },
                {"Free_SelectedIndex", Free_SelectedIndex.ToString()},
                {"Work_SelectedIndex",  Work_SelectedIndex.ToString() },
                {"TextboxText",TextboxText }
            };

            foreach (var pair in keyValueDict)
            {
                XmlElement element = node.SelectSingleNode(string.Format("//add[@key='{0}']", pair.Key)) as XmlElement;

                if (element != null)
                {
                    element.SetAttribute("value", pair.Value);
                }
                else
                {
                    element = doc.CreateElement("add");
                    element.SetAttribute("key", pair.Key);
                    element.SetAttribute("value", pair.Value);
                    node.AppendChild(element);
                }
            }

            doc.Save(Assembly.GetExecutingAssembly().Location + ".config");
        }
    }
}
