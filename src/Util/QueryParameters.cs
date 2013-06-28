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

        /**
         * @return the containerMap
         */
        public Dictionary<string, string> GetMap()
        {
            return containerMap;
        }

        /**
         * Set the count
         * 
         * @param count
         *            Number of items to return.
         */
        public void SetCount(string counter)
        {
            containerMap.Add(count, counter);
        }

        /**
         * Set the startID
         * 
         * @param startid
         *            Resource ID that indicates the starting resource to return.
         */
        public void SetStartId(string startingID)
        {
            containerMap.Add(startID, startingID);
        }

        /**
         * Set the start index
         * 
         * @param startIndex
         *            Start index of the resources to be returned. Typically used to
         *            jump to a specific position in the resource history based on
         *            its order.
         */
        public void SetStartIndex(string startingIndex)
        {
            containerMap.Add(startIndex, startingIndex);
        }

        /**
         * Set the starttime
         * 
         * @param starttime
         *            Resource creation time that indicates the start of a range of
         *            results.
         */
        public void SetStartTime(string startingTime)
        {
            containerMap.Add(startTime, startingTime);
        }

        /**
         * Set the endtime
         * 
         * @param endTime
         *            Resource creation time that indicates the end of a range of
         *            results.
         */
        public void SetEndTime(string endingTime)
        {
            containerMap.Add(endTime, endingTime);
        }

        /**
         * Set the payee id
         * 
         * @param payeeId
         *            PayeeId
         */
        public void SetPayeeId(string payID)
        {
            containerMap.Add(payeeID, payID);
        }

        /**
         * Set the sort by field
         * 
         * @param sortBy
         *            Sort based on create_time or update_time.
         */
        public void SetSortBy(string sortingBy)
        {
            containerMap.Add(sortBy, sortingBy);
        }

        /**
         * Set the sort order
         * 
         * @param sortOrder
         *            Sort based on order of results. Options include asc for
         *            ascending order or dec for descending order.
         */
        public void SetSortOrder(string sortingOrder)
        {
            containerMap.Add(sortOrder, sortingOrder);
        }
    }    
}