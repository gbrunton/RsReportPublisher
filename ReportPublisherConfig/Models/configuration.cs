using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ReportPublisherConfig.model
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public class configuration {
        private List<Policy> policyField;    
        private List<Folder> folderField;
        private string reportLocalPathField;

        public configuration()
        {
            this.policyField = new List<Policy>();
            this.folderField = new List<Folder>();
        }

        /// <remarks/>
        [XmlElement("policy")]
        public List<Policy> policy {
            get {
                return this.policyField;
            }
            set {
                this.policyField = value;
            }
        }
    
        /// <remarks/>
        [XmlElement("folder")]
        public List<Folder> folder {
            get {
                return this.folderField;
            }
            set {
                this.folderField = value;
            }
        }
    
        /// <remarks/>
        [XmlAttribute()]
        public string reportLocalPath {
            get {
                return this.reportLocalPathField;
            }
            set {
                this.reportLocalPathField = value;
            }
        }
    }
}