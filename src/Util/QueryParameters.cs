using System.Collections.Generic;

namespace PayPal.Util
{
    public class QueryParameters
    {
        private static readonly string count = "count";

        private static readonly string startID = "start_id";

        private static readonly string startIndex = "start_index";

        private static readonly string startTime = "start_time";

        private static readonly string endTime = "end_time";

        private static readonly string payeeID = "payee_id";

        private static readonly string sortBy = "sort_by";

        private static readonly string sortOrder = "sort_order";

        private Dictionary<string, string> containerMap;

        public QueryParameters()
        {
            containerMap = new Dictionary<string, string>();
        }

        /// <summary>
        /// Returns the Container Map
        /// </summary>
        /// <returns></returns>        
        public Dictionary<string, string> GetMap()
        {
            return containerMap;
        }

        /// <summary>
        /// Sets the count
        /// </summary>
        /// <param name="counter"></param>
        public void SetCount(string counter)
        {
            containerMap.Add(count, counter);
        }

        /// <summary>
        /// Sets the Start ID
        /// </summary>
        /// <param name="startingID"></param>
        public void SetStartId(string startingID)
        {
            containerMap.Add(startID, startingID);
        }

        /// <summary>
        /// Sets the Start Index
        /// </summary>
        /// <param name="startingIndex"></param>
        public void SetStartIndex(string startingIndex)
        {
            containerMap.Add(startIndex, startingIndex);
        }

        /// <summary>
        /// Sets the Start Time
        /// </summary>
        /// <param name="startingTime"></param>
        public void SetStartTime(string startingTime)
        {
            containerMap.Add(startTime, startingTime);
        }

        /// <summary>
        /// Sets the Endt Time
        /// </summary>
        /// <param name="endingTime"></param>
        public void SetEndTime(string endingTime)
        {
            containerMap.Add(endTime, endingTime);
        }

        /// <summary>
        /// Set the Payee ID
        /// </summary>
        /// <param name="payID"></param>
        public void SetPayeeID(string payID)
        {
            containerMap.Add(payeeID, payID);
        }

        /// <summary>
        /// Sets the Sort By Field
        /// </summary>
        /// <param name="sortingBy"></param>
        public void SetSortBy(string sortingBy)
        {
            containerMap.Add(sortBy, sortingBy);
        }

        /// <summary>
        /// Sets the Sort Order
        /// </summary>
        /// <param name="sortingOrder"></param>
        public void SetSortOrder(string sortingOrder)
        {
            containerMap.Add(sortOrder, sortingOrder);
        }
    }    
}