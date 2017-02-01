//---------------------------------------------------------------------------- 
//
//  Copyright (C) CSharp Labs.  All rights reserved.
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
// 
// History
//  06/28/13    Created 
//  12/23/16    Validated against negative values
//---------------------------------------------------------------------------

namespace System
{
    using System.ComponentModel;

    public static class EnumValidationExtensions
    {
        /// <summary>
        /// Throws an <see cref="InvalidEnumArgumentException"/> if the enumeration <see cref="value"/> is not defined or not a valid combination of flags.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <exception cref="InvalidEnumArgumentException">The <see cref="value"/> is invalid.</exception>
        public static void ThrowIfInvalidEnumValue(this Enum value)
        {
            if (!value.IsDefined())
                throw new InvalidEnumArgumentException($"The System.Enum Type: '{value.GetType()}' Value: '{value}' is not defined or does not contains a valid bit combination.");
        }

        /// <summary>
        /// Determines if the enumeration value is defined or contains a valid combination of flags.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>true if the value is defined or a valid combination of flags; otherwise, false.</returns>
        public static bool IsDefined(this Enum value)
        {
            Type enumType = value.GetType();

            if (value.HasFlags(enumType))
                return value.AreFlagsDefined(enumType);
            else
                return Enum.IsDefined(enumType, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">An enumeration value or bit combination of flags specified in <paramref name="enumType"/>.</param>
        /// <param name="enumType">An enumeration type.</param>
        /// <returns>true if the enumeration can be treated as a set of flags; otherwise, false.</returns>
        private static bool HasFlags(this Enum value, Type enumType)
        {
            return enumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;
        }

        /// <summary>
        /// Determines if the enumeration value contains a combination of valid flags.
        /// </summary>
        /// <param name="value">A bit combination of constant values.</param>
        /// <param name="enumType">An enumeration type.</param>
        /// <returns>true if all flags are defined in the enumeration; otherwise, false.</returns>
        private static bool AreFlagsDefined(this Enum value, Type enumType)
        {
            //get the enumeration type code describing underlying type:
            TypeCode code = value.GetTypeCode();

            switch (code)
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64: //validate unsigned value
                    {
                        ulong flags = Convert.ToUInt64(value); //convert to 64-bit unsigned integer
                        ulong flag;

                        //iterate enumeration constants:
                        foreach (object obj in Enum.GetValues(enumType))
                        {
                            flag = Convert.ToUInt64(obj); //get flag constants

                            if (flag == flags) //last flag
                                return true;

                            //remove bits:
                            flags &= ~flag;
                        }
                    }
                    break;
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64: //validate signed value        
                    {
                        long flags = Convert.ToInt64(value); //convert to 64-bit signed integer

                        if (flags < 0) //validate the flags are greater-than or equal to zero
                            throw new ArgumentOutOfRangeException(nameof(value), $"The specified System.Enum value of '{flags}' and must be greater-than or equal to zero.");

                        long flag;
                        var enumValues = Enum.GetValues(enumType);

                        //iterate enumeration constants:
                        foreach (object obj in enumValues)
                        {
                            flag = Convert.ToInt64(obj); //get flag constants

                            if (flag < 0) //validate the flag is greater-than or equal to zero
                                throw new InvalidOperationException($"Cannot validate System.Enum '{enumType}', TypeCode: '{code}' with value {obj} that is less-than zero.");
                        }

                        //iterate enumeration constants:
                        foreach (object obj in enumValues)
                        {
                            flag = Convert.ToInt64(obj); //get flag constants

                            if (flag == flags) //last flag
                                return true;

                            //remove bits:
                            flags &= ~flag;
                        }
                    }
                    break;
                default:
                    throw new ArgumentException($"Cannot validate System.Enum '{enumType}' with TypeCode: '{code}'.", "value");
            }

            return false;
        }
    }
}
