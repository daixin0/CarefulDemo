// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using PrismCommonLib.Composition.Regions;

namespace PrismCommonLib.MefExtensions.Regions
{
    /// <summary>
    /// Exports the MefRegionNavigationJournal using the Managed Extensibility Framework (MEF).
    /// </summary>
    /// <remarks>
    /// This allows the MefBootstrapper to provide this class as a default implementation.
    /// If another implementation is found, this export will not be used.
    /// </remarks>
    [Export(typeof(IRegionNavigationJournal))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MefRegionNavigationJournal : RegionNavigationJournal
    {
    }
}
