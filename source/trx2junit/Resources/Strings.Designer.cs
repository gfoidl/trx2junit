﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace trx2junit.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("trx2junit.Resources.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --output specified, but no value is given. An output-directory needs to specified in this case..
        /// </summary>
        internal static string Args_Output_no_Value {
            get {
                return ResourceManager.GetString("Args_Output_no_Value", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TestSuite.Key is null.
        /// </summary>
        internal static string TestSuite_key_is_null {
            get {
                return ResourceManager.GetString("TestSuite_key_is_null", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No supported resulttype found in &lt;Results&gt;.
        /// </summary>
        internal static string Trx_not_supported_result_type {
            get {
                return ResourceManager.GetString("Trx_not_supported_result_type", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Given xml file is not a valid junit file.
        /// </summary>
        internal static string Xml_not_valid_junit {
            get {
                return ResourceManager.GetString("Xml_not_valid_junit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Given xml file is not a valid junit file, attribute &apos;tests&apos; is missing on testsuite-element.
        /// </summary>
        internal static string Xml_not_valid_junit_missing_tests {
            get {
                return ResourceManager.GetString("Xml_not_valid_junit_missing_tests", resourceCulture);
            }
        }
    }
}
