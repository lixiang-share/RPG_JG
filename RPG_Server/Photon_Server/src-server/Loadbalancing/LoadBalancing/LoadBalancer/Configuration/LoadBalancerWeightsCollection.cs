// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadBalancerWeightsCollection.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.LoadBalancer.Configuration
{
    using System.Configuration;

    internal class LoadBalancerWeightsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LoadBalancerWeight(); 
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LoadBalancerWeight)element).Level; 
        }
    }
}
