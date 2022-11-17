using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml;

namespace Careful.Core.Mvvm.BindingExtension
{
    public static class BindingExtensions
    {
        public static BindingBase CloneViaXamlSerialization(this BindingBase binding)
        {
            var sb = new StringBuilder();
            var writer = XmlWriter.Create(sb, new XmlWriterSettings
            {
                Indent = true,
                ConformanceLevel = ConformanceLevel.Fragment,
                OmitXmlDeclaration = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
            });
            var mgr = new XamlDesignerSerializationManager(writer);

            // HERE BE MAGIC!!!
            mgr.XamlWriterMode = XamlWriterMode.Expression;
            // THERE WERE MAGIC!!!

            System.Windows.Markup.XamlWriter.Save(binding, mgr);
            StringReader stringReader = new StringReader(sb.ToString());
            XmlReader xmlReader = XmlReader.Create(stringReader);
            object newBinding = (object)XamlReader.Load(xmlReader);
            if (newBinding == null)
            {
                throw new ArgumentNullException("Binding could not be cloned via Xaml Serialization Stack.");
            }

            if (newBinding is Binding)
            {
                return (Binding)newBinding;
            }
            else if (newBinding is MultiBinding)
            {
                return (MultiBinding)newBinding;
            }
            else if (newBinding is PriorityBinding)
            {
                return (PriorityBinding)newBinding;
            }
            else
            {
                throw new InvalidOperationException("Binding could not be cast.");
            }
        }
    }
}