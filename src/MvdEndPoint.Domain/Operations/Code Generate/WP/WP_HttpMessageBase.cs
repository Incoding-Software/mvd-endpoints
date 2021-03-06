﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 14.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Incoding.Endpoint.Operations.Code_Generate.WP
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using Incoding.Extensions;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Workspace\mvd-endpoints\src\MvdEndPoint.Domain\Operations\Code Generate\WP\WP_HttpMessageBase.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public partial class WP_HttpMessageBase : WP_HttpMessageBaseBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\r\nnamespace ");
            
            #line 9 "C:\Workspace\mvd-endpoints\src\MvdEndPoint.Domain\Operations\Code Generate\WP\WP_HttpMessageBase.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write("\n{\n    #region << Using >>\n\n    using System;\n    using System.Collections.Concur" +
                    "rent;\n    using System.Collections.Generic;\n    using System.ComponentModel;\n   " +
                    " using System.Configuration;\n    using System.IO;\n    using System.Linq;\n    usi" +
                    "ng System.Net;\n    using System.Runtime.CompilerServices;\n    using System.Text;" +
                    "\n    using System.Threading.Tasks;    \n    using JetBrains.Annotations;\n    usin" +
                    "g Newtonsoft.Json;\n\n    #endregion\n\n    public class HttpMessageBase : INotifyPr" +
                    "opertyChanged\n    {\n        public static Action OnBefore = () => { };\n\n        " +
                    "public static Action OnAfter = () => { };\n\n        public static Action<object, " +
                    "HttpStatusCode> OnError = (o, code) =>\n                                         " +
                    "                      {\n                                                        " +
                    "           var message = string.Format(\"Http request finished with different ({0" +
                    "}) http status OK. Inner data : {1}\", code.ToString(\"G\"), o != null ? o.ToString" +
                    "() : \"\");\n                                                                   thr" +
                    "ow new ApplicationException(message);\n                                          " +
                    "                     };\n\n        public static readonly ConcurrentDictionary<str" +
                    "ing, string> Headers = new ConcurrentDictionary<string, string>();\n\n        prot" +
                    "ected static string Cookie { get; set; }\n\n        public event PropertyChangedEv" +
                    "entHandler PropertyChanged;\n\n        protected async Task PostAwait<T>(bool isCo" +
                    "mmand, Action<T> onSuccess, Action<object, HttpStatusCode> onError, Action<Model" +
                    "State[]> onValidation, Dictionary<string, object> postData)\n        {\n          " +
                    "  OnBefore();\n            string url = string.Format(\"http://{0}/Dispatcher/{1}\"" +
                    ", ConfigurationManager.AppSettings[\"incoding:domain\"], isCommand ? \"Push\" : \"Que" +
                    "ry\");\n            string response = await post(url, postData);\n            Incod" +
                    "ingResult<object> result = JsonConvert.DeserializeObject<IncodingResult<object>>" +
                    "(response);\n            if (result.statusCode == HttpStatusCode.OK)\n            " +
                    "{\n                if (result.success)\n                {\n                    Inco" +
                    "dingResult<T> deserializeObject = JsonConvert.DeserializeObject<IncodingResult<T" +
                    ">>(response);\n                    onSuccess(deserializeObject.data);\n           " +
                    "         OnAfter();\n                }\n                else\n                {\n   " +
                    "                 var modelState = JsonConvert.DeserializeObject<IncodingResult<M" +
                    "odelState[]>>(response);\n                    if (modelState.data != null && mode" +
                    "lState.data.Any())\n                    {\n                        if (onValidatio" +
                    "n == null)\n                            throw new ArgumentNullException(\"onValida" +
                    "tion\", \"Response have validation errors but onValidation behavior is was missed\"" +
                    ");\n                        onValidation(modelState.data);\n                    }\n" +
                    "                }\n            }\n            else\n            {\n                v" +
                    "ar actual = onError ?? OnError;\n                actual(result.data, result.statu" +
                    "sCode);\n            }\n        }\n\n        protected async Task<T> PostAwait<T>(bo" +
                    "ol isCommand, Action<object, HttpStatusCode> onError, Action<ModelState[]> onVal" +
                    "idation, Dictionary<string, object> postData)\n        {\n            OnBefore();\n" +
                    "            string url = string.Format(\"http://{0}/Dispatcher/{1}\", Configuratio" +
                    "nManager.AppSettings[\"incoding:domain\"], isCommand ? \"Push\" : \"Query\");\n        " +
                    "    string response = await post(url, postData);\n            IncodingResult<obje" +
                    "ct> result = JsonConvert.DeserializeObject<IncodingResult<object>>(response);\n  " +
                    "          if (result.statusCode == HttpStatusCode.OK)\n            {\n            " +
                    "    if (result.success)\n                {\n                    IncodingResult<T> " +
                    "deserializeObject = JsonConvert.DeserializeObject<IncodingResult<T>>(response);\n" +
                    "                    return deserializeObject.data;\n                }\n           " +
                    "     else\n                {\n                    var modelState = JsonConvert.Des" +
                    "erializeObject<IncodingResult<ModelState[]>>(response);\n                    if (" +
                    "modelState.data != null && modelState.data.Any())\n                    {\n        " +
                    "                if (onValidation == null)\n                            throw new " +
                    "ArgumentNullException(\"onValidation\", \"Response have validation errors but onVal" +
                    "idation behavior is was missed\");\n                        onValidation(modelStat" +
                    "e.data);\n                    }\n                }\n            }\n            else\n" +
                    "            {\n                var actual = onError ?? OnError;\n                a" +
                    "ctual(result.data, result.statusCode);\n            }\n            throw new Argum" +
                    "entException();\n        }\n\n        async Task<string> GetWebRequestCallback(Http" +
                    "WebRequest request)\n        {\n            var task = Task<WebResponse>.Factory.F" +
                    "romAsync(request.BeginGetResponse, request.EndGetResponse, null);\n\n            W" +
                    "ebResponse responseObject;\n            if (task.Exception == null)\n             " +
                    "   responseObject = task.Result;\n            else\n            {\n                " +
                    "WebException ex = task.Exception.InnerException as WebException;\n               " +
                    " responseObject = ex.Response;\n            }\n            if (responseObject?.Con" +
                    "tentType != \"application/json; charset=utf-8\")\n                throw new WebExce" +
                    "ption();\n\n            if (Cookie == null && request.CookieContainer != null && r" +
                    "equest.CookieContainer.Count != 0)\n                Cookie = request.CookieContai" +
                    "ner.GetCookieHeader(request.RequestUri);\n            var responseStream = respon" +
                    "seObject.GetResponseStream();\n            var sr = new StreamReader(responseStre" +
                    "am);\n            string received = await sr.ReadToEndAsync();\n\n            retur" +
                    "n received;\n        }\n\n        async Task<string> post(string url, Dictionary<st" +
                    "ring, object> postdata)\n        {\n            var wr = WebRequest.Create(new Uri" +
                    "(url)) as HttpWebRequest;\n            string boundary = \"-----------------------" +
                    "----\" + DateTime.Now.Ticks.ToString(\"x\");\n            byte[] boundarybytes = Enc" +
                    "oding.ASCII.GetBytes(\"\\r\\n--\" + boundary + \"\\r\\n\");\n            wr.Method = \"POS" +
                    "T\";\n            wr.Timeout = 800000;\n            wr.ContentType = \"multipart/for" +
                    "m-data; boundary=\" + boundary;\n            foreach (var header in Headers)\n     " +
                    "           wr.Headers.Add(header.Key, header.Value);\n            wr.CookieContai" +
                    "ner = new CookieContainer();\n\n            Stream rs = wr.GetRequestStream();\n\n  " +
                    "          foreach (var item in postdata)\n            {\n                rs.Write(" +
                    "boundarybytes, 0, boundarybytes.Length);\n\n                var value = item.Value" +
                    ";\n\n                if (value != null && value.GetType() == typeof(byte[]))\n     " +
                    "           {\n                    value = new HttpPostedFileBase()\n              " +
                    "              {\n                                    Content = (byte[])value,\n   " +
                    "                                 FileName = Guid.NewGuid().ToString()\n          " +
                    "                  };\n                }\n\n                if (value != null && val" +
                    "ue.GetType() == typeof(HttpPostedFileBase))\n                {\n                  " +
                    "  var postFile = (HttpPostedFileBase)value;\n\n                    string header =" +
                    " string.Format(\"Content-Disposition: form-data; name=\\\"{0}\\\"; filename=\\\"{1}\\\"\\r" +
                    "\\nContent-Type: {2}\\r\\n\\r\\n\", item.Key, postFile.FileName, \"\");\n                " +
                    "    byte[] headerbytes = Encoding.UTF8.GetBytes(header);\n                    rs." +
                    "Write(headerbytes, 0, headerbytes.Length);\n                    rs.Write(postFile" +
                    ".Content, 0, postFile.Content.Length);\n                    byte[] trailer = Enco" +
                    "ding.ASCII.GetBytes(\"\\r\\n--\" + boundary + \"--\\r\\n\");\n                    rs.Writ" +
                    "e(trailer, 0, trailer.Length);\n                }\n                else\n          " +
                    "      {\n                    string formitem = string.Format(\"Content-Disposition" +
                    ": form-data; name=\\\"{0}\\\"\\r\\n\\r\\n{1}\", item.Key, value);\n                    byt" +
                    "e[] formitembytes = Encoding.UTF8.GetBytes(formitem);\n                    rs.Wri" +
                    "te(formitembytes, 0, formitembytes.Length);\n                }\n            }\n    " +
                    "        rs.Write(boundarybytes, 0, boundarybytes.Length);\n            rs.Close()" +
                    ";\n\n            string response = await GetWebRequestCallback(wr);\n\n            r" +
                    "eturn response;\n        }\n\n        [NotifyPropertyChangedInvocator]\n        prot" +
                    "ected virtual void OnPropertyChanged([CallerMemberName] string propertyName = nu" +
                    "ll)\n        {\n            PropertyChanged?.Invoke(this, new PropertyChangedEvent" +
                    "Args(propertyName));\n        }\n\n        public class HttpPostedFileBase\n        " +
                    "{\n            public byte[] Content { get; set; }\n\n            public string Fil" +
                    "eName { get; set; }\n        }\n\n        public class IncodingResult<T>\n        {\n" +
                    "            public T data;\n\n            public HttpStatusCode statusCode;\n\n     " +
                    "       public bool success;\n        }\n\n        public class ModelState\n        {" +
                    "\n            public string errorMessage;\n\n            public bool isValid;\n\n    " +
                    "        public string name;\n        }\n    }\n}");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "C:\Workspace\mvd-endpoints\src\MvdEndPoint.Domain\Operations\Code Generate\WP\WP_HttpMessageBase.tt"

private string _NamespaceField;

/// <summary>
/// Access the Namespace parameter of the template.
/// </summary>
private string Namespace
{
    get
    {
        return this._NamespaceField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool NamespaceValueAcquired = false;
if (this.Session.ContainsKey("Namespace"))
{
    this._NamespaceField = ((string)(this.Session["Namespace"]));
    NamespaceValueAcquired = true;
}
if ((NamespaceValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("Namespace");
    if ((data != null))
    {
        this._NamespaceField = ((string)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public class WP_HttpMessageBaseBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
