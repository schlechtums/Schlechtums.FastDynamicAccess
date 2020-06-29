/*
Copyright 2012 Brian Adams

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;

namespace Schlechtums.FastDynamicAccess
{
    /// <summary>
    /// Interface used by FastDynamicAccess.  This is required so that the Get/Set methods on FDA accessors can be called from code.  There is one IFastDynamicAccess object per object property.
    /// </summary>
    public interface IFastDynamicAccess
    {
        /// <summary>
        /// Gets the property of an object.
        /// </summary>
        /// <param name="source">The souce object.</param>
        /// <returns></returns>
        Object Get(Object source);

        /// <summary>
        /// Sets the property of an object.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="value">The value to set.</param>
        void Set(Object target, Object value);
    }
}