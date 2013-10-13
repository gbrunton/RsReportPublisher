using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoMapper;
using Newtonsoft.Json;
using ReportPublisherConfig.Configuration.Conventions;
using ReportPublisherConfig.Core;
using ReportPublisherConfig.model;

namespace ReportPublisherConfig.Handlers.Home.save
{
    public class PostHandler
    {
        private readonly XmlSerialization xmlSerialization;
        private readonly ConfigurationVariables configurationVariables;

        public PostHandler(XmlSerialization xmlSerialization, ConfigurationVariables configurationVariables)
        {
            if (xmlSerialization == null) throw new ArgumentNullException("xmlSerialization");
            if (configurationVariables == null) throw new ArgumentNullException("configurationVariables");
            this.xmlSerialization = xmlSerialization;
            this.configurationVariables = configurationVariables;
        }
        
        [AjaxAction]
        public object Execute(jsTreeSaveInputModel jsTreeSaveInputModel)
        {
            var obj = JsonConvert.DeserializeObject<jsTreeSaveInputModel>(jsTreeSaveInputModel.data);

            var configuration = Mapper.Map<jsTreeSaveInputModel, configuration>(obj);
            configuration.reportLocalPath = @"..\";
            configuration.policy.Add(new Policy { name = @"BUILTIN\Administrators", role = new Role { name = "Content Manager" } });
            configuration.policy.Add(new Policy { name = @"NT AUTHORITY\NETWORKSERVICE", role = new Role { name = "Content Manager" } });
            configuration.policy.Add(new Policy { name = @"Domain1\AEA_DatabaseAdmins", role = new Role { name = "Content Manager" } });

            File.WriteAllText(this.configurationVariables.GetPathToConfigFile(), this.xmlSerialization.Serialize(configuration, Encoding.UTF8));
            return null;
        }
    }

    public class jsTreeSaveInputModel
    {
        public jsTreeSaveInputModel()
        {
            this.attr = new Dictionary<string, string>();
            this.children = new List<jsTreeSaveInputModel>();
        }

        public string data { get; set; }
        public Dictionary<string, string> attr { get; set; }
        public List<jsTreeSaveInputModel> children { get; set; }
    }
}