// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FeedbackControlSystemSection.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.LoadShedding.Configuration
{
    using System.Configuration;

    internal class FeedbackControlSystemSection : ConfigurationSection
    {
        [ConfigurationProperty("FeedbackControllers", IsDefaultCollection = true, IsRequired = true)]
        public FeedbackControllerElementCollection FeedbackControllers
        {
            get
            {
                return (FeedbackControllerElementCollection)base["FeedbackControllers"];
            }
        }

        public void Deserialize(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            this.DeserializeElement(reader, serializeCollectionKey);
        }
    }
}
