namespace GourdPool
{
    public interface IPool
    {
        #region Properties

        /// <summary>
        /// The minimum number of pooled instances for this pool. If the number of items is lower than this
        /// value when it is set, the required number of items will be created to meet the minimum.
        /// </summary>
        int capacityMin { get; set; }
        
        /// <summary>
        /// The maximum number of items this pool can contain. When this threshold is reached, the pool will
        /// begin recycling instances rather than creating them anew.
        /// </summary>
        int capacityMax { get; set; }

        /// <summary>
        /// The total number of instances this pool currently contains.
        /// </summary>
        int instanceCount { get; }
    
        /// <summary>
        /// The total number of ACTIVE instances this pool currently contains.
        /// </summary>
        int activeCount { get; }
        
        /// <summary>
        /// The number of times instances in the pool have been recycled for reuse.
        /// </summary>
        int recyclesCount { get; }
        
        /// <summary>
        /// The number of times instances have been used from the pool - either available, or recycled
        /// </summary>
        int pooledUseCount { get; }
        
        /// <summary>
        /// The label for this pool; used for debugging.
        /// </summary>
        string poolLabel { get; set; }

        #endregion Properties


        #region Methods

        /// <summary>
        /// Claims and returns the next available instance from the pool.
        /// </summary>
        IClientPoolable GetNext();

        /// <summary>
        /// Manually claims a specific pooled instance.
        /// </summary>
        void ClaimInstance(IClientPoolable instance);

        /// <summary>
        /// Manually relinquishes a specific pooled instance.
        /// </summary>
        void RelinquishInstance(IClientPoolable instance);

        /// <summary>
        /// When an instance deletes itself, the pool needs to know about it.
        /// </summary>
        /// <param name="instance"></param>
        void DeleteFromInstance(IClientPoolable instance);

        /// <summary>
        /// Destroys any available instances remaining in the pool.
        /// </summary>
        void Clean();

        /// <summary>
        /// Clears all instance from the pool and resets to empty state.
        /// </summary>
        void Clear();

        #endregion
    }
}