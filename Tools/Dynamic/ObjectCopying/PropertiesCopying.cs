using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Dynamic.ObjectCopying
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class CopyablePropertyAttribute : Attribute
    {
        readonly string _path;
        readonly Type _type;

        // This is a positional argument
        public CopyablePropertyAttribute(Type type, string path)
        {
            _path = path;
            _type = type;
        }

        public string Path
        {
            get { return _path; }
        }

        public Type Type
        {
            get { return _type; }
        }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class PropertyPathAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string _path;
        readonly Type _type;

        // This is a positional argument
        public PropertyPathAttribute(Type type, string path)
        {
            _type = type;
            _path = path;
        }

        public string Path
        {
            get { return _path; }
        }

        public Type Type
        {
            get { return _type; }
        }
    }

    public static class ObjectCopying
    {
        public static object GetDeepPropertyValue(object instance, string path)
        {
            var pp = path.Split('.');
            Type t = instance.GetType();
            foreach (var prop in pp)
            {
                PropertyInfo propInfo = t.GetProperty(prop);
                if (propInfo != null)
                {
                    instance = propInfo.GetValue(instance, null);
                    t = propInfo.PropertyType;
                }
                else throw new ArgumentException("Properties path is not correct");
            }
            return instance;
        }

        public static bool CopyValueToProperty(object entity, object value, string path)
        {
            var pp = path.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            
            if (pp.Count() == 1)
            {
                entity.GetType().GetProperty(pp[0]).SetValue(entity, value);
                return true;
            }

            Type t = entity.GetType();
            object instance = entity;
            PropertyInfo propInfo = null;
            for (int i = 0; i < pp.Count(); i++)
            {
                propInfo = t.GetProperty(pp[i]);
                if (propInfo != null)
                {
                    if ((i + 1) < pp.Count())
                    { 
                        instance = propInfo.GetValue(instance, null);
                        t = propInfo.PropertyType;
                    }
                }
                else
                {
                    throw new ArgumentException("Properties path is not correct");
                }
            }
            
            if (instance != null && propInfo != null)
            {
                propInfo.SetValue(instance, value);
                return true;
            }

            return false;
        }

        /*
         get from prop list
         for each
         {
             if (subobject)
             {
                copyprop(prop.getvalue, to, basepath + att.path)
             }
             else
             {
                copyvaluetoprop(to, value, basepath + '.' + att.path)
             }
         }

        return to
        */
        
        public static object CopyProperties(object from, object to, string basePath)
        {
            if (from != null && to != null)
            {
                var fromAtt = from.GetType().GetCustomAttributes<CopyablePropertyAttribute>().FirstOrDefault(x => x.Type.Equals(to.GetType()));
                if (fromAtt != null)
                {
                    var sameType = from.GetType().Equals(to.GetType());
                    var fromProps = from.GetType().GetProperties();
                    var toProps = to.GetType().GetProperties();

                    foreach (var prop in fromProps)
                    {
                        var copyableProp = prop.GetCustomAttributes<CopyablePropertyAttribute>().FirstOrDefault(x => x.Type.Equals(to.GetType()));
                        if (copyableProp != null)
                        {
                            CopyProperties(prop.GetValue(from), to, $"{basePath}.{copyableProp.Path}");
                            continue;
                        }

                        var copyableValue = prop.PropertyType.GetCustomAttributes<CopyablePropertyAttribute>().FirstOrDefault(x => x.Type.Equals(to.GetType()));
                        if (copyableValue != null)
                        {
                            CopyProperties(prop.GetValue(from), to, $"{basePath}.{copyableValue.Path}");
                            continue;
                        }

                        var attributes = prop.GetCustomAttributes<PropertyPathAttribute>();

                        var directAttribute = attributes.FirstOrDefault(x => x.Type.Equals(to.GetType()));

                        if (directAttribute != null)
                        {
                            if (!sameType)
                            {
                                var value = prop.GetValue(from);
                                CopyValueToProperty(to, value, $"{basePath}.{directAttribute.Path}");
                            }
                            else
                            {
                                prop.SetValue(to, prop.GetValue(from));
                            }
                            continue;
                        }
                        
                        PropertyInfo toProp = null;
                        var attribute = attributes.FirstOrDefault(x => toProps.FirstOrDefault(y => { toProp = y; return y.PropertyType.Equals(x.Type); }) != null);

                        if (toProp != null && attribute != null)
                        {
                            CopyValueToProperty(toProp.GetValue(to), prop.GetValue(from), attribute.Path);
                        }
                    }
                }
            }
            return to;
        }
    }
}
