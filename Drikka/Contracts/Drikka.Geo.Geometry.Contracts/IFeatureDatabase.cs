﻿namespace Drikka.Geo.Geometry.Contracts
{
    /// <summary>
    /// Feature Database
    /// </summary>
    public interface IFeatureDatabase
    {
        /// <summary>
        /// Get the FeatureSet
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <returns>FeatureSet</returns>
        IFeatureSet<T> Get<T>() where T : IFeature;

        /// <summary>
        /// Get the spatial reference
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <returns>ISpatialReference</returns>
        ISpatialReference GetSpatialReference<T>();
    }
}
