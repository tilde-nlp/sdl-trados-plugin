﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LetsMT.MTProvider {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class PluginResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PluginResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LetsMT.MTProvider.PluginResources", typeof(PluginResources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static System.Drawing.Icon LetsMT {
            get {
                object obj = ResourceManager.GetObject("LetsMT", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://www.letsmt.eu/ws/Service.asmx.
        /// </summary>
        public static string LetsMTWebServiceUrl {
            get {
                return ResourceManager.GetString("LetsMTWebServiceUrl", resourceCulture);
            }
        }
        
        public static System.Drawing.Bitmap Logo_71x23 {
            get {
                object obj = ResourceManager.GetObject("Logo_71x23", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A machine translation provider plug-in for performing translation with LetsMT! platform..
        /// </summary>
        public static string Plugin_Description {
            get {
                return ResourceManager.GetString("Plugin_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LetsMT! Machine Translation Provider.
        /// </summary>
        public static string Plugin_Name {
            get {
                return ResourceManager.GetString("Plugin_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LetsMT! Machine Translation Provider.
        /// </summary>
        public static string Plugin_NiceName {
            get {
                return ResourceManager.GetString("Plugin_NiceName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This provider retrieves machine translated text from LetsMT! platform..
        /// </summary>
        public static string Plugin_Tooltip {
            get {
                return ResourceManager.GetString("Plugin_Tooltip", resourceCulture);
            }
        }
    }
}
