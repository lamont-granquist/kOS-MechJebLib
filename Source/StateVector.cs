using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Utilities;
using kOS.Suffixed;

namespace kOS.AddOns.kOSMechJebLib
{
    /// <summary>
    ///     Holds both the position and velocity of an object in body-centered World co-ordinates
    /// </summary>
    [KOSNomenclature("StateVector")]
    public class StateVector : Structure
    {
        public Vector Position { get; }
        public Vector Velocity { get; }

        public StateVector(Vector r, Vector v)
        {
            Position = r;
            Velocity = v;
            RegisterInitializer(InitializeSuffixes);
        }

        private void InitializeSuffixes()
        {
            AddSuffix(new[] { "POSITION", "POS" }, new Suffix<Vector>(() => Position));
            AddSuffix(new[] { "VELOCITY", "VEL" }, new Suffix<Vector>(() => Velocity));
        }

        public override string ToString()
        {
            return "StateVector(\n" +
                "  :position=" + Position + ",\n" +
                "  :velocity=" + Velocity + "\n" +
                ")";
        }
    }
}
