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
    [XmlType(AnonymousType=true)]
    public class Report {
    
        private List<Parameter> parameterField;
        private List<Property> propertyField;
    
        private string nameField;
    
        private string commonReportField;
    
        /// <remarks/>
        [XmlElement("parameter")]
        public List<Parameter> parameter {
            get {
                return this.parameterField;
            }
            set {
                this.parameterField = value;
            }
        }
    
        /// <remarks/>
        [XmlElement("property")]
        public List<Property> property {
            get {
                return this.propertyField;
            }
            set {
                this.propertyField = value;
            }
        }
    
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