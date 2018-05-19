using System.Reflection;

using BattleTech;
using BattleTech.UI;
using Harmony;

namespace Pansar
{
    public static class ReflectionUtils
    {
        public static LocationDef GetChassisLocationDef(MechLabLocationWidget locationWidget)
        {
            /*Traverse.Create<LocationDef>().Field("chassisLocationDef").GetValue<Foo>();
            Traverse.Create(foo).Property("myBar").Field("secret").SetValue("world");
            Console.WriteLine(foo.GetSecret());*/
            FieldInfo fi = typeof(MechLabLocationWidget).GetField("chassisLocationDef", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);        
            return (LocationDef) fi.GetValue(locationWidget);
        }
    }
}