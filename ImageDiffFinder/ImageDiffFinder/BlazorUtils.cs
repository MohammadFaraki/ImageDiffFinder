using ImageDiffFinder.Models.Other;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ImageDiffFinder
{
    public static class BlazorUtils
    {
        /// <summary>
        /// Saves data to the browser storage with key that is nameof(T) which T is state object type
        /// </summary>
        /// <typeparam name="T">State object type to save in browser storage</typeparam>
        /// <param name="protectedBrowserStore">It can be ProtectedLocalStorage or ProtectedSessionStorage</param>
        /// /// <param name="stateObject">State object to save in browser storage</param>
        /// <returns></returns>
        public static async Task SetStateAsync<T>(dynamic protectedBrowserStore, T stateObject)
        {
            var name = stateObject.GetType().Name;
            await protectedBrowserStore.SetAsync(name, stateObject);
        }



        /// <summary>
        /// Gets data from the browser storage with key that is nameof(T) which T is state object type
        /// </summary>
        /// <typeparam name="T">State object type to get from browser storage</typeparam>
        /// <param name="protectedBrowserStore">It can be ProtectedLocalStorage or ProtectedSessionStorage</param>
        /// /// <param name="stateObject">State object to get from browser storage</param>
        /// <returns></returns>
        public static async Task<T> GetStateAsync<T>(dynamic protectedBrowserStore)
        {
            var name = typeof(T).Name;
            var result = await protectedBrowserStore.GetAsync<T>(name);
            //stateObject = result.Success ? result.Value : default;

            return result.Success ? result.Value : default;
        }

        

    }
}
