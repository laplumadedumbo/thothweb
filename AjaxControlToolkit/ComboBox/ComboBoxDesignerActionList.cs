using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace AjaxControlToolkit
{
    internal class ComboBoxDesignerActionList : DesignerActionList
    {
        private ComboBox _comboBox;

        public ComboBoxDesignerActionList(IComponent component)
            : base(component)
        {
            _comboBox = (ComboBox)component;
        }

        public bool AppendDataBoundItems
        {
            get { return _comboBox.AppendDataBoundItems; }
            set { SetComponentProperty("AppendDataBoundItems", value); }
        }

        public bool CaseSensitive
        {
            get { return _comboBox.CaseSensitive; }
            set { SetComponentProperty("CaseSensitive", value); }
        }

        public ComboBoxStyle DropDownStyle
        {
            get { return _comboBox.DropDownStyle; }
            set { SetComponentProperty("DropDownStyle", value); }
        }

        public ComboBoxAutoCompleteMode AutoCompleteMode
        {
            get { return _comboBox.AutoCompleteMode; }
            set { SetComponentProperty("AutoCompleteMode", value); }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection actionItems = new DesignerActionItemCollection();
            DesignerActionPropertyItem propertyItem;

            propertyItem = GetPropertyItem("AppendDataBoundItems", "Append DataBound Items");
            if (propertyItem != null)
                actionItems.Add(propertyItem);

            propertyItem = GetPropertyItem("CaseSensitive", "Case Sensitive");
            if (propertyItem != null)
                actionItems.Add(propertyItem);

            propertyItem = GetPropertyItem("DropDownStyle", "DropDown Style");
            if (propertyItem != null)
                actionItems.Add(propertyItem);

            propertyItem = GetPropertyItem("AutoCompleteMode", "AutoComplete Mode");
            if (propertyItem != null)
                actionItems.Add(propertyItem);

            return actionItems;
        }

        protected virtual DesignerActionPropertyItem GetPropertyItem(string propertyName, string displayName)
        {
            PropertyDescriptor property = TypeDescriptor.GetProperties(_comboBox)[propertyName];
            if (property != null && property.IsBrowsable)
                return new DesignerActionPropertyItem(propertyName, displayName, property.Category, property.Description);
            return null;
        }

        protected virtual void SetComponentProperty(string propertyName, object value)
        {
            PropertyDescriptor property = TypeDescriptor.GetProperties(_comboBox)[propertyName];

            if (property == null)
                throw new ArgumentException("Property not found", propertyName);
            else
                property.SetValue(_comboBox, value);
        }

    }
}
