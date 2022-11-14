using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Careful.Controls.PropertyGridControl.Data
{
    public class PropertyCollection : CompositeItem
    {
        #region Initialization

        public PropertyCollection() { }

        //public PropertyCollection(object instance)
        //    : this(instance, false)
        //{ }

        public PropertyCollection(object instance, bool noCategory, bool automaticlyExpandObjects, string filter)
        {
            Dictionary<string, PropertyCategory> groups = new Dictionary<string, PropertyCategory>();
            PropertyDescriptorCollection properties ;
            if (instance != null)
                properties = TypeDescriptor.GetProperties(instance.GetType());  //I changed here from instance to instance.GetType, so that only the Direct Properties are shown!
            else
                properties = new PropertyDescriptorCollection(new PropertyDescriptor[] { });

            List<Property> propertyCollection = new List<Property>();
            
           
            foreach (PropertyDescriptor propertyDescriptor in properties)
            {
                CollectProperties(instance, propertyDescriptor, propertyCollection, automaticlyExpandObjects, filter);
                //if (noCategory)//如果没有目录，按照名字排序
                //    propertyCollection.Sort(Property.CompareByName);
                //else//如果有目录，先按目录排序，再按名称排序
                //    propertyCollection.Sort(Property.CompareByCategoryThenByName);
            }
            
            if (noCategory)
            {
                propertyCollection.Sort(Property.CompareByName);//如果没有目录，按照名字排序
                foreach (Property property in propertyCollection)
                {
                    if (filter == "" || property.Name.ToLower().Contains(filter))
                        Items.Add(property);
                }
            }
            else
            {
                propertyCollection.Sort(Property.CompareByCategoryThenByName);//如果有目录，先按目录排序，再按名称排序
                foreach (Property property in propertyCollection)
                {
                    if (filter == "" || property.Name.ToLower().Contains(filter))
                    {
                        //检测显示filter一直为空，这里可以先不用管filter什么作用
                             
                        PropertyCategory propertyCategory;
                        if (groups.ContainsKey(property.Category))
                        {
                            propertyCategory = groups[property.Category];
                        }
                        else//增加目录
                        {
                            propertyCategory = new PropertyCategory(property.Category);
                            groups[property.Category] = propertyCategory;
                            Items.Add(propertyCategory);
                        }
                        
                        propertyCategory.Items.Add(property);
                    }
                    
                }
            }
        }

        private void CollectProperties(object instance, PropertyDescriptor descriptor, List<Property> propertyCollection, bool automaticlyExpandObjects, string filter)
        {

           
           
            if (descriptor.Attributes[typeof(FlatAttribute)] == null)
            {
                Property property = new Property(instance, descriptor);
                
                if (property.Name.ToLower() == "fontsize")
                {
                    property.Value = "18";
                 
                }       
                
                if (descriptor.IsBrowsable)
                {
                    //Add a property with Name: AutomaticlyExpandObjects
                    Type propertyType = descriptor.PropertyType;
                    if (automaticlyExpandObjects && propertyType.IsClass && !propertyType.IsArray && propertyType != typeof(string))
                    {
                        propertyCollection.Add(new ExpandableProperty(instance, descriptor, automaticlyExpandObjects, filter));
                    }
                    else if (descriptor.Converter.GetType() == typeof(ExpandableObjectConverter))
                    {
                        propertyCollection.Add(new ExpandableProperty(instance, descriptor, automaticlyExpandObjects, filter));
                    }
                    else
                        propertyCollection.Add(property);
                }
                else
                { 
                
                }
            }
            else
            {
                instance = descriptor.GetValue(instance);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(instance);
                foreach (PropertyDescriptor propertyDescriptor in properties)
                {
                    CollectProperties(instance, propertyDescriptor, propertyCollection, automaticlyExpandObjects, filter);
                }
            }
        }

        #endregion
    }
}
