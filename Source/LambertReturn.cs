using System;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Suffixed;

namespace kOS.AddOns.kOSMechJebLib
{
    /// <summary>
    /// Holds both the initial and final velocity return values from the Lambert routine.
    /// </summary>
    [kOS.Safe.Utilities.KOSNomenclature("LambertReturn")]
    public class LambertReturn : Structure
    {
        public Vector Vi { get; }
        public Vector Vf { get; }

        public LambertReturn(Vector vi, Vector vf)
        {
            Vi = vi;
            Vf = vf;
            RegisterInitializer(InitializeSuffixes);
        }

        private void InitializeSuffixes()
        {
            AddSuffix("VI", new Suffix<Vector>(() => Vi));
            AddSuffix("VF", new Suffix<Vector>(() => Vf));
        }

        public override string ToString()
        {
            return "LambertReturn(\n" +
                    "  :vi=" + Vi + ",\n" +
                    "  :vf=" + Vf + "\n" +
                    ")";
        }
    }
}
