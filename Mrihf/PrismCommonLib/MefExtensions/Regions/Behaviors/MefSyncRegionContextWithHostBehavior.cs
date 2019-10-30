// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using PrismCommonLib.Composition.Regions.Behaviors;
using System.ComponentModel.Composition;

namespace PrismCommonLib.MefExtensions.Regions.Behaviors
{
    /// <summary>
    /// Exports the SyncRegionContextWithHostBehavior using the Managed Extensibility Framework (MEF).
    /// </summary>
    /// <remarks>
    /// This allows the MefBootstrapper to provide this class as a default implementation.
    /// If another implementation is found, this export will not be used.
    /// </remarks>
    [Export(typeof(SyncRegionContextWithHostBehavior))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MefSyncRegionContextWithHostBehavior : SyncRegionContextWithHostBehavior
    {
    }
}