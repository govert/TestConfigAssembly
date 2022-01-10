using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace TestConfigAssembly
{
    public class ServerElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                return (string)this["url"];
            }
            set
            {
                this["url"] = value;
            }
        }

    }

    public class ServersCollection : ConfigurationElementCollection
    {
        public ServersCollection()
        {
            // ServerElement server = (ServerElement)CreateNewElement();
            // Add(server);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServerElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ServerElement)element).Name;
        }

        public ServerElement this[int index]
        {
            get
            {
                return (ServerElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public ServerElement this[string Name]
        {
            get
            {
                return (ServerElement)BaseGet(Name);
            }
        }

        public int IndexOf(ServerElement url)
        {
            return BaseIndexOf(url);
        }

        public void Add(ServerElement url)
        {
            BaseAdd(url);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ServerElement url)
        {
            if (BaseIndexOf(url) >= 0)
                BaseRemove(url.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    public class AddInSettings : ConfigurationSection
	{
		[ConfigurationProperty("info")]
        public string Info
        {
            get
            {
                return (string)this["info"];
            }
            set
            {
                this["info"] = value;
            }
        }


        [ConfigurationProperty("servers")]
        [ConfigurationCollection(typeof(ServerElement),
                  AddItemName = "add",
                  ClearItemsName = "clear",
                  RemoveItemName = "remove")]

        public ServersCollection Servers
        {
            get
            {
                ServersCollection serversCollection =
                    (ServersCollection)base["servers"];
                return serversCollection;
            }
        }

    }

	public static class SettingsFunction
	{
		public static object GetSettingsPath()
		{
			try
			{

                var customConfig = ConfigurationManager.GetSection("addInSettings") as AddInSettings;
                return customConfig.Info;
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
		}

        public static object GetServers()
        {
            try
            {

                var customConfig = ConfigurationManager.GetSection("addInSettings") as AddInSettings;
                var serversConfig = customConfig.Servers;
                var servers = new object[serversConfig.Count, 2];
                for (int i = 0; i < serversConfig.Count; i++)
                {
                    var server = serversConfig[i];
                    servers[i, 0] = server.Name;
                    servers[i, 1] = server.Url;
                }
                return servers;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }

}