using System;
using System.Collections.Generic;
using System.Xml;
using ReportPublisherConfig.Core;
using ReportPublisherConfig.model;
using ReportPublisherConfig.Models;

namespace ReportPublisherConfig.Handlers.Home
{
    public class GetHandler
    {
        private readonly XmlSerialization xmlSerialization;
        private readonly ConfigurationVariables configurationVariables;

        public GetHandler(XmlSerialization xmlSerialization, ConfigurationVariables configurationVariables)
        {
            if (xmlSerialization == null) throw new ArgumentNullException("xmlSerialization");
            if (configurationVariables == null) throw new ArgumentNullException("configurationVariables");
            this.xmlSerialization = xmlSerialization;
            this.configurationVariables = configurationVariables;
        }
        
        public HomeViewModel Execute()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(this.configurationVariables.GetPathToConfigFile());
            var configuration = this.xmlSerialization.Deserialize<configuration>(xmlDocument.InnerXml);
            return new HomeViewModel
                       {
                           Title = "Home",
                           Configuration = configuration,
                           Sites = this.configurationVariables.GetSites()
                       };
        }
    }

    public class HomeViewModel
    {
        public string Title { get; set; }
        public configuration Configuration { get; set; }
        public IEnumerable<Site> Sites { get; set; }
    }
}