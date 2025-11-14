using System;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Suffixed;

namespace kOS.AddOns.kOSMechJebLib
{
    /// <summary>
    /// Holds both the initial and final velocity return values from the Lambert routine.
    /// </summary>
    [kOS.Safe.Utilities.KOSNomenclature("KeplerReturn")]
    public class KeplerReturn : Structure
    {
        public double Eanom { get; }
        public double Nu { get; }

        public KeplerReturn(double eanom, double nu)
        {
            Eanom = eanom;
            Nu = nu;
            RegisterInitializer(InitializeSuffixes);
        }

        private void InitializeSuffixes()
        {
            AddSuffix("EANOM", new Suffix<ScalarDoubleValue>(() => Eanom));
            AddSuffix("NU", new Suffix<ScalarDoubleValue>(() => Nu));
        }

        public override string ToString()
        {
            return "KeplerReturn(\n" +
                    "  :eanom=" + Eanom + ",\n" +
                    "  :nu=" + Nu + "\n" +
                    ")";
        }
    }
}
