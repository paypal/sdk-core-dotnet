using System;
using System.Collections.Generic;
using System.Text;

namespace PayPal
{
    /// <summary>
    /// Serializer for XML
    /// </summary>
    public interface XMLMessageSerializer
    {

        /// <summary>
        /// Serialize the object as XML with namespaces
        /// </summary>
        /// <returns>Serialized object as XML String</returns>
        String ToXMLString();
    }
}
