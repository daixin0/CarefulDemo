// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrismCommonLib.Composition.Regions;
using System.ComponentModel.Composition;

namespace PrismCommonLib.MefExtensions.Regions
{
    /// <summary>
    /// Exports the RegionNavigationJournalEntry using the Managed Extensibility Framework (MEF).
    /// </summary>
    /// <remarks>
    /// This allows the MefBootstrapper to provide this class as a default implementation.
    /// If another implementation is found, this export will not be used.
    /// </remarks>
    [Export(typeof(IRegionNavigationJournalEntry))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MefRegionNavigationJournalEntry : RegionNavigationJournalEntry
    {
    }
}
