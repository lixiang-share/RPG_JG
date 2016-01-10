// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadBalancerWeightElement.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.LoadBalancer.Configuration
{
    using System.Configuration;

    using Photon.LoadBalancing.LoadShedding;

    internal class LoadBalancerWeight : ConfigurationElement
    {
        [ConfigurationProperty("Level", IsRequired = true)]
        public FeedbackLevel Level
        {
            get { return (FeedbackLevel)this["Level"]; }
            set { this["Level"] = value; }
        }


        [ConfigurationProperty("Value", IsRequired = true)]
        public int Value
        {
            get { return (int)this["Value"]; }
            set { this["Value"] = value; }
        }
    }
}
