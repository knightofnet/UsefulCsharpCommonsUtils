using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using UsefulCsharpCommonsUtils.collection;
using UsefulCsharpCommonsUtils.lang;
using UsefulCsharpCommonsUtils.lang.ext;
using UsefulCsharpCommonsUtils.ui.usercontrol;

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

        public void AddBindingKeyValueUc(KeyValueUc keyValueUc, string nomProp, string nomPropLbl)
        {
            InBindingKeyValueUc inBinding = new InBindingKeyValueUc(nomPropLbl)
            {
                Elt = keyValueUc,
                PropName = nomProp
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

        public void AddBindingCheckbox(CheckBox cbox, string nomProp)
        {
            InBindingCheckbox inBinding = new InBindingCheckbox()
            {
                Elt = cbox,
                PropName = nomProp
            };

            listBindings.Add(inBinding);
        }

        public void AddBindingPasswordboxNotSecured(PasswordBox tbPassword, string nomProp)
        {
            InBindingPasswordboxNotSecured inBinding = new InBindingPasswordboxNotSecured()
            {
                Elt = tbPassword,
                PropName = nomProp
            };

            listBindings.Add(inBinding);
        }


        public void AddBindingTextBlock(TextBlock tBlock, string nomProp)
        {
            InBindingCustom inBindingCustom = new InBindingCustom()
            {
                Elt = tBlock,
                PropName = nomProp

            };

            inBindingCustom.ReadAction = (obj, l) =>
            {
                string locNomProp = nomProp;
                string text = obj.GetType().GetProperty(locNomProp)?
                    .GetValue(obj)?.ToString() ?? string.Empty;

                ((TextBlock)l).Text = text;

                return text;
            };

            inBindingCustom.UpdateAction = (l, obj) =>
            {
                string locNomProp = nomProp;

                string text = ((TextBlock)l).Text.ToString();
                obj.GetType().GetProperty(locNomProp)?
                    .SetValue(obj, text);

                return text;
            };

            listBindings.Add(inBindingCustom);
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
                string text =  obj.GetType().GetValueOfProp(locNomProp, obj)?.ToString() ?? string.Empty;

                ((Label)l).Content = text;

                return text;
            };

            inBindingCustom.UpdateAction = (l, obj) =>
            {
                string locNomProp = nomProp;

                string text = ((Label)l).Content.ToString();
                obj.GetType().SetValueOfProp(locNomProp, obj, text); 

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
                cacheValue.AddAndReplace(key, value);
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

                string tboxText = ((TextBox)Elt).Text;
                PropertyInfo propInfo = typeof(T1).GetProperty(PropName);
                if (propInfo == null)
                {
                    throw new Exception($"Property {PropName} doesnt exist for type {typeof(T1).Name}.");
                }

                if (propInfo.PropertyType == typeof(bool))
                {

                    bool boolValue = false;
                    if (string.IsNullOrWhiteSpace(tboxText)
                        || "false".Equals(tboxText?.Trim().ToLower())
                        || "0".Equals(tboxText))
                    {
                        boolValue = false;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(tboxText)
                            && ("true".Equals(tboxText?.Trim().ToLower())
                                || "1".Equals(tboxText)))
                        {
                            boolValue = true;
                        }
                        else
                        {
                            bool.TryParse(tboxText, out boolValue);
                        }
                    }

                    obj.GetType().GetProperty(PropName)?.SetValue(obj, boolValue);
                }
                else if (propInfo.PropertyType == typeof(string))
                {
                    obj.GetType().GetProperty(PropName)?.SetValue(obj, tboxText);
                }
                else if (propInfo.PropertyType == typeof(int) || propInfo.PropertyType == typeof(long) || propInfo.PropertyType == typeof(float) || propInfo.PropertyType == typeof(decimal))
                {

                    if (int.TryParse(tboxText, out int intValue))
                    {
                        obj.GetType().GetProperty(PropName)?.SetValue(obj, intValue);
                    }
                    else if (long.TryParse(tboxText, out long longValue))
                    {
                        obj.GetType().GetProperty(PropName)?.SetValue(obj, longValue);
                    }
                    else if (float.TryParse(tboxText, out float floatValue))
                    {
                        obj.GetType().GetProperty(PropName)?.SetValue(obj, floatValue);
                    }
                    else if (decimal.TryParse(tboxText, out decimal decValue))
                    {
                        obj.GetType().GetProperty(PropName)?.SetValue(obj, decValue);
                    }

                }
                else if (propInfo.PropertyType == typeof(DateTime))
                {
                    if (DateTime.TryParse(tboxText, out DateTime dtVal))
                    {
                        obj.GetType().GetProperty(PropName)?.SetValue(obj, dtVal);
                    }
                }
                else
                {
                    obj.GetType().GetProperty(PropName)?.SetValue(obj, tboxText);
                }

                return ((TextBox)Elt).Text;
            }
        }

        private class InBindingKeyValueUc : InBinding
        {
            private string Name { get; set; }

            public InBindingKeyValueUc(string lbl)
            {
                Type = EnumTypeInBinding.KeyValueUc;
                Name = lbl;
            }

            public override string Read(T1 obj)
            {
                KeyValueUc locElt = (KeyValueUc)Elt;

                locElt.Value = obj.GetType().GetProperty(PropName)?
                    .GetValue(obj)?.ToString() ?? string.Empty;

                if (Name != null)
                {
                    locElt.Key = Name ?? string.Empty;
                }

                return ((KeyValueUc)Elt).Value;
            }

            public override string Update(T1 obj)
            {
                KeyValueUc locElt = (KeyValueUc)Elt;
                return locElt.Value;
            }
        }

        private class InBindingPasswordboxNotSecured : InBinding
        {

            public InBindingPasswordboxNotSecured()
            {
                Type = EnumTypeInBinding.PasswordboxNotSecured;
            }

            public override string Read(T1 obj)
            {
                ((PasswordBox)Elt).Password = obj.GetType().GetProperty(PropName)?
                    .GetValue(obj)?.ToString() ?? string.Empty;
                return ((PasswordBox)Elt).Password;
            }

            public override string Update(T1 obj)
            {
                obj.GetType().GetProperty(PropName)?
                    .SetValue(obj, ((PasswordBox)Elt).Password);
                return ((PasswordBox)Elt).Password;
            }
        }


        private class InBindingCheckbox : InBinding
        {

            public InBindingCheckbox()
            {
                Type = EnumTypeInBinding.CheckBox;
            }

            public override string Read(T1 obj)
            {
                bool boolValue = false;
                object rawValue = obj.GetType().GetProperty(PropName)?
                    .GetValue(obj);
                if (rawValue is bool boolValueLoc)
                {
                    boolValue = boolValueLoc;
                }
                else if (rawValue is string stringValueLoc)
                {
                    if (!bool.TryParse(stringValueLoc, out boolValue))
                    {
                        boolValue = stringValueLoc.ToLower().Trim() == "1" ||
                                    stringValueLoc.ToLower().Trim() == "true";
                    }
                }

                ((CheckBox)Elt).IsChecked = boolValue;
                return boolValue.ToString();
            }

            public override string Update(T1 obj)
            {
                bool boolValue = ((CheckBox)Elt).IsChecked ?? false;
                PropertyInfo propInfo = typeof(T1).GetProperty(PropName);
                if (propInfo == null)
                {
                    throw new Exception($"Property {PropName} doesnt exist for type {typeof(T1).Name}.");
                }

                if (propInfo.PropertyType == typeof(bool))
                {
                    obj.GetType().GetProperty(PropName)?.SetValue(obj, boolValue);
                }
                else if (propInfo.PropertyType == typeof(string))
                {
                    obj.GetType().GetProperty(PropName)?.SetValue(obj, boolValue.ToString());
                }
                else if (propInfo.PropertyType == typeof(int) || propInfo.PropertyType == typeof(long) || propInfo.PropertyType == typeof(float) || propInfo.PropertyType == typeof(decimal))
                {
                    obj.GetType().GetProperty(PropName)?.SetValue(obj, boolValue ? 1 : 0);
                }

                return boolValue.ToString();
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
            RichTextBox,
            CheckBox,
            PasswordboxNotSecured,
            TextBlock,
            KeyValueUc
        }


    }
}
