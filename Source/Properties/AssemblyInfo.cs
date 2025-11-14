#define CIBUILD_disabled
using System.Reflection;
using System.Runtime.CompilerServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("kOS-MechJebLib")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("kOS-MechJebLib")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

[assembly: AssemblyVersion ("0.1.0.0")]
#if CIBUILD
[assembly: AssemblyFileVersion("@MAJOR@.@MINOR@.@PATCH@.@BUILD@")]
[assembly: KSPAssembly("kOS-MechJebLib", @MAJOR@, @MINOR@)]
#else
[assembly: AssemblyFileVersion("0.99.0.0")]
[assembly: KSPAssembly("kOS-MechJebLib", 0, 99)]
#endif
[assembly: KSPAssemblyDependency("kOS", 1, 5)]
[assembly: KSPAssemblyDependency("MechJeb2", 2, 5)]
