﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UT_Catch2Interface.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class TestStrings_TestResult {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TestStrings_TestResult() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("UT_Catch2Interface.Resources.TestStrings_TestResult", typeof(TestStrings_TestResult).Assembly);
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
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        ///&lt;Catch name=&quot;QDummy.exe&quot;&gt;
        ///  &lt;Group name=&quot;QDummy.exe&quot;&gt;
        ///    &lt;TestCase name=&quot;Dummy&quot; filename=&quot;d:\dummy.cpp&quot; line=&quot;64&quot;&gt;
        ///      &lt;Expression success=&quot;false&quot; filename=&quot;d:\dummy.cpp&quot; line=&quot;64&quot;&gt;
        ///        &lt;Original&gt;
        ///          {Unknown expression after the reported line}
        ///        &lt;/Original&gt;
        ///        &lt;Expanded&gt;
        ///          {Unknown expression after the reported line}
        ///        &lt;/Expanded&gt;
        ///        &lt;FatalErrorCondition filename=&quot;d:\dummy.cpp&quot; line=&quot;64&quot;&gt;
        ///          SIGSEGV - Segm [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Incomplete {
            get {
                return ResourceManager.GetString("Incomplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        ///&lt;Catch name=&quot;QDummy.exe&quot;&gt;
        ///  &lt;Group name=&quot;QDummy.exe&quot;&gt;
        ///    &lt;TestCase name=&quot;Dummy&quot; filename=&quot;d:\dummy.cpp&quot; line=&quot;64&quot;&gt;
        ///      &lt;Expression success=&quot;false&quot; filename=&quot;d:\dummy.cpp&quot; line=&quot;64&quot;&gt;
        ///        &lt;Original&gt;
        ///          {Unknown expression after the reported line}
        ///        &lt;/Original&gt;
        ///        &lt;Expanded&gt;
        ///          {Unknown expression after the reported line}
        ///        &lt;/Expanded&gt;
        ///        &lt;FatalErrorCondition filename=&quot;d:\dummy.cpp&quot; line=&quot;64&quot;&gt;
        ///          SIGSEGV - Segm [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PostXmlText {
            get {
                return ResourceManager.GetString("PostXmlText", resourceCulture);
            }
        }
    }
}
