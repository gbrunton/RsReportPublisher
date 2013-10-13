using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ReportPublisherConfig.model
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType=true)]
    public partial class StyleSheet {
    
        private string nameField;
    
        private string commonReportField;
    
        /// <remarks/>
        [XmlAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    
        /// <remarks/>
        [XmlAttribute()]
        public string commonReport {
            get {
                return this.commonReportField;
            }
            set {
                this.commonReportField = value;
            }
        }
    }
}