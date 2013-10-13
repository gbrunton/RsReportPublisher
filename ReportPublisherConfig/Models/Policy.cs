using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ReportPublisherConfig.model
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType=true)]
    public class Policy {
    
        private Role roleField;
    
        private string nameField;
    
        /// <remarks/>
        public Role role {
            get {
                return this.roleField;
            }
            set {
                this.roleField = value;
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
    }
}