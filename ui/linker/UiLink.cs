﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace UsefulCsharpCommonsUtils.ui.linker
{
    public partial class UiLink<T1>
    {

        public T1 Object { get; private set; }

        private Dictionary<string, string> cacheValue = new Dictionary<string, string>();

        public Type ObjType { get; }
        public UiLink(T1 obj)
        {
            Object = obj;
            ObjType = obj.GetType();
        }

        private readonly List<InBinding> listBindings = new List<InBinding>();

        public void AddCustumBinding(FrameworkElement elt, string nomProp, Func<T1, FrameworkElement, string> readAction, Func<FrameworkElement, T1, string> updateAction)
        {
            InBindingCustom inBinding = new InBindingCustom()
            {
                Elt = elt,
                ReadAction = readAction,
                UpdateAction = updateAction,
                PropName = nomProp,
            };

            listBindings.Add(inBinding);
        }

        public void AddBindingTextbox(TextBox tbox, string nomProp)
        {
            InBindingTextbox inBinding = new InBindingTextbox()
            {
                Elt = tbox,
                PropName = nomProp
            };

            listBindings.Add(inBinding);
        }

        public void AddBindingRichTextbox(RichTextBox tbox, string nomProp)
        {
            InBindingRichTextbox inBinding = new InBindingRichTextbox()
            {
                Elt = tbox,
                PropName = nomProp
            };

            listBindings.Add(inBinding);
        }

        public void AddBindingLabel(Label label, string nomProp)
        {
            InBindingCustom inBindingCustom = new InBindingCustom()
            {
                Elt = label,
                PropName = nomProp

            };

            inBindingCustom.ReadAction = (obj, l) =>
            {
                string locNomProp = nomProp;
                string text = obj.GetType().GetProperty(locNomProp)?
                    .GetValue(obj)?.ToString() ?? string.Empty;

                ((Label)l).Content = text;

                return text;
            };

            inBindingCustom.UpdateAction = (l, obj) =>
            {
                string locNomProp = nomProp;

                string text = ((Label)l).Content.ToString();
                obj.GetType().GetProperty(locNomProp)?
                    .SetValue(obj, text);

                return text;
            };

            listBindings.Add(inBindingCustom);
        }


        public void DoRead()
        {
            foreach (InBinding inBinding in listBindings)
            {
                string value = inBinding.Read(Object);
                string key = $"{inBinding.Elt.Name}";
                cacheValue.Add(key, value);
            }
        }

        public ResultUpd DoUpdate(T1 compo)
        {
            ResultUpd resultUpd = new ResultUpd();

            foreach (InBinding inBinding in listBindings)
            {
                string newValue = inBinding.Update(compo);
                string key = $"{inBinding.Elt.Name}";

                if (cacheValue.ContainsKey(key) && !cacheValue[key].Equals(newValue))
                {
                    resultUpd.HasBeenUpdated = true;
                    resultUpd.NewValuesByProperties.Add(inBinding.PropName, newValue);
                    resultUpd.OldValuesByProperties.Add(inBinding.PropName, cacheValue[key]);
                }

            }

            Object = compo;
            return resultUpd;
        }


        public class ResultUpd
        {
            public bool HasBeenUpdated { get; set; }

            public Dictionary<string, object> OldValuesByProperties { get; set; }
            public Dictionary<string, object> NewValuesByProperties { get; set; }

            public ResultUpd()
            {
                OldValuesByProperties = new Dictionary<string, object>();
                NewValuesByProperties = new Dictionary<string, object>();
            }

        }



        private abstract class InBinding
        {
            public FrameworkElement Elt { get; internal set; }

            public EnumTypeInBinding Type { get; internal set; }

            public string PropName;


            public InBinding()
            {

            }

            public abstract string Read(T1 obj);
            public abstract string Update(T1 @object);
        }

        private class InBindingCustom : InBinding
        {
            public Func<FrameworkElement, T1, string> UpdateAction { get; internal set; }
            public Func<T1, FrameworkElement, string> ReadAction { get; internal set; }

            public InBindingCustom()
            {
                Type = EnumTypeInBinding.FullCustom;
            }

            public override string Read(T1 obj)
            {
                return ReadAction?.Invoke(obj, Elt);
            }

            public override string Update(T1 obj)
            {
                return UpdateAction?.Invoke(Elt, obj);
            }
        }

        private class InBindingTextbox : InBinding
        {

            public InBindingTextbox()
            {
                Type = EnumTypeInBinding.TextBox;
            }

            public override string Read(T1 obj)
            {
                ((TextBox)Elt).Text = obj.GetType().GetProperty(PropName)?
                    .GetValue(obj)?.ToString() ?? string.Empty;
                return ((TextBox)Elt).Text;
            }

            public override string Update(T1 obj)
            {
                obj.GetType().GetProperty(PropName)?
                    .SetValue(obj, ((TextBox)Elt).Text);
                return ((TextBox)Elt).Text;
            }
        }

        private class InBindingRichTextbox : InBinding
        {

            public InBindingRichTextbox()
            {
                Type = EnumTypeInBinding.RichTextBox;
            }

            public override string Read(T1 obj)
            {
                RichTextBox rb = (RichTextBox)Elt;
                string text = obj.GetType().GetProperty(PropName)?
                    .GetValue(obj)?.ToString() ?? string.Empty;

                rb.Document.Blocks.Clear();
                rb.Document.Blocks.Add(new Paragraph(new Run(text)));

                return text;
            }

            public override string Update(T1 obj)
            {
                RichTextBox rb = (RichTextBox)Elt;
                TextRange rTr = new TextRange(rb.Document.ContentStart, rb.Document.ContentEnd);

                obj.GetType().GetProperty(PropName)?
                    .SetValue(obj, rTr.Text);
                return rTr.Text;
            }
        }


        private enum EnumTypeInBinding
        {
            FullCustom,
            TextBox,
            Combobox,
            RichTextBox
        }

    }
}