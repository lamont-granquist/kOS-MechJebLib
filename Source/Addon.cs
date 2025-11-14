using System.Reflection;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Utilities;
using kOS.Suffixed;
using MechJebLib.Functions;
using MechJebLib.Lambert;
using MechJebLib.Primitives;
using MechJebLib.TwoBody;
using MechJebLibBindings;
using UnityEngine;
using static MechJebLib.Utils.Statics;

namespace kOS.AddOns.kOSMechJebLib
{
    [kOSAddon("mjlib")]
    [KOSNomenclature("MechJebLibAddon")]
    public class KOSMechJebLibAddon : Addon
    {
        public const string Name = "kOS-MechJebLib";

        private readonly FieldInfo orbitField;

        public KOSMechJebLibAddon(SharedObjects shared) : base(shared)
        {
            orbitField = typeof(OrbitInfo).GetField("orbit", BindingFlags.NonPublic | BindingFlags.Instance);
            InitializeSuffixes();
        }

        // TODO:
        //  - interface to more astro functions
        //  - interface to maneuvers
        //  - interface to the ΔV analysis in MJ
        //  - interface to PSG
        //  - maybe interface to actual running MJ modules (this may be required for ΔV stats integration)
        //  - building OrbitInfo's from StateVectors?
        //  - building OrbitInfo's from OrbitalElements?
        private void InitializeSuffixes()
        {
            AddSuffix("TOBCI", new OneArgsSuffix<Vector, Vector>(v => new Vector(QuaternionD.Inverse(Planetarium.fetch.rotation) * v.ToVector3D())));
            AddSuffix("FROMBCI", new OneArgsSuffix<Vector, Vector>(v => new Vector(Planetarium.fetch.rotation * v.ToVector3D())));
            AddSuffix("TORBCI", new OneArgsSuffix<Vector, Vector>(v => new Vector((QuaternionD.Inverse(Planetarium.fetch.rotation) * v.ToVector3D()).xzy)));
            AddSuffix("FROMRBCI", new OneArgsSuffix<Vector, Vector>(v => new Vector(Planetarium.fetch.rotation * v.ToVector3D().xzy)));
            AddSuffix("STATEVECTOR", new TwoArgsSuffix<StateVector, Vector, Vector>(StateVector));
            AddSuffix("ORBITALELEMENTS", new SixArgsSuffix<OrbitalElements, ScalarValue, ScalarValue, ScalarValue, ScalarValue, ScalarValue, ScalarValue>(KeplerianValue));
            AddSuffix("STATEVECTORAT", new TwoArgsSuffix<StateVector, OrbitInfo, TimeStamp>(StateVectorAt));
            AddSuffix("TWOBODY", new ThreeArgsSuffix<StateVector, ScalarValue, ScalarValue, StateVector>(TwoBody));
            AddSuffix("TWOBODY2", new FourArgsSuffix<StateVector, ScalarValue, ScalarValue, Vector, Vector>(TwoBody));
            AddSuffix("LAMBERT", new FiveArgsSuffix<LambertReturn, ScalarValue, Vector, Vector, ScalarValue, ScalarIntValue>(Lambert));
            AddSuffix("KEPLER", new TwoArgsSuffix<KeplerReturn, ScalarValue, ScalarValue>(Kepler));
            AddSuffix("SV2OE", new TwoArgsSuffix<OrbitalElements, ScalarValue, StateVector>(KeplerianFromStateVectors));
            AddSuffix("SV2OE2", new ThreeArgsSuffix<OrbitalElements, ScalarValue, Vector, Vector>(KeplerianFromStateVectors));
            AddSuffix("OE2SV", new TwoArgsSuffix<StateVector, ScalarValue, OrbitalElements>(StateVectorsFromKeplerian));
            AddSuffix("OE2SV2", new SevenArgsSuffix<StateVector, ScalarValue, ScalarValue, ScalarValue, ScalarValue, ScalarValue, ScalarValue, ScalarValue>(StateVectorsFromKeplerian));
        }

        /// <inheritdoc />
        public override BooleanValue Available() => AssemblyLoader.loadedAssemblies.Contains("MechJebLib") && AssemblyLoader.loadedAssemblies.Contains("MechJebLibBindings");

        private static StateVector StateVector(Vector r, Vector v) => new(r, v);

        private static OrbitalElements KeplerianValue(ScalarValue l, ScalarValue ecc, ScalarValue inc, ScalarValue lan, ScalarValue argp, ScalarValue nu) => new(l / (1 - ecc * ecc), ecc, inc, lan, argp, nu, l);

        private StateVector StateVectorAt(OrbitInfo oinfo, TimeStamp ut)
        {
            var o = (Orbit)orbitField.GetValue(oinfo);
            o.GetOrbitalStateVectorsAtUT(ut.ToUnixStyleTime(), out Vector3d pos, out Vector3d vel);
            var rot = QuaternionD.Inverse(Planetarium.fetch.rotation);
            return new StateVector(new Vector((rot * pos.xzy).xzy), new Vector((rot * vel.xzy).xzy));
        }

        private static StateVector TwoBody(ScalarValue mu, ScalarValue dt, StateVector xi)
        {
            (V3 rf, V3 vf) = Shepperd.Solve(mu, dt, xi.Position.ToVector3D().ToV3(), xi.Velocity.ToVector3D().ToV3());
            return new StateVector(new Vector(rf.ToVector3d()), new Vector(vf.ToVector3d()));
        }

        private static StateVector TwoBody(ScalarValue mu, ScalarValue dt, Vector ri, Vector vi)
        {
            (V3 rf, V3 vf) = Shepperd.Solve(mu, dt, ri.ToVector3D().ToV3(), vi.ToVector3D().ToV3());
            return new StateVector(new Vector(rf.ToVector3d()), new Vector(vf.ToVector3d()));
        }

        private static LambertReturn Lambert(ScalarValue mu, Vector r1, Vector r2, ScalarValue dt, ScalarIntValue nrev)
        {
            (V3 vi, V3 vf) = Gooding.Solve(mu, r1.ToVector3D().ToV3(), V3.zero, r2.ToVector3D().ToV3(), dt, nrev, true);
            return new LambertReturn(new Vector(vi.ToVector3d()), new Vector(vf.ToVector3d()));
        }

        private static KeplerReturn Kepler(ScalarValue manom, ScalarValue ecc)
        {
            (double eanom, double nu) = Astro.AnomaliesFromMean(Deg2Rad(manom), ecc);
            return new KeplerReturn(Rad2Deg(eanom), Rad2Deg(nu));
        }

        private static OrbitalElements KeplerianFromStateVectors(ScalarValue mu, StateVector x)
        {
            (double sma, double ecc, double inc, double lan, double argp, double nu, double l) = Astro.KeplerianFromStateVectors(mu, x.Position.ToVector3D().ToV3(), x.Velocity.ToVector3D().ToV3());
            return new OrbitalElements(sma, ecc, Rad2Deg(inc), Rad2Deg(lan), Rad2Deg(argp), Rad2Deg(nu), l);
        }

        private static OrbitalElements KeplerianFromStateVectors(ScalarValue mu, Vector r, Vector v)
        {
            (double sma, double ecc, double inc, double lan, double argp, double nu, double l) = Astro.KeplerianFromStateVectors(mu, r.ToVector3D().ToV3(), v.ToVector3D().ToV3());
            return new OrbitalElements(sma, ecc, Rad2Deg(inc), Rad2Deg(lan), Rad2Deg(argp), Rad2Deg(nu), l);
        }

        private static StateVector StateVectorsFromKeplerian(ScalarValue mu, OrbitalElements elem)
        {
            (V3 r, V3 v) = Astro.StateVectorsFromKeplerian(mu, elem.L, elem.Ecc, Deg2Rad(elem.Inc), Deg2Rad(elem.Lan), Deg2Rad(elem.Argp), Deg2Rad(elem.Nu));
            return new StateVector(new Vector(r.ToVector3d()), new Vector(v.ToVector3d()));
        }

        private static StateVector StateVectorsFromKeplerian(ScalarValue mu, ScalarValue l, ScalarValue ecc, ScalarValue inc, ScalarValue lan, ScalarValue argp, ScalarValue nu)
        {
            (V3 r, V3 v) = Astro.StateVectorsFromKeplerian(mu, l, ecc, Deg2Rad(inc), Deg2Rad(lan), Deg2Rad(argp), Deg2Rad(nu));
            return new StateVector(new Vector(r.ToVector3d()), new Vector(v.ToVector3d()));
        }
    }
}
