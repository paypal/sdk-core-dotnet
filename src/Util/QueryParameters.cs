using System.Collections.Generic;

namespace PayPal.Util
{
    public class QueryParameters
    {
        private static readonly string Count = "count";

        private static readonly string StartID = "start_id";

        private static readonly string StartIndex = "start_index";

        private static readonly string StartTime = "start_time";

        private static readonly string EndTime = "end_time";

        private static readonly string PayeeID = "payee_id";

        private static readonly string SortBy = "sort_by";

        private static readonly string SortOrder = "sort_order";

        private Dictionary<string, string> ContainerMap;

        public QueryParameters()
        {
            ContainerMap = new Dictionary<string, string>();
        }

        /**
         * @return the containerMap
         */
        public Dictionary<string, string> GetMap()
        {
            return ContainerMap;
        }

        /**
         * Set the count
         * 
         * @param count
         *            Number of items to return.
         */
        public void SetCount(string counter)
        {
            ContainerMap.Add(Count, counter);
        }

        /**
         * Set the startID
         * 
         * @param startid
         *            Resource ID that indicates the starting resource to return.
         */
        public void SetStartId(string startingID)
        {
            ContainerMap.Add(StartID, startingID);
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
            ContainerMap.Add(StartIndex, startingIndex);
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
            ContainerMap.Add(StartTime, startingTime);
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
            ContainerMap.Add(EndTime, endingTime);
        }

        /**
         * Set the payee id
         * 
         * @param payeeId
         *            PayeeId
         */
        public void SetPayeeId(string payID)
        {
            ContainerMap.Add(PayeeID, payID);
        }

        /**
         * Set the sort by field
         * 
         * @param sortBy
         *            Sort based on create_time or update_time.
         */
        public void SetSortBy(string sortingBy)
        {
            ContainerMap.Add(SortBy, sortingBy);
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
            ContainerMap.Add(SortOrder, sortingOrder);
        }
    }    
}