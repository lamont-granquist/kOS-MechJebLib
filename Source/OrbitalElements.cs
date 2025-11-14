using System;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Suffixed;

namespace kOS.AddOns.kOSMechJebLib
{
    /// <summary>
    /// Holds both the initial and final velocity return values from the Lambert routine.
    /// </summary>
    [kOS.Safe.Utilities.KOSNomenclature("OrbitalElements")]
    public class OrbitalElements : Structure
    {
        public double Sma  { get; }
        public double Ecc  { get; }
        public double Inc  { get; }
        public double Lan  { get; }
        public double Argp { get; }
        public double Nu   { get; }
        public double L   { get; }

        public OrbitalElements(double sma,  double ecc, double inc, double lan, double argp, double nu, double l)
        {
            Sma = sma;
            Ecc = ecc;
            Inc = inc;
            Lan = lan;
            Argp = argp;
            Nu = nu;
            L = l;
            RegisterInitializer(InitializeSuffixes);
        }

        private void InitializeSuffixes()
        {
            AddSuffix("SMA", new Suffix<ScalarDoubleValue>(() => Sma));
            AddSuffix("ECC", new Suffix<ScalarDoubleValue>(() => Ecc));
            AddSuffix("INC", new Suffix<ScalarDoubleValue>(() => Inc));
            AddSuffix("LAN", new Suffix<ScalarDoubleValue>(() => Lan));
            AddSuffix("ARGP", new Suffix<ScalarDoubleValue>(() => Argp));
            AddSuffix("NU", new Suffix<ScalarDoubleValue>(() => Nu));
            AddSuffix("L", new Suffix<ScalarDoubleValue>(() => L));
        }

        public override string ToString()
        {
            return "OrbitalElements(\n" +
                    "  :sma=" + Sma + ",\n" +
                    "  :ecc=" + Ecc + "\n" +
                    "  :inc=" + Inc + "\n" +
                    "  :lan=" + Lan + "\n" +
                    "  :argp=" + Argp + "\n" +
                    "  :nu=" + Nu + "\n" +
                    "  :l=" + L + "\n" +
                    ")";
        }
    }
}
