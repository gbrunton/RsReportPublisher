using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ReportPublisherConfig.model
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true)]
    public class Folder
    {
        private List<Folder> folderField;
        private List<Report> reportField;

        private string nameField;

        private bool inheritPermissionsField = true;

        private List<string> textField;
        private List<StyleSheet> styleSheetField;
        private List<sharedDataSource> sharedDataSourceField;

        /// <remarks/>
        [XmlText()]
        public List<string> Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [XmlElement("folder")]
        public List<Folder> folder
        {
            get
            {
                return this.folderField;
            }
            set
            {
                this.folderField = value;
            }
        }

        /// <remarks/>
        [XmlElement("report")]
        public List<Report> report
        {
            get
            {
                return this.reportField;
            }
            set
            {
                this.reportField = value;
            }
        }

        [XmlElement("styleSheet")]
        public List<StyleSheet> styleSheet
        {
            get
            {
                return this.styleSheetField;
            }
            set
            {
                this.styleSheetField = value;
            }
        }

        [XmlElement("sharedDataSource")]
        public List<sharedDataSource> sharedDataSource
        {
            get
            {
                return this.sharedDataSourceField;
            }
            set
            {
                this.sharedDataSourceField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool inheritPermissions
        {
            get
            {
                return this.inheritPermissionsField;
            }
            set
            {
                this.inheritPermissionsField = value;
            }
        }
    }
}