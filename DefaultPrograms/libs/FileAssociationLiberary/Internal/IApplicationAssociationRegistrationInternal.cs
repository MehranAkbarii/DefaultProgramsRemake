using System;
using System.Runtime.InteropServices;

namespace FileAssociationLibrary {
    [Guid("a357134e-8c1e-4edd-8474-40a80546f54f")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IApplicationAssociationRegistrationInternal
    {
        [PreserveSig]
        int ClearUserAssociations();

        [PreserveSig]
        int SetProgIdAsDefault([MarshalAs(UnmanagedType.LPWStr)] string appId, [MarshalAs(UnmanagedType.LPWStr)] string association, AssociationType flags);

        [Obsolete]
        void DragLeave();
        [Obsolete]
        void QueryAppIsDefault();
        [Obsolete]
        void QueryAppIsDefaultAll();

        [PreserveSig]
        int QueryCurrentDefault([MarshalAs(UnmanagedType.LPWStr)] string association, AssociationType type, AssociationLevel level, [MarshalAs(UnmanagedType.LPWStr)] out string appId);


        [Obsolete]
        void DragLeave2();
        [Obsolete]
        void IsBrowserAssociation();

        [PreserveSig]
        int ExportUserAssociations([MarshalAs(UnmanagedType.LPWStr)] string fileName);

        [PreserveSig]
        int ApplyUserAssociations([MarshalAs(UnmanagedType.LPWStr)] string fileName);
    }
}
