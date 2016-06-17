using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Dynamic.ObjectCopying
{
    /// <summary>
    /// Attribute indicating that the object contains properties to be copied
    /// </summary>
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

    /// <summary>
    /// Attribute indicating that the value can be copied and where to copy it
    /// </summary>
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

        /// <summary>
        /// This function will copy the "value" into the "entity"'s property marked by the "path"
        /// </summary>
        /// <param name="entity">The targeted object</param>
        /// <param name="value">The value to copy into entity</param>
        /// <param name="path">The path to the entity property</param>
        /// <returns>True if succeeded copying, otherwise false</returns>
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
        
        /// <summary>
        /// This function copies all copyable properties from an object into another object
        /// </summary>
        /// <param name="from">The source of values</param>
        /// <param name="to">The receiver of values</param>
        /// <param name="basePath">The base path, used for recursive calling</param>
        /// <returns>The receiver, whether copy was successfull or not</returns>
        public static object CopyProperties(object from, object to, string basePath = @"")
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
